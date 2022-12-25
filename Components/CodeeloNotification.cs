using CodeeloUI.SupportControls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloNotification), "Icons.CodeeloNotification.bmp")]
    public partial class CodeeloNotification : Component
    {
        #region [ Поля класса ]
        private Size _size = new Size(500, 100);
        private Font _font = new Font("Arial", 12f);
        private double _hoveredOpacity = 1.0;
        private double _regularOpacity = 0.6;
        #endregion

        #region [ Свойства класса ]
        public Size Size { get => _size; set => _size = value; }
        public Color BackColor { get; set; } = Color.DarkGray;
        public Color ForeColor { get; set; } = Color.White;
        public Font Font { get => _font; set => _font = value; }
        public Image LogoImage { get; set; }
        public double HoveredOpacity
        {
            get => _hoveredOpacity;
            set => _hoveredOpacity = value > 1.0 ? 1.0 : value < 0.05 ? 0.05 : value;
        }
        public double RegularOpacity
        {
            get => _regularOpacity;
            set => _regularOpacity = value > 1.0 ? 1.0 : value < 0.05 ? 0.05 : value;
        }
        public int ShowDurationInSeconds { get; set; }
        #endregion

        public CodeeloNotification()
        {
            InitializeComponent();
        }

        public CodeeloNotification(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void ShowNotify(string notificationText)
        {
            var textHeight = TextRenderer.MeasureText(notificationText, Font, Size, TextFormatFlags.WordBreak).Height;
            if (textHeight < 100)
            {
                textHeight += textHeight / 2;
            }
            else
            {
                textHeight += textHeight / 4;
            }

            var newNotification = new Notification(Size, BackColor, Font, ForeColor, textHeight,
                ShowDurationInSeconds, notificationText, LogoImage, HoveredOpacity, RegularOpacity);
            newNotification.ShowNotification();
            //using ()
            //{

            //}

        }
    }
}
