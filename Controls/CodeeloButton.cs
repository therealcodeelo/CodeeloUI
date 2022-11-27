using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeeloUI.Enums;
using CodeeloUI.Graphics;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(Button))]
    [ToolboxItem(true)]
    [Description("Настраиваемая кнопка")]
    public sealed class CodeeloButton : Button
    {
        #region [ Поля класса ]
        private int _borderSize = 3;
        private int _borderRadius = 20;

        private bool _useGradient;
        private Color _firstFillColor = Color.FromArgb(20, 30, 40);
        private Color _secondFillColor = Color.DodgerBlue;
        private LinearGradientMode _gradientDirection = LinearGradientMode.ForwardDiagonal;

        private bool _useGradientBorder = true;
        private Color _firstBorderColor = Color.DodgerBlue;
        private Color _secondBorderColor = Color.SpringGreen;
        private LinearGradientMode _gradientBorderDirection = LinearGradientMode.ForwardDiagonal;

        private TextPosition _textPosition = TextPosition.Center;

        private bool _isEntered;
        private bool _isClicked;
        private Color _onOverFirstFillColor = Color.DimGray;
        private Color _onOverSecondFillColor = Color.DimGray;
        private Color _onOverFirstBorderColor = Color.DimGray;
        private Color _onOverSecondBorderColor = Color.DimGray;
        private Color _onClickFirstFillColor = Color.DarkGray;
        private Color _onClickSecondFillColor = Color.DarkGray;
        private Color _onClickFirstBorderColor = Color.DarkGray;
        private Color _onClickSecondBorderColor = Color.DarkGray;
        #endregion

        #region [ Свойства класса ]
        [Description("Толщина границ"), Category("Свойства границ элемента")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }
        [Description("Радиус закругления"), Category("Свойства границ элемента")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }
        [Description("Использовать градиент?"), Category("Свойства градиента"), DefaultValue(false)]
        public bool UseGradient
        {
            get => _useGradient;
            set
            {
                _useGradient = value;
                BackColor = _useGradient ? Color.Transparent : Parent.BackColor;
                Invalidate();
            }
        }
        [Description("Основной цвет для заполнения фона кнопки"), Category("Свойства градиента")]
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
        [Description("Использовать градиент для границ?"), Category("Свойства границ элемента"), DefaultValue(false)]
        public bool UseGradientBorder
        {
            get => _useGradientBorder;
            set
            {
                _useGradientBorder = value;
                Invalidate();
            }
        }
        [Description("Основной цвет для отрисовки границ"), Category("Свойства границ элемента")]
        public Color GradientBorderColorFirst
        {
            get => _firstBorderColor;
            set
            {
                _firstBorderColor = value;
                Invalidate();
            }
        }
        [Description("Второй цвет для градиентной отрисовки границ"), Category("Свойства границ элемента")]
        public Color GradientBorderColorSecond
        {
            get => _secondBorderColor;
            set
            {
                _secondBorderColor = value;
                Invalidate();
            }
        }
        [Description("Тип (направление) градиента границ"), Category("Свойства границ элемента")]
        public LinearGradientMode GradientBorderDirection
        {
            get => _gradientBorderDirection;
            set
            {
                _gradientBorderDirection = value;
                Invalidate();
            }
        }
        [Description("Положения текста в заголовке"), Category("Внешний вид")]
        public new TextPosition TextAlign
        {
            get => _textPosition;
            set
            {
                _textPosition = value;
                Invalidate();
            }
        }
        [Description("Основной цвет кнопки при наведении на нее указателя мыши"), Category("Свойства поведения")]
        public Color OnOverFirstFillColor
        {
            get => _onOverFirstFillColor;
            set => _onOverFirstFillColor = value;
        }
        [Description("Дополнительный цвет градиента кнопки при наведении на нее указателя мыши"), Category("Свойства поведения")]
        public Color OnOverSecondFillColor
        {
            get => _onOverSecondFillColor;
            set => _onOverSecondFillColor = value;
        }
        [Description("Основной цвет границ кнопки при наведении на нее указателя мыши"), Category("Свойства поведения")]
        public Color OnOverFirstBorderColor
        {
            get => _onOverFirstBorderColor;
            set => _onOverFirstBorderColor = value;
        }
        [Description("Дополнительный цвет градиента границ кнопки при наведении на нее указателя мыши"), Category("Свойства поведения")]
        public Color OnOverSecondBorderColor
        {
            get => _onOverSecondBorderColor;
            set => _onOverSecondBorderColor = value;
        }
        [Description("Основной цвет кнопки при нажатии на нее мыши"), Category("Свойства поведения")]
        public Color OnClickFirstFillColor
        {
            get => _onClickFirstFillColor;
            set => _onClickFirstFillColor = value;
        }
        [Description("Дополнительный цвет градиента кнопки при нажатии на нее мыши"), Category("Свойства поведения")]
        public Color OnClickSecondFillColor
        {
            get => _onClickSecondFillColor;
            set => _onClickSecondFillColor = value;
        }
        [Description("Основной цвет границ кнопки при нажатии на нее мыши"), Category("Свойства поведения")]
        public Color OnClickFirstBorderColor
        {
            get => _onClickFirstBorderColor;
            set => _onClickFirstBorderColor = value;
        }
        [Description("Дополнительный цвет градиента границ кнопки при нажатии на нее мыши"), Category("Свойства поведения")]
        public Color OnClickSecondBorderColor
        {
            get => _onClickSecondBorderColor;
            set => _onClickSecondBorderColor = value;
        }
        #endregion

        #region [ Скрытые свойства класса ]
        [Browsable(false)]
        public new Cursor Cursor { get; set; }
        [Browsable(false)]
        public new RightToLeft RightToLeft { get; set; }
        [Browsable(false)]
        public new bool UseVisualStyleBackColor { get; set; }
        [Browsable(false)]
        public new bool UseWaitCursor { get; set; }
        [Browsable(false)]
        public new bool UseCompatibleTextRendering { get; set; }
        [Browsable(false)]
        public new string AccessibleDescription { get; set; }
        [Browsable(false)]
        public new string AccessibleName { get; set; }
        [Browsable(false)]
        public new string AccessibleRole { get; set; }
        [Browsable(false)]
        public new bool CausesValidation { get; set; }
        [Browsable(false)]
        public new bool AutoEllipsis { get; set; }
        [Browsable(false)]
        public new ContextMenuStrip ContextMenuStrip { get; set; }
        [Browsable(false)]
        public new bool TabStop { get; set; }
        [Browsable(false)]
        public new bool AllowDrop { get; set; }
        [Browsable(false)]
        public new bool DialogResult { get; set; }
        [Browsable(false)]
        public new bool AutoSize { get; set; }
        [Browsable(false)]
        public new AutoSizeMode AutoSizeMode { get; set; }
        [Browsable(false)]
        public new bool UseMnemonic { get; set; }
        #endregion

        public CodeeloButton()
        {
            FlatStyle = FlatStyle.Flat;
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.WhiteSmoke;
            Font = new Font(FontFamily.GenericSerif, 14, FontStyle.Regular);
            Size = new Size(180, 60);
            Resize += CodeeloButton_Resize;
            SetStyle(ControlStyles.Opaque
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw
                , true);
        }

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            InvokePaintBackground(this, e);
            
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int borderSize = _borderSize > 1 ? _borderSize : 2;

            Color firstFillColor, secondFillColor, firstBorderColor, secondBorderColor;
            if (_isEntered)
            {
                if(_isClicked)
                {
                    firstFillColor = _onClickFirstFillColor;
                    secondFillColor = _onClickSecondFillColor;
                    firstBorderColor = _onClickFirstBorderColor;
                    secondBorderColor = _onClickSecondBorderColor;
                }
                else
                {
                    firstFillColor = _onOverFirstFillColor;
                    secondFillColor = _onOverSecondFillColor;
                    firstBorderColor = _onOverFirstBorderColor;
                    secondBorderColor = _onOverSecondBorderColor;
                }
            }
            else
            {
                firstFillColor = _firstFillColor;
                secondFillColor = _secondFillColor;
                firstBorderColor = _firstBorderColor;
                secondBorderColor = _secondBorderColor;
            }


            using (var gradientBrush = new LinearGradientBrush(ClientRectangle, firstFillColor, secondFillColor, _gradientDirection))
            using (var borderGradientBrush = new LinearGradientBrush(ClientRectangle, firstBorderColor, secondBorderColor, _gradientBorderDirection))
            using (var penBorder = _useGradientBorder ? new Pen(borderGradientBrush, borderSize) : new Pen(new SolidBrush(firstBorderColor), borderSize))
            {
                if (_borderRadius > 2)
                {
                    using (var borderPath = GraphicsUtils.GetFigurePath(Rectangle.Inflate(ClientRectangle, -2, -2), _borderRadius))
                    using (var path = GraphicsUtils.GetFigurePath(Rectangle.Inflate(ClientRectangle, -1, -1), _borderRadius)) 
                    {
                        if (_useGradient)
                            graphics.FillPath(gradientBrush, path);
                        else
                            graphics.FillPath(new SolidBrush(firstFillColor), path);

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

                    if (_useGradient)
                        graphics.FillRectangle(gradientBrush, ClientRectangle);
                    else
                        graphics.FillRectangle(new SolidBrush(firstFillColor), ClientRectangle);

                    if(borderSize > 0)
                    {
                        penBorder.Alignment = PenAlignment.Center;
                        graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
                    }
                }
            }

            var textSize = graphics.MeasureString(Text, Font);
            float positionX = 5F + _borderSize;
            float positionY = (Height - textSize.Height) / 2;

            switch (_textPosition)
            {
                case TextPosition.Right:
                    positionX = Width - textSize.Width - _borderSize;
                    break;
                case TextPosition.Center:
                    positionX = (Width - textSize.Width) / 2;
                    break;
                case TextPosition.Left:
                    positionX = 5F + _borderSize;
                    break;
            }

            graphics.DrawString(Text, Font, new SolidBrush(ForeColor), positionX, positionY);
            graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(64, ForeColor)), positionX + 1, positionY + 1);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isEntered = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isEntered = false;
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            _isClicked = true;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _isClicked = false;
            Invalidate();
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += Container_BackColorChanged;
        }
        private void Container_BackColorChanged(object sender, EventArgs e) => Invalidate();
        private void CodeeloButton_Resize(object sender, EventArgs e)
        {
            if (_borderRadius > Height)
                _borderRadius = Height;
        }
        #endregion     
    }
}
