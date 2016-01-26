// --------------------------------------------------
// ThemeEditor.Common - Theme.cs
// --------------------------------------------------

// Uncomment to Skip Not-Set Colors
// don't uncomment until all unidentified color values are reproducible
//#define SKIP_COLOR_NOT_SET

using System;
using System.IO;
using System.Text;

using ThemeEditor.Common.Graphics;
using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.Common.Themes.Enums;
using ThemeEditor.Common.Themes.Offsets;

namespace ThemeEditor.Common.Themes
{
    public struct Theme
    {
        public Flags Flags;
        public Textures Textures;
        public Colors Colors;
        public byte[] CWAV;

        public static Theme Read(Stream s)
        {
            if (!s.CanSeek)
                throw new ArgumentException("Stream can't Seek", nameof(s));

            var body = new Theme();
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                var flag = br.ReadUInt32() == 1;
                if (!flag)
                    throw new InvalidDataException("Not a Theme File (Was File Decompressed?)");

                var flags = Read_Flags(s);

                var cOffs = Read_ColorOffsets(s, flags);
                var tOffs = Read_TextureOffsets(s, flags);

                var colors = Read_Colors(s, flags, cOffs);
                var textures = Read_Textures(s, flags, tOffs);

                var cwav = Read_CWav(s, flags);

                body.Flags = flags;
                body.Colors = colors;
                body.Textures = textures;
                body.CWAV = cwav;
            }
            return body;
        }

        private static byte[] Read_CWav(Stream stream, Flags flags)
        {
            if (flags.SoundEffect)
            {
                using (var br = new BinaryReader(stream, Encoding.ASCII, true))
                {
                    stream.Position = 0xBC;
                    var len = br.ReadUInt32();
                    var offset = br.ReadUInt32();
                    stream.Position = offset;
                    return br.ReadBytes((int) len);
                }
            }
            return new byte[0];
        }

        private static Textures Read_Textures(Stream s, Flags flags, TextureOffsets offsets)
        {
            var texes = new Textures();
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                // Top
                if (flags.TopDrawType == TopDrawType.Texture)
                {
                    s.Position = offsets.Top;
                    switch (flags.TopFrameType)
                    {
                        case TopFrameType.Single:
                            texes.Top = new RawTexture(512, 256, RawTexture.DataFormat.Bgr565);
                            break;
                        case TopFrameType.SlowScroll:
                        case TopFrameType.FastScroll:
                            texes.Top = new RawTexture(1024, 256, RawTexture.DataFormat.Bgr565);
                            break;
                        default:
                            throw new ArgumentException("Invalid Texture Format",
                                nameof(flags) + "." + nameof(flags.TopFrameType));
                    }
                    texes.Top.Read(s);
                    texes.TopExt = new RawTexture();
                }
                else if (flags.TopDrawType == TopDrawType.SolidColorTexture)
                {
                    s.Position = offsets.Top;
                    texes.Top = new RawTexture(64, 64, RawTexture.DataFormat.A8);
                    texes.Top.Read(s);
                    if (offsets.TopExt != 0)
                    {
                        s.Position = offsets.TopExt;
                        texes.TopExt = new RawTexture(64, 64, RawTexture.DataFormat.A8);
                        texes.TopExt.Read(s);
                    }
                }
                else
                {
                    texes.Top = new RawTexture();
                    texes.TopExt = new RawTexture();
                }

                // Bottom
                if (flags.BottomDrawType == BottomDrawType.Texture)
                {
                    s.Position = offsets.Bottom;
                    switch (flags.BottomFrameType)
                    {
                        case BottomFrameType.Single:
                            texes.Bottom = new RawTexture(512, 256, RawTexture.DataFormat.Bgr565);
                            break;
                        case BottomFrameType.SlowScroll:
                        case BottomFrameType.FastScroll:
                        case BottomFrameType.BounceScroll:
                        case BottomFrameType.PageScroll:
                            texes.Bottom = new RawTexture(1024, 256, RawTexture.DataFormat.Bgr565);
                            break;
                        default:
                            throw new ArgumentException("Invalid Texture Format",
                                nameof(flags) + "." + nameof(flags.BottomFrameType));
                    }
                    texes.Bottom.Read(s);
                }
                else
                {
                    texes.Bottom = new RawTexture();
                }

                if (flags.FolderTexture)
                {
                    // Folder Closed
                    s.Position = offsets.FolderClosed;
                    texes.FolderClosed = new RawTexture(128, 64, RawTexture.DataFormat.Bgr888);
                    texes.FolderClosed.Read(s);

                    // Folder Open
                    s.Position = offsets.FolderOpen;
                    texes.FolderOpen = new RawTexture(128, 64, RawTexture.DataFormat.Bgr888);
                    texes.FolderOpen.Read(s);
                }
                else
                {
                    texes.FolderOpen = new RawTexture();
                    texes.FolderClosed = new RawTexture();
                }

                if (flags.FileTexture)
                {
                    // Folder Closed
                    s.Position = offsets.FileLarge;
                    texes.FileLarge = new RawTexture(64, 128, RawTexture.DataFormat.Bgr888);
                    texes.FileLarge.Read(s);

                    // Folder Open
                    s.Position = offsets.FileSmall;
                    texes.FileSmall = new RawTexture(32, 64, RawTexture.DataFormat.Bgr888);
                    texes.FileSmall.Read(s);
                }
                else
                {
                    texes.FileLarge = new RawTexture();
                    texes.FileSmall = new RawTexture();
                }
            }
            return texes;
        }

        private static Flags Read_Flags(Stream s)
        {
            var flags = new Flags();
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                s.Position = 0x05;
                flags.BackgroundMusic = br.ReadByte() > 0;

                s.Position = 0x0C;
                flags.TopDrawType = (TopDrawType) br.ReadUInt32();
                flags.TopFrameType = (TopFrameType) br.ReadUInt32();

                s.Position = 0x20;
                flags.BottomDrawType = (BottomDrawType) br.ReadUInt32();
                flags.BottomFrameType = (BottomFrameType) br.ReadUInt32();

                s.Position = 0x2C;
                flags.CursorColor = br.ReadUInt32() > 0;

                s.Position = 0x34;
                flags.FolderColor = br.ReadUInt32() > 0;

                s.Position = 0x3C;
                flags.FolderTexture = br.ReadUInt32() > 0;

                s.Position = 0x48;
                flags.FileColor = br.ReadUInt32() > 0;

                s.Position = 0x50;
                flags.FileTexture = br.ReadUInt32() > 0;

                s.Position = 0x5C;
                flags.ArrowButtonColor = br.ReadUInt32() > 0;

                s.Position = 0x64;
                flags.ArrowColor = br.ReadUInt32() > 0;

                s.Position = 0x6C;
                flags.OpenCloseColor = br.ReadUInt32() > 0;

                s.Position = 0x78;
                flags.GameTextColor = br.ReadUInt32() > 0;

                s.Position = 0x80;
                flags.BottomBackgroundInnerColor = br.ReadUInt32() > 0;

                s.Position = 0x88;
                flags.BottomBackgroundOuterColor = br.ReadUInt32() > 0;

                s.Position = 0x90;
                flags.FolderBackgroundColor = br.ReadUInt32() > 0;

                s.Position = 0x98;
                flags.FolderArrowColor = br.ReadUInt32() > 0;

                s.Position = 0xA0;
                flags.BottomCornerButtonColor = br.ReadUInt32() > 0;

                s.Position = 0xA8;
                flags.TopCornerButtonColor = br.ReadUInt32() > 0;

                s.Position = 0xB0;
                flags.DemoTextColor = br.ReadUInt32() > 0;

                s.Position = 0xB8;
                flags.SoundEffect = br.ReadUInt32() > 0;
            }
            return flags;
        }

        private static ColorOffsets Read_ColorOffsets(Stream s, Flags flags)
        {
            var offsets = new ColorOffsets();

            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                //Top Solid
                {
                    s.Position = 0x14;
                    offsets.TopBackground = br.ReadUInt32();
                }

                // Cursor
                {
                    s.Position = 0x30;
                    offsets.Cursor = br.ReadUInt32();
                }

                // 3D Folder
                {
                    s.Position = 0x38;
                    offsets.Folder = br.ReadUInt32();
                }

                // 3D File
                {
                    s.Position = 0x4C;
                    offsets.File = br.ReadUInt32();
                }

                // Arrow Button
                {
                    s.Position = 0x60;
                    offsets.ArrowButton = br.ReadUInt32();
                }

                // Arrows
                {
                    s.Position = 0x68;
                    offsets.Arrow = br.ReadUInt32();
                }

                // Open Button
                {
                    s.Position = 0x70;
                    offsets.Open = br.ReadUInt32();
                    offsets.Close = br.ReadUInt32();
                }

                // Game Text
                {
                    s.Position = 0x7C;
                    offsets.GameText = br.ReadUInt32();
                }

                // Bottom Solid
                {
                    s.Position = 0x84;
                    offsets.BottomSolid = br.ReadUInt32();
                }

                // Bottom Outer
                {
                    s.Position = 0x8C;
                    offsets.BottomOuter = br.ReadUInt32();
                }

                // Folder BG
                {
                    s.Position = 0x94;
                    offsets.FolderBackground = br.ReadUInt32();
                }

                // Folder Arr
                {
                    s.Position = 0x9C;
                    offsets.FolderArrow = br.ReadUInt32();
                }

                // Bottom Corner
                {
                    s.Position = 0xA4;
                    offsets.BottomCornerButton = br.ReadUInt32();
                }

                // Top Corner
                {
                    s.Position = 0xAC;
                    offsets.TopCornerButton = br.ReadUInt32();
                }

                // Demo Text
                {
                    s.Position = 0xB4;
                    offsets.DemoText = br.ReadUInt32();
                }
            }
            return offsets;
        }

        private static Colors Read_Colors(Stream s, Flags flags, ColorOffsets offsets)
        {
            var colors = new Colors();

            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                // Top Solid
#if SKIP_COLOR_NOT_SET
                if (flags.TopDrawType == TopDrawType.SolidColor || flags.TopDrawType == TopDrawType.SolidColorTexture)
#endif
                {
                    s.Position = offsets.TopBackground;
                    var sevenBytes = flags.TopDrawType == TopDrawType.SolidColorTexture;
                    colors.TopBackground = TopBackgroundSet.Read(br, sevenBytes);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.TopBackground = new TopBackgroundSet();
                }
#endif

                // Cursor
#if SKIP_COLOR_NOT_SET
                if (flags.CursorColor)
#endif

                {
                    s.Position = offsets.Cursor;
                    colors.Cursor = CursorSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.Cursor = new CursorSet();
                }
#endif

                // 3D Folder
#if SKIP_COLOR_NOT_SET
                if (flags.FolderColor)
#endif

                {
                    s.Position = offsets.Folder;
                    colors.Folder = FolderSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.Folder = new FolderSet();
                }
#endif

                // 3D File
#if SKIP_COLOR_NOT_SET
                if (flags.FileColor)
#endif

                {
                    s.Position = offsets.File;
                    colors.File = FileSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.File = new FileSet();
                }
#endif

                // Arrow Button
#if SKIP_COLOR_NOT_SET
                if (flags.ArrowButtonColor)
#endif

                {
                    s.Position = offsets.ArrowButton;
                    colors.ArrowButton = ArrowButtonSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.ArrowButton = new ArrowButtonSet();
                }
#endif

                // Arrows
#if SKIP_COLOR_NOT_SET
                if (flags.ArrowColor)
#endif

                {
                    s.Position = offsets.Arrow;
                    colors.Arrow = ArrowSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.Arrow = new ArrowSet();
                }
#endif

                // Open Button
#if SKIP_COLOR_NOT_SET
                if (flags.OpenCloseColor)
#endif

                {
                    s.Position = offsets.Open;
                    colors.Open = OpenCloseSet.Read(br);

                    s.Position = offsets.Close;
                    colors.Close = OpenCloseSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.Open = new OpenCloseSet();
                    colors.Close = new OpenCloseSet();
                }
#endif

                // Game Text
#if SKIP_COLOR_NOT_SET
                if (flags.GameTextColor)
#endif

                {
                    s.Position = offsets.GameText;
                    colors.GameText = GameTextSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.GameText = new GameTextSet();
                }
#endif

                // Bottom Solid
#if SKIP_COLOR_NOT_SET
                if (flags.BottomBackgroundInnerColor)
#endif

                {
                    s.Position = offsets.BottomSolid;
                    colors.BottomBackgroundInner = BottomBackgroundInnerSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.BottomBackgroundInner = new BottomBackgroundInnerSet();
                }
#endif

                // Bottom Outer
#if SKIP_COLOR_NOT_SET
                if (flags.BottomBackgroundOuterColor)
#endif

                {
                    s.Position = offsets.BottomOuter;
                    colors.BottomBackgroundOuter = BottomBackgroundOuterSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.BottomBackgroundOuter = new BottomBackgroundOuterSet();
                }
#endif

                // Folder BG
#if SKIP_COLOR_NOT_SET
                if (flags.FolderBackgroundColor)
#endif

                {
                    s.Position = offsets.FolderBackground;
                    colors.FolderBackground = BottomBackgroundInnerSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.FolderBackground = new BottomBackgroundInnerSet();
                }
#endif

                // Folder Arr
#if SKIP_COLOR_NOT_SET
                if (flags.FolderArrowColor)
#endif

                {
                    s.Position = offsets.FolderArrow;
                    colors.FolderArrow = FolderArrowSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.FolderArrow = new FolderArrowSet();
                }
#endif

                // Bottom Corner
#if SKIP_COLOR_NOT_SET
                if (flags.BottomCornerButtonColor)
#endif

                {
                    s.Position = offsets.BottomCornerButton;
                    colors.BottomCornerButton = BottomCorner.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.BottomCornerButton = new BottomCorner();
                }
#endif

                // Top Corner
#if SKIP_COLOR_NOT_SET
                if (flags.TopCornerButtonColor)
#endif

                {
                    s.Position = offsets.TopCornerButton;
                    colors.TopCornerButton = TopCornerSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.TopCornerButton = new TopCornerSet();
                }
#endif

                // Demo Text
#if SKIP_COLOR_NOT_SET
                if (flags.DemoTextColor)
#endif

                {
                    s.Position = offsets.DemoText;
                    colors.DemoText = DemoTextSet.Read(br);
                }
#if SKIP_COLOR_NOT_SET
                else
                {
                    colors.DemoText = new DemoTextSet();
                }
#endif
            }
            return colors;
        }

        private static TextureOffsets Read_TextureOffsets(Stream s, Flags flags)
        {
            var offsets = new TextureOffsets();

            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {

                {
                    s.Position = 0x18;
                    offsets.Top = br.ReadUInt32();
                    offsets.TopExt = br.ReadUInt32();
                }

                {
                    s.Position = 0x28;
                    offsets.Bottom = br.ReadUInt32();
                }

                {
                    s.Position = 0x40;
                    offsets.FolderClosed = br.ReadUInt32();
                    offsets.FolderOpen = br.ReadUInt32();
                }

                {
                    s.Position = 0x54;
                    offsets.FileLarge = br.ReadUInt32();
                    offsets.FileSmall = br.ReadUInt32();
                }
            }
            return offsets;
        }

        public static void Write(Theme b, Stream s)
        {
            if (!s.CanSeek || !s.CanWrite)
                throw new ArgumentException("Provided Stream can't Seek or Write", nameof(s));

            int start = (int) s.Position;

            using (BinaryWriter bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                // Data Pad
                bw.Seek(0xD0 + start, SeekOrigin.Current);

                // Write Data
                var cOffs = Write_Colors(s, b);
                var tOffs = Write_Textures(s, b);
                var wOff = Write_CWavs(s, b);

                // Offset into Start
                cOffs.Offset(-start);
                tOffs.Offset(-start);
                wOff = (uint) (wOff - start);

                // Header Offset
                bw.Seek(0 + start, SeekOrigin.Begin);

                // Write Header
                Write_Flags(s, b, tOffs, cOffs, wOff);

                // 0x10c Pad
                int rem = (int) (s.Position % 0x10);
                bw.Seek(rem - 1, SeekOrigin.End);
                bw.Write((byte) 0);
            }
        }

        private static uint Write_CWavs(Stream s, Theme theme)
        {
            var pos = s.Position;
            s.Write(theme.CWAV, 0, theme.CWAV.Length);
            return (uint) pos;
        }

        private static void Write_Flags
            (Stream s, Theme body, TextureOffsets tOff, ColorOffsets cOff, uint wOff)
        {
            using (BinaryWriter bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                // Version
                bw.Write(1);
                // BGM Flag
                bw.Write((byte) 0);
                bw.Write((byte) (body.Flags.BackgroundMusic ? 1 : 0));
                bw.Write((byte) 0);
                bw.Write((byte) 0);
                // Unkown
                bw.Write((uint) 0);
                // Top Screen
                bw.Write((uint) body.Flags.TopDrawType);
                bw.Write((uint) body.Flags.TopFrameType);
                bw.Write((uint) cOff.TopBackground);
                bw.Write((uint) tOff.Top);
                bw.Write((uint) tOff.TopExt);
                // Bottom Screen
                bw.Write((uint) body.Flags.BottomDrawType);
                bw.Write((uint) body.Flags.BottomFrameType);
                bw.Write((uint) tOff.Bottom);
                // Cursor
                bw.Write((uint) (body.Flags.CursorColor ? 1 : 0));
                bw.Write((uint) cOff.Cursor);
                // 3D Folder
                bw.Write((uint) (body.Flags.FolderColor ? 1 : 0));
                bw.Write((uint) cOff.Folder);
                // 2D Folder
                bw.Write((uint) (body.Flags.FolderTexture ? 1 : 0));
                bw.Write((uint) tOff.FolderClosed);
                bw.Write((uint) tOff.FolderOpen);
                // 3D File
                bw.Write((uint) (body.Flags.FileColor ? 1 : 0));
                bw.Write((uint) cOff.File);
                // 2D File
                bw.Write((uint) (body.Flags.FileTexture ? 1 : 0));
                bw.Write((uint) tOff.FileLarge);
                bw.Write((uint) tOff.FileSmall);
                // Arrow Button
                bw.Write((uint) (body.Flags.ArrowButtonColor ? 1 : 0));
                bw.Write((uint) cOff.ArrowButton);
                // Arrow
                bw.Write((uint) (body.Flags.ArrowColor ? 1 : 0));
                bw.Write((uint) cOff.Arrow);
                // Open Close
                bw.Write((uint) (body.Flags.OpenCloseColor ? 1 : 0));
                bw.Write((uint) cOff.Open);
                bw.Write((uint) cOff.Close);
                // Game Text
                bw.Write((uint) (body.Flags.GameTextColor ? 1 : 0));
                bw.Write((uint) cOff.GameText);
                // Bottom Solid
                bw.Write((uint) (body.Flags.BottomBackgroundInnerColor ? 1 : 0));
                bw.Write((uint) cOff.BottomSolid);
                // Bottom Outer
                bw.Write((uint) (body.Flags.BottomBackgroundOuterColor ? 1 : 0));
                bw.Write((uint) cOff.BottomOuter);
                // Folder BG
                bw.Write((uint) (body.Flags.FolderBackgroundColor ? 1 : 0));
                bw.Write((uint) cOff.FolderBackground);
                // Folder Arrow
                bw.Write((uint) (body.Flags.FolderArrowColor ? 1 : 0));
                bw.Write((uint) cOff.FolderArrow);
                // Bottom Corner
                bw.Write((uint) (body.Flags.BottomCornerButtonColor ? 1 : 0));
                bw.Write((uint) cOff.BottomCornerButton);
                // Top Corner
                bw.Write((uint) (body.Flags.TopCornerButtonColor ? 1 : 0));
                bw.Write((uint) cOff.TopCornerButton);
                // Demo Text
                bw.Write((uint) (body.Flags.DemoTextColor ? 1 : 0));
                bw.Write((uint) cOff.DemoText);
                // SFX
                bw.Write((uint) (body.Flags.SoundEffect ? 1 : 0));
                bw.Write((uint) body.CWAV.Length);
                bw.Write((uint) wOff);
            }
        }

        private static ColorOffsets Write_Colors(Stream s, Theme b)
        {
            var offsets = new ColorOffsets();

            using (BinaryWriter bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                // Top Solid
#if SKIP_COLOR_NOT_SET
                if (b.Flags.TopDrawType == TopDrawType.SolidColor || b.Flags.TopDrawType == TopDrawType.SolidColorTexture)
                #endif
                {
                    offsets.TopBackground = (uint) s.Position;
                    var seven = b.Flags.TopDrawType == TopDrawType.SolidColorTexture;
                    b.Colors.TopBackground.Write(bw, seven);
                }
                // Cursor
#if SKIP_COLOR_NOT_SET
                if (b.Flags.CursorColor)
#endif
                {
                    offsets.Cursor = (uint) s.Position;
                    b.Colors.Cursor.Write(bw);
                }
                // 3D Folder
#if SKIP_COLOR_NOT_SET
                if (b.Flags.FolderColor)
#endif
                {
                    offsets.Folder = (uint) s.Position;
                    b.Colors.Folder.Write(bw);
                }
                // 3D File
#if SKIP_COLOR_NOT_SET
                if (b.Flags.FileColor)
#endif
                {
                    offsets.File = (uint) s.Position;
                    b.Colors.File.Write(bw);
                }
                // Arrow Button
#if SKIP_COLOR_NOT_SET
                if (b.Flags.ArrowButtonColor)
#endif
                {
                    offsets.ArrowButton = (uint) s.Position;
                    b.Colors.ArrowButton.Write(bw);
                }
                // Arrow
#if SKIP_COLOR_NOT_SET
                if (b.Flags.ArrowColor)
#endif
                {
                    offsets.Arrow = (uint) s.Position;
                    b.Colors.Arrow.Write(bw);
                }
                // Open Close Button
#if SKIP_COLOR_NOT_SET
                if (b.Flags.OpenCloseColor)
#endif
                {
                    offsets.Open = (uint) s.Position;
                    b.Colors.Open.Write(bw);
                    offsets.Close = (uint) s.Position;
                    b.Colors.Close.Write(bw);
                }
                // Game Text
#if SKIP_COLOR_NOT_SET
                if (b.Flags.GameTextColor)
#endif
                {
                    offsets.GameText = (uint) s.Position;
                    b.Colors.GameText.Write(bw);
                }
                // Bottom Solid
#if SKIP_COLOR_NOT_SET
                if (b.Flags.BottomBackgroundInnerColor)
#endif
                {
                    offsets.BottomSolid = (uint) s.Position;
                    b.Colors.BottomBackgroundInner.Write(bw);
                }
                // Bottom Outer
#if SKIP_COLOR_NOT_SET
                if (b.Flags.BottomBackgroundOuterColor)
#endif
                {
                    offsets.BottomOuter = (uint) s.Position;
                    b.Colors.BottomBackgroundOuter.Write(bw);
                }
                // Folder BG
#if SKIP_COLOR_NOT_SET
                if (b.Flags.FolderBackgroundColor)
#endif
                {
                    offsets.FolderBackground = (uint) s.Position;
                    b.Colors.FolderBackground.Write(bw);
                }
                // Folder Arrow
#if SKIP_COLOR_NOT_SET
                if (b.Flags.FolderArrowColor)
#endif
                {
                    offsets.FolderArrow = (uint) s.Position;
                    b.Colors.FolderArrow.Write(bw);
                }
                // Bottom Corner
#if SKIP_COLOR_NOT_SET
                if (b.Flags.BottomCornerButtonColor)
#endif
                {
                    offsets.BottomCornerButton = (uint) s.Position;
                    b.Colors.BottomCornerButton.Write(bw);
                }
                // Top Corner
#if SKIP_COLOR_NOT_SET
                if (b.Flags.TopCornerButtonColor)
#endif
                {
                    offsets.TopCornerButton = (uint) s.Position;
                    b.Colors.TopCornerButton.Write(bw);
                }
                // Demo Text
#if SKIP_COLOR_NOT_SET
                if (b.Flags.DemoTextColor)
#endif
                {
                    offsets.DemoText = (uint) s.Position;
                    b.Colors.DemoText.Write(bw);
                }
            }
            return offsets;
        }

        private static TextureOffsets Write_Textures(Stream s, Theme b)
        {
            TextureOffsets offsets = new TextureOffsets();

            // Top
            if (b.Flags.TopDrawType == TopDrawType.Texture
                && b.Textures.Top.Format != RawTexture.DataFormat.Invalid)
            {
                offsets.Top = (uint) s.Position;
                var data = b.Textures.Top.Data;
                s.Write(data, 0, data.Length);
            }
            else if (b.Flags.TopDrawType == TopDrawType.SolidColorTexture
                     && b.Textures.TopExt.Format != RawTexture.DataFormat.Invalid)
            {
                offsets.TopExt = (uint) s.Position;
                var data = b.Textures.TopExt.Data;
                s.Write(data, 0, data.Length);
            }

            // Bottom
            if (b.Flags.BottomDrawType == BottomDrawType.Texture
                && b.Textures.Bottom.Format != RawTexture.DataFormat.Invalid)
            {
                offsets.Bottom = (uint) s.Position;
                var data = b.Textures.Bottom.Data;
                s.Write(data, 0, data.Length);
            }

            // Folder
            if (b.Flags.FolderTexture
                && b.Textures.FolderClosed.Format != RawTexture.DataFormat.Invalid
                && b.Textures.FolderOpen.Format != RawTexture.DataFormat.Invalid)
            {
                offsets.FolderClosed = (uint) s.Position;
                var data = b.Textures.FolderClosed.Data;
                s.Write(data, 0, data.Length);

                offsets.FolderOpen = (uint) s.Position;
                data = b.Textures.FolderOpen.Data;
                s.Write(data, 0, data.Length);
            }

            // Files
            if (b.Flags.FileTexture
                && b.Textures.FileLarge.Format != RawTexture.DataFormat.Invalid
                && b.Textures.FileSmall.Format != RawTexture.DataFormat.Invalid)
            {
                offsets.FileLarge = (uint) s.Position;
                var data = b.Textures.FileLarge.Data;
                s.Write(data, 0, data.Length);

                offsets.FileSmall = (uint) s.Position;
                data = b.Textures.FileSmall.Data;
                s.Write(data, 0, data.Length);
            }

            return offsets;
        }
    }
}
