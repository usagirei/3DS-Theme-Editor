// --------------------------------------------------
// 3DS Theme Editor - TopWallpaperRenderer.cs
// --------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ThemeEditor.Common.Graphics;
using ThemeEditor.WPF.Effects;
using ThemeEditor.WPF.Localization.Enums;
using ThemeEditor.WPF.RenderTools;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls.Renderers
{
    internal class TopWallpaperRenderer : FrameworkElement
    {
        private static readonly TextureViewModel DefaultTopSquares;

        private static readonly RenderToolFactory RenderToolFactory = new RenderToolFactory();
        private static readonly Rect ScreenArea;

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register
            (nameof(Theme),
                typeof(ThemeViewModel),
                typeof(TopWallpaperRenderer),
                new FrameworkPropertyMetadata(default(ThemeViewModel), FrameworkPropertyMetadataOptions.AffectsRender));

        private bool _enableWarp;
        private bool _isListening;

        private WarpEffect _warpEffect;

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static TopWallpaperRenderer()
        {
            ScreenArea = new Rect(0, 0, 412, 240);

            var defTopAlt = new BitmapImage();
            defTopAlt.BeginInit();
            //defTopAlt.StreamSource = (Stream) Extensions.GetResources(@"TopAlt_DefMask\.png").First().Value;
            defTopAlt.UriSource = new Uri(@"pack://application:,,,/ThemeEditor.WPF;component/Resources/TopAlt_DefMask.png");
            defTopAlt.CacheOption = BitmapCacheOption.OnLoad;
            defTopAlt.EndInit();

            var bgrData = defTopAlt.GetBgr24Data();
            RawTexture rTex = new RawTexture(defTopAlt.PixelWidth, defTopAlt.PixelHeight, RawTexture.DataFormat.A8);
            rTex.Encode(bgrData);
            DefaultTopSquares = new TextureViewModel(rTex, null);

            RenderToolFactory.RegisterTool<PenTool, Pen>
                (key => new Pen(new SolidColorBrush(key.Color)
                {
                    Opacity = key.Opacity
                },
                            key.Width));

            RenderToolFactory.RegisterTool<SolidColorBrushTool, Brush>
                (key => new SolidColorBrush(key.Color)
                {
                    Opacity = key.Opacity
                });

            RenderToolFactory.RegisterTool<LinearGradientBrushTool, Brush>
                (key => new LinearGradientBrush(key.ColorA, key.ColorB, key.Angle)
                {
                    Opacity = key.Opacity
                });

            RenderToolFactory.RegisterTool<ImageBrushTool, Brush>
                (key => new ImageBrush(key.Image)
                {
                    TileMode = key.Mode,
                    ViewportUnits = key.ViewportUnits,
                    Viewport = key.Viewport,
                    Opacity = key.Opacity
                });

            Type ownerType = typeof(TopWallpaperRenderer);
            IsEnabledProperty
                .OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false, OnIsEnabledChanged));

            ClipToBoundsProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(true, null, (o, value) => true));
            WidthProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(412.0, null, (o, value) => 412.0));
            HeightProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(240.0, null, (o, value) => 240.0));

            EffectProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(default(WarpEffect),
                    null,
                    (o, value) => ((TopWallpaperRenderer) o).GetWarpEffectInstance()));
        }

        public TopWallpaperRenderer()
        {
            ViewModelBase.ViewModelChanged += ViewModelBaseOnViewModelChanged;
        }

        private static void OnIsEnabledChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var target = elem as TopWallpaperRenderer;
            if (target == null)
            {
                return;
            }
            bool oldValue = (bool) args.OldValue;
            bool newValue = (bool) args.NewValue;
            target.OnIsEnabledChanged(oldValue, newValue);
        }

        private static void OnRender_3DCorners(DrawingContext dc)
        {
            var rect3DL = new Rect(0, 0, 6, 240);
            var rect3DR = new Rect(406, 0, 6, 240);
            var brush3D = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            dc.DrawRectangle(brush3D, null, rect3DL);
            dc.DrawRectangle(brush3D, null, rect3DR);
        }

        protected override void OnInitialized(EventArgs e)
        {
            Effect = _warpEffect;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (Theme == null)
            {
                var topTex = DefaultTopSquares.Bitmap;
                const float OPACITY = 0.5f;
                var background = Color.FromArgb(255, 205, 205, 217);
                const float GRADIENT = 0.5f;

                OnRender_BackgroundSolidTexture(dc, topTex, topTex, OPACITY, background, GRADIENT);
                return;
            }

            var drawType = Theme.Flags.TopDrawType;
            switch (drawType)
            {
                case TopDrawType.SolidColor:
                {
                    OnRender_BackgroundSolid(dc);

                    break;
                }
                case TopDrawType.Texture:
                {
                    OnRender_BackgroundTexture(dc, Theme.Flags.TopFrameType, Theme.Textures.Top.Bitmap);

                    break;
                }
                case TopDrawType.None:
                {
                    var topTex = DefaultTopSquares.Bitmap;
                    const float OPACITY = 0.5f;
                    var background = Color.FromArgb(255, 205, 205, 217);
                    const float GRADIENT = 0.5f;

                    OnRender_BackgroundSolidTexture(dc, topTex, topTex, OPACITY, background, GRADIENT);

                    break;
                }
                case TopDrawType.SolidColorTexture:
                {
                    var topStatic = Theme.Textures.TopAlt.Bitmap;
                    var topDynamic = Theme.Textures.Top.Bitmap;
                    var opacity = Theme.Colors.TopBackground.TextureOpacity;
                    var background = Theme.Colors.TopBackground.Main;
                    var gradient = Theme.Colors.TopBackground.Gradient;

                    OnRender_BackgroundSolidTexture(dc, topStatic, topDynamic, (float) opacity, background, (float) gradient);

                    break;
                }
            }

            OnRender_3DCorners(dc);
        }

        private object GetWarpEffectInstance()
        {
            return _warpEffect ?? (_warpEffect = new WarpEffect());
        }

        private void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                if (!_isListening)
                {
                    StartListening();
                }
            }
            else
            {
                if (_isListening)
                {
                    StopListening();
                }
            }

            if (oldValue != newValue)
            {
                InvalidateVisual();
            }
        }

        private void OnRender_BackgroundSolid(DrawingContext dc)
        {
            SetEnableWarp(false);

            var tss = Theme.Colors.TopBackground;
            var opaque = tss.Main;
            var faded = opaque.Blend(Colors.White, (float) tss.Gradient);
            var lgBrush = RenderToolFactory.GetTool<Brush>(new LinearGradientBrushTool(faded, opaque, 90));
            dc.DrawRectangle(lgBrush, null, ScreenArea);
        }

        private void OnRender_BackgroundSolidTexture(
            DrawingContext dc,
            ImageSource topStatic,
            ImageSource topDynamic,
            float textureOpacity,
            Color background,
            float gradient)
        {
            SetEnableWarp(true);

            var opaque = background;
            var faded = background.Blend(Colors.White, gradient);
            //LinearGradientBrush lgBrush = new LinearGradientBrush(faded, opaque, 90);
            var lgBrush = RenderToolFactory.GetTool<Brush>(new LinearGradientBrushTool(faded, opaque, 90));
            dc.DrawRectangle(lgBrush, null, ScreenArea);

            const int SQ_WIDTH = 66;
            const int SQ_HEIGHT = 66;
            const int SQ_X_OFF = SQ_WIDTH / 3;
            const int SQ_Y_OFF = SQ_HEIGHT / 3;

            var stBrush = RenderToolFactory.GetTool<Brush>
                (new ImageBrushTool(topStatic,
                    TileMode.Tile,
                    new Rect(-SQ_X_OFF, SQ_Y_OFF, SQ_WIDTH, SQ_HEIGHT),
                    BrushMappingMode.Absolute,
                    textureOpacity));
            dc.DrawRectangle(stBrush, null, ScreenArea);

            var off = _isListening
                          ? CompositionTargetEx.SecondsFromStart * 4
                          : (int) (SQ_WIDTH / 2);

            // Dynamic Brush is Constantly Changing
            const float WARP = (float) SQ_WIDTH / SQ_HEIGHT;
            ImageBrush dyBrush = new ImageBrush(topDynamic)
            {
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
                Viewport = new Rect(off * WARP - SQ_X_OFF, off + SQ_Y_OFF, SQ_WIDTH, SQ_HEIGHT),
                Opacity = textureOpacity
            };
            dc.DrawRectangle(dyBrush, null, ScreenArea);

            OnRender_3DCorners(dc);
        }

        private void OnRender_BackgroundTexture(DrawingContext dc, TopFrameType frameType, ImageSource wallpaper)
        {
            SetEnableWarp(false);

            var scrollEnable = frameType == TopFrameType.SlowScroll || frameType == TopFrameType.FastScroll;

            if (scrollEnable)
            {
                const int SCR_OFFSET = 1008 - (412 / 2);
                var posMap = _isListening
                                 ? (Math.Sin(CompositionTargetEx.SecondsFromStart / 3) + 1) * SCR_OFFSET - 6
                                 : 0;

                if (posMap < 1008)
                    dc.DrawImage(wallpaper, new Rect(-posMap, 0, wallpaper.Width, wallpaper.Height));
                if (posMap + 412 > 1008)
                    dc.DrawImage(wallpaper, new Rect(-posMap + 1007, 0, wallpaper.Width, wallpaper.Height));
            }
            else
            {
                dc.DrawImage(wallpaper, new Rect(0, 0, wallpaper.Width, wallpaper.Height));
            }

            OnRender_3DCorners(dc);
        }

        private void OnRendering(object sender, EventArgs eventArgs)
        {
            InvalidateVisual();
        }

        private void SetEnableWarp(bool value)
        {
            if (_enableWarp == value)
                return;

            _enableWarp = value;
            if (_enableWarp)
            {
                _warpEffect.Scale = 1.3f;
                _warpEffect.Pinch = 0.2f;
            }
            else
            {
                _warpEffect.Pinch = 0;
                _warpEffect.Scale = 1;
            }
        }

        private void StartListening()
        {
            VerifyAccess();
            if (_isListening)
                return;
            _isListening = true;
            CompositionTargetEx.FrameUpdating += OnRendering;
        }

        private void StopListening()
        {
            VerifyAccess();
            if (!_isListening)
                return;
            _isListening = false;
            CompositionTargetEx.FrameUpdating -= OnRendering;
        }

        private void ViewModelBaseOnViewModelChanged(ViewModelBase.ViewModelChangedArgs args)
        {
            if (Theme == null)
                return;
            if (args.ViewModel.GetTag() == Theme.GetTag())
                InvalidateVisual();
        }
    }
}
