using CodeeloUI.SupportClasses.Animation.Animator;
using CodeeloUI.SupportClasses.Animation.Effects.Color;
using CodeeloUI.SupportClasses.Animation.Effects.Opacity;
using CodeeloUI.SupportClasses.Animation.Effects.Transform;
using CodeeloUI.SupportClasses.Animation.Effects.Font;
using CodeeloUI.SupportClasses.Animation.Effects;
using CodeeloUI.Enums;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace CodeeloUI.Components
{
    [ToolboxBitmap(typeof(CodeeloAnimator), "Icons.CodeeloAnimator.bmp")]
    public partial class CodeeloAnimator : Component
    {
        #region [ Поля класса ]
        private List<AnimationStatus> _cancellationTokens;
        private EasingDelegate _easingFunction;
        private EasingFunction _selectedEasingFunction;
        private IEffect _animationEffect;
        #endregion

        #region [ Свойства класса ]
        public AnimationEffect AnimationEffect { get; set; } = AnimationEffect.XLocation;
        public ColorChannelShiftEffect.ColorChannels ColorChannel { get; set; } = ColorChannelShiftEffect.ColorChannels.R;
        public Control SourceControl { get; set; }
        public EasingFunction EasingFunction
        {
            get => _selectedEasingFunction;
            set
            {
                switch (value)
                {
                    case EasingFunction.SineIn:
                        _easingFunction = EasingFunctions.SineEaseIn;
                        break;
                    case EasingFunction.SineOut:
                        _easingFunction = EasingFunctions.SineEaseOut;
                        break;
                    case EasingFunction.SineInOut:
                        _easingFunction = EasingFunctions.SineEaseInOut;
                        break;
                    case EasingFunction.SineOutIn:
                        _easingFunction = EasingFunctions.SineEaseOutIn;
                        break;
                    case EasingFunction.CubicIn:
                        _easingFunction = EasingFunctions.CubicEaseIn;
                        break;
                    case EasingFunction.CubicOut:
                        _easingFunction = EasingFunctions.CubicEaseOut;
                        break;
                    case EasingFunction.CubicInOut:
                        _easingFunction = EasingFunctions.CubicEaseInOut;
                        break;
                    case EasingFunction.CubicOutIn:
                        _easingFunction = EasingFunctions.CubicEaseOutIn;
                        break;
                    case EasingFunction.QuintIn:
                        _easingFunction = EasingFunctions.QuintEaseIn;
                        break;
                    case EasingFunction.QuintOut:
                        _easingFunction = EasingFunctions.QuintEaseOut;
                        break;
                    case EasingFunction.QuintInOut:
                        _easingFunction = EasingFunctions.QuintEaseInOut;
                        break;
                    case EasingFunction.QuintOutIn:
                        _easingFunction = EasingFunctions.QuintEaseOutIn;
                        break;
                    case EasingFunction.CircIn:
                        _easingFunction = EasingFunctions.CircEaseIn;
                        break;
                    case EasingFunction.CircOut:
                        _easingFunction = EasingFunctions.CircEaseOut;
                        break;
                    case EasingFunction.CircInOut:
                        _easingFunction = EasingFunctions.CircEaseInOut;
                        break;
                    case EasingFunction.CircOutIn:
                        _easingFunction = EasingFunctions.CircEaseOutIn;
                        break;
                    case EasingFunction.ElasticIn:
                        _easingFunction = EasingFunctions.ElasticEaseIn;
                        break;
                    case EasingFunction.ElasticOut:
                        _easingFunction = EasingFunctions.ElasticEaseOut;
                        break;
                    case EasingFunction.ElasticInOut:
                        _easingFunction = EasingFunctions.ElasticEaseInOut;
                        break;
                    case EasingFunction.ElasticOutIn:
                        _easingFunction = EasingFunctions.ElasticEaseOutIn;
                        break;
                    case EasingFunction.QuadIn:
                        _easingFunction = EasingFunctions.QuadEaseIn;
                        break;
                    case EasingFunction.QuadOut:
                        _easingFunction = EasingFunctions.QuadEaseOut;
                        break;
                    case EasingFunction.QuadInOut:
                        _easingFunction = EasingFunctions.QuadEaseInOut;
                        break;
                    case EasingFunction.QuadOutIn:
                        _easingFunction = EasingFunctions.QuadEaseOutIn;
                        break;
                    case EasingFunction.QuartIn:
                        _easingFunction = EasingFunctions.QuartEaseIn;
                        break;
                    case EasingFunction.QuartOut:
                        _easingFunction = EasingFunctions.QuartEaseOut;
                        break;
                    case EasingFunction.QuartInOut:
                        _easingFunction = EasingFunctions.QuartEaseInOut;
                        break;
                    case EasingFunction.QuartOutIn:
                        _easingFunction = EasingFunctions.QuartEaseOutIn;
                        break;
                    case EasingFunction.ExpoIn:
                        _easingFunction = EasingFunctions.ExpoEaseIn;
                        break;
                    case EasingFunction.ExpoOut:
                        _easingFunction = EasingFunctions.ExpoEaseOut;
                        break;
                    case EasingFunction.ExpoInOut:
                        _easingFunction = EasingFunctions.ExpoEaseInOut;
                        break;
                    case EasingFunction.ExpoOutIn:
                        _easingFunction = EasingFunctions.ExpoEaseOutIn;
                        break;
                    case EasingFunction.BackIn:
                        _easingFunction = EasingFunctions.BackEaseIn;
                        break;
                    case EasingFunction.BackOut:
                        _easingFunction = EasingFunctions.BackEaseOut;
                        break;
                    case EasingFunction.BackInOut:
                        _easingFunction = EasingFunctions.BackEaseInOut;
                        break;
                    case EasingFunction.BackOutIn:
                        _easingFunction = EasingFunctions.BackEaseOutIn;
                        break;
                    case EasingFunction.BounceIn:
                        _easingFunction = EasingFunctions.BounceEaseIn;
                        break;
                    case EasingFunction.BounceOut:
                        _easingFunction = EasingFunctions.BounceEaseOut;
                        break;
                    case EasingFunction.BounceInOut:
                        _easingFunction = EasingFunctions.BounceEaseInOut;
                        break;
                    case EasingFunction.BounceOutIn:
                        _easingFunction = EasingFunctions.BounceEaseOutIn;
                        break;
                    case EasingFunction.Linear:
                        _easingFunction = EasingFunctions.Linear;
                        break;
                    default:
                        _easingFunction = EasingFunctions.Linear;
                        break;
                }
                _selectedEasingFunction = value;
            }
        }
        public int ValueTo { get; set; } = 100;
        public int Duration { get; set; } = 3;
        public int Delay { get; set; } = 0;
        public int LoopsCount { get; set; } = 1;
        public bool Reverse { get; set; } = false;
        #endregion
        public CodeeloAnimator()
        {
            InitializeComponent();
            _cancellationTokens = new List<AnimationStatus>();
        }

        public CodeeloAnimator(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            _cancellationTokens = new List<AnimationStatus>();
        }
        public void CancelAnimation()
        {
            foreach (var token in _cancellationTokens)
                token.CancellationToken.Cancel();

            _cancellationTokens.Clear();
        }
        public void PlaAnimationImmediately()
        {
            if (SourceControl == null)
                return;

            CancelAnimation();
            SelectEffect();
            var cancellationToken = SourceControl.Animate(
                _animationEffect,
                _easingFunction,
                ValueTo,
                Duration * 1000,
                0,
                Reverse,
                LoopsCount
                );

            _cancellationTokens.Add(cancellationToken);
        }
        public void PlayAnimation()
        {
            if (SourceControl == null)
                return;

            CancelAnimation();
            SelectEffect();
            var cancellationToken = SourceControl.Animate(
                _animationEffect,
                _easingFunction,
                ValueTo,
                Duration * 1000,
                Delay * 1000,
                Reverse,
                LoopsCount);

            _cancellationTokens.Add(cancellationToken);
        }
        public void PlayAnimationAndDoAction(Action doSomething)
        {
            if (SourceControl == null)
                return;

            CancelAnimation();
            SelectEffect();
            var cancellationToken = SourceControl.Animate(
                _animationEffect,
                _easingFunction,
                ValueTo,
                Duration * 1000,
                Delay * 1000,
                doSomething,
                Reverse,
                LoopsCount);

            _cancellationTokens.Add(cancellationToken);
        }
        private void SelectEffect()
        {
            switch (AnimationEffect)
            {
                case AnimationEffect.XLocation:
                    _animationEffect = new XLocationEffect();
                    break;
                case AnimationEffect.YLocation:
                    _animationEffect = new YLocationEffect();
                    break;
                case AnimationEffect.VerticalFold:
                    _animationEffect = new VerticalFoldEffect();
                    break;
                case AnimationEffect.HorizontalFold:
                    _animationEffect = new HorizontalFoldEffect();
                    break;
                case AnimationEffect.Fold:
                    _animationEffect = new FoldEffect();
                    break;
                case AnimationEffect.BottomAnchoredHeight:
                    _animationEffect = new BottomAnchoredHeightEffect();
                    break;
                case AnimationEffect.TopAnchoredHeight:
                    _animationEffect = new TopAnchoredHeightEffect();
                    break;
                case AnimationEffect.LeftAnchoredWidth:
                    _animationEffect = new LeftAnchoredWidthEffect();
                    break;
                case AnimationEffect.RightAnchoredWidth:
                    _animationEffect = new RightAnchoredWidthEffect();
                    break;
                case AnimationEffect.ControlFade:
                    _animationEffect = new ControlFadeEffect(SourceControl);
                    break;
                case AnimationEffect.ControlFadeWithImageBlend:
                    _animationEffect = new ControlFadeEffectWithImageBlend(SourceControl);
                    break;
                case AnimationEffect.FormFade:
                    _animationEffect = new FormFadeEffect();
                    break;
                case AnimationEffect.FontSize:
                    _animationEffect = new FontSizeEffect();
                    break;
                case AnimationEffect.ColorShift:
                    _animationEffect = new ColorShiftEffect();
                    break;
                case AnimationEffect.ColorChannelShift:
                    _animationEffect = new ColorChannelShiftEffect(ColorChannel);
                    break;
                default:
                    _animationEffect = new XLocationEffect();
                    break;
            }
        }
    }
}
