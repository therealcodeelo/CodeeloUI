using CodeeloUI.SupportClasses;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CodeeloGradientPanel), "Icons.CodeeloGradientPanel.bmp")]
    [ToolboxItem(true)]
    [Description("Панель с возможностью градиентной заливки фона")]
    public class CodeeloGradientPanel : Panel
    {
        #region [ Поля класса ]
        private Color _firstFillColor = Color.FromArgb(3, 233, 172);
        private Color _secondFillColor = Color.FromArgb(21, 152, 255);
        private Color _firstBorderColor = Color.FromArgb(3, 233, 172);
        private Color _secondBorderColor = Color.FromArgb(21, 152, 255);
        private LinearGradientMode _gradientDirection = LinearGradientMode.ForwardDiagonal;
        private LinearGradientMode _gradientBorderDirection = LinearGradientMode.ForwardDiagonal;
        private int _borderRadius = 0;
        private int _borderThickness = 0;
        #endregion

        #region [ Свойства класса ]
        [Description("Второй цвет для градиентного заполнения границы"), Category("Свойства градиента границы")]
        public Color ColorBorderSecond
        {
            get => _secondBorderColor;
            set
            {
                _secondBorderColor = value;
                Invalidate();
            }
        }
        [Description("Первый цвет для градиентного заполнения границы"), Category("Свойства градиента границы")]
        public Color ColorBorderFirst
        {
            get => _firstBorderColor;
            set
            {
                _firstBorderColor = value;
                Invalidate();
            }
        }
        [Description("Толщина границы"), Category("Свойства границы")]
        public int BorderThickness
        {
            get => _borderThickness;
            set
            {
                _borderThickness = value;
                Invalidate();
            }
        }
        [Description("Радиус закругления"), Category("Свойства границы")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }
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
        [Description("Тип (направление) градиента"), Category("Свойства градиента границы")]
        public LinearGradientMode GradientBorderDirection
        {
            get => _gradientBorderDirection;
            set
            {
                _gradientBorderDirection = value;
                Invalidate();
            }
        }
        [Description("Рисовать градиент"), Category("Свойства градиента")]
        public bool DrawGradient { get; set; } = true;
        [Description("Рисовать градиент на границе"), Category("Свойства градиента границы")]
        public bool DrawBorderGradient { get; set; } = true;
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
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var lgb = new LinearGradientBrush(ClientRectangle, ColorFillFirst, ColorFillSecond, GradientDirection))
            {
                var graphics = e.Graphics;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int borderSize = BorderThickness > 1 ? BorderThickness : 2;

                using (var gradientBrush = new LinearGradientBrush(ClientRectangle, ColorFillFirst, ColorFillSecond, _gradientDirection))
                using (var borderGradientBrush = new LinearGradientBrush(ClientRectangle, ColorBorderFirst, ColorBorderSecond, GradientDirection))
                using (var penBorder = DrawBorderGradient ? new Pen(borderGradientBrush, borderSize) : new Pen(new SolidBrush(ColorBorderFirst), borderSize))
                {
                    if (BorderRadius > 2)
                    {
                        using (var borderPath = GraphicsUtils.GetFigurePath(Rectangle.Inflate(ClientRectangle, -2, -2), BorderRadius))
                        using (var path = GraphicsUtils.GetFigurePath(Rectangle.Inflate(ClientRectangle, -1, -1), BorderRadius))
                        {
                            if (DrawGradient)
                                graphics.FillPath(gradientBrush, path);
                            else
                                graphics.FillPath(new SolidBrush(ColorFillFirst), path);

                            if (borderSize >= 1)
                            {
                                penBorder.Alignment = PenAlignment.Center;
                                graphics.DrawPath(penBorder, borderPath);
                            }
                        }
                    }
                    else
                    {
                        graphics.SmoothingMode = SmoothingMode.HighQuality;

                        if (DrawGradient)
                            graphics.FillRectangle(gradientBrush, ClientRectangle);
                        else
                            graphics.FillRectangle(new SolidBrush(ColorFillFirst), ClientRectangle);

                        if (borderSize > 0)
                        {
                            penBorder.Alignment = PenAlignment.Center;
                            graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                        }
                    }
                }
            }
            base.OnPaint(e);
        }
        #endregion
    }
}
