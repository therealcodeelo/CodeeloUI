using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CodeeloUI
{
    
    internal class CodeeloBorder : IDisposable
    {
        #region [ Поля класса ]
        private float[] _compoundArray;
        private readonly Pen _pen;
        private CodeeloBorderDirection _direction;
        #endregion

        #region [ Свойства класса ]
        private float[] CompoundArray
        {
            get
            {
                if (_compoundArray == null)
                {
                    _compoundArray = new float[2];
                }
                switch (_direction)
                {
                    case CodeeloBorderDirection.Middle: goto default;
                    case CodeeloBorderDirection.Inner: _compoundArray[0] = 0.5f; _compoundArray[1] = 1f; break;
                    case CodeeloBorderDirection.Outer: _compoundArray[0] = 0f; _compoundArray[1] = 0.5f; break;
                    default: _compoundArray[0] = 0.25f; _compoundArray[1] = 0.75f; break;
                }
                return _compoundArray;
            }
        }
        public Pen Pen => _pen;
        public int Width
        {
            get => (int)Pen.Width / 2;
            set => Pen.Width = value * 2;
        }
        public Color Color
        {
            get => Pen.Color;
            set => Pen.Color = value;
        }
        public CodeeloBorderDirection Direction
        {
            get => _direction;
            set
            {
                if (_direction == value)
                    return;
                _direction = value;
                Pen.CompoundArray = CompoundArray;
            }
        }
        public bool Behind { get; set; }
        #endregion

        public CodeeloBorder(Color color) : this(new Pen(color), false) { }
        public CodeeloBorder(Pen pen) : this(pen, true) { }

        #region [ Методы ]
        public Rectangle GetBounds(Rectangle rectangle)
        {
            if (!IsValid() || _direction == CodeeloBorderDirection.Inner)
            {
                return rectangle;
            }
            var inflate = _direction == CodeeloBorderDirection.Middle
                ? (int)Math.Ceiling(Width / 2d)
                : Width;
            rectangle.Inflate(inflate, inflate);
            return rectangle;
        }
        protected CodeeloBorder(Pen pen, bool useCopy)
        {
            _pen = useCopy ? (Pen)pen.Clone() : pen;
            _pen.Alignment = PenAlignment.Center;
            _pen.Width = Convert.ToInt32(_pen.Width * 2);
            _pen.CompoundArray = CompoundArray;
        }
        public bool IsValid() => Width > 0 && (Pen.PenType != PenType.SolidColor || Color.A > 0);
        public static bool IsValid(CodeeloBorder border) => border != null && border.IsValid();
        public void Dispose() => Pen?.Dispose();
        #endregion

    }
}
