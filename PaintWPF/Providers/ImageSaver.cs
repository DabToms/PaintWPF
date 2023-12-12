using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PaintWPF.Providers;
internal static class ImageSaver
{
    public static void SaveBitmapToPngFile(SaveFileDialog dialog, RenderTargetBitmap bitmap)
    {
        using (var stream = new FileStream(dialog.FileName, FileMode.Create))
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }
    }

    public static void SaveBitmapToP1File(SaveFileDialog dialog, RenderTargetBitmap bitmap)
    {
        int width = bitmap.PixelWidth;
        int height = bitmap.PixelHeight;
        int stride = width * ((bitmap.Format.BitsPerPixel + 7) / 8);

        byte[] pixels = new byte[height * stride];
        bitmap.CopyPixels(pixels, stride, 0);

        using (StreamWriter writer = new StreamWriter(dialog.FileName))
        {
            // Write the header
            writer.WriteLine("P1");
            writer.WriteLine($"{width} {height}");

            // Convert pixels to PBM format
            for (int i = 0; i < pixels.Length; i += 4)
            {
                // Assuming the bitmap is 32bit with alpha (Pbgra32)
                byte red = pixels[i + 2];
                byte green = pixels[i + 1];
                byte blue = pixels[i];
                byte alpha = pixels[i + 3];

                // Simple threshold for black/white
                int color = (red + green + blue) / 3;
                bool isWhite = color > 128 || alpha == 0; // Transparent pixels treated as white
                writer.Write(isWhite ? "0 " : "1 ");
            }
        }
    }

    public static void SaveBitmapToP2File(SaveFileDialog saveFileDialog, RenderTargetBitmap renderBitmap)
    {
        int width = renderBitmap.PixelWidth;
        int height = renderBitmap.PixelHeight;
        int stride = width * ((renderBitmap.Format.BitsPerPixel + 7) / 8);

        byte[] pixels = new byte[height * stride];
        renderBitmap.CopyPixels(pixels, stride, 0);

        byte[] grayPixels = ConvertToGrayscale(pixels, renderBitmap.Format.BitsPerPixel);

        // Write to PGM file
        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
        {
            // PGM Header
            writer.WriteLine("P2"); // Magic number for plain PGM file
            writer.WriteLine($"{width} {height}");
            writer.WriteLine("255"); // Max gray value

            // Pixel data
            for (int i = 0; i < grayPixels.Length; i++)
            {
                writer.WriteLine(grayPixels[i].ToString());
            }
        }
    }

    public static void SaveBitmapToP3File(SaveFileDialog saveFileDialog, RenderTargetBitmap renderBitmap)
    {
        int width = renderBitmap.PixelWidth;
        int height = renderBitmap.PixelHeight;
        int stride = width * ((renderBitmap.Format.BitsPerPixel + 7) / 8);

        byte[] pixels = new byte[height * stride];
        renderBitmap.CopyPixels(pixels, stride, 0);

        using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
        {
            // P3 for ASCII PPM format
            sw.WriteLine("P3");
            sw.WriteLine($"{width} {height}");
            sw.WriteLine("255"); // Max color value

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // Convert BGRA to RGB
                sw.WriteLine($"{pixels[i + 2]} {pixels[i + 1]} {pixels[i]}");
            }
        }
    }


    public static void SaveBitmapToP4File(SaveFileDialog dialog, RenderTargetBitmap bitmap)
    {
        int width = bitmap.PixelWidth;
        int height = bitmap.PixelHeight;
        int stride = width * ((bitmap.Format.BitsPerPixel + 7) / 8);

        byte[] pixels = new byte[height * stride];
        bitmap.CopyPixels(pixels, stride, 0);

        using (StreamWriter writer = new StreamWriter(dialog.FileName.AddBinaryToFileName()))
        {
            // Write the header
            writer.Write("P4"); // TODO: poprawić
            writer.Write($"{width} {height}\n");// TODO: poprawić
            int counter = 0;
            // Convert pixels to PBM format
            byte tmp = 0;
            var list = new List<byte>();
            for (int i = 0; i < pixels.Length; i += 4)
            {
                // Assuming the bitmap is 32bit with alpha (Pbgra32)
                byte red = pixels[i + 2];
                byte green = pixels[i + 1];
                byte blue = pixels[i];
                byte alpha = pixels[i + 3];

                if (counter == 8)
                {
                    counter = 0;
                    list.Add(tmp);
                    tmp = 0;
                }
                // Simple threshold for black/white
                int color = (red + green + blue) / 3;
                bool bit = color > 128 || alpha == 0; // Transparent pixels treated as white
                if (bit)
                {
                    tmp += (byte)(0b00000001 << counter);
                }
                writer.Write(bit ? "0" : "1");// TODO: poprawić
                counter++;
            }
            foreach (var i in list)
            {
                writer.Write(i);
            }
        }
    }

    public static void SaveBitmapToP5File(SaveFileDialog saveFileDialog, RenderTargetBitmap renderBitmap)
    {
        using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName.AddBinaryToFileName()))
        { }
    }

    public static void SaveBitmapToP6File(SaveFileDialog saveFileDialog, RenderTargetBitmap renderBitmap)
    {
        using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName.AddBinaryToFileName()))
        { }
        throw new NotImplementedException();
    }

    private static byte[] ConvertToGrayscale(byte[] pixels, int bitsPerPixel)
    {
        byte[] grayPixels = new byte[pixels.Length / (bitsPerPixel / 8)];
        for (int i = 0; i < pixels.Length; i += bitsPerPixel / 8)
        {
            // Simple grayscale conversion averaging the color channels
            grayPixels[i / (bitsPerPixel / 8)] = (byte)((pixels[i] + pixels[i + 1] + pixels[i + 2]) / 3);
        }
        return grayPixels;
    }
}
