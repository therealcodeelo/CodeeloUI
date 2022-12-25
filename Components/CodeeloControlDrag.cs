using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloControlDrag), "Icons.CodeeloControlDrag.bmp")]
    public partial class CodeeloControlDrag : Component
    {
        private Control _sourceControl;
        private bool _clicked;
        private Point _clickedPoint;
        public Control SourceControl
        {
            get => _sourceControl;
            set
            {
                if (_sourceControl == value)
                    return;

                if (_sourceControl != null)
                {
                    _sourceControl.MouseDown -= SourceControl_MouseDown;
                    _sourceControl.MouseUp -= SourceControl_MouseUp;
                    _sourceControl.MouseMove -= SourceControl_MouseMove;
                }

                _sourceControl = value;

                if (_sourceControl == null)
                    return;

                _sourceControl.MouseDown += SourceControl_MouseDown;
                _sourceControl.MouseUp += SourceControl_MouseUp;
                _sourceControl.MouseMove += SourceControl_MouseMove;
            }
        }

        private void SourceControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_clicked)
            {
                var p = new Point();
                p.X = e.X + SourceControl.Left;
                p.Y = e.Y + SourceControl.Top;
                SourceControl.Left = p.X - _clickedPoint.X;
                SourceControl.Top = p.Y - _clickedPoint.Y;
            }
        }
        private void SourceControl_MouseUp(object sender, MouseEventArgs e) => _clicked = false;

        private void SourceControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _clickedPoint = new Point(e.X, e.Y);
                SourceControl.Location = _clickedPoint;
                _clicked = true;
            }
        }

        public CodeeloControlDrag()
        {
            InitializeComponent();
        }

        public CodeeloControlDrag(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

    }
}
