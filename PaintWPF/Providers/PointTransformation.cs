using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace PaintWPF.Providers;
internal class PointTransformation
{
    public static BitmapImage? Add(BitmapSource bitmapSource, Color color)
    {
        return ModifyInRGB(bitmapSource, color, (b, v) => b + v);
    }
    public static BitmapImage? Subtract(BitmapSource bitmapSource, Color color)
    {
        return ModifyInRGB(bitmapSource, color, (b, v) => b - v);
    }
    public static BitmapImage? Multiply(BitmapSource bitmapSource, Color color)
    {
        return ModifyInRGB(bitmapSource, color, (b, v) => b * v);
    }
    public static BitmapImage? Divide(BitmapSource bitmapSource, Color color)
    {
        return ModifyInRGB(bitmapSource, color, (b, v) => b / v);
    }
    public static BitmapImage? ChangeBrightness(BitmapSource bitmapSource, int brightness)
    {
        return ModifyInRGB(bitmapSource, Color.FromArgb(brightness, brightness, brightness), (b, v) => b * v / 100);
    }
    public static BitmapImage? ConvertToGrayscaleRGBAvarage(BitmapSource bitmapSource)
    {
        return GrayscaleModulation(bitmapSource, (r, g, b) => (r + g + b) / 3);
    }
    public static BitmapImage? ConvertToGrayscaleRChannel(BitmapSource bitmapSource)
    {
        return GrayscaleModulation(bitmapSource, (r, g, b) => r);
    }

    private static BitmapImage? ModifyInRGB(BitmapSource? bitmapSource, Color color, Func<int, int, int> operation)
    {
        if (bitmapSource == null)
        {
            return null;
        }

        var bitmap = bitmapSource.ToBitmap();
        if (bitmap == null)
        {
            return null;
        }

        var bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var data = new byte[bitmapData.Stride * bitmapData.Height];
        Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);

        for (int i = 0; i < data.Length; i++)
        {
            byte colorByte;
            switch (i % 4)
            {
                case 0:
                    colorByte = color.B;
                    break;
                case 1:
                    colorByte = color.G;
                    break;
                case 2:
                    colorByte = color.R;
                    break;
                default:
                    continue;
            }

            int value = operation(data[i], colorByte);
            data[i] = (byte)((value < 0) ? 0 : (value > 255) ? 255 : value);
        }

        Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
        bitmap.UnlockBits(bitmapData);
        return bitmap.ToBitmapImage();
    }
    private static BitmapImage? GrayscaleModulation(BitmapSource? bitmapSource, Func<int, int, int, int> operation)
    {
        if (bitmapSource == null)
        {
            return null;
        }

        var bitmap = bitmapSource.ToBitmap();
        if (bitmap == null)
        {
            return null;

        }
        var bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var data = new byte[bitmapData.Stride * bitmapData.Height];
        Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);

        for (int i = 0; i < data.Length; i += 4)
        {
            byte value = (byte)operation(data[i + 2], data[i + 1], data[i]);
            data[i] = value;
            data[i + 1] = value;
            data[i + 2] = value;
        }

        Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
        bitmap.UnlockBits(bitmapData);
        return bitmap.ToBitmapImage();
    }
}
