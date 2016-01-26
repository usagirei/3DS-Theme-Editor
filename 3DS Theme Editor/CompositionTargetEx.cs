// --------------------------------------------------
// 3DS Theme Editor - CompositionTargetEx.cs
// --------------------------------------------------

using System;
using System.Windows.Media;

namespace ThemeEditor.WPF
{
    public static class CompositionTargetEx
    {
        private static readonly TimeSpan LastRenderingTime = TimeSpan.Zero;

        public static long StartTicks = DateTime.Now.Ticks;
        public static double SecondsFromStart => TicksFromStart / (double) TimeSpan.TicksPerSecond;

        public static long TicksFromStart => DateTime.Now.Ticks - StartTicks;

        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            RenderingEventArgs args = (RenderingEventArgs) e;
            if (args.RenderingTime == LastRenderingTime)
                return;

            if (OnFrameUpdating != null)
                OnFrameUpdating(sender, args);
        }

        public static event EventHandler<RenderingEventArgs> FrameUpdating
        {
            add
            {
                if (OnFrameUpdating == null)
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                OnFrameUpdating += value;
            }
            remove
            {
                OnFrameUpdating -= value;
                if (OnFrameUpdating == null)
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
        }

        private static event EventHandler<RenderingEventArgs> OnFrameUpdating;
    }
}
