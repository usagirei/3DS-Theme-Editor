using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

using ThemeEditor.WPF.RenderTools;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls.Renderers
{
    class BottomFolderRenderer : FrameworkElement
    {
        private const int BOT_SCR_HEIGHT = 240;
        private const int BOT_SCR_WIDTH = 320;

        private static readonly RenderToolFactory RenderToolFactory = new RenderToolFactory();

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register
            (nameof(Theme),
                typeof(ThemeViewModel),
                typeof(BottomFolderRenderer),
                new FrameworkPropertyMetadata(default(ThemeViewModel), FrameworkPropertyMetadataOptions.AffectsRender));

        private bool _isListening;

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        public BottomFolderRenderer()
        {
            ViewModelBase.ViewModelChanged += ViewModelBaseOnViewModelChanged;
        }


        static BottomFolderRenderer()
        {
            Type ownerType = typeof(BottomFolderRenderer);
            IsEnabledProperty
                .OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false, OnIsEnabledChanged));

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

            ClipToBoundsProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata(true, null, (o, value) => true));
            WidthProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata((double) BOT_SCR_WIDTH, null, (o, value) => (double) BOT_SCR_WIDTH));
            HeightProperty.OverrideMetadata(ownerType,
                new FrameworkPropertyMetadata((double) BOT_SCR_HEIGHT, null, (o, value) => (double) BOT_SCR_HEIGHT));
        }

        private static void OnIsEnabledChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var target = elem as BottomFolderRenderer;
            if (target == null)
            {
                return;
            }
            bool oldValue = (bool) args.OldValue;
            bool newValue = (bool) args.NewValue;
            target.OnIsEnabledChanged(oldValue, newValue);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (Theme == null)
            {
                OnRender_SolidColor(dc, false);
                return;
            }

            OnRender_SolidColor(dc, Theme.Flags.FolderBackgroundColor);
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

        private void OnRender_BackgroundColor(
            DrawingContext dc,
            Color innerMain,
            Color innerEdge,
            Color innerGlow,
            Color innerShadow)
        {
            var shadowBrush = RenderToolFactory.GetTool<Brush>
                (new SolidColorBrushTool(innerShadow, 1 / 25f));

            var innerMainBrush = RenderToolFactory.GetTool<Brush>(new SolidColorBrushTool(innerMain));

            var innerEdgePen = RenderToolFactory.GetTool<Pen>(new PenTool(innerEdge, 1));
            var innerGlowPen = RenderToolFactory.GetTool<Pen>(new PenTool(innerGlow, 0.5));

            // Inner Area
            {
                const double RADIUS = 10;

                Rect area = new Rect(12, 69, BOT_SCR_WIDTH, 140);

                for (int pX = -2; pX <= 2; pX++)
                {
                    for (int pY = -2; pY <= 2; pY++)
                    {
                        var offArea = area;
                        offArea.Offset(pX, pY + 2);
                        dc.DrawRoundedRectangle(shadowBrush, null, offArea, RADIUS, RADIUS);
                    }
                }

                dc.DrawRoundedRectangle(innerMainBrush, innerEdgePen, area, RADIUS, RADIUS);
                Point p0 = new Point(area.X + RADIUS, area.Y + area.Height + 1);
                Point p1 = new Point(area.X + area.Width - 2 * RADIUS, area.Y + area.Height + 1);
                dc.DrawLine(innerGlowPen, p0, p1);
            }
            // Slots Area
            {
                for (int pX = 1; pX < 7; pX++)
                    for (int pY = 0; pY < 2; pY++)
                    {
                        const double GAP = BOT_SCR_WIDTH / 6.0;
                        const double OFFSET = 86 + (140 - 2 * GAP) / 2;
                        const double RADIUS = 2;

                        Rect area = new Rect(GAP * pX - 8, Math.Floor(GAP * pY + OFFSET), 16, 16);

                        Point p0 = new Point(area.X + RADIUS, area.Y);
                        Point p1 = new Point(area.X + area.Width - RADIUS, area.Y);
                        dc.DrawRoundedRectangle(null, innerEdgePen, area, RADIUS, RADIUS);
                        dc.DrawLine(innerGlowPen, p0, p1);
                    }
            }
        }

        private void OnRender_SolidColor(DrawingContext dc, bool custom)
        {
            Color innerMain = Color.FromRgb(223, 220, 215);
            Color innerEdge = Color.FromRgb(234, 234, 234);
            Color innerGlow = Color.FromRgb(127, 127, 127);
            Color innerShadow = Color.FromArgb(255, 63, 63, 63);

            if (custom)
            {
                innerMain = Theme.Colors.FolderBackground.Main;
                innerEdge = Theme.Colors.FolderBackground.Border;
                innerGlow = Theme.Colors.FolderBackground.Highlight;
                innerShadow = Theme.Colors.FolderBackground.Glow;
            }

            OnRender_BackgroundColor(dc, innerMain, innerEdge, innerGlow, innerShadow);
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
            Debug.Write("Bottom Folder Start Listening");
            _isListening = true;
            CompositionTargetEx.FrameUpdating += OnRendering;
        }

        private void StopListening()
        {
            VerifyAccess();
            if (!_isListening)
                return;
            Debug.Write("Bottom Folder Stop Listening");
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
