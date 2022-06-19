using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeeloUI.Properties;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(DateTimePicker))]
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
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                _calendarIcon = _backColor.GetBrightness() >= 0.8F ? Resources.calendarBlack_34px : Resources.calendarWhite_34px;
                Invalidate();
            }
        }
        [Description("Цвет текста"), Category("Настройки внешнего вида"), Browsable(true)]
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
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
        #endregion
        public CodeeloDateTimePicker()
        {
            SetStyle(ControlStyles.UserPaint, true);
            MinimumSize = new Size(0, 35);
            Font = new Font(Font.Name, 9.5F);
            DoubleBuffered = true;
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
            using (SolidBrush textBrush = new SolidBrush(_foreColor))
            using (StringFormat textFormat = new StringFormat())
            {
                RectangleF clientArea = new RectangleF(0, 0, Width - 0.5F, Height - 0.5F);
                RectangleF iconArea = new RectangleF(clientArea.Width - CALENDAR_ICON_WIDTH, 0, CALENDAR_ICON_WIDTH, clientArea.Height);
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
            int iconWidth = GetIconButtonWidth();
            _iconButtonArea = new RectangleF(Width - iconWidth, 0, iconWidth, Height);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = _iconButtonArea.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
        }
        #endregion

        #region [ Методы ]
        private int GetIconButtonWidth()
        {
            int textWidth = TextRenderer.MeasureText(Text, Font).Width;
            if (textWidth <= Width - (CALENDAR_ICON_WIDTH + 20))
                return CALENDAR_ICON_WIDTH;
            return ARROW_ICON_WIDTH;
        }
        #endregion
    }
}
