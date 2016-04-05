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
        #region Fields

        public static readonly DependencyProperty ScaleProperty
            = DependencyProperty.Register(nameof(Scale),
                typeof (float),
                typeof (WarpEffect),
                new UIPropertyMetadata(1.0f, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty PinchProperty
            = DependencyProperty.Register(nameof(Pinch),
                typeof (float),
                typeof (WarpEffect),
                new UIPropertyMetadata(0.0f, PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty BlendProperty
            = DependencyProperty.Register(nameof(BlendWeight),
                typeof (float),
                typeof (WarpEffect),
                new UIPropertyMetadata(1.0f, PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty InputProperty
            = RegisterPixelShaderSamplerProperty(nameof(Input), typeof (WarpEffect), 0, SamplingMode.Auto);

        public static readonly DependencyProperty OverlayBrushProperty
            = RegisterPixelShaderSamplerProperty(nameof(OverlayBrush), typeof (WarpEffect), 1, SamplingMode.Auto);

        private static readonly PixelShader Shader = new PixelShader();

        #endregion

        #region Properties

        public Brush Input
        {
            get { return (Brush) GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public Brush OverlayBrush
        {
            get { return (Brush) GetValue(OverlayBrushProperty); }
            set { SetValue(OverlayBrushProperty, value); }
        }

        public float BlendWeight
        {
            get { return (float) GetValue(BlendProperty); }
            set { SetValue(BlendProperty, value); }
        }

        public float Scale
        {
            get { return (float) GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public float Pinch
        {
            get { return (float) GetValue(PinchProperty); }
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
            UpdateShaderValue(OverlayBrushProperty);
            UpdateShaderValue(ScaleProperty);
            UpdateShaderValue(PinchProperty);
            UpdateShaderValue(BlendProperty);
        }

        #endregion
    }
}