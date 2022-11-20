using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CodeeloUI.Enums;
using CodeeloUI.Properties;

namespace CodeeloUI.SupportControls
{
    public partial class Notification : Form
    {
        private NotificationAction _action;
        private int _positionX, _positionY;
        private BackgroundWorker _backgroundWorker;
        private bool isPinned;

        private readonly int _lifeTime;
        private readonly string _message;
        private readonly double _hoveredOpacity = 1.0;
        private readonly double _regularOpacity = 0.6;
        

        public Notification(Size size, Color backColor, Font font,Color foreColor, int textHeight, int lifeTime, 
            string message, Image logoImage, double hoveredOpacity, double regularOpacity)
        {
            InitializeComponent();

            Size = size;
            BackColor = backColor;
            Font = font;
            ForeColor = foreColor;
            NotificationLogo.Image = logoImage;

            if (textHeight > Height)
                Height = textHeight;

            _lifeTime = lifeTime < 0 ? 0 : lifeTime;

            _message = message;
            _hoveredOpacity = hoveredOpacity;
            _regularOpacity = regularOpacity;

            CloseButton.FlatAppearance.BorderColor = backColor;
            PinButton.FlatAppearance.BorderColor = backColor;
        }
        public void ShowNotification()
        {
            Opacity = 0.0;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            string formName;
            int usedHeight = 0;

            for (int i = 1; i < 99; i++)
            {
                formName = "NotificationForm_" + i;
                Notification frm = (Notification)Application.OpenForms[formName];

                if (frm == null)
                {
                    Name = formName;
                    if (usedHeight + Height > Screen.PrimaryScreen.WorkingArea.Height)
                    {
                        _positionX = Screen.PrimaryScreen.WorkingArea.Width - 2 * Width - 10;
                        _positionY = usedHeight + 100 - Screen.PrimaryScreen.WorkingArea.Height;
                        if (_positionY + Height > Screen.PrimaryScreen.WorkingArea.Height)
                        {
                            return;
                        }
                    }
                    else
                    {
                        _positionX = Screen.PrimaryScreen.WorkingArea.Width - Width - 5;
                        _positionY = Screen.PrimaryScreen.WorkingArea.Height - usedHeight - Height - 5;
                    }
                    Location = new Point(_positionX, _positionY);
                    break;
                }

                usedHeight += frm.Height + 5;
            }

            NotificationText.Text = _message;

            Show();

            _action = NotificationAction.Started;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.RunWorkerAsync();

        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_action == NotificationAction.Closed && _lifeTime > 0)
                Close();
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_action == NotificationAction.Started)
            {
                new List<Control> { HeaderPanel, NotificationLogo, CloseButton, PinButton, NotificationText }.ForEach(x =>
                {
                    x.MouseEnter += Notification_MouseEnter;
                    x.MouseLeave += Notification_MouseLeave;
                });
                PinButton.Click += PinButton_Click;

                while (Opacity < _regularOpacity)
                {
                    Invoke(new Action(() => { Opacity += 0.03; }));
                    Thread.Sleep(10);
                }
                _action = NotificationAction.Active;
            }
            if (_action == NotificationAction.Active)
            {
                Thread.Sleep(_lifeTime * 1000);
                _action = NotificationAction.Closed;
            }
            if(_action == NotificationAction.Closed && !isPinned && _lifeTime > 0)
            {
                while (Opacity > 0.0)
                {
                    Invoke(new Action(() =>
                    {
                        Opacity -= 0.03;
                        Left -= 3;
                    }));
                    Thread.Sleep(10);
                }
            }
        }
        private void PinButton_Click(object sender, EventArgs e)
        {
            isPinned = !isPinned;
            if (isPinned)
            {
                new List<Control> { HeaderPanel, NotificationLogo, CloseButton, PinButton, NotificationText }.ForEach(x =>
                {
                    x.MouseLeave -= Notification_MouseLeave;
                });

                Opacity = _hoveredOpacity;
                PinButton.Image = Resources.unpin_20px;
                _backgroundWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
            }
            else
            {
                PinButton.Image = Resources.pin_20px;
                Opacity = _regularOpacity;
                new List<Control> { HeaderPanel, NotificationLogo, CloseButton, PinButton, NotificationText }.ForEach(x =>
                {
                    x.MouseLeave += Notification_MouseLeave;
                });
                _action = NotificationAction.Active;
                _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                if(!_backgroundWorker.IsBusy)
                    _backgroundWorker.RunWorkerAsync();
            }
        }
        private void CloseButton_Click(object sender, EventArgs e) => Close();
        private void Notification_MouseLeave(object sender, EventArgs e) 
        {
            try
            {
                Opacity = _regularOpacity;
            }
            catch (Exception)
            {
            }
        } 
        private void Notification_MouseEnter(object sender, EventArgs e) => Opacity = _hoveredOpacity;
    }
}
