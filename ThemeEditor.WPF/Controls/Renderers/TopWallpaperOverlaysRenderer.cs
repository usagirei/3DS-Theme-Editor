using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ThemeEditor.Common.Graphics;
using ThemeEditor.WPF.Localization.Enums;
using ThemeEditor.WPF.RenderTools;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls.Renderers
{
    internal class TopWallpaperOverlaysRenderer : FrameworkElement
    {
        private static readonly RenderToolFactory RenderToolFactory = new RenderToolFactory();
        private static readonly TextureViewModel DefaultTopSquares;

        private static readonly Rect ScreenArea;

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register
            (nameof(Theme),
                typeof (ThemeViewModel),
                typeof (TopWallpaperOverlaysRenderer),
                new FrameworkPropertyMetadata(default(ThemeViewModel), FrameworkPropertyMetadataOptions.AffectsRender));

        private bool _isListening;

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static TopWallpaperOverlaysRenderer()
        {
            ScreenArea = new Rect(0, 0, 412, 240);

            RenderToolFactory.RegisterTool<ImageBrushTool, Brush>
                (key => new ImageBrush(key.Image)
                {
                    TileMode = key.Mode,
                    ViewportUnits = key.ViewportUnits,
                    Viewport = key.Viewport,
                    Opacity = key.Opacity
                });

            var defTopAlt = new BitmapImage();
            defTopAlt.BeginInit();
            //defTopAlt.StreamSource = (Stream) Extensions.GetResources(@"TopAlt_DefMask\.png").First().Value;
            defTopAlt.UriSource =
                new Uri(@"pack://application:,,,/ThemeEditor.WPF;component/Resources/TopAlt_DefMask.png");
            defTopAlt.CacheOption = BitmapCacheOption.OnLoad;
            defTopAlt.EndInit();

            var bgrData = defTopAlt.GetBgr24Data();
            RawTexture rTex = new RawTexture(defTopAlt.PixelWidth, defTopAlt.PixelHeight, RawTexture.DataFormat.A8);
            rTex.Encode(bgrData);
            DefaultTopSquares = new TextureViewModel(rTex, null);

            Type ownerType = typeof (TopWallpaperOverlaysRenderer);
            IsEnabledProperty
                .OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false, OnIsEnabledChanged));

            ClipToBoundsProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(true, null, (o, value) => true));
            WidthProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(412.0, null, (o, value) => 412.0));
            HeightProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(240.0, null, (o, value) => 240.0));
        }

        public TopWallpaperOverlaysRenderer()
        {
            ViewModelBase.ViewModelChanged += ViewModelBaseOnViewModelChanged;
        }

        private void ViewModelBaseOnViewModelChanged(ViewModelBase.ViewModelChangedArgs args)
        {
            if (Theme == null)
                return;
            if (args.ViewModel.Tag == Theme.Tag)
                InvalidateVisual();
        }

        private static void OnIsEnabledChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var target = elem as TopWallpaperOverlaysRenderer;
            if (target == null)
            {
                return;
            }
            bool oldValue = (bool) args.OldValue;
            bool newValue = (bool) args.NewValue;
            target.OnIsEnabledChanged(oldValue, newValue);
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

        private void OnRendering(object sender, EventArgs eventArgs)
        {
            InvalidateVisual();
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

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.Black, null, ScreenArea);
            if (Theme == null)
            {
                OnRender_BackgroundSolidTexture(dc, DefaultTopSquares.Bitmap, DefaultTopSquares.Bitmap, true, 0.05f);
                return;
            }

            switch (Theme?.Flags.TopDrawType)
            {
                case TopDrawType.SolidColorTexture:
                {
                    var topStatic = Theme.Textures.TopAlt.Exists
                        ? Theme.Textures.TopAlt.Bitmap
                        : DefaultTopSquares.Bitmap;
                    var topDynamic = Theme.Textures.Top.Exists
                        ? Theme.Textures.Top.Bitmap
                        : DefaultTopSquares.Bitmap;
                    var opacity = (float) Theme.Colors.TopBackground.TextureOpacity;
                    var enableAlt = Theme.Colors.TopBackground.EnableAlt;
                    OnRender_BackgroundSolidTexture(dc, topStatic, topDynamic, enableAlt, opacity);
                    break;
                }
                case TopDrawType.SolidColor:
                {
                    var opacity = (float) Theme.Colors.TopBackground.TextureOpacity;
                    OnRender_BackgroundSolidTexture(dc,
                        DefaultTopSquares.Bitmap,
                        DefaultTopSquares.Bitmap,
                        true,
                        opacity);
                    break;
                }
                case TopDrawType.None:
                {
                    OnRender_BackgroundSolidTexture(dc, DefaultTopSquares.Bitmap, DefaultTopSquares.Bitmap, true, 0.05f);
                    break;
                }
            }
        }

        private void OnRender_BackgroundSolidTexture(
            DrawingContext dc,
            ImageSource topStatic,
            ImageSource topDynamic,
            bool enableStatic,
            float textureOpacity)
        {
            const int SQ_WIDTH = 66;
            const int SQ_HEIGHT = 66;
            const int SQ_X_OFF = SQ_WIDTH / 3;
            const int SQ_Y_OFF = SQ_HEIGHT / 3;

            if (enableStatic)
            {
                var stBrush = RenderToolFactory.GetTool<Brush>
                    (new ImageBrushTool(topStatic,
                        TileMode.Tile,
                        new Rect(-SQ_X_OFF, SQ_Y_OFF, SQ_WIDTH, SQ_HEIGHT),
                        BrushMappingMode.Absolute,
                        textureOpacity));
                dc.DrawRectangle(stBrush, null, ScreenArea);
            }

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
        }
    }
}