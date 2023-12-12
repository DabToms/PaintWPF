using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PaintWPF.Providers;

internal class FilterProvider
{
   /* public static BitmapImage? Median(BitmapSource bitmapSource)
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
        var result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            if (i % 4 == 3) continue;
            var bytes = new List<byte>();
            if (i - bitmap.Width * 4 - 4 >= 0)
            {
                bytes.Add(data[i - bitmap.Width * 4 - 4]);
            }
            if (i - bitmap.Width * 4 >= 0)
            {
                bytes.Add(data[i - bitmap.Width * 4]);
            }
            if (i - bitmap.Width * 4 + 4 >= 0)
            {
                bytes.Add(data[i - bitmap.Width * 4 + 4]);
            }
            if (i - 4 >= 0)
            {
                bytes.Add(data[i - 4]);
            }

            bytes.Add(data[i]);

            if (i + 4 < data.Length)
            {
                bytes.Add(data[i + 4]);
            }
            if (i + bitmap.Width * 4 - 4 < data.Length)
            {
                bytes.Add(data[i + bitmap.Width * 4 - 4]);
            }
            if (i + bitmap.Width * 4 < data.Length)
            {
                bytes.Add(data[i + bitmap.Width * 4]);
            }
            if (i + bitmap.Width * 4 + 4 < data.Length)
            {
                bytes.Add(data[i + bitmap.Width * 4 + 4]);
            }

            bytes.Sort();

            int mid = bytes.Count / 2;
            if (bytes.Count % 2 == 0)
            {
                result[i] = (byte)((bytes[mid - 1] + bytes[mid]) / 2);
            }
            else
            {
                result[i] = bytes[mid];
            }
        }

        Marshal.Copy(result, 0, bitmapData.Scan0, result.Length);
        bitmap.UnlockBits(bitmapData);
        return bitmap.ToBitmapImage();
    }

    public static BitmapImage? Average(BitmapSource bitmapSource)
    {
        return Filter(bitmapSource, new List<int> { 1, 1, 1,
                                                    1, 1, 1,
                                                    1, 1, 1 });
    }
    public static BitmapImage? HighPass(BitmapSource bitmapSource)
    {
        return Filter(bitmapSource, new List<int> { -1, -1, -1,
                                                    -1,  8, -1,
                                                    -1, -1, -1 });
    }
    public static BitmapImage? Gaussian(BitmapSource bitmapSource)
    {
        return Filter(bitmapSource, new List<int> { 1, 2, 1,
                                                    2, 4, 2,
                                                    1, 2, 1 });
    }

    public static BitmapImage? Sobel(BitmapSource bitmapSource)
    {
        if (bitmapSource == null)
        {
            return null;
        }

        var horizontal = Filter(bitmapSource, new List<int> { -1,  0,  1,
                                                              -2,  0,  2,
                                                              -1,  0,  1 });
        if (horizontal == null)
        {
            return null;
        }

        var horizontalBitmap = horizontal.ToBitmap();
        if (horizontalBitmap == null)
        {
            return null;
        }

        var vertical = Filter(bitmapSource, new List<int> {  1,  2,  1,
                                                             0,  0,  0,
                                                            -1, -2, -1 });
        if (vertical == null)
        {
            return null;
        }

        var verticalBitmap = vertical.ToBitmap();
        if (verticalBitmap == null)
        {
            return null;
        }

        var horizontalBitmapData = horizontalBitmap.LockBits(new Rectangle(Point.Empty, horizontalBitmap.Size), ImageLockMode.ReadWrite, horizontalBitmap.PixelFormat);
        var VerticalBitmapData = verticalBitmap.LockBits(new Rectangle(Point.Empty, verticalBitmap.Size), ImageLockMode.ReadWrite, verticalBitmap.PixelFormat);
        var horizontalData = new byte[horizontalBitmapData.Stride * horizontalBitmapData.Height];
        var verticalData = new byte[VerticalBitmapData.Stride * VerticalBitmapData.Height];

        Marshal.Copy(horizontalBitmapData.Scan0, horizontalData, 0, horizontalData.Length);
        Marshal.Copy(VerticalBitmapData.Scan0, verticalData, 0, verticalData.Length);

        var result = new byte[verticalData.Length];

        for (int i = 0; i < verticalData.Length; i++)
        {
            result[i] = (byte)Math.Sqrt(horizontalData[i] * horizontalData[i] + verticalData[i] * verticalData[i]);
        }

        Marshal.Copy(result, 0, VerticalBitmapData.Scan0, result.Length);
        verticalBitmap.UnlockBits(VerticalBitmapData);
        return verticalBitmap.ToBitmapImage();
    }
    private static BitmapImage? Filter(BitmapSource? bitmapSource, List<int> filterMatrix)
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
        var result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            if (i % 4 == 3) continue;
            double sum = 0;
            int participated = 0;
            if (i - bitmap.Width * 4 - 4 >= 0)
            {
                sum += data[i - bitmap.Width * 4 - 4] * filterMatrix[0];
                participated += filterMatrix[0];
            }
            if (i - bitmap.Width * 4 >= 0)
            {
                sum += data[i - bitmap.Width * 4] * filterMatrix[1];
                participated += filterMatrix[1];
            }
            if (i - bitmap.Width * 4 + 4 >= 0)
            {
                sum += data[i - bitmap.Width * 4 + 4] * filterMatrix[2];
                participated += filterMatrix[2];
            }
            if (i - 4 >= 0)
            {
                sum += data[i - 4] * filterMatrix[3];
                participated += filterMatrix[3];
            }
            sum += data[i] * filterMatrix[4];
            participated += filterMatrix[4];
            if (i + 4 < data.Length)
            {
                sum += data[i + 4] * filterMatrix[5];
                participated += filterMatrix[5];
            }
            if (i + bitmap.Width * 4 - 4 < data.Length)
            {
                sum += data[i + bitmap.Width * 4 - 4] * filterMatrix[6];
                participated += filterMatrix[6];
            }
            if (i + bitmap.Width * 4 < data.Length)
            {
                sum += data[i + bitmap.Width * 4] * filterMatrix[7];
                participated += filterMatrix[7];
            }
            if (i + bitmap.Width * 4 + 4 < data.Length)
            {
                sum += data[i + bitmap.Width * 4 + 4] * filterMatrix[8];
                participated += filterMatrix[8];
            }
            result[i] = (byte)(sum / (participated == 0 ? 1 : participated));
        }

        Marshal.Copy(result, 0, bitmapData.Scan0, result.Length);
        bitmap.UnlockBits(bitmapData);
        return bitmap.ToBitmapImage();
    }*/
}

