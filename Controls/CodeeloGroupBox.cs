using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeeloUI.Enums;
using CodeeloUI.Graphics;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(GroupBox))]
    [ToolboxItem(true)]
    [Description("Элемент для группировки объектов")]
    public sealed class CodeeloGroupBox : GroupBox
    {
        #region [ Поля класса ]
        private int _headerHeight;
        private Color _borderColor;
        private Color _constBorderColor;
        private Color _constForeColor;
        private Color _fillColor;
        private int _borderWidth;
        private int _borderRadius;
        private TextPosition _textPosition;
        #endregion

        #region [ Свойства класса ]
        [Description("Цвет текста"),Category("Внешний вид")]
        public override Color ForeColor 
        { 
            get => base.ForeColor; 
            set 
            {
                if (ForeColorOnSelect != value)
                    _constForeColor = value;
                base.ForeColor = value; 
            } 
        }
        [Description("Высота заголовка"),Category("Настройки внешнего вида")]
        public int HeaderHeight
        {
            get => _headerHeight;
            set
            {
                _headerHeight = value > 24 ? value : 24;
                Invalidate();
            }
        }
        [Description("Цвет границ элемента"), Category("Настройки внешнего вида")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                if (BorderColorOnSelect != value)
                    _constBorderColor = value;
                Invalidate();
            }
        }
        [Description("Цвет заливки области элемента"), Category("Настройки внешнего вида")]
        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                Invalidate();
            }
        }
        [Description("Цвет границ элемента при его выборе"), Category("Настройки внешнего вида")]
        public Color BorderColorOnSelect { get; set; }
        [Description("Цвет текса элемента при его выборе"),Category("Внешний вид")]
        public Color ForeColorOnSelect { get; set; }
        [Description("Толщина границ элемента"), Category("Настройки внешнего вида")]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                Invalidate();
            }
        }
        [Description("Радиус закругления границ элемента"), Category("Настройки внешнего вида")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }
        [Description("Положения текста в заголовке"), Category("Настройки внешнего вида")]
        public TextPosition TextAlign
        {
            get => _textPosition;
            set
            {
                _textPosition = value;
                Invalidate();
            }
        }
        #endregion
       
        public CodeeloGroupBox()
        {
            Font = new Font(FontFamily.GenericSerif, 14, FontStyle.Regular);

            _constForeColor = ForeColor = Color.Gray;
            _constBorderColor = _borderColor = Color.LightGray;
            _fillColor = Color.White;
            BorderColorOnSelect = Color.DodgerBlue;
            ForeColorOnSelect = Color.White;
            BackColor = Color.Transparent;

            _textPosition = TextPosition.Left;
            _headerHeight = 45;
            _borderWidth = 2;
            _borderRadius = 0;

            DoubleBuffered = true;
            Size = new Size(300, 180);
        }

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            InvokePaintBackground(this, e);
            var borderRadius = BorderRadius > 2 ? BorderRadius : 2;
            var borderWidth = BorderWidth > 0 ? BorderWidth : 1;

            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var borderPen = new Pen(BorderColor, borderWidth))
            using (var headerBrush = new SolidBrush(BorderColor))
            using (var borderPath = CustomGraphicsPath.GetFigurePath(Rectangle.Inflate(ClientRectangle, -1, -1), borderRadius)) 
            using (var figurePath = CustomGraphicsPath.GetFigurePath(Rectangle.Inflate(ClientRectangle, -borderWidth + borderRadius / 3, -borderWidth + borderRadius / 3), borderRadius)) 
            {
                if (borderRadius > 2)
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    graphics.DrawPath(borderPen, borderPath);
                    graphics.FillPath(new SolidBrush(FillColor), figurePath);
                    graphics.FillRectangle(headerBrush, 0.8f, (HeaderHeight / 2f)+3, Width - 3f, HeaderHeight / 2f);
                    graphics.FillPath(headerBrush, CustomGraphicsPath.GetFigurePath(new RectangleF(0f, 0f, Width - 1F, HeaderHeight), borderRadius));
                }
                else
                {                   
                    graphics.DrawRectangle(borderPen, 0f, 0f, Width - 1f, Height - 1f);
                    graphics.FillRectangle(new SolidBrush(FillColor), 0f + borderWidth / 2f, 0f, Width - 1f - borderWidth, Height - 1f - borderWidth / 2f);
                    graphics.FillRectangle(headerBrush, 0f, 0f, Width - 1f, _headerHeight);
                }
            }
            int leftPadding = Padding.Left < borderWidth / 2 ? borderWidth / 2 : Padding.Left;
            int rightPadding = Padding.Right < borderWidth / 2 ? borderWidth / 2 : Padding.Right;
            int bottomPadding = Padding.Bottom < borderWidth / 2 ? borderWidth / 2 : Padding.Bottom;
            int topPadding = Padding.Top < HeaderHeight / 2 ? HeaderHeight / 2 : Padding.Top;
            Padding = new Padding(leftPadding, topPadding, rightPadding, bottomPadding);

            var textSize = graphics.MeasureString(Text, Font);
            float positionX = 5F;
            float positionY = (_headerHeight - textSize.Height) / 2;

            switch (_textPosition)
            {
                case TextPosition.Right:
                    positionX = Width - textSize.Width;
                    break;
                case TextPosition.Center:
                    positionX = (Width - textSize.Width) / 2;
                    break;
                case TextPosition.Left:
                    positionX = 5F;
                    break;
            }
            graphics.DrawString(Text, Font, new SolidBrush(ForeColor), positionX, positionY);
            graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(64,ForeColor)), positionX+1, positionY+1);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            CheckMousePosition();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CheckMousePosition();
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            BorderColor = BorderColorOnSelect;
            ForeColor = ForeColorOnSelect;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            BorderColor = _constBorderColor;
            ForeColor = _constForeColor;
        }
        #endregion

        #region [ Методы ]
        private void CheckMousePosition()
        {
            if (ClientRectangle.Contains(PointToClient(MousePosition)))
            {
                BorderColor = BorderColorOnSelect;
                ForeColor = ForeColorOnSelect;
            }
            else
            {
                BorderColor = _constBorderColor;
                ForeColor = _constForeColor;
            }
        }
        #endregion
    }
}
