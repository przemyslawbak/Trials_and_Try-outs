﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class ColorNames
    {
        public List<string> SvgColors { get; set; }

        public ColorNames()
        {
            SvgColors = GetColorNames();
        }

        private List<string> GetColorNames()
        {
            var colors = new List<string>
            {
                "aliceBlue",
                "antiqueWhite",
"aqua",
"aquamarine",
"azure",
"beige",
"bisque",
"black",
"blanchedAlmond",
"blue",
"blueViolet",
"brown",
"burlywood",
"cadetBlue",
"chartreuse",
"chocolate",
"coral",
"cornflowerBlue",
"cornsilk",
"crimson",
"cyan",
"darkBlue",
"darkCyan",
"darkGoldenrod",
"darkGray",
"darkGreen",
"darkGrey",
"darkKhaki",
"darkMagenta",
"darkOliveGreen",
"darkOrange",
"darkOrchid",
"darkRed",
"darkSalmon",
"darkSeaGreen",
"darkSlateBlue",
"darkSlateGray",
"darkSlateGrey",
"darkTurquoise",
"darkViolet",
"deepPink",
"deepSkyBlue",
"dimGray",
"dimGrey",
"dodgerBlue",
"firebrick",
"floralWhite",
"forestGreen",
"fuchsia",
"gainsboro",
"ghostWhite",
"gold",
"goldenrod",
"gray",
"green",
"greenYellow",
"grey",
"honeydew",
"hotPink",
"indianRed",
"indigo",
"ivory",
"khaki",
"lavender",
"lavenderBlush",
"lawnGreen",
"lemonChiffon",
"lightBlue",
"lightCoral",
"lightCyan",
"lightGoldenrodYellow",
"lightGray",
"lightGreen",
"lightGrey",
"lightPink",
"lightSalmon",
"lightSeaGreen",
"lightSkyBlue",
"lightSlateGray",
"lightSlateGrey",
"lightSteelBlue",
"lightYellow",
"lime",
"limeGreen",
"linen",
"magenta",
"maroon",
"mediumAquamarine",
"mediumBlue",
"mediumOrchid",
"mediumPurple",
"mediumSeaGreen",
"mediumSlateBlue",
"mediumSpringGreen",
"mediumTurquoise",
"mediumVioletRed",
"midnightBlue",
"mintCream",
"mistyRose",
"moccasin",
"navajoWhite",
"navy",
"oldLace",
"olive",
"oliveDrab",
"orange",
"orangeRed",
"orchid",
"paleGoldenrod",
"paleGreen",
"paleTurquoise",
"paleVioletRed",
"papayaWhip",
"peachPuff",
"peru",
"pink",
"plum",
"powderBlue",
"purple",
"red",
"rosyBrown",
"royalBlue",
"saddleBrown",
"salmon",
"sandyBrown",
"seaGreen",
"seashell",
"sienna",
"silver",
"skyBlue",
"slateBlue",
"slateGray",
"slateGrey",
"snow",
"springGreen",
"steelBlue",
"tan",
"teal",
"thistle",
"tomato",
"turquoise",
"violet",
"wheat",
"white",
"whiteSmoke",
"yellow",
"yellowGreen"
            };
            return colors;
        }
    }
}
