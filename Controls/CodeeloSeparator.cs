using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeeloUI.Graphics;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CodeeloSeparator), "Icons.CodeeloSeparator.bmp")]
    public class CodeeloSeparator : Control
    {
        #region [ Поля класса ]
        private int _thickness = 3;
        private bool _isVertical;
        private Color _lineColor = Color.Silver;
        #endregion

        #region [ Свойства класса ]
        [Description("Расположить по вертикали"), Category("Настройки внешнего вида")]
        public bool IsVertical
        {
            get => _isVertical;
            set
            {
                _isVertical = value;
                Size = new Size(Height, Width);
                Invalidate();
            }
        }
        [Description("Толщина линии"), Category("Настройки внешнего вида")]
        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                if (Height < _thickness)
                    Height = _thickness;
                else
                    Invalidate();
            }
        }
        [Description("Цвет линии"), Category("Настройки внешнего вида")]
        public Color LineColor
        {
            get => _lineColor;
            set
            {
                _lineColor = value;
                Invalidate();
            }
        }
        #endregion
        public CodeeloSeparator()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
        }

        #region [ События ]
        protected override void OnPaint(PaintEventArgs e)
        {
            var separatorSize = _isVertical ? Height / 2 : Width / 2;
            float centerX = _isVertical ? Width / 2 - Thickness / 2 : Width / 2;
            float centerY = _isVertical ? Height / 2 : Height / 2 - Thickness/2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var firstRect = _isVertical ? new RectangleF(centerX, 1, _thickness, separatorSize) : new RectangleF(centerX, centerY, separatorSize, _thickness);
            var secondRect = _isVertical ? new RectangleF(centerX, centerY, _thickness, separatorSize) : new RectangleF(1, centerY, separatorSize, _thickness);

            using (var secondBrush = _isVertical ? new LinearGradientBrush(secondRect, LineColor, Color.FromArgb(64, LineColor), LinearGradientMode.Vertical) : new LinearGradientBrush(secondRect, Color.FromArgb(64, LineColor), LineColor, LinearGradientMode.Horizontal))
            using (var firstBrush = _isVertical ?  new LinearGradientBrush(firstRect, Color.FromArgb(64, LineColor), LineColor, LinearGradientMode.Vertical) : new LinearGradientBrush(firstRect, LineColor, Color.FromArgb(64, LineColor),  LinearGradientMode.Horizontal))
            {
                if (_isVertical)
                {
                    e.Graphics.FillPath(firstBrush, GraphicsUtils.GetTopSeparatorPart(firstRect));
                    e.Graphics.FillPath(secondBrush, GraphicsUtils.GetBottomSeparatorPart(secondRect));
                }
                else
                {
                    e.Graphics.FillPath(firstBrush, GraphicsUtils.GetRightSeparatorPart(firstRect));
                    e.Graphics.FillPath(secondBrush, GraphicsUtils.GetLeftSeparatorPart(secondRect));
                }
            }
        }
        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            Invalidate();
        }
        #endregion
    }
}
