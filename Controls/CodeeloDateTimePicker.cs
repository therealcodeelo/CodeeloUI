using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CodeeloUI.Properties;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CodeeloDateTimePicker), "Icons.CodeeloCalendar.bmp")]
    [ToolboxItem(true)]
    [Description("Элемент для отображения даты")]
    public sealed class CodeeloDateTimePicker : DateTimePicker
    {
        #region [ Поля класса ]
        private Color _backColor = Color.White;
        private Color _foreColor = Color.Black;
        private Color _borderColor = Color.Black;
        private int _borderSize = 1;
        private bool _droppedDown;
        private Image _calendarIcon = Resources.calendarBlack_34px;
        private RectangleF _iconButtonArea;
        private const int CALENDAR_ICON_WIDTH = 34;
        private const int ARROW_ICON_WIDTH = 17;
        #endregion

        #region [ Свойства класса ]
        [Description("Цвет заливки фона"), Category("Настройки внешнего вида"), Browsable(true)]
        public Color FillColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                _calendarIcon = _backColor.GetBrightness() >= 0.8F ? Resources.calendarBlack_34px : Resources.calendarWhite_34px;
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
                Invalidate();
            }
        }
        [Description("Ширина границ элемента"), Category("Настройки внешнего вида")]
        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }
        [Description("Цвет текста"), Category("Настройки внешнего вида")]
        public Color TextColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                Invalidate();
            }
        }
        #endregion

        #region [ Скрытые свойства класса ]
        [Browsable(false)]
        public new Color CalendarForeColor {get;set;}
        [Browsable(false)]
        public new Color CalendarMonthBackground { get; set; }
        [Browsable(false)]
        public new Color CalendarTitleBackColor { get; set; }
        [Browsable(false)]
        public new Color CalendarTitleForeColor { get; set; }
        [Browsable(false)]
        public new Color CalendarTrailingForeColor { get; set; }
        [Browsable(false)]
        public new Font CalendarFont { get; set; }
        [Browsable(false)]
        public new bool ShowCheckBox { get; set; }
        [Browsable(false)]
        public new bool Checked { get; set; }
        [Browsable(false)]
        public new bool ShowUpDown { get; set; }
        #endregion
        public CodeeloDateTimePicker()
        {
            SetStyle(ControlStyles.UserPaint, true);
            MinimumSize = new Size(0, 35);
            Font = new Font(Font.Name, 9.5F);
        }
        #region [ События ]
        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
            _droppedDown = true;
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            _droppedDown = false;
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (System.Drawing.Graphics graphics = CreateGraphics())
            using (Pen penBorder = new Pen(_borderColor, _borderSize))
            using (SolidBrush skinBrush = new SolidBrush(_backColor))
            using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
            using (SolidBrush textBrush = new SolidBrush(TextColor))
            using (StringFormat textFormat = new StringFormat())
            {
                var clientArea = new RectangleF(0, 0, Width - 0.5F, Height - 0.5F);
                var iconArea = new RectangleF(clientArea.Width - CALENDAR_ICON_WIDTH - 9, 0, CALENDAR_ICON_WIDTH, clientArea.Height);
                penBorder.Alignment = PenAlignment.Inset;
                textFormat.LineAlignment = StringAlignment.Center;
                graphics.FillRectangle(skinBrush, clientArea);
                graphics.DrawString("   " + Text, Font, textBrush, clientArea, textFormat);
                if (_droppedDown) 
                    graphics.FillRectangle(openIconBrush, iconArea);

                if (_borderSize >= 1) 
                    graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);

                graphics.DrawImage(_calendarIcon, Width - _calendarIcon.Width - 9, (Height - _calendarIcon.Height) / 2);
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _iconButtonArea = new RectangleF(Width - CALENDAR_ICON_WIDTH - 9, 0, CALENDAR_ICON_WIDTH, Height);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = _iconButtonArea.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
        }
        #endregion
    }
}
