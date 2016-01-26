// --------------------------------------------------
// 3DS Theme Editor - BottomWallpaperRenderer.cs
// --------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

using ThemeEditor.WPF.Localization.Enums;
using ThemeEditor.WPF.RenderTools;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls.Renderers
{
    internal class BottomWallpaperRenderer : FrameworkElement
    {
        private const int BOT_SCR_HEIGHT = 240;
        private const int BOT_SCR_WIDTH = 320;

        private static readonly RenderToolFactory RenderToolFactory = new RenderToolFactory();

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register
            (nameof(Theme),
                typeof(ThemeViewModel),
                typeof(BottomWallpaperRenderer),
                new FrameworkPropertyMetadata(default(ThemeViewModel), FrameworkPropertyMetadataOptions.AffectsRender));

        private bool _isListening;

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static BottomWallpaperRenderer()
        {
            Type ownerType = typeof(BottomWallpaperRenderer);
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

        public BottomWallpaperRenderer()
        {
            ViewModelBase.ViewModelChanged += ViewModelBaseOnViewModelChanged;
        }

        private static void OnIsEnabledChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var target = elem as BottomWallpaperRenderer;
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
                OnRender_SolidColor(dc, false, false);
                return;
            }

            var drawType = Theme.Flags.BottomDrawType;
            switch (drawType)
            {
                case BottomDrawType.SolidColor:
                {
                    OnRender_SolidColor(dc, Theme.Flags.BottomBackgroundInnerColor, Theme.Flags.BottomBackgroundOuterColor);
                    break;
                }
                case BottomDrawType.None:
                {
                    OnRender_SolidColor(dc, false, false);
                    break;
                }
                case BottomDrawType.Texture:
                {
                    OnRender_BackgroundTexture(dc);
                    break;
                }
            }
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
            Color outerMain,
            Color outerEdgeStripes,
            Color outerGlow,
            Color innerMain,
            Color innerEdge,
            Color innerGlow,
            Color innerShadow)
        {
            var outerMainBrush = RenderToolFactory.GetTool<Brush>
                (new SolidColorBrushTool(outerMain));
            var outerLeftGlowBrush = RenderToolFactory.GetTool<Brush>
                (new LinearGradientBrushTool(outerGlow, outerMain, 0, 0.3));
            var outerRightGlowBrush = RenderToolFactory.GetTool<Brush>
                (new LinearGradientBrushTool(outerMain, outerGlow, 0, 0.3));
            var shadowBrush = RenderToolFactory.GetTool<Brush>
                (new SolidColorBrushTool(innerShadow, 1 / 25f));

            var innerMainBrush = RenderToolFactory.GetTool<Brush>(new SolidColorBrushTool(innerMain));

            var innerEdgePen = RenderToolFactory.GetTool<Pen>(new PenTool(innerEdge, 1));
            var innerGlowPen = RenderToolFactory.GetTool<Pen>(new PenTool(innerGlow, 0.5));

            // Outer Area
            {
                dc.DrawRectangle(outerMainBrush, null, new Rect(0, 0, BOT_SCR_WIDTH, BOT_SCR_HEIGHT));

                dc.DrawRectangle(outerLeftGlowBrush, null, new Rect(0, 0, 24, BOT_SCR_HEIGHT));
                dc.DrawRectangle(outerRightGlowBrush, null, new Rect(BOT_SCR_WIDTH - 24, 0, 24, BOT_SCR_HEIGHT));

                for (int i = 0; i < 7; i++)
                {
                    var opacity = (1 - i / 7f) / 2;
                    var pen = RenderToolFactory.GetTool<Pen>(new PenTool(outerEdgeStripes, 0.5, opacity));
                    dc.DrawLine(pen,
                        new Point(0, BOT_SCR_HEIGHT - 4 * i - 2.5),
                        new Point(BOT_SCR_WIDTH, BOT_SCR_HEIGHT - 4 * i - 2.5));
                }
            }
            // Inner Area
            {
                const double RADIUS = 10;

                Rect area = new Rect(12, 39, BOT_SCR_WIDTH, 170);

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
                    for (int pY = 0; pY < 3; pY++)
                    {
                        const double GAP = BOT_SCR_WIDTH / 6.0;
                        const double OFFSET = 31 + (170 - 2 * GAP) / 2;
                        const double RADIUS = 2;

                        Rect area = new Rect(GAP * pX - 8, Math.Floor(GAP * pY + OFFSET), 16, 16);

                        Point p0 = new Point(area.X + RADIUS, area.Y);
                        Point p1 = new Point(area.X + area.Width - RADIUS, area.Y);
                        dc.DrawRoundedRectangle(null, innerEdgePen, area, RADIUS, RADIUS);
                        dc.DrawLine(innerGlowPen, p0, p1);
                    }
            }
        }

        private void OnRender_BackgroundTexture(DrawingContext dc)
        {
            // Outer Area
            {
                var frameType = Theme.Flags.BottomFrameType;
                var scrollEnable = frameType == BottomFrameType.SlowScroll || frameType == BottomFrameType.FastScroll;
                var flipEnable = frameType == BottomFrameType.PageScroll;
                var bounceEnable = frameType == BottomFrameType.BounceScroll;

                var bgBitmap = Theme.Textures.Bottom.Bitmap;

                if (scrollEnable)
                {
                    // 802 = ...? Eyeballed it until it synced
                    var posMap = _isListening
                                     ? (Math.Sin(CompositionTargetEx.SecondsFromStart / 3) + 1) * 802
                                     : 0;

                    if (posMap < 1008)
                        dc.DrawImage(bgBitmap, new Rect(-posMap, 0, bgBitmap.Width, bgBitmap.Height));
                    if (posMap + BOT_SCR_WIDTH > 1008)
                        dc.DrawImage(bgBitmap, new Rect(-posMap + 1007, 0, bgBitmap.Width, bgBitmap.Height));
                }
                else if (flipEnable)
                {
                    var posMap = _isListening
                                     ? (int) Math.Floor(CompositionTargetEx.SecondsFromStart % 3)
                                     : 0;

                    dc.DrawImage(bgBitmap, new Rect(-posMap * BOT_SCR_WIDTH, 0, bgBitmap.Width, bgBitmap.Height));
                }
                else if (bounceEnable)
                {
                    var posMap = _isListening
                                     ? (int) Math.Floor(CompositionTargetEx.SecondsFromStart % 4)
                                     : 0;

                    posMap = posMap == 3 ? 1 : posMap;
                    dc.DrawImage(bgBitmap, new Rect(-posMap * BOT_SCR_WIDTH, 0, bgBitmap.Width, bgBitmap.Height));
                }
                else
                {
                    dc.DrawImage(bgBitmap, new Rect(0, 0, bgBitmap.Width, bgBitmap.Height));
                }
            }
            // Slots Area
            {
                var innerSlotBrush = RenderToolFactory.GetTool<Brush>(new SolidColorBrushTool(Colors.White, 0.2));
                for (int pX = 1; pX < 7; pX++)
                    for (int pY = 0; pY < 3; pY++)
                    {
                        const double GAP = BOT_SCR_WIDTH / 6.0;
                        const double OFFSET = 31 + (170 - 2 * GAP) / 2;
                        const double RADIUS = 2;

                        Rect area = new Rect(GAP * pX - 8, GAP * pY + OFFSET, 16, 16);
                        dc.DrawRoundedRectangle(innerSlotBrush, null, area, RADIUS, RADIUS);
                    }
            }
        }

        private void OnRender_SolidColor(DrawingContext dc, bool inner, bool outer)
        {
            Color outerMain = Color.FromRgb(226, 226, 234);
            Color outerEdgeStripes = Color.FromRgb(208, 208, 216);
            Color outerGlow = Color.FromRgb(227, 228, 233);

            Color innerMain = Color.FromRgb(223, 220, 215);
            Color innerEdge = Color.FromRgb(234, 234, 234);
            Color innerGlow = Color.FromRgb(127, 127, 127);
            Color innerShadow = Color.FromArgb(255, 63, 63, 63);

            if (inner)
            {
                innerMain = Theme.Colors.BottomBackgroundInner.Main;
                innerEdge = Theme.Colors.BottomBackgroundInner.Border;
                innerGlow = Theme.Colors.BottomBackgroundInner.Highlight;
                innerShadow = Theme.Colors.BottomBackgroundInner.Shadow;
            }

            if (outer)
            {
                outerMain = Theme.Colors.BottomBackgroundOuter.Main;
                outerEdgeStripes = Theme.Colors.BottomBackgroundOuter.Stripe;
                outerGlow = Theme.Colors.BottomBackgroundOuter.Glow;
            }

            OnRender_BackgroundColor(dc, outerMain, outerEdgeStripes, outerGlow, innerMain, innerEdge, innerGlow, innerShadow);
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
            Debug.Write("Bottom Start Listening");
            _isListening = true;
            CompositionTargetEx.FrameUpdating += OnRendering;
        }

        private void StopListening()
        {
            VerifyAccess();
            if (!_isListening)
                return;
            Debug.Write("Bottom Stop Listening");
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
