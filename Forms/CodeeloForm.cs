using CodeeloUI.Enums;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CodeeloUI.SupportClasses;

namespace CodeeloUI.Forms
{
    public partial class CodeeloForm : Form
    {
        #region [ Поля класса ]
        private Color _lightColor = Color.FromArgb(233, 233, 234);
        private Color _darkColor = Color.FromArgb(22, 22, 21);
        private DwmSystemBackdropTypeFlgs _backdropFlag;
        #endregion

        #region [ Свойства класса ]
        public Color LightColor
        {
            get => _lightColor;
            set
            {
                _lightColor = value;
                Invalidate();
            }
        }
        public Color DarkColor
        {
            get => _darkColor;
            set
            {
                _darkColor = value;
                Invalidate();
            }
        }
        public DwmSystemBackdropTypeFlgs BackdropFlag
        {
            get => _backdropFlag;
            set
            {
                _backdropFlag = value;
                Invalidate();
            }
        }
        #endregion


        public CodeeloForm()
        {
            InitializeComponent();
            BackdropFlag = DwmSystemBackdropTypeFlgs.DWMSBT_TRANSIENTWINDOW;
            Apply_Backdrop_Effect(Handle, BackdropFlag);
            Apply_Transparent_Form(Handle, true);
            Apply_Light_Theme(Handle, true);
        }
        

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        private void Apply_Backdrop_Effect(IntPtr HWnd, DwmSystemBackdropTypeFlgs BackdropFlag = DwmSystemBackdropTypeFlgs.DWMSBT_MAINWINDOW)
        {
            int key = (int)BackdropFlag;
            GraphicsUtils.DwmSetWindowAttribute(HWnd, DwmSetWindowAttributeFlags.DWM_SYSTEMBACKDROP_TYPE, ref key, Marshal.SizeOf(typeof(int)));
        }
        private void Apply_Transparent_Form(IntPtr HWnd, bool Dark = false)
        {
            Form form = (Form)Control.FromHandle(HWnd);
            form.ForeColor = Dark ? LightColor : DarkColor;
            form.TransparencyKey = form.BackColor = Dark ? DarkColor : LightColor;
        }
        private void Apply_Light_Theme(IntPtr HWnd, bool Dark = false)
        {
            int key = Dark ? 1 : 0;
            GraphicsUtils.DwmSetWindowAttribute(HWnd, DwmSetWindowAttributeFlags.DWM_USE_IMMERSIVE_DARK_MODE, ref key, Marshal.SizeOf(typeof(int)));
        }
    }
}

