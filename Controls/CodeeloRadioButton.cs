using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(RadioButton))]
    [ToolboxItem(true)]
    [Description("Элемент для выбора одного варианта в определенном контейнере")]
    public sealed class CodeeloRadioButton : RadioButton
    {
        #region [ Поля класса ]
        private Color _buttonColorChecked = Color.FromArgb(110, 220, 95);
        private Color _buttonColor = Color.FromArgb(221, 221, 221);
        private float _buttonSize = 18F;
        private float _buttonToggleSize = 12F;
        private bool _drawCircle = true;
        private bool _drawCircleToggle = true;
        private float _buttonBorderSize = 1.6F; 
        #endregion

        #region [ Свойства класса ]
        [Description("Цвет кнопки после переключения"), Category("Настройки внешнего вида")]
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
        [Description("Размер переключателя внутри кнопки"), Category("Настройки внешнего вида")]
        public float ButtonToggleSize
        {
            get => _buttonToggleSize;
            set
            {
                _buttonToggleSize = value < _buttonSize ? value : _buttonSize;

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
        [Description("Рисовать круглый переключатель внутри кнопки"), Category("Настройки внешнего вида")]
        public bool DrawCircleToggle
        {
            get => _drawCircleToggle;
            set
            {
                _drawCircleToggle = value;
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

        public CodeeloRadioButton()
        {
            MinimumSize = new Size(0, 21);
            Padding = new Padding(10, 0, 0, 0);
            BackColor = Color.Transparent;
        }

        #region [ События ]

        protected override void OnPaint(PaintEventArgs e)
        {
            InvokePaintBackground(this, e);
            System.Drawing.Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rectButton = new RectangleF(3, (Height - _buttonSize) / 2, _buttonSize,_buttonSize);
            var rectButtonChecked = new RectangleF(rectButton.X + ((rectButton.Width - _buttonToggleSize) / 2)
                , (Height - _buttonToggleSize) / 2
                , _buttonToggleSize,_buttonToggleSize);

            using (var penBorder = new Pen(_buttonColorChecked, _buttonBorderSize))
            using (var brushRbCheck = new SolidBrush(_buttonColorChecked))
            using (var brushText = new SolidBrush(ForeColor))
            {
                if (Checked)
                {
                    if(_drawCircle)
                        graphics.DrawEllipse(penBorder, rectButton);
                    else
                        graphics.DrawRectangles(penBorder, new[] { rectButton });

                    if (_drawCircleToggle)
                        graphics.FillEllipse(brushRbCheck, rectButtonChecked);
                    else
                        graphics.FillRectangle(brushRbCheck, rectButtonChecked);
                }
                else
                {
                    penBorder.Color = _buttonColor;

                    if(_drawCircle)
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
