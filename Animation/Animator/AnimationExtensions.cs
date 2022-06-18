using System.Windows.Forms;
using CodeeloUI.Animation.Effects;

namespace CodeeloUI.Animation.Animator
{
    public static class AnimationExtensions
    {
        public static AnimationStatus Animate(
            this Control control, 
            IEffect iAnimation,
            EasingDelegate easing, 
            int valueToReach, 
            int duration, 
            int delay, 
            bool reverse = false, 
            int loops = 1) 
            => Animator.Animate(control, iAnimation, easing, valueToReach, duration, delay, reverse, loops);
    }
}
