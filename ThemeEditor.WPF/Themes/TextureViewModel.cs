// --------------------------------------------------
// 3DS Theme Editor - TextureViewModel.cs
// --------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.WPF.Themes
{
    public sealed class TextureViewModel : ViewModelBase
    {
        private IEnumerable<Color> _palette;
        private BitmapSource _src;

        public BitmapSource Bitmap
        {
            get { return _src ?? (_src = DecodeTexture(Model)); }
            set
            {
                if (value.PixelWidth != Width || value.PixelHeight != Height)
                    return;
                Model.Encode(value.GetBgr24Data());
                Invalidate();
            }
        }

        public RawTexture.DataFormat DataFormat => Model.Format;

        public bool Exists => !Model.Format.Equals(RawTexture.DataFormat.Invalid);

        public int Height => Model.Height;

        private new RawTexture Model => (RawTexture) base.Model;

        public IEnumerable<Color> Palette => _palette ?? (_palette = GeneratePalette());

        public int Width => Model.Width;

        public TextureViewModel(RawTexture model, string tag) : base(model, tag) {}

        public void ClearTexture()
        {
            Model.Encode(null, 0, 0, RawTexture.DataFormat.Invalid);
            Invalidate();
        }

        public override void Dispose()
        {
            _src = null;
            _palette = null;

            base.Dispose();
        }

        public void EncodeTexture(BitmapSource bitmap, RawTexture.DataFormat targetFormat)
        {
            var bgrData = bitmap.GetBgr24Data();
            Model.Encode(bgrData, bitmap.PixelWidth, bitmap.PixelHeight, targetFormat);
            Invalidate();
        }

        public void Invalidate()
        {
            var oldBitmap = _src;
            var oldWidth = Width;
            var oldHeight = Height;
            var oldPal = _palette;
            _src = null;
            _palette = null;
            RaiseViewModelChanged(nameof(Bitmap), oldBitmap, null);
            RaiseViewModelChanged(nameof(Width), oldWidth, 0);
            RaiseViewModelChanged(nameof(Height), oldHeight, 0);
            RaiseViewModelChanged(nameof(Palette), oldPal, 0);
        }

        private BitmapSource DecodeTexture(RawTexture model)
        {
            BitmapSource bmp;
            switch (model.Format)
            {
                default:
                case RawTexture.DataFormat.Invalid:
                {
                    return null;
                }
                case RawTexture.DataFormat.Bgr888:
                case RawTexture.DataFormat.Bgr565:
                {
                    var rgb888 = model.Decode();
                    bmp = BitmapSource.Create(model.Width,
                        model.Height,
                        96,
                        96,
                        PixelFormats.Bgr24,
                        null,
                        rgb888,
                        model.Width * 3);
                    break;
                }
                case RawTexture.DataFormat.A8:
                {
                    var rgb888 = model.Decode();
                    var alpha = GscToAlpha(rgb888);
                    bmp = BitmapSource.Create(model.Width,
                        model.Height,
                        96,
                        96,
                        PixelFormats.Bgra32,
                        null,
                        alpha,
                        model.Width * 4);
                    break;
                }
            }
            bmp.Freeze();
            return bmp;
        }

        private IEnumerable<Color> GeneratePalette()
        {
            if (Bitmap == null)
                return Enumerable.Empty<Color>();
            byte[] bgrData = null;
            switch (Width)
            {
                case 512:
                {
                    var crop = new CroppedBitmap(Bitmap, new Int32Rect(0, 0, 412, 240));
                    bgrData = crop.GetBgr24Data();
                    break;
                }
                case 1024:
                {
                    // It Has a 920 Version, but the Max data is 1008, bear with a little gray
                    var crop = new CroppedBitmap(Bitmap, new Int32Rect(0, 0, 1008, 240));
                    bgrData = crop.GetBgr24Data();
                    break;
                }
                default:
                {
                    bgrData = Bitmap.GetBgr24Data();
                    break;
                }
            }

            return GraphicUtils.PaletteGen(bgrData, 20).Select(Extensions.ToMediaColor).ToList();
        }

        private byte[] GscToAlpha(byte[] rgbData)
        {
            byte[] argbData = new byte[4 * rgbData.Length / 3];
            for (int i = 0, j = 0; j < rgbData.Length; i += 4, j += 3)
            {
                argbData[i + 0] = 255; // b
                argbData[i + 1] = 255; // g
                argbData[i + 2] = 255; // r
                argbData[i + 3] = rgbData[j + 0]; // 1-a ?
            }
            return argbData;
        }
    }
}
