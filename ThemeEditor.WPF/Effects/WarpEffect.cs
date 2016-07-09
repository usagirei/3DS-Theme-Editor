// --------------------------------------------------
// 3DS Theme Editor - WarpEffect.cs
// --------------------------------------------------

#region Usings

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

#endregion

namespace ThemeEditor.WPF.Effects
{

    public class WarpEffect : ShaderEffect
    {
        public static DependencyProperty RegisterPixelShaderConstantProperty<T>(string dpName, Type ownerType, int constantRegisterIndex, T defaultValue)
        {
            return DependencyProperty.Register(dpName,
                typeof(T),
                ownerType,
                new UIPropertyMetadata(defaultValue, PixelShaderConstantCallback(constantRegisterIndex)));
        }


        #region Fields

        public static readonly DependencyProperty EnableProperty
            = RegisterPixelShaderConstantProperty(nameof(Enable), typeof(WarpEffect), 0, 0.0f);

        public static readonly DependencyProperty GrayLevelProperty
            = RegisterPixelShaderConstantProperty(nameof(GrayLevel), typeof(WarpEffect), 1, 0.0f);

        public static readonly DependencyProperty TimesProperty
            = RegisterPixelShaderConstantProperty(nameof(Times), typeof(WarpEffect), 2, 3.5f);

        public static readonly DependencyProperty PinchProperty
            = RegisterPixelShaderConstantProperty(nameof(Pinch), typeof(WarpEffect), 3, 0.15f);

        public static readonly DependencyProperty OffsetProperty
            = RegisterPixelShaderConstantProperty(nameof(Offset), typeof(WarpEffect), 4, 0.5f);

        public static readonly DependencyProperty AspectProperty
            = RegisterPixelShaderConstantProperty(nameof(Aspect), typeof(WarpEffect), 5, 400f / 240f);

        public static readonly DependencyProperty GradientProperty
            = RegisterPixelShaderConstantProperty(nameof(Gradient), typeof(WarpEffect), 6, 0.6f);

        public static readonly DependencyProperty PatternOpacityProperty
            = RegisterPixelShaderConstantProperty(nameof(PatternOpacity), typeof(WarpEffect), 7, 0.25f);

        public static readonly DependencyProperty AlternateOpacityProperty
            = RegisterPixelShaderConstantProperty(nameof(AlternateOpacity), typeof(WarpEffect), 8, 1f);

        public static readonly DependencyProperty InputProperty
            = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(WarpEffect), 0, SamplingMode.Auto);

        public static readonly DependencyProperty MovingBrushProperty
            = RegisterPixelShaderSamplerProperty(nameof(MovingBrush), typeof(WarpEffect), 1, SamplingMode.Auto);

        public static readonly DependencyProperty FixedBrushProperty
            = RegisterPixelShaderSamplerProperty(nameof(FixedBrush), typeof(WarpEffect), 2, SamplingMode.Auto);

        private static readonly PixelShader Shader = new PixelShader();

        #endregion

        #region Properties

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public Brush MovingBrush
        {
            get { return (Brush)GetValue(MovingBrushProperty); }
            set { SetValue(MovingBrushProperty, value); }
        }
        public Brush FixedBrush
        {
            get { return (Brush)GetValue(FixedBrushProperty); }
            set { SetValue(FixedBrushProperty, value); }
        }


        public float AlternateOpacity
        {
            get { return (float)GetValue(AlternateOpacityProperty); }
            set { SetValue(AlternateOpacityProperty, value); }
        }


        public float PatternOpacity
        {
            get { return (float)GetValue(PatternOpacityProperty); }
            set { SetValue(PatternOpacityProperty, value); }
        }

        public float Gradient
        {
            get { return (float)GetValue(GradientProperty); }
            set { SetValue(GradientProperty, value); }
        }

        public float Aspect
        {
            get { return (float)GetValue(AspectProperty); }
            set { SetValue(AspectProperty, value); }
        }

        public float Offset
        {
            get { return (float)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        public float Enable
        {
            get { return (float) GetValue(EnableProperty); }
            set { SetValue(EnableProperty, value); }
        }

        public float GrayLevel
        {
            get { return (float)GetValue(GrayLevelProperty); }
            set { SetValue(GrayLevelProperty, value); }
        }

        public float Times
        {
            get { return (float)GetValue(TimesProperty); }
            set { SetValue(TimesProperty, value); }
        }

        public float Pinch
        {
            get { return (float)GetValue(PinchProperty); }
            set { SetValue(PinchProperty, value); }
        }


        #endregion

        #region (De)Constructors

        static WarpEffect()
        {
            // Associate _pixelShader with our compiled pixel shader
            Shader.UriSource
                = new Uri(@"pack://application:,,,/ThemeEditor.WPF;component/Effects/FxBin/WarpEffect.ps");
        }

        public WarpEffect()
        {
            PixelShader = Shader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(MovingBrushProperty);
            UpdateShaderValue(FixedBrushProperty);

            UpdateShaderValue(EnableProperty);
            UpdateShaderValue(GradientProperty);
            UpdateShaderValue(TimesProperty);
            UpdateShaderValue(PinchProperty);
            UpdateShaderValue(OffsetProperty);
            UpdateShaderValue(AspectProperty);
            UpdateShaderValue(GradientProperty);
            UpdateShaderValue(PatternOpacityProperty);
            UpdateShaderValue(AlternateOpacityProperty);

        }

        #endregion
    }
}