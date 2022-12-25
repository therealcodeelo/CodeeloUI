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

    public class CodeeloComboBox : Control
    {
        #region [ Поля класса ]
        private Color _backColor = Color.WhiteSmoke;
        private Color _iconColor = Color.MediumSlateBlue;
        private Color _listBackColor = Color.FromArgb(230, 228, 245);
        private Color _listForeColor = Color.DimGray;
        private Color _borderColor = Color.MediumSlateBlue;
        private int _borderSize = 1;
        private readonly ComboBox _comboBox;
        private readonly Button _button;
        private Label _labelText;

        public event EventHandler OnSelectedIndexChanged;
        public new event KeyPressEventHandler OnKeyPress;

        private System.Windows.Forms.Timer _caretTimer;
        private bool _selected;
        private bool _ticked;
        #endregion

        #region [ Свойства класса ]
        public Color FillColor
        {
            get => _backColor;
            set
            {
                _labelText.BackColor = _button.BackColor = _backColor = value;
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
            set => _comboBox.BackColor = _listBackColor = value;
        }

        public Color ListTextColor
        {
            get => _listForeColor;
            set => _comboBox.ForeColor = _listForeColor = value;
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
            set
            {
                _comboBox.ForeColor = base.ForeColor = value;
                Invalidate();
            }
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                _labelText.Font = _comboBox.Font = base.Font = value;
                Invalidate();
            }
        }

        public override string Text
        {
            get => _comboBox.Text;
            set => _comboBox.Text = value;
        }

        public ComboBoxStyle DropDownStyle
        {
            get => _comboBox.DropDownStyle;
            set
            {
                if (_comboBox.DropDownStyle != ComboBoxStyle.Simple)
                    _comboBox.DropDownStyle = value;
            }
        }

        public int DropDownWidth
        {
            get => _comboBox.DropDownWidth;
            set => _comboBox.DropDownWidth = value;
        }

        [Category("Привязка данных")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
       // [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        public ComboBox.ObjectCollection Items => _comboBox.Items;

        [Category("Привязка данных"), DefaultValue(null)]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get => _comboBox.DataSource;
            set => _comboBox.DataSource = value;
        }
        [Category("Привязка данных")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
       // [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get => _comboBox.AutoCompleteCustomSource;
            set => _comboBox.AutoCompleteCustomSource = value;
        }
        [Category("Привязка данных"), DefaultValue(AutoCompleteSource.None)]
        public AutoCompleteSource AutoCompleteSource
        {
            get => _comboBox.AutoCompleteSource;
            set => _comboBox.AutoCompleteSource = value;
        }
        [Category("Привязка данных"), DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode AutoCompleteMode
        {
            get => _comboBox.AutoCompleteMode;
            set => _comboBox.AutoCompleteMode = value;
        }
        [Category("Привязка данных"), DefaultValue("")]
       // [Editor("System.Windows.Forms.Design.DataMemberFieldEditor", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter")]
        public string DisplayMember
        {
            get => _comboBox.DisplayMember;
            set => _comboBox.DisplayMember = value;
        }
        [Category("Привязка данных"), DefaultValue("")]
      //  [Editor("System.Windows.Forms.Design.DataMemberFieldEditor", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter")]
        public string ValueMember
        {
            get => _comboBox.ValueMember;
            set => _comboBox.ValueMember = value;
        }

        [Browsable(false), Bindable(true)]
        public object SelectedItem
        {
            get => _comboBox.SelectedItem;
            set => _comboBox.SelectedItem = value;
        }
        [Browsable(false)]
        public int SelectedIndex
        {
            get => _comboBox.SelectedIndex;
            set => _comboBox.SelectedIndex = value;
        }
        [Browsable(false), Bindable(true)]
        public object SelectedValue
        {
            get => _comboBox.SelectedValue;
            set => _comboBox.SelectedValue = value;
        }
        [Browsable(false)]
        public string SelectedText
        {
            get => _comboBox.SelectedText;
            set => _comboBox.SelectedText = value;
        }
        #endregion
        #region [ Скрытые свойства класса ]
        [Browsable(false)]
        public override Color BackColor { get; set; }
        #endregion
        public CodeeloComboBox()
        {
            DoubleBuffered = true;
            _comboBox = new ComboBox();
            _labelText = new Label();
            _button = new Button();
            Size = new Size(200, 30);
            SuspendLayout();

            _comboBox.BackColor = _listBackColor;
            _comboBox.Font = Font;
            _comboBox.ForeColor = _listForeColor;
            _comboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            _comboBox.TextChanged += new EventHandler(ComboBox_TextChanged);

            _button.Dock = DockStyle.Right;
            _button.FlatStyle = FlatStyle.Flat;
            _button.FlatAppearance.BorderSize = 0;
            _button.BackColor = _backColor;
            _button.Size = new Size(30, 30);
            _button.Cursor = Cursors.Hand;
            _button.Click += new EventHandler(Icon_Click);
            _button.Paint += new PaintEventHandler(Icon_Paint);

            _labelText.Dock = DockStyle.Fill;
            _labelText.AutoSize = false;
            _labelText.BackColor = _backColor;
            _labelText.TextAlign = ContentAlignment.MiddleLeft;
            _labelText.Padding = new Padding(8, 0, 0, 0);
            _labelText.Font = Font;
            _labelText.Click += new EventHandler(Surface_Click);
            _labelText.MouseEnter += new EventHandler(Surface_MouseEnter);
            _labelText.MouseLeave += new EventHandler(Surface_MouseLeave);
            _labelText.MouseEnter += LabelText_MouseEnter;
            _labelText.MouseLeave += LabelText_MouseLeave;

            Controls.Add(_labelText);
            Controls.Add(_button);
            Controls.Add(_comboBox);


            ForeColor = Color.DimGray;
            Padding = new Padding(_borderSize);
            Font = new Font(Font.Name, 10F);
            base.BackColor = _borderColor;
            ResumeLayout();
            AdjustComboBoxDimensions();
        }
        #region [ Методы ]
        private void ShowCaret()
        {
            _selected = true;
            if (_caretTimer == null)
            {
                _caretTimer = new System.Windows.Forms.Timer();
                _caretTimer.Interval = 400;
                _caretTimer.Tick += Timer_Tick;
                _caretTimer.Start();
            }
        }
        private void AdjustComboBoxDimensions()
        {
            _comboBox.Width = _labelText.Width;
            _comboBox.Location = new Point()
            {
                X = Padding.Right,
                Y = _labelText.Bottom - _comboBox.Height
            };
        }
        #endregion

        #region [ События ]
        private void LabelText_MouseLeave(object sender, EventArgs e)
        {
            _selected = false;
            _labelText.Text = _labelText.Text.Split('|')[0];
        }


        private void LabelText_MouseEnter(object sender, EventArgs e) => ShowCaret();
        private void Timer_Tick(object sender, EventArgs e)
        {
            _ticked = !_ticked;
            if (_selected)
            {
                if (_ticked)
                    _labelText.Text += '|';
                else
                    _labelText.Text = _labelText.Text.Split('|')[0];
            }
        }
        private void ComboBox_TextChanged(object sender, EventArgs e) => _labelText.Text = _comboBox.Text;

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
                    if (_comboBox.DataSource == null)
                    {
                        e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                               tBrush, e.Bounds, StringFormat.GenericDefault);
                    }
                }
            }
            e.DrawFocusRectangle();
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
            _labelText.Focus();
            _comboBox.Select();

            if (_comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                _comboBox.DroppedDown = true;
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
            _comboBox.Select();
            _comboBox.DroppedDown = true;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged.Invoke(sender, e);
            _labelText.Text = _comboBox.Text;
        }
        #endregion
    }
}
