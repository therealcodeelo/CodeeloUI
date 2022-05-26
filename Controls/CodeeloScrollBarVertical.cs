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
    [ToolboxItem(true)]
    [Description("Настраиваемая кнопка")]
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



        protected int moValue;
        

        private bool mosliderDown;
        private bool mosliderDragging;

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
            get => moValue;
            set
            {
                moValue = value;

                int sliderHeight = GetSliderHeight();

                //figure out value
                int nPixelRange = AreaHeight - sliderHeight;
                int nRealRange = (MaxValue - MinValue) - MaxIncreaseValue;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = moValue / (float)nRealRange;
                }

                float fTop = fPerc * nPixelRange;
                _sliderTop = (int)fTop;

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
        public Color ArrowFillColor
        {
            get => _arrowFillColor;
            set => _arrowFillColor = value;
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
                new Size(12*2, 2048);
            Console.WriteLine(GetSliderHeight());
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
            int nRealRange = MaxValue - MinValue;

            int sliderHeight = GetSliderHeight();

            int nSpot = _clickPoint;

            int nPixelRange = AreaHeight - sliderHeight;
            if (mosliderDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewSliderTop = y - (Width + nSpot);

                    if (nNewSliderTop < 0)
                    {
                        _sliderTop = nNewSliderTop = 0;
                    }
                    else if (nNewSliderTop > nPixelRange)
                    {
                        _sliderTop = nNewSliderTop = nPixelRange;
                    }
                    else
                    {
                        _sliderTop = y - (Width + nSpot);
                    }

                    //figure out value
                    float fPerc = _sliderTop / (float)nPixelRange;
                    float fValue = fPerc * (MaxValue - MaxIncreaseValue);
                    moValue = (int)fValue;

                    Application.DoEvents();

                    Invalidate();
                }
            }
        }
        #endregion

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var areaPath = CustomGraphicsPath.GetFigurePath(new Rectangle(0, Width, Width, AreaHeight)))
            using (var sliderPath = CustomGraphicsPath.GetFigurePath(new Rectangle(4, Width + 4 + _sliderTop, Width - 8, GetSliderHeight())))
            using (var topTriangle = CustomGraphicsPath.GetBottomSideTriangle(Width - 4, (Width - (Width - 4)) / 2, Width - 5 - (float)((Width - 4) * Math.Sqrt(3) / 2)))
            using (var bottomTriangle = CustomGraphicsPath.GetTopSideTriangle(Width - 4, (Width - (Width - 4)) / 2, AreaHeight + Width + 5))
            using (var areaBrush = new LinearGradientBrush(ClientRectangle, AreaFirstFillColor, AreaSecondFillColor, AreaGradientDirection)) 
            using (var sliderBrush = new LinearGradientBrush(ClientRectangle, SliderFirstFillColor, SliderSecondFillColor, SliderGradientDirection))
            using (var arrowBrush = new SolidBrush(ArrowFillColor))
            {
                e.Graphics.FillPath(arrowBrush, topTriangle);
                e.Graphics.FillPath(areaBrush, areaPath);
                e.Graphics.FillPath(sliderBrush, sliderPath);
                e.Graphics.FillPath(arrowBrush, bottomTriangle);
            }
        }
        private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            var clickPoint = PointToClient(Cursor.Position);
            int sliderHeight = GetSliderHeight();
            int nTop = _sliderTop;

            //   nTop += Width;

            var sliderRect = new Rectangle(0, nTop, Width, sliderHeight);
            if (sliderRect.Contains(clickPoint))
            {
                _clickPoint = (clickPoint.Y - nTop);
                mosliderDown = true;
            }

            var upArrowRect = new Rectangle(0, 0, Width, Width);
            if (upArrowRect.Contains(clickPoint))
            {
                int nRealRange = (MaxValue - MinValue) - MaxIncreaseValue;
                int nPixelRange = AreaHeight - sliderHeight;
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if ((_sliderTop - MinIncreaseValue) < 0)
                            _sliderTop = 0;
                        else
                            _sliderTop -= MinIncreaseValue;

                        //figure out value
                        float fPerc = _sliderTop / (float)nPixelRange;
                        float fValue = fPerc * (MaxValue - MaxIncreaseValue);

                        moValue = (int)fValue;

                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());

                        if (Scroll != null)
                            Scroll(this, new EventArgs());

                        Invalidate();
                    }
                }
            }

            Rectangle downArrowRect = new Rectangle(0, Width + AreaHeight, Width, Width);
            if (downArrowRect.Contains(clickPoint))
            {
                int nRealRange = (MaxValue - MinValue) - MaxIncreaseValue;
                int nPixelRange = AreaHeight - sliderHeight;
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if ((_sliderTop + MinIncreaseValue) > nPixelRange)
                            _sliderTop = nPixelRange;
                        else
                            _sliderTop += MinIncreaseValue;

                        //figure out value
                        float fPerc = _sliderTop / (float)nPixelRange;
                        float fValue = fPerc * (MaxValue - MaxIncreaseValue);

                        moValue = (int)fValue;

                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());

                        if (Scroll != null)
                            Scroll(this, new EventArgs());

                        Invalidate();
                    }
                }
            }
        }

        private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            mosliderDown = false;
            mosliderDragging = false;
        }

        private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (mosliderDown)
            {
                mosliderDragging = true;
            }

            if (mosliderDragging)
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
