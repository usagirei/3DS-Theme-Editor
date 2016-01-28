// --------------------------------------------------
// 3DS Theme Editor - GestureCommandWrapper.cs
// --------------------------------------------------

using System.Globalization;
using System.Windows.Input;

namespace ThemeEditor.WPF
{
    public class GestureCommandWrapper
    {
        public ICommand Command { get; }
        public KeyGesture Gesture { get; }

        public string GestureText => Gesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture);

        public GestureCommandWrapper(ICommand command, KeyGesture gesture)
        {
            Command = command;
            Gesture = gesture;
        }
    }
}
