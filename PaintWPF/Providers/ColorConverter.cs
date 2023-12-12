using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintWPF.Providers;

public static class ColorConverter
{
    public static (float c, float m, float y, float k) HSVToCMYK(float h, float s, float val)
    {
        (int r, int g, int b) = HSVToRGB(h, s, val);
        return RGBToCMYK(r, g, b);
    }
    public static (float h, float s, float val) CMYKToHSV(float c, float m, float y, float k)
    {
        (int r, int g, int b) = CMYKToRGB(c, m, y, k);
        return RGBToHSV(r, g, b);
    }
    public static (float c, float m, float y, float k) RGBToCMYK(int r, int g, int b)
    {
        float rf = r / 255f;
        float gf = g / 255f;
        float bf = b / 255f;

        float k = 1 - (Math.Max(Math.Max(rf, gf), bf));
        if (k == 1)
        {
            return (0, 0, 0, 1);
        }

        float c = (1 - rf - k) / (1 - k);
        float m = (1 - gf - k) / (1 - k);
        float y = (1 - bf - k) / (1 - k);
        return (c, m, y, k);
    }
    public static (int r, int g, int b) CMYKToRGB(float c, float m, float y, float k)
    {
        int r = (int)(255 * (1 - c) * (1 - k));
        int g = (int)(255 * (1 - m) * (1 - k));
        int b = (int)(255 * (1 - y) * (1 - k));
        return (r, g, b);
    }

    public static (int r, int g, int b) HSVToRGB(float h, float s, float val)
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

    public static (float h, float s, float v) RGBToHSV(int r, int g, int b)
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

        if (min == max)
        {
            return (0, 0, value);
        }

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
