using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace CodeeloUI
{
    [SuppressUnmanagedCodeSecurity]
    public class CodeeloLayeredWindow : IDisposable
    {
        #region [ Поля класса ]
        private int _left;
        private int _top;
        private float _opacity;
        private bool _continueLoop = true;
        private short _wndClass;
        private bool _disposed;

        private const string ClassName = "CodeeloLayeredWindow";

        private static WndProcDelegate _wndProc;

        private readonly object syncObj = new object();
        private readonly PointOrSize _size;
        private IntPtr _dcMemory;
        private IntPtr _hBmp;
        private IntPtr _oldObj;
        private static IntPtr _hInstance;
        private IntPtr _activeWindow;
        private IntPtr _hWnd;

        private BLENDFUNCTION _blend = new BLENDFUNCTION { BlendOp = 0, BlendFlags = 0, SourceConstantAlpha = 255, AlphaFormat = 1 };

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public event EventHandler Showing;
        public event CancelEventHandler Closing;
        #endregion

        #region [ Win32 API ]

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int iconId);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern short RegisterClass(WNDCLASS wc);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetMessage(ref MSG msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage(ref MSG msg);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage(ref MSG msg);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, PointOrSize pptDst, PointOrSize pSizeDst, IntPtr hdcSrc, PointOrSize pptSrc, int crKey, ref BLENDFUNCTION pBlend, int dwFlags);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class WNDCLASS
        {
            public int style;
            public WndProcDelegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr HWnd;
            public int Message;
            public IntPtr WParam;
            public IntPtr LParam;
            public int Time;
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class PointOrSize
        {
            public int XOrWidth, YOrHeight;

            public static readonly PointOrSize Empty = new PointOrSize();

            public PointOrSize() { XOrWidth = 0; YOrHeight = 0; }

            public PointOrSize(int xOrWidth, int yOrHeight)
            {
                XOrWidth = xOrWidth;
                YOrHeight = yOrHeight;
            }
        }
        #endregion

        #region [ Свойства класса ]
        private static IntPtr HInstance
        {
            get
            {
                if (_hInstance == IntPtr.Zero)
                {
                    _hInstance = Marshal.GetHINSTANCE(Assembly.GetEntryAssembly().ManifestModule);
                }
                return _hInstance;
            }
        }
        private PointOrSize LocationInternal => new PointOrSize(_left, _top);
        public IntPtr Handle => _hWnd;
        public bool Visible { get; private set; }
        public int Left
        {
            get => _left;
            set
            {
                if (_left == value)
                    return;
                _left = value;
                Update();
            }
        }
        public int Top
        {
            get => _top;
            set
            {
                if (_top == value)
                    return;
                _top = value;
                Update();
            }
        }
        public Point Location
        {
            get => new Point(_left, _top);
            set
            {
                if (_left == value.X && _top == value.Y)
                    return;
                _left = value.X;
                _top = value.Y;
                Update();
            }
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Opacity
        {
            get => _opacity;
            set
            {
                if (_opacity.Equals(value))
                    return;
                _opacity = value < 0 ? 0 : value > 1 ? 1 : value;
                _blend.SourceConstantAlpha = (byte)(_opacity * 255);
                Update();
            }
        }
        public byte Alpha
        {
            get => _blend.SourceConstantAlpha;
            set
            {
                if (_blend.SourceConstantAlpha == value)
                    return;
                _blend.SourceConstantAlpha = value;
                Update();
            }
        }
        public string Name { get; set; }
        public bool IsModal { get; private set; }
        public bool TopMost { get; set; }
        public bool Activation { get; set; }
        public bool ShowInTaskbar { get; set; }
        public bool MouseThrough { get; set; }
        public bool IsDisposed
        {
            get { lock (syncObj) { return _disposed; } }
        }
        public object Tag { get; set; }
        #endregion

        public CodeeloLayeredWindow(Bitmap bmp, bool keepBmp = false)
        {
            try
            {
                RegisterWindowClass();

                _dcMemory = CreateCompatibleDC(IntPtr.Zero);
                _hBmp = bmp.GetHbitmap(Color.Empty);
                _oldObj = SelectObject(_dcMemory, _hBmp);

                Width = bmp.Width;
                Height = bmp.Height;
                _size = new PointOrSize(Width, Height);
            }
            finally
            {
                if (!keepBmp)
                {
                    bmp.Dispose();
                }
            }
        }
        ~CodeeloLayeredWindow()
        {
            Dispose(false);
        }

        #region [ Методы ]
        private void RegisterWindowClass()
        {
            if (_wndProc == null)
            {
                _wndProc = WndProc;
            }

            var wc = new WNDCLASS
            {
                hInstance = HInstance,
                lpszClassName = ClassName,
                lpfnWndProc = _wndProc,
                hCursor = LoadCursor(IntPtr.Zero, 32512),
            };

            _wndClass = RegisterClass(wc);

            if (_wndClass == 0)
            {
                if (Marshal.GetLastWin32Error() != 0x582)
                {
                    throw new Win32Exception();
                }
            }
        }


        private void CreateWindow()
        {
            int exStyle = 0x80000;
            if (TopMost) { exStyle |= 0x8; }
            if (!Activation) { exStyle |= 0x08000000; }
            if (MouseThrough) { exStyle |= 0x20; }
            if (ShowInTaskbar) { exStyle |= 0x40000; }

            int style = unchecked((int)0x80000000)
                | 0x80000;

            _hWnd = CreateWindowEx(exStyle, ClassName, null, style,
                0, 0, 0, 0,
                IntPtr.Zero, IntPtr.Zero, HInstance, IntPtr.Zero);

            if (_hWnd == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }

        private int DoMessageLoop()
        {
            var m = new MSG();
            int result;

            while (_continueLoop && (result = GetMessage(ref m, IntPtr.Zero, 0, 0)) != 0)
            {
                if (result == -1)
                {
                    return Marshal.GetLastWin32Error();
                }
                TranslateMessage(ref m);
                DispatchMessage(ref m);
            }
            return 0;
        }

        protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case 0x10:
                    Close();
                    break;
                case 0x2:
                    EnableWindow(_activeWindow, true);
                    _continueLoop = false;
                    break;
            }
            return DefWndProc(hWnd, msg, wParam, lParam);
        }

        protected virtual IntPtr DefWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam) =>
            DefWindowProc(hWnd, msg, wParam, lParam);

        private void ShowCore(bool modal)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(Name ?? string.Empty);
            }
            lock (syncObj)
            {
                if (Visible) { return; }
                if (modal)
                {
                    IsModal = true;
                    Activation = true;
                    _activeWindow = GetActiveWindow();
                    EnableWindow(_activeWindow, false);
                }
                CreateWindow();
                ShowWindow(_hWnd, Activation ? 5 : 8);
                Visible = true;
            }
            OnShowing(EventArgs.Empty);
            if (IsDisposed) { return; }
            Update();
            if (modal)
            {
                var result = DoMessageLoop();
                SetActiveWindow(_activeWindow);
                if (result != 0)
                {
                    throw new Win32Exception(result);
                }
            }
        }

        public void Show() => ShowCore(false);

        public void ShowDialog() => ShowCore(true);

        protected virtual void OnShowing(EventArgs e)
        {
            var handle = Showing;
            handle?.Invoke(this, e);
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            var handle = Closing;
            handle?.Invoke(this, e);
        }
        protected virtual void Update()
        {
            if (!Visible)
                return;

            if (!UpdateLayeredWindow(_hWnd,
                IntPtr.Zero, LocationInternal, _size,
                _dcMemory, PointOrSize.Empty,
                0, ref _blend, 2))
            {
                if (Marshal.GetLastWin32Error() == 0x578)
                    return;

                throw new Win32Exception();
            }
        }

        public void Close()
        {
            var e = new CancelEventArgs();
            OnClosing(e);
            if (!e.Cancel)
            {
                Visible = false;
                Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (syncObj)
            {
                if (_disposed)
                    return;

                if (disposing)
                    Tag = null;

                Debug.WriteLineIf(!DestroyWindow(_hWnd), "Failed", "DestroyWindow");
                _hWnd = IntPtr.Zero;

                if (_wndClass != 0)
                {
                    if (UnregisterClass(ClassName, HInstance))
                    {
                        _wndProc = null;
                    }
                    _wndClass = 0;
                }

                Debug.WriteLineIf(SelectObject(_dcMemory, _oldObj) == IntPtr.Zero, "Failed", "Restore _oldObj");
                Debug.WriteLineIf(!DeleteDC(_dcMemory), "Failed", "Delete _dcMemory");
                Debug.WriteLineIf(!DeleteObject(_hBmp), "Failed", "Delete _hBmp");
                _oldObj = IntPtr.Zero;
                _dcMemory = IntPtr.Zero;
                _hBmp = IntPtr.Zero;

                _disposed = true;
            }
        }
        #endregion       
    }
}
