using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloFormDrag), "Icons.CodeeloFormDrag.bmp")]
    public partial class CodeeloFormDrag : Component
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private Control _sourceControl;

        public Control SourceControl
        {
            get => _sourceControl;
            set
            {
                if (_sourceControl == value)
                    return;

                if (_sourceControl != null)
                    _sourceControl.MouseDown -= new MouseEventHandler(this.SourceControl_MouseDown);

                _sourceControl = value;

                if (_sourceControl == null)
                    return;

                _sourceControl.MouseDown += new MouseEventHandler(this.SourceControl_MouseDown);
            }
        }

        public CodeeloFormDrag()
        {
            InitializeComponent();
        }

        public CodeeloFormDrag(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void SourceControl_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button != MouseButtons.Left ? 0 : (_sourceControl != null ? 1 : 0)) == 0)
                return;
            ReleaseCapture();
            SendMessage(_sourceControl.FindForm().Handle, 161, 2, 0);
        }
    }
}
