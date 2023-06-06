using UnityEngine;

namespace RiskOfOptions.Lib
{
    public static class ColorExtensions
    {
        private const float Scale = 255f;
        
        public static Color FromRGBHex(this Color orig, int hex)
        {
            const int rMask = 0xFF0000;
            const int gMask = 0x00FF00;
            const int bMask = 0x0000FF;

            int r = (hex & rMask) >> 16;
            int g = (hex & gMask) >> 8;
            int b = hex & bMask;

            return new Color(r / Scale, g / Scale, b / Scale, orig.a);
        }

        public static int ToRGBHex(this Color orig)
        {
            int r = (int)(orig.r * Scale) << 16;
            int g = (int)(orig.g * Scale) << 8;
            int b = (int)(orig.b * Scale);

            return r | g | b;
        }
        
        internal static Color ColorFromHSV(float hue, float sat, float val) {           
            float chroma = val * sat;
            float piStep = Mathf.PI / 3f;
            float intermediate = chroma * (1 - Mathf.Abs((float)(hue / piStep % 2.0 - 1)));
            float shift = val - chroma;
                        
            if (hue < 1 * piStep)
                return new Color(shift + chroma, shift + intermediate, shift + 0, 1);
            if (hue < 2 * piStep)
                return new Color(shift + intermediate, shift + chroma, shift + 0, 1);
            if (hue < 3 * piStep)
                return new Color(shift + 0, shift + chroma, shift + intermediate, 1);
            if (hue < 4 * piStep)
                return new Color(shift + 0, shift + intermediate, shift + chroma, 1);
            if (hue < 5 * piStep)
                return new Color(shift + intermediate, shift + 0, shift + chroma, 1);
            return new Color(shift + chroma, shift + 0, shift + intermediate, 1);
        }
        
        /// <param name="orig">Color to perform this on</param>
        /// <param name="inHue">Range 0 - 6.28</param>
        /// <param name="outHue">Range 0 - 6.28</param>
        /// <param name="saturation">Range 0 - 1</param>
        /// <param name="value">Range 0 - 1</param>
        internal static void ToHSV(this Color orig, float inHue, out float outHue, out float saturation, out float value)
        {
            outHue = 0;
            
            float min = Mathf.Min(orig.r, Mathf.Min(orig.g, orig.b));
            float max = Mathf.Max(orig.r, Mathf.Max(orig.g, orig.b));
            float delta = max - min;

            value = max;

            if (delta == 0)
            {
                saturation = 0;
                outHue = inHue.Remap(0, 6.28f, 0, 1);
            }
            else
            {
                saturation = delta / max;

                var rDelta = ((max - orig.r) / 6 + delta / 2) / delta;
                var gDelta = ((max - orig.g) / 6 + delta / 2) / delta;
                var bDelta = ((max - orig.b) / 6 + delta / 2) / delta;

                if (orig.r == delta)
                {
                    outHue = bDelta - gDelta;
                }
                else if (orig.g == delta)
                {
                    outHue = 1 / 3f + rDelta - bDelta;
                }
                else if (orig.b == delta)
                {
                    outHue = 2 / 3f + gDelta - rDelta;
                }

                if (outHue < 0)
                    outHue += 1;

                if (outHue > 1)
                    outHue -= 1;
            }

            outHue = outHue.Remap(0, 1, 0, 6.28f);
        }
    }
}