using System.IO;

using ThemeEditor.Common.SMDH;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes
{
    public class ThemeInfoViewModel : ViewModelBase
    {
        [DisplayName("Theme_Info_Author", typeof(ThemeResources))]
        [Description("Theme_Info_Author_Desc", typeof(ThemeResources))]
        public string Author
        {
            get { return Model.AppTitles[(int) AppTitleEnum.English].Publisher; }
            set
            {
                var oldValue = Model.AppTitles[(int) AppTitleEnum.English].Publisher;
                foreach (var title in Model.AppTitles)
                    title.Publisher = value;
                RaiseViewModelChanged(nameof(Author), oldValue, value);
            }
        }

        [DisplayName("Theme_Info_LargeIcon", typeof(ThemeResources))]
        [Description("Theme_Info_LargeIcon_Desc", typeof(ThemeResources))]
        public TextureViewModel LargeIcon { get; }

        [DisplayName("Theme_Info_LongDesc", typeof(ThemeResources))]
        [Description("Theme_Info_LongDesc_Desc", typeof(ThemeResources))]
        public string LongDescription
        {
            get { return Model.AppTitles[(int) AppTitleEnum.English].LongDesc; }
            set
            {
                var oldValue = Model.AppTitles[(int) AppTitleEnum.English].LongDesc;
                foreach (var title in Model.AppTitles)
                    title.LongDesc = value;
                RaiseViewModelChanged(nameof(LongDescription), oldValue, value);
            }
        }

        private new SMDH Model => (SMDH) base.Model;

        [DisplayName("Theme_Info_ShortDesc", typeof(ThemeResources))]
        [Description("Theme_Info_ShortDesc_Desc", typeof(ThemeResources))]
        public string ShortDescription
        {
            get { return Model.AppTitles[(int) AppTitleEnum.English].ShortDesc; }
            set
            {
                var oldValue = Model.AppTitles[(int) AppTitleEnum.English].ShortDesc;
                foreach (var title in Model.AppTitles)
                    title.ShortDesc = value;
                RaiseViewModelChanged(nameof(ShortDescription), oldValue, value);
            }
        }

        [DisplayName("Theme_Info_SmallIcon", typeof(ThemeResources))]
        [Description("Theme_Info_SmallIcon_Desc", typeof(ThemeResources))]
        public TextureViewModel SmallIcon { get; }

        public ThemeInfoViewModel(SMDH model, string tag) : base(model, tag)
        {
            SmallIcon = new TextureViewModel(model.SmallIcon, Tag);
            LargeIcon = new TextureViewModel(model.LargeIcon, Tag);

            SmallIcon.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(SmallIcon), null, null);
            };
            LargeIcon.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(LargeIcon), null, null);
            };
        }

        public override void Dispose()
        {
            LargeIcon.Dispose();
            SmallIcon.Dispose();

            base.Dispose();
        }

        public void Save(Stream s)
        {
            SMDH.Write(Model, s);
        }
    }
}
