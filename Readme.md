### Usagi 3DS Theme Editor

Support/Discussion Topic at [GBATemp](https://gbatemp.net/threads/412233/)

WPF Based, Theme Editor for Nintendo 3DS
- Requires .NET Framework 4.5

---
#### Features

* **Image Palette**: Select colors from the *(up to)* 20 most used colors on the Top/Bottom Screen Background Images
* **Animated Live Preview**: See Changes in Realtime, Animation is Toggleable
* **BGM Preview**: Preview Only, *No BCSTM Conversion Support*
* **Exports Preview Image**: Upon Exporting, preview.png will be generated
* **Image Dithering**: Reduced Banding, Bayer8x8 Ordered, [Example (2x Nearest Neighbor)][DITHERING]
* **Top Screen Render Modes**: None, Solid Color, Solid Color & Texture, Texture
* **Bottom Screen Render Modes**: None, Solid Color, Texture
* **Top Screen Frame Types**: Fixed, Slow/Fast Scroll
* **Bottom Screen Render Modes**: Fixed, Slow/Fast Scroll, Flip/Bounce Tiled
* **Bottom Screen UI Elements**: Folder and Icon Borders, Solid Color and Textured
* **Experimental CWAV Manager**: Add Sound Effects to your themes, optional support for ctr_WaveConverter (Required for Wav Conversion)
* **SMDH Generation**: Loads/Creates info.smdh file for CHMM2

#### Localization

Avaiable on the Following Languages:

* English
* Portuguese (Brazil)
* Spanish (by dsoldier @ GBATemp)
* Italian (by RayFirefist @ GBATemp)

---
### ThemeEditor Common

Object Oriented, Standalone Library for Handling the Theme Files
- Requires .NET Framework 4.5

#### Features

* **LZ11 De/Compression**
* **Texture Encoding/Decoding**
* **Image Data Operations**: Dithering, Blitting, Rotating, Palette Generation, Grayscale
* **Theme Reading/Writing**

---
#### Credits

* **Halley Comet Software** - [vgmstream] - BGM Preview
* **Reisyukaku** - [YATA] - LZ11 De/Compression, Texture Decoding
* **kwsch** - [pk3DS] - LZ11 De/Compression
* **Barubary** - [DSDecmp] - LZ11 De/Compression
* **3DBrew WIKI** - [WIKI] - File Formats Specification

[vgmstream]:https://www.hcs64.com/vgmstream.html
[DSDecmp]:https://github.com/Barubary/dsdecmp/tree/master/CSharp/DSDecmp
[pk3DS]:https://github.com/kwsch/pk3DS/blob/master/pk3DS/3DS/LZSS.cs
[YATA]:https://github.com/Reisyukaku/YATA
[WIKI]:https://www.3dbrew.org/wiki/
[DITHERING]:http://i.imgur.com/W6wcvhS.png