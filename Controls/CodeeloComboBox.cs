using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Controls
{
    [ToolboxBitmap(typeof(ComboBox))]
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

        public event EventHandler OnSelectedIndexChanged;
        public new event KeyPressEventHandler OnKeyPress;
        #endregion

        #region [ Свойства класса ]
        public Color FillColor
        {
            get => _backColor;
            set
            {
                _combobox.BackColor = _button.BackColor = _backColor = value;
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
            set => BackColor = _borderColor = value;
        }

        public int BorderSize
        {
            get => _borderSize;
            set
            {
                _borderSize = value;
                Padding = new Padding(_borderSize);
                AdjustComboBoxDimensions();
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

        #endregion
        public CodeeloComboBox()
        {
            DoubleBuffered = true;
            _combobox = new ComboBox();
            _button = new Button();
            SuspendLayout();
            _combobox.BackColor = _listBackColor;

            _combobox.DrawMode = DrawMode.Normal;
            _combobox.ItemHeight = 22;
            _combobox.FlatStyle = FlatStyle.Flat;
            _combobox.Dock = DockStyle.Left;
            _combobox.Font = new Font(Font.Name, 10F);
            _combobox.ForeColor = _listForeColor;


            _combobox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            _combobox.Click += Surface_Click;
            _combobox.MouseEnter += Surface_MouseEnter;
            _combobox.MouseLeave += Surface_MouseLeave;
            _combobox.DrawItem += DrawItemEventHandler;
            _combobox.KeyPress += KeyPressEventHandler;

            _button.Dock = DockStyle.Right;
            _button.FlatStyle = FlatStyle.Flat;
            _button.FlatAppearance.BorderSize = 0;
            _button.BackColor = _backColor;
            _button.TabStop = false;
            _button.Size = new Size(28, 26);
            _button.Cursor = Cursors.Hand;
            _button.Click += Icon_Click;
            _button.Paint += Icon_Paint;

            Controls.Add(_button);
            Controls.Add(_combobox);
            MinimumSize = new Size(200, 30);
            Size = new Size(200, 30);
            ForeColor = Color.DimGray;
            Padding = new Padding(_borderSize);
            Font = new Font(Font.Name, 10F);
            BackColor = _borderColor;
            ResumeLayout();
            AdjustComboBoxDimensions();
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
                Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ?
                               new SolidBrush(Color.FromArgb(255, 68, 71)) : new SolidBrush(e.BackColor);
                Brush tBrush = new SolidBrush(e.ForeColor);

                g.FillRectangle(brush, e.Bounds);
                if (_combobox.DataSource == null)
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                           tBrush, e.Bounds, StringFormat.GenericDefault);
                }

                brush.Dispose();
                tBrush.Dispose();
            }
            e.DrawFocusRectangle();
        }

        private void AdjustComboBoxDimensions()
        {
            _combobox.Width = Width - 2;
            _button.Location = new Point(Width - 30, 2);
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
            _combobox.Select();

            if (_combobox.DropDownStyle == ComboBoxStyle.DropDownList)
                _combobox.DroppedDown = true;
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
        }
    }
}
