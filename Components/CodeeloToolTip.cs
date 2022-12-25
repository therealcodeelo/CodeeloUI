using CodeeloUI.SupportClasses;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CodeeloUI.SupportClasses.ToolTip;
using System.ComponentModel;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloToolTip), "Icons.CodeeloToolTip.bmp")]
    public partial class CodeeloToolTip : Component
    {
        #region [ Свойства класса ]
        private readonly Font DefaultFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);
        private readonly StringFormat DefStringFormat = StringFormat.GenericTypographic;

        private CodeeloTipStyle DefaultStyle { get; set; } = CodeeloTipStyle.Gray;
        private CodeeloTipStyle OkStyle { get; set; } = CodeeloTipStyle.Green;
        private CodeeloTipStyle WarningStyle { get; set; } = CodeeloTipStyle.Orange;
        private CodeeloTipStyle ErrorStyle { get; set; } = CodeeloTipStyle.Red;
        public Bitmap Bitmap24 { get; set; }
        public int Fade { get; set; } = 100;
        public bool Floating { get; set; } = true;
        public int Delay { get; set; } = 600;
        #endregion

        #region [ Win32 API ]
        private static Point GetCaretPosition()
        {
            GetCaretPos(out Point pt);
            return pt;
        }

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool GetCaretPos(out Point pt);

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        #endregion
        public CodeeloToolTip()
        {
            InitializeComponent();
        }

        public CodeeloToolTip(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region [ Приватные методы ]
        private void FadeEffect(CodeeloLayeredWindow window, bool fadeIn)
        {
            byte target = fadeIn ? byte.MaxValue : byte.MinValue;
            const int Updateinterval = 10;
            int step = Fade < Updateinterval ? 0 : (Fade / Updateinterval);

            for (int i = 1; i <= step; i++)
            {
                Thread.Sleep(Updateinterval);

                if (i == step) { break; }

                var tmp = (double)(fadeIn ? i : (step - i));
                window.Alpha = (byte)(tmp / step * 255);
            }

            window.Alpha = target;
        }

        private Point DetemineActive()
        {
            var point = Control.MousePosition;

            var focusControl = Control.FromHandle(GetFocus());
            if (focusControl is TextBoxBase)
            {
                var pt = GetCaretPosition();
                pt.Y += focusControl.Font.Height / 2;
                point = focusControl.PointToScreen(pt);
            }
            else if (focusControl is ButtonBase)
            {
                point = GetCenterPosition(focusControl);
            }
            return point;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private Bitmap CreateTipImage(string text, CodeeloTipStyle style, out Rectangle contentBounds)
        {
            var size = Size.Empty;
            var iconBounds = Rectangle.Empty;
            var textBounds = Rectangle.Empty;

            if (style.Icon != null)
            {
                size = style.Icon.Size;
                iconBounds.Size = size;
                textBounds.X = size.Width;
            }

            if (text.Length != 0)
            {
                if (style.Icon != null)
                {
                    size.Width += style.IconSpacing;
                    textBounds.X += style.IconSpacing;
                }

                textBounds.Size = Size.Truncate(GraphicsUtils.MeasureString(text, style.TextFont ?? DefaultFont, 0, DefStringFormat));
                size.Width += textBounds.Width;

                if (size.Height < textBounds.Height)
                {
                    size.Height = textBounds.Height;
                }
                else if (size.Height > textBounds.Height)
                {
                    textBounds.Y += (size.Height - textBounds.Height) / 2;
                }
                textBounds.Offset(style.TextOffset);
            }
            size += style.Padding.Size;
            iconBounds.Offset(style.Padding.Left, style.Padding.Top);
            textBounds.Offset(style.Padding.Left, style.Padding.Top);

            contentBounds = new Rectangle(Point.Empty, size);
            var fullBounds = GraphicsUtils.GetBounds(contentBounds, style.Border, style.ShadowRadius, style.ShadowOffset.X, style.ShadowOffset.Y);
            contentBounds.Offset(-fullBounds.X, -fullBounds.Y);
            iconBounds.Offset(-fullBounds.X, -fullBounds.Y);
            textBounds.Offset(-fullBounds.X, -fullBounds.Y);

            var bmp = new Bitmap(fullBounds.Width, fullBounds.Height);

            Graphics g = null;
            Brush backBrush = null;
            Brush textBrush = null;
            try
            {
                g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                backBrush = (style.BackBrush ?? (r => new SolidBrush(style.BackColor)))(contentBounds);
                GraphicsUtils.DrawRectangle(g, contentBounds,
                    backBrush,
                    style.Border,
                    style.CornerRadius,
                    style.ShadowColor,
                    style.ShadowRadius,
                    style.ShadowOffset.X,
                    style.ShadowOffset.Y);

                if (style.Icon != null)
                {
                    g.DrawImageUnscaled(style.Icon, iconBounds.Location);
                }
                if (text.Length != 0)
                {
                    textBrush = new SolidBrush(style.TextColor);
                    g.DrawString(text, style.TextFont ?? DefaultFont, textBrush, textBounds.Location, DefStringFormat);
                }

                g.Flush(FlushIntention.Sync);
                return bmp;
            }
            finally
            {
                g?.Dispose();
                backBrush?.Dispose();
                textBrush?.Dispose();
            }
        }
        private Point GetLocation(Rectangle contentBounds, Point basePoint, bool centerByBasePoint, out bool floatDown)
        {
            var screen = Screen.FromPoint(basePoint).Bounds;

            var p = basePoint;
            p.X -= contentBounds.Width / 2;

            int spacing = 10;
            int left, right;
            if (p.X < (left = screen.Left + spacing))
            {
                p.X = left;
            }
            else if (p.X > (right = screen.Width + screen.Left - spacing - contentBounds.Width))
            {
                p.X = right;
            }

            if (centerByBasePoint)
            {
                p.Y -= contentBounds.Height / 2;
            }
            else
            {
                spacing = 20;
                p.Y -= contentBounds.Height + spacing;
            }

            floatDown = false;
            if (p.Y < screen.Top + 50)
            {
                if (!centerByBasePoint)
                {
                    p.Y += contentBounds.Height + 2 * spacing;
                }

                floatDown = true;
            }

            p.Offset(-contentBounds.X, -contentBounds.Y);
            return p;
        }

        private Point GetCenterPosition(Component controlOrItem)
        {
            if (controlOrItem is Control c)
            {
                var size = c.ClientSize;
                return c.PointToScreen(new Point(size.Width / 2, size.Height / 2));
            }

            if (controlOrItem is ToolStripItem item)
            {
                var pos = item.Bounds.Location;
                pos.X += item.Width / 2;
                pos.Y += item.Height / 2;
                return item.Owner.PointToScreen(pos);
            }

            throw new ArgumentException("Параметр может быть только Control или ToolStripItem!");
        }

        private bool IsContainerLike(Component controlOrItem)
        {
            if (controlOrItem is ContainerControl
                || controlOrItem is GroupBox
                || controlOrItem is Panel
                || controlOrItem is TabControl
                || controlOrItem is DataGridView
                || controlOrItem is ListBox
                || controlOrItem is ListView)
            {
                return true;
            }

            if (controlOrItem is TextBox txb && txb.Multiline)
            {
                return true;
            }

            if (controlOrItem is RichTextBox rtb && rtb.Multiline)
            {
                return true;
            }

            return false;
        }
        private void layer_Showing(object sender, EventArgs e)
        {
            var layer = (CodeeloLayeredWindow)sender;
            var args = layer.Tag as object[];
            var delay = (int)args[0];
            var floating = (bool)args[1];
            var floatDown = (bool)args[2];

            if (floating)
            {
                new Thread(arg =>
                {
                    int adj = floatDown ? 1 : -1;

                    while (layer.Visible)
                    {
                        layer.Top += adj;
                        Thread.Sleep(30);
                    }
                })
                { IsBackground = true, Name = "T_Floating" }.Start();
            }

            FadeEffect(layer, true);
            Thread.Sleep(delay < 0 ? 0 : delay);
            layer.Close();
        }

        private void layer_Closing(object sender, CancelEventArgs e) =>
            FadeEffect((CodeeloLayeredWindow)sender, false);
        #endregion

        #region [ Публичные методы ]
        public void ShowOk(Component controlOrItem, string text = null, int delay = -1, bool? floating = null, bool? centerInControl = null) =>
            Show(controlOrItem, text, OkStyle ?? CodeeloTipStyle.Green, delay, floating, centerInControl);
        public void ShowOk(string text = null, int delay = -1, bool? floating = null, Point? point = null, bool centerByPoint = false) =>
            Show(text, OkStyle ?? CodeeloTipStyle.Green, delay, floating, point, centerByPoint);
        public void ShowWarning(Component controlOrItem, string text = null, int delay = 1000, bool? floating = false, bool? centerInControl = null) =>
            Show(controlOrItem, text, WarningStyle ?? CodeeloTipStyle.Orange, delay, floating, centerInControl);
        public void ShowWarning(string text = null, int delay = 1000, bool? floating = false, Point? point = null, bool centerByPoint = false) =>
            Show(text, WarningStyle ?? CodeeloTipStyle.Orange, delay, floating, point, centerByPoint);
        public void ShowError(Component controlOrItem, string text = null, int delay = 1000, bool? floating = false, bool? centerInControl = null) =>
            Show(controlOrItem, text, ErrorStyle ?? CodeeloTipStyle.Red, delay, floating, centerInControl);
        public void ShowError(string text = null, int delay = 1000, bool? floating = false, Point? point = null, bool centerByPoint = false) =>
            Show(text, ErrorStyle ?? CodeeloTipStyle.Red, delay, floating, point, centerByPoint);
        public void Show(Component controlOrItem, string text, CodeeloTipStyle style = null, int delay = -1, bool? floating = null, bool? centerInControl = null)
        {
            if (controlOrItem == null)
            {
                throw new ArgumentNullException(nameof(controlOrItem));
            }
            Show(text, style, delay, floating, GetCenterPosition(controlOrItem), centerInControl ?? IsContainerLike(controlOrItem));
        }
        public void Show(string text, CodeeloTipStyle style = null, int delay = -1, bool? floating = null, Point? point = null, bool centerByPoint = false)
        {
            var basePoint = point ?? DetemineActive();

            new Thread(arg =>
            {
                CodeeloLayeredWindow layer = null;
                try
                {
                    layer = new CodeeloLayeredWindow(CreateTipImage(text ?? string.Empty, style ?? DefaultStyle ?? CodeeloTipStyle.Gray, out Rectangle contentBounds))
                    {
                        Alpha = 0,
                        Location = GetLocation(contentBounds, basePoint, centerByPoint, out var floatDown),
                        MouseThrough = true,
                        TopMost = true,
                        Tag = new object[] { delay < 0 ? Delay : delay, floating ?? Floating, floatDown }
                    };
                    layer.Showing += layer_Showing;
                    layer.Closing += layer_Closing;
                    layer.Show();
                }
                finally
                {
                    if (layer != null)
                    {
                        layer.Showing -= layer_Showing;
                        layer.Closing -= layer_Closing;
                        layer.Dispose();
                    }
                }
            })
            { IsBackground = true, Name = "T_Showing" }.Start();
        }
        #endregion
    }
}
