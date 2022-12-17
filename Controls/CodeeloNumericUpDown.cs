using CodeeloUI.Animation.Animator;
using CodeeloUI.Animation.Effects.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    public class CodeeloNumericUpDown : Control
    {
        #region [ Поля класса ]
        private Rectangle _minusCircleArea;
        private Rectangle _plusCircleArea;
        private Rectangle _mainArea;
        private int _minValue = 0;
        private int _maxValue = 100;
        private int _value = 0;
        private int _radius = 15;
        private Color _fillColor = Color.MediumSlateBlue;
        private Color _secondFillColor = Color.LightCoral;
        private LinearGradientMode _gradientDirection = LinearGradientMode.Horizontal;
        private List<AnimationStatus> _cancellationTokens;
        private EasingDelegate _easingFunction;
        private Point _startPoint;

        public event EventHandler ValueChanged;
        #endregion
        #region [ Свойства класса ]
        public Color FillColor 
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                Invalidate();
            } 
        }
        public Color FillColorSecond
        {
            get => _secondFillColor;
            set
            {
                _secondFillColor = value;
                Invalidate();
            }
        }
        public LinearGradientMode GradientDirection
        {
            get => _gradientDirection;
            set
            {
                _gradientDirection = value;
                Invalidate();
            }
        }
        public int MinValue { get => _minValue; 
            set
            {
                _minValue = value < 0 ? 0 : value > MaxValue ? MaxValue : value;
            }
        }
        public int MaxValue { get => _maxValue; set
            {
                _maxValue = value < MinValue ? MinValue : value;
            }
        }
        public int Interval { get; set; } = 10;
        public int Value 
        { 
            get => _value; 
            set 
            {
                _value = value < MinValue ? MinValue : value > MaxValue ? MaxValue : value;
                Invalidate();
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
            }  
        }
        public int Radius 
        {
            get => _radius; 
            set
            {
                _radius = value;
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
        public new bool BackgroundImage { get; set; }
        [Browsable(false)]
        public new bool BackgroundImageLayout { get; set; }
        [Browsable(false)]
        public new bool ImeMode { get; set; }
        #endregion
        public CodeeloNumericUpDown()
        {
            Height = 32;
            Width = 100;
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            _cancellationTokens = new List<AnimationStatus>();
            _easingFunction = EasingFunctions.BounceEaseIn;
        }

        #region [ Методы ]
        #endregion
        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int x = (Width * 4) / 20;

            _minusCircleArea = new Rectangle(5, 2, x, Height - 4);
            _plusCircleArea = new Rectangle(Width - 5 - x, 2, x, Height - 4);
            _mainArea = new Rectangle(5 + (x / 2), 4, Width - x - 10, Height - 8);

            using (var path = Graphics.GraphicsUtils.GetFigurePath(Rectangle.Inflate(ClientRectangle, -1, -1), Radius))
            using (var gradientBrush = new LinearGradientBrush(ClientRectangle, FillColor, FillColorSecond, GradientDirection))
            {
                graphics.FillPath(gradientBrush, path);
            }

            var textSize = TextRenderer.MeasureText(Value.ToString(), Font);
            var minusTextSize = TextRenderer.MeasureText("–", new Font(FontFamily.GenericSansSerif, 24));
            var plusTextSize = TextRenderer.MeasureText("+", new Font(FontFamily.GenericSansSerif, 24));

            graphics.DrawString(Value.ToString(), Font, new SolidBrush(ForeColor)
                , _mainArea.X + (_mainArea.Width / 2 - textSize.Width / 2)
                , _mainArea.Y + (_mainArea.Height / 2 - textSize.Height / 2));

            graphics.DrawString("–", new Font(FontFamily.GenericSansSerif, 24), new SolidBrush(ForeColor)
                , _minusCircleArea.X + 3 + (_minusCircleArea.Width / 4 - minusTextSize.Width / 4)
                , _minusCircleArea.Y-3 + (_minusCircleArea.Height / 2 - minusTextSize.Height / 2));

            graphics.DrawString("+", new Font(FontFamily.GenericSansSerif, 24), new SolidBrush(ForeColor)
                , _plusCircleArea.X + _plusCircleArea.Width/2+(_plusCircleArea.Width / 4 - plusTextSize.Width / 2)
                , _plusCircleArea.Y + (_plusCircleArea.Height / 2 - plusTextSize.Height / 2));
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if(_minusCircleArea.Contains(e.Location))
                Value -= Interval;
            if (_plusCircleArea.Contains(e.Location))
                Value += Interval;

            base.OnMouseClick(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _startPoint = Location;
            foreach (var token in _cancellationTokens)
                token.CancellationToken.Cancel();

            _cancellationTokens.Clear();
            int valueTo = _minusCircleArea.Contains(e.Location) ? Location.Y + 3 : _plusCircleArea.Contains(e.Location) ? Location.Y - 3 : Location.Y;
            var cancellationToken = this.Animate(
                new YLocationEffect(),
                _easingFunction,
                valueTo,
                30,
                0);

            _cancellationTokens.Add(cancellationToken);

            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            foreach (var token in _cancellationTokens)
                token.CancellationToken.Cancel();

            _cancellationTokens.Clear();
            Location = _startPoint;
            base.OnMouseUp(e);
        }
        #endregion
    }
}
