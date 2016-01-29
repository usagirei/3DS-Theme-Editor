using System;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace ThemeEditor.WPF.Markup
{
    /// <summary>
    ///     Simple extension for ico, let you choose icon with specific size.
    ///     Usage sample: Image Stretch="None" Source="{common:Icon /ControlsTester;component/icons/custom-reports.ico, 16}"
    ///     Or: Image Source="{common:Icon Source={Binding IconResource},Size=16} "
    /// </summary>
    internal class IconExtension : MarkupExtension
    {
        private string _source;

        public int Size { get; set; }

        public string Source
        {
            get { return this._source; }

            set
            {
                // Have to make full pack URI from short form, so System.Uri can regognize it.
                this._source = "pack://application:,,," + value;
            }
        }

        public IconExtension(string source, int size)
        {
            this.Source = source;
            this.Size = size;
        }

        public IconExtension() {}

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var decoder = BitmapDecoder.Create(new Uri(this.Source), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);

            var result = decoder.Frames.SingleOrDefault(f => Math.Abs(f.Width - Size) < 0.01f);
            if (result == default(BitmapFrame))
                result = decoder.Frames.OrderBy(f => f.Width).First();
            return result;
        }
    }
}
