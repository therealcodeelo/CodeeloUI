using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeeloUI.Graphics;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CheckBox))]
    [ToolboxItem(true)]
    [Description("Элемент для переключения между двумя вариантами")]
    public sealed class CodeeloToggleButton : CheckBox
    {
        #region [ Поля класса ]
        private Color _areaColorChecked = Color.FromArgb(110, 220, 95);
        private Color _toggleColorChecked = Color.FromArgb(245, 245, 245);
        private Color _backColor = Color.FromArgb(221, 221, 221);
        private Color _toggleColor = Color.FromArgb(245, 245, 245);
        private bool _transparentBackColor = true;
        private bool _drawCircle = true;
        #endregion

        #region [ Свойства класса ]
        [Description("Цвет заливки фона после переключении"), Category("Настройки внешнего вида")]
        public Color AreaColorChecked
        {
            get => _areaColorChecked;
            set
            {
                _areaColorChecked = value;
                Invalidate();
            }
        }
        [Description("Цвет переключателя после переключении"), Category("Настройки внешнего вида")]
        public Color ToggleColorChecked
        {
            get => _toggleColorChecked;
            set
            {
                _toggleColorChecked = value;
                Invalidate();
            }
        }
        [Description("Цвет заливки фона"), Category("Настройки внешнего вида"), Browsable(true)]
        public Color AreaColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                Invalidate();
            }
        }
        [Description("Цвет переключателя"), Category("Настройки внешнего вида")]
        public Color ToggleColor
        {
            get => _toggleColor;
            set
            {
                _toggleColor = value;
                Invalidate();
            }
        }
        [Browsable(false)]
        public override string Text
        {
            get => base.Text;
            set { }
        }
        [Description("Использовать прозрачный фон"), Category("Настройки внешнего вида")]
        public bool UseTransparentBackColor
        {
            get => _transparentBackColor;
            set
            {
                _transparentBackColor = value;
                Invalidate();
            }
        }
        [Description("Рисовать овальную форму"), Category("Настройки внешнего вида")]
        public bool DrawCircle
        {
            get => _drawCircle;
            set
            {
                _drawCircle = value;
                Invalidate();
            }
        }
        #endregion
        public CodeeloToggleButton()
        {
            MinimumSize = new Size(45, 22);
            Size = new Size(60, 22);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
        }
        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            InvokePaintBackground(this, e);
            var graphics = e.Graphics;
            int toggleSize = Height - 5;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            var toggleRect = Checked ? new Rectangle(Width - Height + 1, 2, toggleSize, toggleSize) 
                : new Rectangle(2, 2, toggleSize, toggleSize);

            using (var pen = Checked ? new Pen(_areaColorChecked, 2): new Pen(_backColor, 2))
            using (var brush = Checked ? new SolidBrush(_areaColorChecked) : new SolidBrush(_backColor))
            using (var toggleBrush = Checked ? new SolidBrush(_toggleColorChecked) : new SolidBrush(_toggleColor))
            {
                if (_drawCircle)
                {
                    if (_transparentBackColor)
                        graphics.DrawPath(pen, CustomGraphicsPath.GetFigurePath(this));
                    else
                        graphics.FillPath(brush, CustomGraphicsPath.GetFigurePath(this));

                    graphics.FillEllipse(toggleBrush, toggleRect);
                }
                else
                {
                    if (_transparentBackColor)
                        graphics.DrawRectangle(pen, rect);
                    else
                        graphics.FillRectangle(brush, rect);

                    graphics.FillRectangle(toggleBrush, toggleRect);
                }
            }
        }
        #endregion
    }
}
