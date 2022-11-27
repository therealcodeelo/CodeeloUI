using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using CodeeloUI.Graphics;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(VScrollBar))]
    [ToolboxItem(false)]
    [Description("Вертикальная полоса прокрутки")]
    public class CodeeloScrollBarVertical : UserControl
    {
        #region [ Поля класса ]
        private Color _areaFirstFillColor = Color.FromArgb(126, 232, 250);
        private Color _areaSecondFillColor = Color.FromArgb(238, 192, 198);
        private Color _sliderFirstFillColor = Color.FromArgb(3, 233, 172);
        private Color _sliderSecondFillColor = Color.FromArgb(21, 152, 255);
        private Color _arrowFillColor = Color.Silver;
        private LinearGradientMode _areaGradientDirection = LinearGradientMode.ForwardDiagonal;
        private LinearGradientMode _sliderGradientDirection = LinearGradientMode.ForwardDiagonal;

        private int _maxIncreaseValue = 10;
        private int _minIncreaseValue = 1;
        private int _minValue;
        private int _maxValue = 100;
        private int _clickPoint;
        private int _sliderTop;

        private int _mouseValue;
        private bool _mouseSliderDown;
        private bool _mouseSliderDrag;

        public new event EventHandler Scroll;
        public event EventHandler ValueChanged;
        #endregion

        #region [ Свойства класса ]

        public int MaxIncreaseValue
        {
            get => _maxIncreaseValue;
            set
            {
                _maxIncreaseValue = value;
                Invalidate();
            }
        }
        public int MinIncreaseValue
        {
            get => _minIncreaseValue;
            set
            {
                _minIncreaseValue = value;
                Invalidate();
            }
        }
        public int MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                Invalidate();
            }
        }
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                Invalidate();
            }
        }
        public int Value
        {
            get => _mouseValue;
            set
            {
                _mouseValue = value;

                var sliderHeight = GetSliderHeight();

                //figure out value
                var pixelRange = AreaHeight - sliderHeight;
                var realRange = (MaxValue - MinValue) - MaxIncreaseValue;

                var percent = 0.0f;
                if (realRange != 0)
                {
                    percent = _mouseValue / (float)realRange;
                }

                _sliderTop = (int)(percent * pixelRange);

                Invalidate();
            }
        }
        public Color AreaFirstFillColor
        {
            get => _areaFirstFillColor;
            set => _areaFirstFillColor = value;
        }
        public Color AreaSecondFillColor
        {
            get => _areaSecondFillColor;
            set => _areaSecondFillColor = value;
        }
        public Color SliderFirstFillColor
        {
            get => _sliderFirstFillColor;
            set => _sliderFirstFillColor = value;
        }
        public Color SliderSecondFillColor
        {
            get => _sliderSecondFillColor;
            set => _sliderSecondFillColor = value;
        }

        public LinearGradientMode AreaGradientDirection
        {
            get => _areaGradientDirection;
            set => _areaGradientDirection = value;
        }
        public LinearGradientMode SliderGradientDirection
        {
            get => _sliderGradientDirection;
            set => _sliderGradientDirection = value;
        }

        private int AreaHeight => Height - 2 * Width;

        #endregion

        public CodeeloScrollBarVertical()
        {
            MouseDown += CustomScrollbar_MouseDown;
            MouseMove += CustomScrollbar_MouseMove;
            MouseUp += CustomScrollbar_MouseUp;

            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            Width = 18;
            base.MinimumSize =
                new Size(12, 2 * 12 + GetSliderHeight());
            base.MaximumSize =
                new Size(12 * 2, 2048);

            DoubleBuffered = true;
        }
        #region [ Методы ]
        private int GetSliderHeight()
        {
            float sliderHeight = MaxIncreaseValue / (float)MaxValue * AreaHeight;

            if ((int)sliderHeight > AreaHeight)
            {
                sliderHeight = AreaHeight;
            }
            if ((int)sliderHeight < 56)
            {
                sliderHeight = 56;
            }

            return (int)sliderHeight;
        }
        private void MoveSlider(int y)
        {
            var realRange = MaxValue - MinValue;
            var sliderHeight = GetSliderHeight();
            var clickPoint = _clickPoint;
            var pixelRange = AreaHeight - sliderHeight - Width / 2;
            
            if (!_mouseSliderDown || realRange <= 0) 
                return;
            
            if (pixelRange <= 0) 
                return;
                
            if (y - (Width + clickPoint) < 0)
            {
                _sliderTop = 0;
            }
            else if (y - (Width + clickPoint) > pixelRange)
            {
                _sliderTop = pixelRange;
            }
            else
            {
                _sliderTop = y - (Width + clickPoint);
            }

            //figure out value
            float fPerc = _sliderTop / (float)pixelRange;
            float fValue = fPerc * (MaxValue - MaxIncreaseValue);
            _mouseValue = (int)fValue;

            Application.DoEvents();

            Invalidate();
        }
        #endregion

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var areaPath = GraphicsUtils.GetFigurePath(new Rectangle(0, Width, Width, AreaHeight)))
            using (var sliderPath = GraphicsUtils.GetFigurePath(new Rectangle(4, Width + 4 + _sliderTop, Width - 8, GetSliderHeight())))
            using (var topTriangle = GraphicsUtils.GetBottomSideTriangle(Width - 4, (Width - (Width - 4)) / 2, Width - 5 - (float)((Width - 4) * Math.Sqrt(3) / 2)))
            using (var bottomTriangle = GraphicsUtils.GetTopSideTriangle(Width - 4, (Width - (Width - 4)) / 2, AreaHeight + Width + 5))
            using (var areaBrush = new LinearGradientBrush(ClientRectangle, AreaFirstFillColor, AreaSecondFillColor, AreaGradientDirection)) 
            using (var sliderBrush = new LinearGradientBrush(ClientRectangle, SliderFirstFillColor, SliderSecondFillColor, SliderGradientDirection))
            {
                e.Graphics.FillPath(areaBrush, areaPath);
                e.Graphics.FillPath(sliderBrush, sliderPath);
            }
        }
        private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            var clickPoint = PointToClient(Cursor.Position);
            int sliderHeight = GetSliderHeight();
            int topPoint = _sliderTop;

            var sliderRect = new Rectangle(0, topPoint, Width, sliderHeight);
            if (sliderRect.Contains(clickPoint))
            {
                _clickPoint = (clickPoint.Y - topPoint);
                _mouseSliderDown = true;
            }

            int realRange = (MaxValue - MinValue) - MaxIncreaseValue;
            int pixelRange = AreaHeight - sliderHeight;

            if (realRange <= 0)
                return;
            if (pixelRange <= 0)
                return;


            float percent = _sliderTop / (float)pixelRange;
            float value = percent * (MaxValue - MaxIncreaseValue);

            _mouseValue = (int)value;

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if (Scroll != null)
                Scroll(this, new EventArgs());

            Invalidate();
        }

        private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseSliderDown = false;
            _mouseSliderDrag = false;
        }

        private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseSliderDown)
            {
                _mouseSliderDrag = true;
            }

            if (_mouseSliderDrag)
            {
                MoveSlider(e.Y);
            }

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if (Scroll != null)
                Scroll(this, new EventArgs());
        }
        #endregion
    }

    internal class ScrollbarControlDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(Component)["AutoSize"];
                if (propDescriptor != null)
                {
                    bool autoSize = (bool)propDescriptor.GetValue(Component);
                    if (autoSize)
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable;
                    }
                    else
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
                    }
                }
                return selectionRules;
            }
        }
    }
}
