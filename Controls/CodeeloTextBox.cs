using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeeloUI.Graphics;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(TextBox))]
    [ToolboxItem(true)]
    [Description("Текстовое поле")]
    public partial class CodeeloTextBox : UserControl
    {
        #region [ Поля класса ]
        private Color _borderColor = Color.DodgerBlue;
        private Color _borderFocusColor = Color.BlueViolet;
        private Color _placeholderColor = Color.DarkGray;

        private int _borderSize = 2;

        private bool _underlinedStyle;
        private bool _isFocused;
        private bool _usePlaceholder;
        private bool _usePasswordChar;

        private string _placeholderText = string.Empty;

        public new event EventHandler TextChanged;
        #endregion

        #region [ Свойства класса ]
        [Description("Цвет границ"), Category("Настройки внешнего вида")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }
        [Description("Цвет границ при наличии фокуса"), Category("Настройки внешнего вида")]
        public Color BorderFocusColor
        {
            get => _borderFocusColor;
            set => _borderFocusColor = value;
        }
        [Description("Ширина границ"), Category("Настройки внешнего вида")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value >= 1 ? value : 1;
                Invalidate();
            }
        }
        [Description("Использовать стиль, при котором у элемента используется только нижняя граница"), Category("Настройки внешнего вида")]
        public bool UnderlinedStyle
        {
            get => _underlinedStyle;
            set
            {
                _underlinedStyle = value;
                Invalidate();
            }
        }
        [Description("Заменять буквы на символы (для ввода паролей, к примеру)"), Category("Настройки текста")]
        public bool UsePasswordChar
        {
            get => _usePasswordChar;
            set
            {
                _usePasswordChar = value;
                if(!_usePasswordChar)
                    textBox.UseSystemPasswordChar = value;
            }
        }
        [Description("Разрешить перенос строк в элементе"), Category("Настройки текста")]
        public bool Multiline
        {
            get => textBox.Multiline;
            set => textBox.Multiline = value;
        }
        [Description("Цвет заливки"), Category("Настройки внешнего вида")]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = textBox.BackColor = value;
        }
        [Description("Цвет текста"), Category("Настройки текста")]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = textBox.ForeColor = value;
        }
        [Description("Используемый шрифт"), Category("Настройки текста")]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = textBox.Font = value;
                if (DesignMode)
                    UpdateControlHeight();
            }
        }
        [Description("Текст элемента"), Category("Настройки текста")]
        [Browsable(true)]
        public override string Text
        {
            get
            {
                if (_usePlaceholder) return string.Empty;
                return textBox.Text;
            }
            set
            {
                textBox.Text = value;
                SetPlaceholder();
            }
        }
        [Description("Цвет заполнителя текста"), Category("Настройки текста")]
        public Color PlaceholderColor
        {
            get => _placeholderColor;
            set
            {
                _placeholderColor = value;
                if (_usePlaceholder)
                    textBox.ForeColor = value;
            }
        }
        [Description("Текст заполнителя"), Category("Настройки текста")]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                textBox.Text = string.Empty;
                SetPlaceholder();
            }
        }
        #endregion
        public CodeeloTextBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        #region [ Методы ]
        private void SetPlaceholder()
        {
            if (string.IsNullOrEmpty(_placeholderText))
                return;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                _usePlaceholder = true;
                textBox.Text = _placeholderText;
                textBox.ForeColor = _placeholderColor;
                if (_usePasswordChar)
                    textBox.UseSystemPasswordChar = false;
            }
        }
        private void RemovePlaceholder()
        {
            if (string.IsNullOrEmpty(_placeholderText))
                return;

            if (_usePlaceholder)
            {
                _usePlaceholder = false;
                textBox.Text = string.Empty;
                textBox.ForeColor = ForeColor;
                if (_usePasswordChar)
                    textBox.UseSystemPasswordChar = true;
            }
        }
        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath textPath;
            if (Multiline)
            {
                textPath = CustomGraphicsPath.GetFigurePath(textBox.ClientRectangle, -1 * _borderSize);
                textBox.Region = new Region(textPath);
            }
            else
            {
                textPath = CustomGraphicsPath.GetFigurePath(textBox.ClientRectangle, _borderSize * 2);
                textBox.Region = new Region(textPath);
            }
            textPath.Dispose();
        }
        private void UpdateControlHeight()
        {
            if (textBox.Multiline)
                return;

            int textHeight = TextRenderer.MeasureText("Text", Font).Height + 1;
            textBox.Multiline = true;
            textBox.MinimumSize = new Size(0, textHeight);
            textBox.Multiline = false;
            Height = textBox.Height + Padding.Top + Padding.Bottom;
        }
        #endregion
        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        //    InvokePaintBackground(this, e);
            var graphics = e.Graphics;

            using (Pen penBorder = new Pen(_borderColor, _borderSize))
            {
                Region = new Region(ClientRectangle);
                penBorder.Alignment = PenAlignment.Inset;

                if (_isFocused)
                    penBorder.Color = _borderFocusColor;

                if (_underlinedStyle)
                    graphics.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                else
                    graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (DesignMode)
                UpdateControlHeight();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
                TextChanged.Invoke(sender, e);
        }

        private void textBox_Click(object sender, EventArgs e) => OnClick(e);

        private void textBox_MouseLeave(object sender, EventArgs e) => OnMouseLeave(e);

        private void textBox_MouseEnter(object sender, EventArgs e) => OnMouseEnter(e);
        private void textBox_KeyPress(object sender, KeyPressEventArgs e) => OnKeyPress(e);

        private void textBox_Enter(object sender, EventArgs e)
        {
            _isFocused = true;
            Invalidate();
            RemovePlaceholder();
        }
        private void textBox_Leave(object sender, EventArgs e)
        {
            _isFocused = false;
            Invalidate();
            SetPlaceholder();
        }
        #endregion
    }
}
