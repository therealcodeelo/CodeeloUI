using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(Panel))]
    [ToolboxItem(true)]
    [Description("Панель с возможностью градиентной заливки фона")]
    public class CodeeloGradientPanel : Panel
    {
        #region [ Поля класса ]
        private Color _firstFillColor = Color.FromArgb(3, 233, 172);
        private Color _secondFillColor = Color.FromArgb(21, 152, 255);
        private LinearGradientMode _gradientDirection = LinearGradientMode.ForwardDiagonal;
        #endregion

        #region [ Свойства класса ]
        
        [Description("Первый цвет для градиентного заполнения"), Category("Свойства градиента")]
        public Color ColorFillFirst
        {
            get => _firstFillColor;
            set
            {
                _firstFillColor = value;
                Invalidate();
            }
        }
        [Description("Второй цвет для градиентного заполнения"), Category("Свойства градиента")]
        public Color ColorFillSecond
        {
            get => _secondFillColor;
            set 
            { 
                _secondFillColor = value;
                Invalidate();
            }
        }

        [Description("Тип (направление) градиента"), Category("Свойства градиента")]
        public LinearGradientMode GradientDirection
        {
            get => _gradientDirection;
            set
            {
                _gradientDirection = value;
                Invalidate();
            }
        }
        #endregion

        #region [ Скрытые свойства класса ]
        [Browsable(false)]
        public new Cursor Cursor { get; set; }
        [Browsable(false)]
        public new RightToLeft RightToLeft { get; set; }

        [Browsable(false)]
        public new bool UseWaitCursor { get; set; }

        [Browsable(false)]
        public new string AccessibleDescription { get; set; }
        [Browsable(false)]
        public new string AccessibleName { get; set; }
        [Browsable(false)]
        public new string AccessibleRole { get; set; }
        [Browsable(false)]
        public new bool CausesValidation { get; set; }

        [Browsable(false)]
        public new ContextMenuStrip ContextMenuStrip { get; set; }
        [Browsable(false)]
        public new bool TabStop { get; set; }
        [Browsable(false)]
        public new bool AllowDrop { get; set; }

        [Browsable(false)]
        public new bool AutoSize { get; set; }
        [Browsable(false)]
        public new AutoSizeMode AutoSizeMode { get; set; }
        #endregion

        public CodeeloGradientPanel()
        {
            DoubleBuffered = true;
        }

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            var lgb = new LinearGradientBrush(ClientRectangle, _firstFillColor, _secondFillColor, _gradientDirection);
            var graphics = e.Graphics;
            graphics.FillRectangle(lgb, ClientRectangle);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            SetStyle(ControlStyles.ResizeRedraw, true);
            base.OnPaint(e);
        }
        #endregion
    }
}
