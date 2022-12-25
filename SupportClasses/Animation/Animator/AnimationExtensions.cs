using CodeeloUI.SupportClasses.Animation.Effects;
using System;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Animator
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
        public static AnimationStatus Animate(
            this Control control,
            IEffect iAnimation,
            EasingDelegate easing,
            int valueToReach,
            int duration,
            int delay,
            Action doSomething,
            bool reverse = false,
            int loops = 1)
            => Animator.Animate(control, iAnimation, easing, valueToReach, duration, delay, doSomething, reverse, loops);
    }
}
