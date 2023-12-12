using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PaintWPF.Providers;
internal static class ImageLoader
{
    internal static BitmapFrame LoadPNGFileToBitmap(string path)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
        return  decoder.Frames.First();
    }
    public static Bitmap LoadP1FileToBitmap(string path)
    {
        var words = File.ReadAllText(path).Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
        var bitmap = new Bitmap(int.Parse(words[1]), int.Parse(words[2]));
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                int value = int.Parse(words[3 + y * bitmap.Width + x]) * 255;
                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }
        }
        return bitmap;
    }
    public static Bitmap LoadP2FileToBitmap(string path)
    {
        var words = File.ReadAllText(path).Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
        var bitmap = new Bitmap(int.Parse(words[1]), int.Parse(words[2]));
        double delimiter = int.Parse(words[3]);
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                int value = (int)Math.Round(int.Parse(words[4 + y * bitmap.Width + x]) / delimiter * 255);
                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }
        }
        return bitmap;
    }
    public static Bitmap LoadP3FileToBitmap(string path)
    {
        var words = File.ReadAllText(path).Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
        var bitmap = new Bitmap(int.Parse(words[1]), int.Parse(words[2]));
        double delimiter = int.Parse(words[3]);
        for (int y = 0; y < bitmap.Height * 3; y += 3)
        {
            for (int x = 0; x < bitmap.Width * 3; x += 3)
            {
                bitmap.SetPixel(
                    x / 3,
                    y / 3,
                    Color.FromArgb(
                        (int)Math.Round(int.Parse(words[4 + y * bitmap.Width + x]) / delimiter * 255),
                        (int)Math.Round(int.Parse(words[4 + y * bitmap.Width + x + 1]) / delimiter * 255),
                        (int)Math.Round(int.Parse(words[4 + y * bitmap.Width + x + 2]) / delimiter * 255)));
            }
        }
        return bitmap;
    }
    public static Bitmap LoadP4FileToBitmap(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using BinaryReader br = new BinaryReader(fs);

        var headers = new string[2];
        for (int i = 0; i < 3; i++)
        {
            var chunk = new List<char>();
            byte current;
            while ((current = br.ReadByte()) != '\n')
            {
                chunk.Add((char)current);
            }
            headers[i] = string.Concat(chunk);
        }
        var dimensions = headers[1].Split();
        var bitmap = new Bitmap(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
        var data = new List<byte>();
        while (br.BaseStream.Position != br.BaseStream.Length)
        {
            data.Add(br.ReadByte());
        }
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                int value = data[y * bitmap.Width + x] * 255;
                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }
        }
        return bitmap;
    }
    public static Bitmap LoadP5FileToBitmap(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using BinaryReader br = new BinaryReader(fs);
        var headers = new string[3];
        for (int i = 0; i < 3; i++)
        {
            List<char> chunk = new List<char>();
            byte current;
            while ((current = br.ReadByte()) != '\n')
            {
                chunk.Add((char)current);
            }
            headers[i] = string.Concat(chunk);
        }
        var dimensions = headers[1].Split();
        var bitmap = new Bitmap(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
        var data = new List<byte>();
        while (br.BaseStream.Position != br.BaseStream.Length)
        {
            data.Add(br.ReadByte());
        }
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                int value = (int)Math.Round((double)data[y * bitmap.Width + x] / int.Parse(headers[2]) * 255);
                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }
        }
        return bitmap;
    }
    public static Bitmap LoadP6FileToBitmap(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using BinaryReader br = new BinaryReader(fs);
        var headers = new string[3];

        for (int i = 0; i < 3; i++)
        {
            var chunk = new List<char>();
            byte current;
            while ((current = br.ReadByte()) != '\n')
            {
                chunk.Add((char)current);
            }
            headers[i] = string.Concat(chunk);
        }

        var dimensions = headers[1].Split();
        var bitmap = new Bitmap(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
        var data = new List<byte>();
        while (br.BaseStream.Position != br.BaseStream.Length)
        {
            data.Add(br.ReadByte());
        }

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                bitmap.SetPixel(
                    x,
                    y,
                    Color.FromArgb(
                        (int)Math.Round((double)data[y * bitmap.Width * 3 + x * 3] / int.Parse(headers[2]) * 255),
                        (int)Math.Round((double)data[y * bitmap.Width * 3 + x * 3 + 1] / int.Parse(headers[2]) * 255),
                        (int)Math.Round((double)data[y * bitmap.Width * 3 + x * 3 + 2] / int.Parse(headers[2]) * 255)));
            }
        }
        return bitmap;
    }
}
