using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CodeeloPictureBox), "Icons.CodeeloPictureBox.bmp")]
    [ToolboxItem(true)]
    [Description("Элемент для отображения изображений")]
    public class CodeeloPictureBox : PictureBox
    {
        #region [ Поля класса ]
        private bool _useGradient;
        private bool _drawBorder;
        private int _borderSize = 2;
        private Color _borderColorFirst = Color.RoyalBlue;
        private Color _borderColorSecond = Color.HotPink;
        private DashStyle _borderLineStyle = DashStyle.Solid;
        private DashCap _borderCapStyle = DashCap.Flat;
        private LinearGradientMode _gradientDirection = LinearGradientMode.ForwardDiagonal;
        private bool _makeCircle;
        private bool _borderInside;
        #endregion

        #region [ Свойства класса ]
        [Description("Сделать границу PictureBox круглой"), Category("Границы элемента")]
        public bool DrawCircle
        {
            get => _makeCircle;
            set
            {
                _makeCircle = value;
                Invalidate();
            }
        }
        [Description("Рисовать границу внутри круга"), Category("Границы элемента")]
        public bool DrawBorderInside
        {
            get => _borderInside;
            set
            {
                _borderInside = value;
                Invalidate();
            }
        }
        [Description("Рисовать границу элемента"), Category("Границы элемента")]
        public bool DrawBorder
        {
            get => _drawBorder;
            set
            {
                _drawBorder = value;
                Invalidate();
            }
        }
        [Description("Толщина границы"), Category("Границы элемента")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }
        [Description("Основной цвет"), Category("Границы элемента")]
        public Color BorderColorFirst
        {
            get => _borderColorFirst;
            set
            {
                _borderColorFirst = value;
                Invalidate();
            }
        }
        [Description("Второй цвет для градиентного заполнения границ"), Category("Границы элемента")]
        public Color BorderColorSecond
        {
            get => _borderColorSecond; 
            set
            {
                _borderColorSecond = value;
                Invalidate();
            }
        }
        [Description("Тип рисовки линии границы"), Category("Границы элемента")]
        public DashStyle BorderLineStyle
        {
            get => _borderLineStyle;
            set
            {
                _borderLineStyle = value;
                Invalidate();
            }
        }
        [Description("Тип конца линии (прямоугольный, закругленный, треугольный)"), Category("Границы элемента")]
        public DashCap BorderCapStyle
        {
            get => _borderCapStyle; 
            set
            {
                _borderCapStyle = value;
                Invalidate();
            }
        }
        [Description("Тип (направление) градиента"), Category("Границы элемента")]
        public LinearGradientMode GradientDirection
        {
            get => _gradientDirection;
            set
            {
                _gradientDirection = value;
                Invalidate();
            }
        }
        [Description("Использовать градиент при заливке границы"), Category("Границы элемента")]
        public bool UseGradient
        {
            get => _useGradient;
            set
            {
                _useGradient = value;
                Invalidate();
            }
        }
        #endregion

        public CodeeloPictureBox()
        {
            Size = new Size(200, 200);
            SizeMode = PictureBoxSizeMode.Zoom;
            SetStyle(ControlStyles.UserPaint 
                | ControlStyles.ResizeRedraw 
                | ControlStyles.Selectable 
                | ControlStyles.UserMouse 
                | ControlStyles.SupportsTransparentBackColor 
                | ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
        }

        #region [ События ]
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_makeCircle)
                Size = new Size(Width, Width);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            var contourOn = new RectangleF(_borderSize, _borderSize, ClientRectangle.Width - 2 * _borderSize, ClientRectangle.Height - 2 * _borderSize);
            var contourIn = Rectangle.Inflate(ClientRectangle, -1, -1);
            var borderIn = Rectangle.Inflate(contourIn, (-1 * _borderSize / 2), (-1 * _borderSize / 2));

            var rectContourSmooth = _borderInside ? contourIn : contourOn;
            var rectBorder = _borderInside ? borderIn : rectContourSmooth;
            
            using (var lgb = new LinearGradientBrush(rectBorder, _borderColorFirst, _borderColorSecond, _gradientDirection))
            using (var penBorder = _useGradient ? new Pen(lgb, _borderSize) : new Pen(new SolidBrush(_borderColorFirst), _borderSize))
            using (var textureBrush = new TextureBrush(GetCanvasImage()))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.GammaCorrected;

                using (GraphicsPath pathRegion = new GraphicsPath())
                {
                    if(_makeCircle)
                        pathRegion.AddEllipse(rectContourSmooth);
                    else
                        pathRegion.AddRectangle(rectContourSmooth);

                    graphics.FillPath(textureBrush, pathRegion);
                }

                if (_borderSize > 0 && _drawBorder)
                {
                    penBorder.DashStyle = _borderLineStyle;
                    penBorder.DashCap = _borderCapStyle;

                    if (_makeCircle)
                        graphics.DrawEllipse(penBorder, rectBorder);
                    else
                        graphics.DrawRectangles(penBorder, new[] { rectBorder });
                }
            }
        }
        #endregion

        #region [ Методы ]
        private Bitmap GetCanvasImage()
        {
            var canvasImage = new Bitmap(Width, Height);
            using (var graphics = System.Drawing.Graphics.FromImage(canvasImage))
            {
                if (Image == null)
                {
                    graphics.FillRectangle(new SolidBrush(Color.Blue), ClientRectangle);
                }
                else
                {
                    graphics.DrawImage(Image, GetRectangleBySizeMode());
                }
                return canvasImage;
            }
        }
        private Rectangle GetPaddedRectangle()
        {
            var rectangle = ClientRectangle;
            checked { rectangle.X += Padding.Left; }
            checked { rectangle.Y += Padding.Top; }
            checked { rectangle.Width -= Padding.Horizontal; }
            checked { rectangle.Height -= Padding.Vertical; }
            return rectangle;
        }
        private Rectangle GetRectangleBySizeMode()
        {
            var rectangle = GetPaddedRectangle();
            if (Image != null)
            {
                switch (SizeMode)
                {
                    case PictureBoxSizeMode.Normal:
                    case PictureBoxSizeMode.AutoSize:
                        rectangle.Size = Image.Size;
                        break;
                    case PictureBoxSizeMode.CenterImage:
                        rectangle.X = checked((int)Math.Round(rectangle.X + checked(rectangle.Width - Image.Width) / 2.0));
                        rectangle.Y = checked((int)Math.Round(rectangle.Y + checked(rectangle.Height - Image.Height) / 2.0));
                        rectangle.Size = Image.Size;
                        break;
                    case PictureBoxSizeMode.Zoom:
                        Size size = Image.Size;
                        float num = Math.Min(ClientRectangle.Width / (float)size.Width, ClientRectangle.Height / (float)size.Height);
                        rectangle.Width = checked((int)Math.Round(size.Width * (double)num));
                        rectangle.Height = checked((int)Math.Round(size.Height * (double)num));
                        rectangle.X = checked((int)Math.Round(checked(ClientRectangle.Width - rectangle.Width) / 2.0));
                        rectangle.Y = checked((int)Math.Round(checked(ClientRectangle.Height - rectangle.Height) / 2.0));
                        break;
                }
            }
            return rectangle;
        }
        #endregion
    }
}
