using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Icons
{
    public static class CommonIcons
    {
        public static MarkupString ExpandLess = 
            (MarkupString)"<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24\" viewBox=\"0 -960 960 960\" width=\"24\"><path d=\"m296-345-56-56 240-240 240 240-56 56-184-184-184 184Z\"/></svg>";

        public static MarkupString ExpandMore =
            (MarkupString)"<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24\" viewBox=\"0 -960 960 960\" width=\"24\"><path d=\"M480-345 240-585l56-56 184 184 184-184 56 56-240 240Z\"/></svg>";
    }
}
