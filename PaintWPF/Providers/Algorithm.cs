using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PaintWPF.Providers;
public delegate int ColorCallback(int x, int y);

public static class Algorithm
{
    public static Bitmap Gradient()
    {
        (ColorCallback r, ColorCallback g, ColorCallback b) = _values[_index++ % _values.Length];
        return Gradient(r, g, b);
    }

    public unsafe static Bitmap Gradient(
            ColorCallback red,
            ColorCallback green,
            ColorCallback blue
        )
    {
        var bmp = new Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        var data = bmp.LockBits(
            new Rectangle(Point.Empty, bmp.Size),
            ImageLockMode.ReadWrite,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );

        byte* bytes = (byte*)data.Scan0.ToPointer();

        int len = data.Stride * data.Height;

        for (int y = 0; y < data.Height; y++)
        {
            int o = y * data.Stride;

            for (int x = 0; x < data.Width; x++)
            {
                bytes[o + x * 3 + 0] = (byte)red(x, y);
                bytes[o + x * 3 + 1] = (byte)green(x, y);
                bytes[o + x * 3 + 2] = (byte)blue(x, y);
            }
        }

        bmp.UnlockBits(data);
        return bmp;
    }

    private static int _index = 0;
    private static readonly (ColorCallback r, ColorCallback g, ColorCallback b)[] _values = new (ColorCallback r, ColorCallback g, ColorCallback b)[]
    {
        ((x, y) => x, (x, y) => y, (x, y) => 0),
        ((x, y) => 0, (x, y) => x, (x, y) => y),
        ((x, y) => y, (x, y) => 0, (x, y) => x),
        ((x, y) => y, (x, y) => x, (x, y) => 0),
        ((x, y) => x, (x, y) => 0, (x, y) => y),
        ((x, y) => 0, (x, y) => y, (x, y) => x),
    };
}
