// --------------------------------------------------
// 3DS Theme Editor - ThemeViewModel.Rules.cs
// --------------------------------------------------

using System;
using System.Linq;
using System.Windows.Media.Imaging;
using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization.Enums;
using ThemeEditor.WPF.Themes.ColorSets;

namespace ThemeEditor.WPF.Themes
{
    partial class ThemeViewModel
    {
        private ViewModelRules Rules { get; set; }

        private T FilterEnum<T>(T newValue, T oldValue, params T[] valid) where T : struct, IConvertible
        {
            if (!typeof (T).IsEnum)
                throw new ArgumentException("Generic Type must be an enumerated type", nameof(T));
            if (valid.Length == 0)
                return newValue;
            return valid.Contains(newValue)
                ? newValue
                : valid.Contains(oldValue)
                    ? oldValue
                    : valid[0];
        }

        partial void SetupRules()
        {
            Rules = new ViewModelRules(Tag);

            SetupRules_FrameTypes();
            SetupRules_Textures();
            SetupRules_ColorToggles();
            SetupRules_SolidColorOpts();

            Rules.Apply(Flags);
            Rules.Apply(Textures);
            Rules.Apply(Colors.TopBackground);
        }

        private void SetupRules_SolidColorOpts()
        {
            Rules.AddRule<TopSolidSetViewModel, double>(nameof(TopSolidSetViewModel.GradientColor),
                (v, o, n) => { Colors.TopBackground.GradientColor = Flags.TopDrawType != TopDrawType.SolidColorTexture ? 1 : n; });
            Rules.AddRule<TopSolidSetViewModel, double>(nameof(TopSolidSetViewModel.AlternateOpacity),
                (v, o, n) => { Colors.TopBackground.AlternateOpacity = Flags.TopDrawType != TopDrawType.SolidColorTexture ? 1 : n; });
            Rules.AddRule<TopSolidSetViewModel, double>(nameof(TopSolidSetViewModel.TextureOpacity),
                (v, o, n) => { Colors.TopBackground.TextureOpacity = Flags.TopDrawType == TopDrawType.None ? 0.25 : n; });
            Rules.AddRule<TopSolidSetViewModel, double>(nameof(TopSolidSetViewModel.Gradient),
                (v, o, n) => { Colors.TopBackground.Gradient = Flags.TopDrawType == TopDrawType.None ? 0.75 : n; });
        }

        private void SetupRules_ColorToggles()
        {
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.TopBackgroundColor), (v, o, n) => Colors.TopBackground.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.TopCornerButtonColor), (v, o, n) => Colors.TopCorner.Enabled = n);

            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.BottomBackgroundInnerColor),
                    (v, o, n) =>
                    {
                        Validate_Dependency_BottomBackgroundInnerColor(v, o, n);
                        Colors.BottomBackgroundInner.Enabled = v.BottomBackgroundInnerColor;
                    });
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.BottomBackgroundOuterColor),
                    (v, o, n) =>
                    {
                        Validate_Dependency_BottomBackgroundOuterColor(v, o, n);
                        Colors.BottomBackgroundOuter.Enabled = v.BottomBackgroundOuterColor;
                    });

            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.BottomCornerButtonColor), (v, o, n) => Colors.BottomCorner.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.OpenCloseColor), (v, o, n) => Colors.Open.Enabled = Colors.Close.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.FolderBackgroundColor), (v, o, n) => Colors.FolderBackground.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.FolderColor), (v, o, n) => Colors.Folder.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.FolderArrowColor), (v, o, n) => Colors.FolderArrow.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.FileColor), (v, o, n) => Colors.File.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.CursorColor), (v, o, n) => Colors.Cursor.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.ArrowButtonColor), (v, o, n) => Colors.ArrowButton.Enabled = n);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.ArrowColor), (v, o, n) => Colors.Arrow.Enabled = n);
            Rules.AddRule<FlagsViewModel, GameTextDrawType>
                (nameof(FlagsViewModel.GameTextDrawType), (v, o, n) => Colors.GameText.Enabled = n == GameTextDrawType.Colored);
            Rules.AddRule<FlagsViewModel, bool>
                (nameof(FlagsViewModel.DemoTextColor), (v, o, n) => Colors.DemoText.Enabled = n);
        }

        private void SetupRules_FrameTypes()
        {
            Rules.AddRule<FlagsViewModel, TopDrawType>
                (nameof(FlagsViewModel.TopDrawType), Validate_TopDrawType);

            Rules.AddRule<FlagsViewModel, BottomDrawType>
                (nameof(FlagsViewModel.BottomDrawType), Validate_BottomDrawType);

            Rules.AddRule<FlagsViewModel, TopFrameType>
                (nameof(FlagsViewModel.TopFrameType), Validate_TopFrameType);

            Rules.AddRule<FlagsViewModel, BottomFrameType>
                (nameof(FlagsViewModel.BottomFrameType), Validate_BottomFrameType);
        }

        private void SetupRules_Textures()
        {
            Rules.AddRule<TexturesViewModel, TextureViewModel>
                (nameof(TexturesViewModel.Top), Validate_Texture_Top);

            Rules.AddRule<TexturesViewModel, TextureViewModel>
                (nameof(TexturesViewModel.Bottom), Validate_Texture_Bottom);
        }

        private void Validate_Dependency_BottomBackgroundInnerColor(FlagsViewModel model, bool oldValue, bool newValue)
        {
            model.BottomBackgroundInnerColor = newValue && model.BottomDrawType == BottomDrawType.SolidColor;

            // NOTE: Only Directly Call if Absolutely Sure no Recursion will Ensue!
            Validate_Dependency_BottomBackgroundOuterColor(model,
                model.BottomBackgroundOuterColor,
                model.BottomBackgroundOuterColor);
        }

        private void Validate_Dependency_BottomBackgroundOuterColor(FlagsViewModel model, bool oldValue, bool newValue)
        {
            model.BottomBackgroundOuterColor = newValue
                                               && model.BottomBackgroundInnerColor
                                               && model.BottomDrawType == BottomDrawType.SolidColor;
        }

        private void Validate_BottomDrawType(FlagsViewModel model, BottomDrawType oldValue, BottomDrawType newValue)
        {
            switch (Textures.Bottom.Width)
            {
                case 0:
                    model.BottomDrawType
                        = FilterEnum(newValue, oldValue, BottomDrawType.None, BottomDrawType.SolidColor);
                    break;
                case 512:
                case 1024:
                    model.BottomDrawType
                        = FilterEnum(newValue,
                            oldValue,
                            BottomDrawType.None,
                            BottomDrawType.SolidColor,
                            BottomDrawType.Texture);
                    break;
            }

            // NOTE: Only Directly Call if Absolutely Sure no Recursion will Ensue!
            Validate_Dependency_BottomBackgroundInnerColor(model,
                model.BottomBackgroundInnerColor,
                model.BottomBackgroundInnerColor);
            // Inner will Also Validate Outer
            Validate_BottomFrameType(model, model.BottomFrameType, model.BottomFrameType);
        }

        private void Validate_BottomFrameType(FlagsViewModel model, BottomFrameType oldValue, BottomFrameType newValue)
        {
            if (!model.BottomDrawType.Equals(BottomDrawType.Texture))
            {
                model.BottomFrameType = BottomFrameType.Single;
            }
            else
            {
                var tW = Textures.Bottom.Width;
                switch (tW)
                {
                    case 512:
                        model.BottomFrameType
                            = FilterEnum(newValue, oldValue, BottomFrameType.Single);
                        break;
                    case 1024:
                        model.BottomFrameType
                            = FilterEnum(newValue,
                                oldValue,
                                BottomFrameType.FastScroll,
                                BottomFrameType.SlowScroll,
                                BottomFrameType.PageScroll,
                                BottomFrameType.BounceScroll);
                        break;
                }
            }
        }

        private void Validate_Texture_Bottom(TexturesViewModel model,
            TextureViewModel oldValue,
            TextureViewModel newValue)
        {
            // NOTE: Avoid Setting other values Directly, Textures are Special Cases
            switch (model.Bottom.Width)
            {
                case 0:
                    Flags.BottomDrawType
                        = FilterEnum(Flags.BottomDrawType,
                            BottomDrawType.None,
                            BottomDrawType.None,
                            BottomDrawType.SolidColor);
                    break;
                case 512:
                    Flags.BottomFrameType
                        = FilterEnum(Flags.BottomFrameType, BottomFrameType.Single, BottomFrameType.Single);
                    Flags.BottomDrawType
                        = FilterEnum(Flags.BottomDrawType,
                            BottomDrawType.Texture,
                            BottomDrawType.Texture);
                    break;
                case 1024:
                    Flags.BottomFrameType
                        = FilterEnum(Flags.BottomFrameType,
                            BottomFrameType.FastScroll,
                            BottomFrameType.SlowScroll,
                            BottomFrameType.FastScroll,
                            BottomFrameType.PageScroll,
                            BottomFrameType.BounceScroll);
                    Flags.BottomDrawType
                        = FilterEnum(Flags.BottomDrawType,
                            BottomDrawType.Texture,
                            BottomDrawType.Texture);
                    break;
            }
        }

        private void Validate_Texture_Top(TexturesViewModel model, TextureViewModel oldValue, TextureViewModel newValue)
        {
            // NOTE: Avoid Setting other values Directly, Textures are Special Cases
            switch (model.Top.Width)
            {
                case 0:
                    Flags.TopDrawType = FilterEnum(Flags.TopDrawType,
                        TopDrawType.None,
                        TopDrawType.None,
                        TopDrawType.SolidColor);
                    break;
                case 64:
                    Flags.TopDrawType = FilterEnum(Flags.TopDrawType,
                        TopDrawType.SolidColorTexture,
                        TopDrawType.SolidColorTexture);
                    break;
                case 512:
                    Flags.TopFrameType = FilterEnum(Flags.TopFrameType,
                        TopFrameType.Single,
                        TopFrameType.Single);
                    Flags.TopDrawType = FilterEnum(Flags.TopDrawType,
                        TopDrawType.Texture,
                        TopDrawType.Texture);
                    break;
                case 1024:
                    Flags.TopFrameType = FilterEnum(Flags.TopFrameType,
                        TopFrameType.FastScroll,
                        TopFrameType.SlowScroll,
                        TopFrameType.FastScroll);
                    Flags.TopDrawType = FilterEnum(Flags.TopDrawType,
                        TopDrawType.Texture,
                        TopDrawType.Texture);
                    break;
            }

            
        }

        private void Validate_TopDrawType(FlagsViewModel model, TopDrawType oldValue, TopDrawType newValue)
        {
            switch (Textures.Top.Width)
            {
                case 0:
                    model.TopDrawType
                        = FilterEnum(newValue, oldValue, TopDrawType.None, TopDrawType.SolidColor);
                    break;
                case 64:
                    model.TopDrawType
                        = FilterEnum(newValue,
                            oldValue,
                            TopDrawType.None,
                            TopDrawType.SolidColor,
                            TopDrawType.SolidColorTexture);
                    break;
                case 512:
                case 1024:
                    model.TopDrawType
                        = FilterEnum(newValue, oldValue, TopDrawType.None, TopDrawType.SolidColor, TopDrawType.Texture);
                    break;
            }

            // NOTE: Only Directly Call if BOTH Validators Only Modify Themselves!
            Validate_TopFrameType(model, model.TopFrameType, model.TopFrameType);
        }

        private void Validate_TopFrameType(FlagsViewModel model, TopFrameType oldValue, TopFrameType newValue)
        {
            if (!model.TopDrawType.Equals(TopDrawType.Texture))
            {
                model.TopFrameType = TopFrameType.Single;
            }
            else
            {
                switch (Textures.Top.Width)
                {
                    case 512:
                        model.TopFrameType
                            = FilterEnum(newValue, oldValue, TopFrameType.Single);
                        break;
                    case 1024:
                        Flags.TopFrameType
                            = FilterEnum(Flags.TopFrameType,
                                TopFrameType.FastScroll,
                                TopFrameType.SlowScroll,
                                TopFrameType.FastScroll);
                        break;
                }
            }
        }
    }
}