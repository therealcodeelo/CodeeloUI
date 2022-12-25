using CodeeloUI.Enums;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloAnimateForm), @"Icons.CodeeloAnimateForm.bmp")]
    public partial class CodeeloAnimateForm : Component
    {
        #region [ Поля класса ]
        private Form _sourceForm;
        private FormAnimateEffects _closeEffect = FormAnimateEffects.ПОЯВЛЕНИЕ_ЗАТУХАНИЕ;
        private FormAnimateEffects _activateEffect = FormAnimateEffects.РАСКРЫТИЕ_СКРЫТИЕ;
        #endregion

        #region [ Свойства класса ]
        public Control SourceForm
        {
            get => _sourceForm;
            set
            {
                if (value is Form)
                    _sourceForm = (value as Form);
            }
        }
        public FormAnimateEffects ActivateEffect
        {
            get => _activateEffect;
            set => _activateEffect = value;
        }
        public FormAnimateEffects CloseEffect
        {
            get => _closeEffect;
            set => _closeEffect = value;
        }
        #endregion

        public CodeeloAnimateForm()
        {
            InitializeComponent();
        }

        public CodeeloAnimateForm(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region [ Методы ]

        [DllImport("user32")]
        static extern bool AnimateWindow(IntPtr hwnd, int time, uint flags);
        public void ShowForm(int millisecond)
        {
            AnimateWindow(_sourceForm.Handle, millisecond, (uint)_activateEffect | 0x00020000);
            _sourceForm.Show();
        }
        public void ShowForm(Form source, int millisecond)
        {
            AnimateWindow(source.Handle, millisecond, (uint)_activateEffect | 0x00020000);
            source.Show();
        }
        public void ShowForm(Form source, int millisecond, FormAnimateEffects activateEffect)
        {
            AnimateWindow(source.Handle, millisecond, (uint)activateEffect | 0x00020000);
            source.Show();
        }
        public void CloseForm(int millisecond)
        {
            AnimateWindow(_sourceForm.Handle, millisecond, (uint)_closeEffect | 0x00010000);
            _sourceForm.Close();
        }
        public void CloseForm(Form source, int millisecond)
        {
            AnimateWindow(source.Handle, millisecond, (uint)_closeEffect | 0x00010000);
            source.Close();
        }
        public void CloseForm(Form source, int millisecond, FormAnimateEffects closeEffect)
        {
            AnimateWindow(source.Handle, millisecond, (uint)closeEffect | 0x00010000);
            source.Close();
        }
        #endregion
    }
}
