using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(CodeeloComboBox), "Icons.CodeeloComboBox.bmp")]
    [ToolboxItem(true)]
    [Description("Элемент отображения выпадающих списков")]
    public sealed class CodeeloComboBox : UserControl
    {
        #region [ Поля класса ]
        private Color _backColor = Color.WhiteSmoke;
        private Color _iconColor = Color.MediumSlateBlue;
        private Color _listBackColor = Color.FromArgb(230, 228, 245);
        private Color _listForeColor = Color.DimGray;
        private Color _borderColor = Color.MediumSlateBlue;
        private int _borderSize = 1;
        private readonly ComboBox _combobox;
        private readonly Button _button;
        private Label lblText;

        public event EventHandler OnSelectedIndexChanged;
        public new event KeyPressEventHandler OnKeyPress;


        private Timer _caretTimer;
        #endregion

        #region [ Свойства класса ]
        public Color FillColor
        {
            get => _backColor;
            set
            {
                lblText.BackColor = _button.BackColor = _backColor = value;
                Invalidate();
            }
        }
  
        public Color ArrowColor
        {
            get => _iconColor;
            set
            {
                _iconColor = value;
                _button.Invalidate();
            }
        }
        public Color ListBackColor
        {
            get => _listBackColor;
            set => _combobox.BackColor = _listBackColor = value;
        }

        public Color ListTextColor
        {
            get => _listForeColor;
            set => _combobox.ForeColor = _listForeColor = value;
        }
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                BackColor = _borderColor = value;
                Invalidate();
            }
        }

        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Padding = new Padding(_borderSize);
                AdjustComboBoxDimensions();
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor; 
            set => _combobox.ForeColor = base.ForeColor = value;
        }

        public override Font Font
        {
            get => base.Font;
            set => _combobox.Font = base.Font = value;
        }

        public override string Text
        {
            get => _combobox.Text; 
            set => _combobox.Text = value; 
        }
  
        public ComboBoxStyle DropDownStyle
        {
            get => _combobox.DropDownStyle; 
            set
            {
                if (_combobox.DropDownStyle != ComboBoxStyle.Simple)
                    _combobox.DropDownStyle = value;
            }
        }

        public int DropDownWidth
        { 
            get => _combobox.DropDownWidth;
            set => _combobox.DropDownWidth = value;
        }

        [Category("Привязка данных")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        public ComboBox.ObjectCollection Items => _combobox.Items;

        [Category("Привязка данных"), DefaultValue(null)]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get => _combobox.DataSource;
            set => _combobox.DataSource = value;
        }
        [Category("Привязка данных")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get => _combobox.AutoCompleteCustomSource;
            set => _combobox.AutoCompleteCustomSource = value;
        }
        [Category("Привязка данных"), DefaultValue(AutoCompleteSource.None)]
        public AutoCompleteSource AutoCompleteSource
        {
            get => _combobox.AutoCompleteSource;
            set => _combobox.AutoCompleteSource = value;
        }
        [Category("Привязка данных"), DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode AutoCompleteMode
        {
            get => _combobox.AutoCompleteMode; 
            set => _combobox.AutoCompleteMode = value;
        }
        [Category("Привязка данных"), DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter")]
        public string DisplayMember
        {
            get => _combobox.DisplayMember;
            set => _combobox.DisplayMember = value;
        }
        [Category("Привязка данных"), DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter")]
        public string ValueMember
        {
            get => _combobox.ValueMember;
            set => _combobox.ValueMember = value;
        }

        [Browsable(false), Bindable(true)]
        public object SelectedItem
        {
            get => _combobox.SelectedItem;
            set => _combobox.SelectedItem = value;
        }
        [Browsable(false)]
        public int SelectedIndex
        {
            get => _combobox.SelectedIndex;
            set => _combobox.SelectedIndex = value;
        }
        [Browsable(false), Bindable(true)]
        public object SelectedValue
        {
            get => _combobox.SelectedValue;
            set => _combobox.SelectedValue = value;
        }
        [Browsable(false)]
        public string SelectedText
        {
            get => _combobox.SelectedText;
            set => _combobox.SelectedText = value;
        }
        #endregion
        #region [ Скрытые свойства класса ]
        [Browsable(false)]
        public override Color BackColor { get; set; }
        #endregion
        public CodeeloComboBox()
        {
            DoubleBuffered = true;
            _combobox = new ComboBox();
            lblText = new Label();
            _button = new Button();
            SuspendLayout();

            //ComboBox: Dropdown list
            _combobox.BackColor = _listBackColor;
            _combobox.Font = new Font(this.Font.Name, 10F);
            _combobox.ForeColor = _listForeColor;
            _combobox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);//Default event
            _combobox.TextChanged += new EventHandler(ComboBox_TextChanged);//Refresh text
                                                                          //Button: Icon
            _button.Dock = DockStyle.Right;
            _button.FlatStyle = FlatStyle.Flat;
            _button.FlatAppearance.BorderSize = 0;
            _button.BackColor = _backColor;
            _button.Size = new Size(30, 30);
            _button.Cursor = Cursors.Hand;
            _button.Click += new EventHandler(Icon_Click);//Open dropdown list
            _button.Paint += new PaintEventHandler(Icon_Paint);//Draw icon
                                                               //Label: Text
            lblText.Dock = DockStyle.Fill;
            lblText.AutoSize = false;
            lblText.BackColor = _backColor;
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            lblText.Padding = new Padding(8, 0, 0, 0);
            lblText.Font = new Font(this.Font.Name, 10F);
            //->Attach label events to user control event
            lblText.Click += new EventHandler(Surface_Click);//Select combo box
            lblText.MouseEnter += new EventHandler(Surface_MouseEnter);
            lblText.MouseLeave += new EventHandler(Surface_MouseLeave);
            lblText.GotFocus += LblText_GotFocus;
            lblText.LostFocus += LblText_LostFocus;

            //User Control
            this.Controls.Add(lblText);//2
            this.Controls.Add(_button);//1
            this.Controls.Add(_combobox);//0

         
            this.ForeColor = Color.DimGray;
            this.Padding = new Padding(_borderSize);//Border Size
            this.Font = new Font(this.Font.Name, 10F);
            base.BackColor = _borderColor; //Border Color
            this.ResumeLayout();
            AdjustComboBoxDimensions();
        }


        private void LblText_GotFocus(object sender, EventArgs e)
        {
            ShowCaret();
        }

        private void LblText_LostFocus(object sender, EventArgs e)
        {
            //MessageBox.Show("Test");
            selected = false;
            //_caretTimer.Stop();
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            lblText.Text = _combobox.Text;
        }

        private void KeyPressEventHandler(object sender, KeyPressEventArgs e)
        {
            if (OnKeyPress != null)
                OnKeyPress.Invoke(sender, e);
        }

        private void DrawItemEventHandler(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                System.Drawing.Graphics g = e.Graphics;
                using (Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ? new SolidBrush(Color.FromArgb(255, 68, 71)) : new SolidBrush(e.BackColor))
                using (Brush tBrush = new SolidBrush(e.ForeColor))
                {
                    g.FillRectangle(brush, e.Bounds);
                    if (_combobox.DataSource == null)
                    {
                        e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                               tBrush, e.Bounds, StringFormat.GenericDefault);
                    }
                }
            }
            e.DrawFocusRectangle();
        }

        private void AdjustComboBoxDimensions()
        {
            _combobox.Width = lblText.Width;
            _combobox.Location = new Point()
            {
                X = this.Width - this.Padding.Right - _combobox.Width,
                Y = lblText.Bottom - _combobox.Height
            };
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustComboBoxDimensions();
        }
        private void Surface_MouseLeave(object sender, EventArgs e) => OnMouseLeave(e);

        private void Surface_MouseEnter(object sender, EventArgs e) => OnMouseEnter(e);

        private void Surface_Click(object sender, EventArgs e)
        {
            OnClick(e);
            lblText.Focus();
            _combobox.Select();

            if (_combobox.DropDownStyle == ComboBoxStyle.DropDownList)
                _combobox.DroppedDown = true;
        }
        private void ShowCaret()
        {
            selected = true;
            if(_caretTimer == null)
            {
                _caretTimer = new Timer();
                _caretTimer.Interval = 700;
                _caretTimer.Tick += Timer_Tick;
                _caretTimer.Start();
            }
        }
        private bool selected;
        private bool ticked;
        private void Timer_Tick(object sender, EventArgs e)
        {
            ticked = !ticked;
            if (selected)
            {
                if(ticked)
                    lblText.Text += '✟';
                else
                    lblText.Text = lblText.Text.Split('✟')[0];
            } 
        }

        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            int iconWidth = 14;
            int iconHeight = 6;
            var rectIcon = new Rectangle((_button.Width - iconWidth) / 2, (_button.Height - iconHeight) / 2, iconWidth, iconHeight);
            System.Drawing.Graphics graph = e.Graphics;

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(_iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidth / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidth / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);
            }
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            _combobox.Select();
            _combobox.DroppedDown = true;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged.Invoke(sender, e);
            //Refresh text
            lblText.Text = _combobox.Text;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CodeeloComboBox
            // 
            this.MinimumSize = new System.Drawing.Size(200, 30);
            this.Name = "CodeeloComboBox";
            this.Size = new System.Drawing.Size(200, 30);
            this.ResumeLayout(false);

        }
    }
}
