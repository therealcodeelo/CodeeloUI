﻿using CodeeloUI.Enums;
using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects.Transform
{
    public class VerticalFoldEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Width;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {
            var center = new Point((control.Left + control.Right) / 2, control.Top);

            var size = new Size(newValue, control.Height);
            var location = new Point(center.X - (newValue / 2), control.Top);

            control.Bounds = new Rectangle(location, size);
        }

        public int GetMinimumValue(Control control)
            => control.MinimumSize.IsEmpty ? int.MinValue : control.MinimumSize.Width;

        public int GetMaximumValue(Control control)
            => control.MaximumSize.IsEmpty ? int.MaxValue : control.MaximumSize.Width;

        public EffectInteractions Interaction => EffectInteractions.BOUNDS;
    }
}
