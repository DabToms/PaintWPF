using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintWPF.Providers;

public static class ColorConverter
{
    public static (int R, int G, int B) HSVToRGB(float h, float s, float val)
    {
        int hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
        double f = h / 60 - Math.Floor(h / 60);

        val = val * 255;
        int v = Convert.ToInt32(val);
        int p = Convert.ToInt32(val * (1 - s));
        int q = Convert.ToInt32(val * (1 - f * s));
        int t = Convert.ToInt32(val * (1 - (1 - f) * s));

        if (hi == 0)
            return (v, t, p);
        else if (hi == 1)
            return (q, v, p);
        else if (hi == 2)
            return (p, v, t);
        else if (hi == 3)
            return (p, q, v);
        else if (hi == 4)
            return (t, p, v);
        else
            return (v, p, q);

    }

    public static (float H, float S, float V) RGBToHSV(int r, int g, int b)
    {
        float value = 0f;
        float hue = 0f;
        float saturation = 0f;

        float R = (float)r / 255;
        float G = (float)g / 255;
        float B = (float)b / 255;

        var min = Math.Min(R, Math.Min(G, B));
        var max = Math.Max(R, Math.Max(G, B));
        value = max;

        if (max == R)
        {
            hue = ((float)G - (float)B) / ((float)max - (float)min) % 6;
        }
        else if (max == G)
        {
            hue = 2f + ((float)B - (float)R) / ((float)max - (float)min);
        }
        else if (max == B)
        {
            hue = 4f + ((float)R - (float)G) / ((float)max - (float)min);
        }
        hue = hue * 60;

        if (value == 0)
        {
            saturation = 0f;
        }
        else
        {
            saturation = (max - min) / max;
        }

        return (hue, saturation, value);
    }
}
