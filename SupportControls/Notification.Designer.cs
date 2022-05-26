using System.ComponentModel;
using System.Windows.Forms;

namespace CodeeloUI.SupportControls
{
    partial class Notification
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.PinButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.NotificationText = new System.Windows.Forms.Label();
            this.NotificationLogo = new System.Windows.Forms.PictureBox();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NotificationLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Controls.Add(this.PinButton);
            this.HeaderPanel.Controls.Add(this.CloseButton);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(100, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(400, 32);
            this.HeaderPanel.TabIndex = 1;
            // 
            // PinButton
            // 
            this.PinButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.PinButton.FlatAppearance.BorderSize = 0;
            this.PinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PinButton.Image = global::CodeeloUI.Properties.Resources.pin_20px;
            this.PinButton.Location = new System.Drawing.Point(335, 0);
            this.PinButton.Name = "PinButton";
            this.PinButton.Size = new System.Drawing.Size(27, 32);
            this.PinButton.TabIndex = 1;
            this.PinButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Image = global::CodeeloUI.Properties.Resources.cancel_24px;
            this.CloseButton.Location = new System.Drawing.Point(362, 0);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(38, 32);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // NotificationText
            // 
            this.NotificationText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotificationText.Font = new System.Drawing.Font("Bahnschrift", 9.75F);
            this.NotificationText.ForeColor = System.Drawing.Color.White;
            this.NotificationText.Location = new System.Drawing.Point(100, 32);
            this.NotificationText.Name = "NotificationText";
            this.NotificationText.Size = new System.Drawing.Size(400, 68);
            this.NotificationText.TabIndex = 2;
            this.NotificationText.Text = "label1";
            // 
            // NotificationLogo
            // 
            this.NotificationLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.NotificationLogo.Location = new System.Drawing.Point(0, 0);
            this.NotificationLogo.Name = "NotificationLogo";
            this.NotificationLogo.Size = new System.Drawing.Size(100, 100);
            this.NotificationLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.NotificationLogo.TabIndex = 0;
            this.NotificationLogo.TabStop = false;
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(500, 100);
            this.Controls.Add(this.NotificationText);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.NotificationLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Notification";
            this.Text = "Notification";
            this.TopMost = true;
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NotificationLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox NotificationLogo;
        private Panel HeaderPanel;
        private Button PinButton;
        private Button CloseButton;
        private Label NotificationText;
    }
}