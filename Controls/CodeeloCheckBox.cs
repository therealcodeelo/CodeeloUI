using CodeeloUI.Enums;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CheckBox))]
    [ToolboxItem(true)]
    [Description("Элемент для выбора определенных вариантов")]
    public sealed class CodeeloCheckBox : CheckBox
    {
        #region [ Поля класса ]
        private Color _buttonColorChecked = Color.FromArgb(110, 220, 95);
        private Color _buttonColor = Color.FromArgb(221, 221, 221);
        private Color _markColor = Color.FromArgb(221, 221, 221);
        private float _buttonSize = 18F;
        private bool _drawCircle = true;
        private float _buttonBorderSize = 1.6F;
        private int _markWidth = 3;
        private CheckBoxMark _mark = CheckBoxMark.Mark;
        #endregion

        #region [ Свойства класса ]
        [Description("Цвет выбранной кнопки"), Category("Настройки внешнего вида")]
        public Color ButtonColorChecked
        {
            get => _buttonColorChecked;
            set
            {
                _buttonColorChecked = value;
                Invalidate();
            }
        }
        [Description("Цвет кнопки"), Category("Настройки внешнего вида")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set
            {
                _buttonColor = value;
                Invalidate();
            }
        }
        [Description("Цвет символа"), Category("Настройки внешнего вида")]
        public Color MarkColor
        {
            get => _markColor;
            set
            {
                _markColor = value;
                Invalidate();
            }
        }
        [Description("Тип символа"), Category("Настройки внешнего вида")]
        public CheckBoxMark MarkType
        {
            get => _mark;
            set
            {
                _mark = value;
                Invalidate();
            }
        }
        [Description("Ширина линий символа (радиус, если это круг)"), Category("Настройки внешнего вида")]
        public int MarkWidth
        {
            get => _markWidth;
            set
            {
                _markWidth = value;
                Invalidate();
            }
        }
        [Description("Размер кнопки"), Category("Настройки внешнего вида")]
        public float ButtonCircleSize
        {
            get => _buttonSize;
            set
            {
                _buttonSize = value;
                Invalidate();
            }
        }
        [Description("Рисовать круглую кнопку"), Category("Настройки внешнего вида")]
        public bool DrawCircleButton
        {
            get => _drawCircle;
            set
            {
                _drawCircle = value;
                Invalidate();
            }
        }
        [Description("Ширина границ кнопки"), Category("Настройки внешнего вида")]
        public float ButtonBorderSize
        {
            get => _buttonBorderSize;
            set
            {
                _buttonBorderSize = value;
                Invalidate();
            }
        }
        #endregion

        public CodeeloCheckBox()
        {
            Font = new Font(FontFamily.GenericSerif, 12);
        }

        #region [ События ]

        protected override void OnPaint(PaintEventArgs e)
        {
            InvokePaintBackground(this, e);
            System.Drawing.Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rectButton = new RectangleF(3, (Height - _buttonSize) / 2, _buttonSize, _buttonSize);

            using (var penBorder = new Pen(_buttonColorChecked, _buttonBorderSize))
            using (var penMark = new Pen(_markColor, _markWidth))
            using (var brushRbCheck = new SolidBrush(_buttonColorChecked))
            using (var brushText = new SolidBrush(ForeColor))
            {
                if (Checked)
                {
                    if (_drawCircle)
                        graphics.DrawEllipse(penBorder, rectButton);
                    else
                        graphics.DrawRectangles(penBorder, new[] { rectButton });

                    switch (_mark)
                    {
                        case CheckBoxMark.Mark:
                            {
                                graphics.DrawLine(penMark,
                                    rectButton.X + rectButton.Width / 4, rectButton.Y + rectButton.Height / 2,
                                    rectButton.X + rectButton.Width / 2, rectButton.Y + rectButton.Height - 2);

                                graphics.DrawLine(penMark,
                                    rectButton.X - 2 + rectButton.Width / 2, rectButton.Y + rectButton.Height - 2,
                                    rectButton.X + rectButton.Width, rectButton.Y);
                            }
                            break;
                        case CheckBoxMark.Plus:
                            {
                                graphics.DrawLine(penMark,
                                    rectButton.X + rectButton.Width / 2, rectButton.Y + rectButton.Height / 4,
                                    rectButton.X + rectButton.Width / 2, rectButton.Y + rectButton.Height - rectButton.Height / 4);

                                graphics.DrawLine(penMark,
                                    rectButton.X + rectButton.Width / 4, rectButton.Y + rectButton.Height / 2,
                                    rectButton.X + rectButton.Width - rectButton.Width / 4, rectButton.Y + rectButton.Height / 2);
                            }
                            break;
                        case CheckBoxMark.Minus:
                            {
                                graphics.DrawLine(penMark,
                                    rectButton.X + rectButton.Width / 4, rectButton.Y + rectButton.Height / 2,
                                    rectButton.X + rectButton.Width - rectButton.Width / 4, rectButton.Y + rectButton.Height / 2);
                            }
                            break;
                        case CheckBoxMark.CheckBoxFigure:
                            {
                                var rectButtonChecked = new RectangleF(rectButton.X + ((rectButton.Width - (_markWidth*2)) / 2)
                , (Height - (_markWidth * 2)) / 2
                , (_markWidth * 2), (_markWidth * 2));

                                if (_drawCircle)
                                    graphics.FillEllipse(brushRbCheck, rectButtonChecked);
                                else
                                    graphics.FillRectangle(brushRbCheck, rectButtonChecked);
                            }
                            break;
                        case CheckBoxMark.X:
                            {
                                graphics.DrawLine(penMark,
                                    rectButton.X + rectButton.Width / 4, rectButton.Y + rectButton.Height / 4,
                                    rectButton.X + rectButton.Width - rectButton.Width/4, rectButton.Y + rectButton.Height - rectButton.Height / 4);

                                graphics.DrawLine(penMark,
                                    rectButton.X + rectButton.Width / 4, rectButton.Y + rectButton.Height - rectButton.Height / 4,
                                    rectButton.X + rectButton.Width - rectButton.Width / 4, rectButton.Y + rectButton.Height / 4);
                            }
                            break;
                    }
                }
                else
                {
                    penBorder.Color = _buttonColor;

                    if (_drawCircle)
                        graphics.DrawEllipse(penBorder, rectButton);
                    else
                        graphics.DrawRectangles(penBorder, new[] { rectButton });
                }
                graphics.DrawString(Text, Font, brushText,
                    _buttonSize + 8, (Height - TextRenderer.MeasureText(Text, Font).Height) / 2);
            }
        }
        #endregion
    }
}
