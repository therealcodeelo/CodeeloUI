using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    partial class CodeeloTextBox
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox = new TextBox();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.BorderStyle = BorderStyle.None;
            this.textBox.Dock = DockStyle.Fill;
            this.textBox.Location = new Point(7, 7);
            this.textBox.Name = "textBox";
            this.textBox.Size = new Size(236, 16);
            this.textBox.TabIndex = 0;
            this.textBox.Click += new EventHandler(this.textBox_Click);
            this.textBox.TextChanged += new EventHandler(this.textBox_TextChanged);
            this.textBox.Enter += new EventHandler(this.textBox_Enter);
            this.textBox.KeyPress += new KeyPressEventHandler(this.textBox_KeyPress);
            this.textBox.Leave += new EventHandler(this.textBox_Leave);
            this.textBox.MouseEnter += new EventHandler(this.textBox_MouseEnter);
            this.textBox.MouseLeave += new EventHandler(this.textBox_MouseLeave);
            // 
            // CodeeloTextBox
            // 
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.White;
            this.Controls.Add(this.textBox);
            this.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new Padding(4);
            this.Name = "CodeeloTextBox";
            this.Padding = new Padding(7);
            this.Size = new Size(250, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox;
    }
}
