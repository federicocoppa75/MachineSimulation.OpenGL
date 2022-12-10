using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Materials
{
    public class Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }


        public Color(byte red, byte green, byte blue, byte alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public static Color FromBgra(int color)
        {
            return new Color((byte)((uint)(color >> 16) & 0xFFu), (byte)((uint)(color >> 8) & 0xFFu), (byte)((uint)color & 0xFFu), (byte)((uint)(color >> 24) & 0xFFu));
        }

        public static Color FromBgra(uint color)
        {
            return FromBgra((int)color);
        }

        public static Color FromAbgr(int color)
        {
            return new Color((byte)(color >> 24), (byte)(color >> 16), (byte)(color >> 8), (byte)color);
        }

        public static implicit operator Vector4(Color c) => new Vector4(c.R, c.G, c.B, c.A);

        public static readonly Color Zero = FromBgra(0);

        public static readonly Color Transparent = FromBgra(0);

        public static readonly Color AliceBlue = FromBgra(4293982463u);

        public static readonly Color AntiqueWhite = FromBgra(4294634455u);

        public static readonly Color Aqua = FromBgra(4278255615u);

        public static readonly Color Aquamarine = FromBgra(4286578644u);

        public static readonly Color Azure = FromBgra(4293984255u);

        public static readonly Color Beige = FromBgra(4294309340u);

        public static readonly Color Bisque = FromBgra(4294960324u);

        public static readonly Color Black = FromBgra(4278190080u);

        public static readonly Color BlanchedAlmond = FromBgra(4294962125u);

        public static readonly Color Blue = FromBgra(4278190335u);

        public static readonly Color BlueViolet = FromBgra(4287245282u);

        public static readonly Color Brown = FromBgra(4289014314u);

        public static readonly Color BurlyWood = FromBgra(4292786311u);

        public static readonly Color CadetBlue = FromBgra(4284456608u);

        public static readonly Color Chartreuse = FromBgra(4286578432u);

        public static readonly Color Chocolate = FromBgra(4291979550u);

        public static readonly Color Coral = FromBgra(4294934352u);

        public static readonly Color CornflowerBlue = FromBgra(4284782061u);

        public static readonly Color Cornsilk = FromBgra(4294965468u);

        public static readonly Color Crimson = FromBgra(4292613180u);

        public static readonly Color Cyan = FromBgra(4278255615u);

        public static readonly Color DarkBlue = FromBgra(4278190219u);

        public static readonly Color DarkCyan = FromBgra(4278225803u);

        public static readonly Color DarkGoldenrod = FromBgra(4290283019u);

        public static readonly Color DarkGray = FromBgra(4289309097u);

        public static readonly Color DarkGreen = FromBgra(4278215680u);

        public static readonly Color DarkKhaki = FromBgra(4290623339u);

        public static readonly Color DarkMagenta = FromBgra(4287299723u);

        public static readonly Color DarkOliveGreen = FromBgra(4283788079u);

        public static readonly Color DarkOrange = FromBgra(4294937600u);

        public static readonly Color DarkOrchid = FromBgra(4288230092u);

        public static readonly Color DarkRed = FromBgra(4287299584u);

        public static readonly Color DarkSalmon = FromBgra(4293498490u);

        public static readonly Color DarkSeaGreen = FromBgra(4287609995u);

        public static readonly Color DarkSlateBlue = FromBgra(4282924427u);

        public static readonly Color DarkSlateGray = FromBgra(4281290575u);

        public static readonly Color DarkTurquoise = FromBgra(4278243025u);

        public static readonly Color DarkViolet = FromBgra(4287889619u);

        public static readonly Color DeepPink = FromBgra(4294907027u);

        public static readonly Color DeepSkyBlue = FromBgra(4278239231u);

        public static readonly Color DimGray = FromBgra(4285098345u);

        public static readonly Color DodgerBlue = FromBgra(4280193279u);

        public static readonly Color Firebrick = FromBgra(4289864226u);

        public static readonly Color FloralWhite = FromBgra(4294966000u);

        public static readonly Color ForestGreen = FromBgra(4280453922u);

        public static readonly Color Fuchsia = FromBgra(4294902015u);

        public static readonly Color Gainsboro = FromBgra(4292664540u);

        public static readonly Color GhostWhite = FromBgra(4294506751u);

        public static readonly Color Gold = FromBgra(4294956800u);

        public static readonly Color Goldenrod = FromBgra(4292519200u);

        public static readonly Color Gray = FromBgra(4286611584u);

        public static readonly Color Green = FromBgra(4278222848u);

        public static readonly Color GreenYellow = FromBgra(4289593135u);

        public static readonly Color Honeydew = FromBgra(4293984240u);

        public static readonly Color HotPink = FromBgra(4294928820u);

        public static readonly Color IndianRed = FromBgra(4291648604u);

        public static readonly Color Indigo = FromBgra(4283105410u);

        public static readonly Color Ivory = FromBgra(4294967280u);

        public static readonly Color Khaki = FromBgra(4293977740u);

        public static readonly Color Lavender = FromBgra(4293322490u);

        public static readonly Color LavenderBlush = FromBgra(4294963445u);

        public static readonly Color LawnGreen = FromBgra(4286381056u);

        public static readonly Color LemonChiffon = FromBgra(4294965965u);

        public static readonly Color LightBlue = FromBgra(4289583334u);

        public static readonly Color LightCoral = FromBgra(4293951616u);

        public static readonly Color LightCyan = FromBgra(4292935679u);

        public static readonly Color LightGoldenrodYellow = FromBgra(4294638290u);

        public static readonly Color LightGray = FromBgra(4292072403u);

        public static readonly Color LightGreen = FromBgra(4287688336u);

        public static readonly Color LightPink = FromBgra(4294948545u);

        public static readonly Color LightSalmon = FromBgra(4294942842u);

        public static readonly Color LightSeaGreen = FromBgra(4280332970u);

        public static readonly Color LightSkyBlue = FromBgra(4287090426u);

        public static readonly Color LightSlateGray = FromBgra(4286023833u);

        public static readonly Color LightSteelBlue = FromBgra(4289774814u);

        public static readonly Color LightYellow = FromBgra(4294967264u);

        public static readonly Color Lime = FromBgra(4278255360u);

        public static readonly Color LimeGreen = FromBgra(4281519410u);

        public static readonly Color Linen = FromBgra(4294635750u);

        public static readonly Color Magenta = FromBgra(4294902015u);

        public static readonly Color Maroon = FromBgra(4286578688u);

        public static readonly Color MediumAquamarine = FromBgra(4284927402u);

        public static readonly Color MediumBlue = FromBgra(4278190285u);

        public static readonly Color MediumOrchid = FromBgra(4290401747u);

        public static readonly Color MediumPurple = FromBgra(4287852763u);

        public static readonly Color MediumSeaGreen = FromBgra(4282168177u);

        public static readonly Color MediumSlateBlue = FromBgra(4286277870u);

        public static readonly Color MediumSpringGreen = FromBgra(4278254234u);

        public static readonly Color MediumTurquoise = FromBgra(4282962380u);

        public static readonly Color MediumVioletRed = FromBgra(4291237253u);

        public static readonly Color MidnightBlue = FromBgra(4279834992u);

        public static readonly Color MintCream = FromBgra(4294311930u);

        public static readonly Color MistyRose = FromBgra(4294960353u);

        public static readonly Color Moccasin = FromBgra(4294960309u);

        public static readonly Color NavajoWhite = FromBgra(4294958765u);

        public static readonly Color Navy = FromBgra(4278190208u);

        public static readonly Color OldLace = FromBgra(4294833638u);

        public static readonly Color Olive = FromBgra(4286611456u);

        public static readonly Color OliveDrab = FromBgra(4285238819u);

        public static readonly Color Orange = FromBgra(4294944000u);

        public static readonly Color OrangeRed = FromBgra(4294919424u);

        public static readonly Color Orchid = FromBgra(4292505814u);

        public static readonly Color PaleGoldenrod = FromBgra(4293847210u);

        public static readonly Color PaleGreen = FromBgra(4288215960u);

        public static readonly Color PaleTurquoise = FromBgra(4289720046u);

        public static readonly Color PaleVioletRed = FromBgra(4292571283u);

        public static readonly Color PapayaWhip = FromBgra(4294963157u);

        public static readonly Color PeachPuff = FromBgra(4294957753u);

        public static readonly Color Peru = FromBgra(4291659071u);

        public static readonly Color Pink = FromBgra(4294951115u);

        public static readonly Color Plum = FromBgra(4292714717u);

        public static readonly Color PowderBlue = FromBgra(4289781990u);

        public static readonly Color Purple = FromBgra(4286578816u);

        public static readonly Color Red = FromBgra(4294901760u);

        public static readonly Color RosyBrown = FromBgra(4290547599u);

        public static readonly Color RoyalBlue = FromBgra(4282477025u);

        public static readonly Color SaddleBrown = FromBgra(4287317267u);

        public static readonly Color Salmon = FromBgra(4294606962u);

        public static readonly Color SandyBrown = FromBgra(4294222944u);

        public static readonly Color SeaGreen = FromBgra(4281240407u);

        public static readonly Color SeaShell = FromBgra(4294964718u);

        public static readonly Color Sienna = FromBgra(4288696877u);

        public static readonly Color Silver = FromBgra(4290822336u);

        public static readonly Color SkyBlue = FromBgra(4287090411u);

        public static readonly Color SlateBlue = FromBgra(4285160141u);

        public static readonly Color SlateGray = FromBgra(4285563024u);

        public static readonly Color Snow = FromBgra(4294966010u);

        public static readonly Color SpringGreen = FromBgra(4278255487u);

        public static readonly Color SteelBlue = FromBgra(4282811060u);

        public static readonly Color Tan = FromBgra(4291998860u);

        public static readonly Color Teal = FromBgra(4278222976u);

        public static readonly Color Thistle = FromBgra(4292394968u);

        public static readonly Color Tomato = FromBgra(4294927175u);

        public static readonly Color Turquoise = FromBgra(4282441936u);

        public static readonly Color Violet = FromBgra(4293821166u);

        public static readonly Color Wheat = FromBgra(4294303411u);

        public static readonly Color White = FromBgra(uint.MaxValue);

        public static readonly Color WhiteSmoke = FromBgra(4294309365u);

        public static readonly Color Yellow = FromBgra(4294967040u);

        public static readonly Color YellowGreen = FromBgra(4288335154u);

    }
}
