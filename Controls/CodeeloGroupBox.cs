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
        private Color _currentBorderColor;
        private Color _currentForeColor;
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
                    _currentForeColor = value;
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
                    _currentBorderColor = value;
                Invalidate();
            }
        }
        [Browsable(false)]
        public override Color BackColor
        {
            get => Color.Transparent;
            set
            {
                base.BackColor = value;
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

            _currentForeColor = ForeColor = Color.DimGray;
            _currentBorderColor = _borderColor = Color.PowderBlue;
            _fillColor = Color.MintCream;
            BorderColorOnSelect = Color.DodgerBlue;
            ForeColorOnSelect = Color.White;
            BackColor = Color.Transparent;

            _textPosition = TextPosition.Left;
            _headerHeight = 45;
            _borderWidth = 2;
            _borderRadius = 0;

            DoubleBuffered = true;
            Size = new Size(300, 200);
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
            using (var borderPath = GraphicsUtils.GetFigurePath(Rectangle.Inflate(ClientRectangle, -2, -2), borderRadius)) 
            {
                if (borderRadius > 2)
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    graphics.FillPath(new SolidBrush(FillColor), 
                        GraphicsUtils.GetFigurePath(new RectangleF(borderWidth, HeaderHeight / 2, 
                        Width - 1F - borderWidth * 2, Height - (HeaderHeight + 2f) / 2f), borderRadius));
                    graphics.DrawPath(borderPen, borderPath);
                   
                    graphics.FillRectangle(headerBrush, 0.7f, (HeaderHeight / 2f) + 3, Width - 1.7f, HeaderHeight / 2f);
                    graphics.FillPath(headerBrush, GraphicsUtils.GetFigurePath(new RectangleF(0f, 0f, Width - 1F, HeaderHeight), borderRadius));
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
            int topPadding = Padding.Top < (HeaderHeight + 4) / 2 ? (HeaderHeight + 2) / 2 : Padding.Top;
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
            BorderColor = _currentBorderColor;
            ForeColor = _currentForeColor;
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
                BorderColor = _currentBorderColor;
                ForeColor = _currentForeColor;
            }
        }
        #endregion
    }
}
