using CodeeloUI.SupportClasses;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloShadowEffect), "Icons.CodeeloShadow.bmp")]
    public partial class CodeeloShadowEffect : Component
    {
        #region [ Поля класса ]
        private Control _sourceControl;
        private Color _shadowColor = Color.Black;
        private Padding _shadowPadding = new Padding(5);
        private bool _useShadowEffect = true;
        private int _depth = 30;
        private int _borderRadius = 6;

        private Bitmap _shadowImage;

        #endregion

        #region [ Свойства класса ]
        [Description("Элемент для которого будет применятся эффект тени")]
        public Control SourceControl
        {
            get => _sourceControl;
            set
            {
                _sourceControl = value;
                if (_sourceControl?.Parent != null)
                    _sourceControl.Parent.Paint += SourceControlParent_Paint;

            }
        }
        [Description("Использовать эффект тени")]
        public bool UseShadowEffect
        {
            get => _useShadowEffect;
            set => _useShadowEffect = value;
        }
        [Description("Цвет тени")]
        public Color Color
        {
            get => _shadowColor;
            set => _shadowColor = value;
        }
        [Description("Насыщенность тени")]
        public int Depth
        {
            get => _depth;
            set => _depth = value > byte.MaxValue ? byte.MaxValue : (value < 0 ? 0 : value);
        }
        [Description("Выступ тени над элементом")]
        public Padding ShadowPadding
        {
            get => _shadowPadding;
            set => _shadowPadding = value;
        }
        [Description("Радиус закругления углов прямоугольника тени")]
        public int BorderRadius
        {
            get => _borderRadius;
            set => _borderRadius = value < 0 ? 0 : value;
        }
        #endregion
        public CodeeloShadowEffect()
        {
            InitializeComponent();
        }
        public CodeeloShadowEffect(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #region [ Методы ]
        private Rectangle GetRectangleWithShadow()
        {
            int locationX = SourceControl.Location.X - ShadowPadding.Left;
            int locationY = SourceControl.Location.Y - ShadowPadding.Top;

            int widthIncrease = ShadowPadding.Left + ShadowPadding.Right;
            int heightIncrease = ShadowPadding.Top + ShadowPadding.Bottom;

            int width = SourceControl.Width + widthIncrease;
            int height = SourceControl.Height + heightIncrease;

            return new Rectangle(locationX, locationY, width, height);
        }

        private int GetMaxSidePadding()
        {
            int maxLeftRightPadding = _shadowPadding.Left >= _shadowPadding.Right ? _shadowPadding.Left : _shadowPadding.Right;

            int maxTopBottomPadding = _shadowPadding.Top >= _shadowPadding.Bottom ? _shadowPadding.Top : _shadowPadding.Bottom;

            return maxLeftRightPadding > maxTopBottomPadding ? maxLeftRightPadding : maxTopBottomPadding;
        }

        internal static GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
        #endregion

        #region [ События ]
        private void SourceControlParent_Paint(object sender, PaintEventArgs e)
        {
            if (!SourceControl.Visible)
                return;
            if (!_useShadowEffect)
                return;
            if (DesignMode)
                return;

            Rectangle rectangle = GetRectangleWithShadow();

            if (_shadowImage == null)
            {
                using (var bitmap = new Bitmap(rectangle.Width / 2, rectangle.Height / 2))
                using (var brush = new SolidBrush(Color.FromArgb(_depth, _shadowColor)))
                {
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

                    var rect1 = new RectangleF(.0f, .0f, bitmap.Width, bitmap.Height);
                    var rect2 = new RectangleF(rect1.X, rect1.Y, rect1.Width - 1f, rect1.Height - 1f);

                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.FillPath(brush, GraphicsUtils.GetFigurePath(rect2, BorderRadius < 2 ? 1F : BorderRadius / 2F));


                    _shadowImage = new Bitmap(rectangle.Width, rectangle.Height);

                    System.Drawing.Graphics shadowGraphics = System.Drawing.Graphics.FromImage(_shadowImage);

                    shadowGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    int iterationCount = GetMaxSidePadding() >= 10 ? 10 : GetMaxSidePadding();
                    int iteration = 0;
                    while (iteration <= iterationCount)
                    {
                        int width = rectangle.Width - (iteration * 2);
                        int height = rectangle.Height - (iteration * 2);

                        Rectangle rect = new Rectangle(iteration, iteration, width, height);
                        shadowGraphics.DrawImage(bitmap, rect);
                        ++iteration;
                    }
                }
            }
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.DrawImage(_shadowImage, rectangle.X, rectangle.Y);
        }
        #endregion
    }
}
