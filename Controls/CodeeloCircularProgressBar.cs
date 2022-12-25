using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CodeeloCircularProgressBar), "Icons.CodeeloCircularProgressBar.bmp")]
    [ToolboxItem(true)]
    [Description("Элемент для отображения прогресса")]
    public sealed class CodeeloCircularProgressBar : Control
    {
        #region [ Перечисления ]
        public enum TextMode
        {
            None,
            Value,
            Percentage,
            AnyText
        }
        #endregion

        #region [ Поля класса ]

        private int _value = 25;
        private int _maximum = 100;
        private int _lineWidth = 1;
        private float _barWidth = 14f;

        private Color _progressFirstColor = Color.Red;
        private Color _progressSecondColor = Color.Orange;
        private Color _lineColor = Color.DimGray;
        private LinearGradientMode _gradientDirection = LinearGradientMode.ForwardDiagonal;
        private LineCap _startShape = LineCap.Round;
        private LineCap _endShape = LineCap.Round;
        private TextMode _textMode;

        #endregion

        #region [ Свойства класса ]
        [Description("Значение прогресса"), Category("Настройки значений")]
        public int Value
        {
            get => _value;
            set
            {
                _value = value > _maximum ? _maximum : value;
                Invalidate();
            }
        }
        [Description("Максимальное значение прогресса"), Category("Настройки значений")]
        public int MaxValue
        {
            get => _maximum;
            set
            {
                _maximum = value < 1 ? 1 : value;
                Invalidate();
            }
        }
        [Description("Основной цвет для столбца прогресса"), Category("Настройки внешнего вида")]
        public Color BarFirstColor
        {
            get => _progressFirstColor;
            set
            {
                _progressFirstColor = value;
                Invalidate();
            }
        }
        [Description("Дополнительный цвет для градиентной заливки столбца прогресса"), Category("Настройки внешнего вида")]
        public Color BarSecondColor
        {
            get => _progressSecondColor;
            set
            {
                _progressSecondColor = value;
                Invalidate();
            }
        }
        [Description("Толщина столбца прогресса"), Category("Настройки внешнего вида")]
        public float BarWidth
        {
            get => _barWidth;
            set
            {
                _barWidth = value;
                Invalidate();
            }
        }
        [Description("Тип (направление) градиента"), Category("Настройки внешнего вида")]
        public LinearGradientMode GradientDirection
        {
            get => _gradientDirection;
            set
            {
                _gradientDirection = value;
                Invalidate();
            }
        }
        [Description("Цвет для направляющей линии"), Category("Настройки внешнего вида")]
        public Color LineColor
        {
            get => _lineColor;
            set
            {
                _lineColor = value;
                Invalidate();
            }
        }
        [Description("Ширина направляющей линии"), Category("Настройки внешнего вида")]
        public int LineWidth
        {
            get => _lineWidth;
            set
            {
                _lineWidth = value;
                Invalidate();
            }
        }
        [Description("Вид окончания направляющей линии"), Category("Настройки внешнего вида")]
        public LineCap BarShapeStart
        {
            get => _startShape;
            set
            {
                _startShape = value;
                Invalidate();
            }
        }
        [Description("Вид окончания направляющей линии"), Category("Настройки внешнего вида")]
        public LineCap BarShapeEnd
        {
            get => _endShape;
            set
            {
                _endShape = value;
                Invalidate();
            }
        }
        [Description("Формат отображаемого текста"), Category("Настройки внешнего вида")]
        public TextMode DisplayedTextType
        {
            get => _textMode;
            set
            {
                _textMode = value;
                Invalidate();
            }
        }
        [Description("Отображаемый текст"), Category("Настройки внешнего вида")]
        public override string Text { get; set; }
        #endregion

        public CodeeloCircularProgressBar()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.Opaque, true);

            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;

            Size = new Size(130, 130);
            Font = new Font(FontFamily.GenericSerif, 14, FontStyle.Regular);
            MinimumSize = new Size(100, 100);
            DoubleBuffered = true;

            DisplayedTextType = TextMode.Value;
        }

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var bitmap = new Bitmap(Width, Height))
            {
                using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    PaintTransparentBackground(this, e);

                    using (var backgroundBrush = new SolidBrush(BackColor))
                    {
                        graphics.FillEllipse(backgroundBrush, 18, 18, Width - 36, Height - 36);
                    }
                    using (var linePen = new Pen(LineColor, LineWidth))
                    {
                        graphics.DrawEllipse(linePen, 18, 18, Width - 36, Height - 36);
                    }

                    using (var lgb = new LinearGradientBrush(ClientRectangle, _progressFirstColor, _progressSecondColor, _gradientDirection))
                    using (var pen = new Pen(lgb, BarWidth))
                    {
                        pen.StartCap = BarShapeStart;
                        pen.EndCap = BarShapeEnd;

                        graphics.DrawArc(pen, 18, 18, Width - 37, Height - 37, -90
                            , (int)Math.Round((double)(360 * _value / _maximum)));
                    }


                    switch (DisplayedTextType)
                    {
                        case TextMode.None:
                            Text = string.Empty;
                            break;

                        case TextMode.Value:
                            Text = _value.ToString();
                            break;

                        case TextMode.Percentage:
                            Text = (100 * _value / _maximum).ToString() + '%';
                            break;
                    }

                    if (string.IsNullOrEmpty(Text))
                    { }

                    using (var fontColor = new SolidBrush(ForeColor))
                    {
                        var textSize = graphics.MeasureString(Text, Font);

                        graphics.DrawString(Text, Font, fontColor,
                            Convert.ToInt32(Width / 2 - textSize.Width / 2),
                            Convert.ToInt32(Height / 2 - textSize.Height / 2));
                    }
                    e.Graphics.DrawImage(bitmap, 0, 0);
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetStandardSize();
        }
        private static void PaintTransparentBackground(Control control, PaintEventArgs e)
        {
            if (control.Parent == null || !Application.RenderWithVisualStyles)
                return;

            ButtonRenderer.DrawParentBackground(e.Graphics, control.ClientRectangle, control);
        }
        #endregion

        #region [ Методы ]
        private void SetStandardSize() => Size = new Size(Height, Height);
        public void Increment(int value)
        {
            _value += value;
            Invalidate();
        }
        public void Decrement(int value)
        {
            _value -= value;
            Invalidate();
        }
        #endregion
    }
}
