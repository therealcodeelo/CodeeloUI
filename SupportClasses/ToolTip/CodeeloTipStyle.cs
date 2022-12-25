using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.ToolTip
{
    public delegate Brush BrushSelector<in T>(T arg);
    public sealed class CodeeloTipStyle
    {
        #region [ Поля класса ]
        private readonly CodeeloBorder _border;

        private static CodeeloTipStyle _gray;
        private static CodeeloTipStyle _green;
        private static CodeeloTipStyle _orange;
        private static CodeeloTipStyle _red;
        #endregion

        #region [ Свойства класса ]
        internal CodeeloBorder Border => _border;
        public Bitmap Icon { get; set; }
        public int IconSpacing { get; set; }
        public Font TextFont { get; set; }
        public Point TextOffset { get; set; }
        public Color TextColor { get; set; }
        public Color BackColor { get; set; }
        public BrushSelector<Rectangle> BackBrush { get; set; }
        public Color BorderColor
        {
            get => _border.Color;
            set => _border.Color = value;
        }
        public int BorderWidth
        {
            get => _border.Width / 2;
            set => _border.Width = value * 2;
        }
        public int CornerRadius { get; set; }
        public Color ShadowColor { get; set; }
        public int ShadowRadius { get; set; }
        public Point ShadowOffset { get; set; }
        public Padding Padding { get; set; }

        public static CodeeloTipStyle Gray => _gray ?? (_gray = CreatePresetsStyle(0));
        public static CodeeloTipStyle Green => _green ?? (_green = CreatePresetsStyle(1));
        public static CodeeloTipStyle Orange => _orange ?? (_orange = CreatePresetsStyle(2));
        public static CodeeloTipStyle Red => _red ?? (_red = CreatePresetsStyle(3));
        #endregion

        public CodeeloTipStyle()
        {
            _border = new CodeeloBorder(PresetsResources.Colors[0, 0])
            {
                Behind = true,
                Width = 2
            };
            IconSpacing = 5;
            TextFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);
            TextColor = Color.Black;
            BackColor = Color.FromArgb(252, 252, 252);
            CornerRadius = 3;
            ShadowColor = PresetsResources.Colors[0, 2];
            ShadowRadius = 4;
            ShadowOffset = new Point(0, 3);
            Padding = new Padding(10, 5, 10, 5);
        }
        #region [ Методы ]
        private static CodeeloTipStyle CreatePresetsStyle(int index)
        {
            var style = new CodeeloTipStyle
            {
                Icon = PresetsResources.Icons[index],
                BorderColor = PresetsResources.Colors[index, 0],
                ShadowColor = PresetsResources.Colors[index, 2],
            };
            style.BackBrush = r =>
            {
                var brush = new LinearGradientBrush(r,
                    PresetsResources.Colors[index, 1],
                    Color.White,
                    LinearGradientMode.Horizontal);
                brush.SetBlendTriangularShape(0.5f);
                return brush;
            };
            return style;
        }
        #endregion

        private static class PresetsResources
        {
            public static readonly Color[,] Colors =
            {
                {Color.FromArgb(150, 150, 150), Color.FromArgb(245, 245, 245), Color.FromArgb(110, 0, 0, 0)},
                {Color.FromArgb(0, 189, 0), Color.FromArgb(232, 255, 232), Color.FromArgb(150, 0, 150, 0)},
                {Color.FromArgb(255, 150, 0), Color.FromArgb(255, 250, 240), Color.FromArgb(150, 250, 100, 0)},
                {Color.FromArgb(255, 79, 79), Color.FromArgb(255, 245, 245), Color.FromArgb(140, 255, 30, 30)}
            };

            public static readonly Bitmap[] Icons =
            {
                CreateIcon(0),
                CreateIcon(1),
                CreateIcon(2),
                CreateIcon(3)
            };

            private static Bitmap CreateIcon(int index)
            {
                if (index < 0 || index > 3)
                    return null;
                System.Drawing.Graphics g = null;
                Pen pen = null;
                Brush brush = null;
                Bitmap bmp = null;
                try
                {
                    bmp = new Bitmap(24, 24);
                    g = System.Drawing.Graphics.FromImage(bmp);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    var color = Colors[index, 0];
                    if (index == 0)
                    {
                        brush = new SolidBrush(Color.FromArgb(103, 148, 186));
                        g.FillEllipse(brush, 3, 3, 18, 18);

                        pen = new Pen(Colors[index, 1], 2);
                        g.DrawLine(pen, new Point(12, 6), new Point(12, 8));
                        g.DrawLine(pen, new Point(12, 10), new Point(12, 18));
                    }
                    else if (index == 1)
                    {
                        pen = new Pen(color, 4);
                        g.DrawLines(pen, new[] { new Point(3, 11), new Point(10, 18), new Point(20, 5) });
                    }
                    else if (index == 2)
                    {
                        var points = new[] { new Point(12, 3), new Point(3, 20), new Point(21, 20) };
                        pen = new Pen(color, 2) { LineJoin = LineJoin.Bevel };
                        g.DrawPolygon(pen, points);

                        brush = new SolidBrush(color);
                        g.FillPolygon(brush, points);

                        pen.Color = Colors[index, 1];
                        g.DrawLine(pen, new Point(12, 8), new Point(12, 15));
                        g.DrawLine(pen, new Point(12, 17), new Point(12, 19));
                    }
                    else
                    {
                        pen = new Pen(color, 4);
                        g.DrawLine(pen, 5, 5, 19, 19);
                        g.DrawLine(pen, 5, 19, 19, 5);
                    }
                    return bmp;
                }
                catch
                {
                    bmp?.Dispose();
                    throw;
                }
                finally
                {
                    pen?.Dispose();
                    brush?.Dispose();
                    g?.Dispose();
                }
            }
        }
    }
}
