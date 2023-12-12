using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Path = System.Windows.Shapes.Path;
using Brush = System.Windows.Media.Brush;

namespace PaintWPF.Providers;
internal static class Extensions
{
    public static BitmapImage ToBitmapImage(this Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Bmp);
        stream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = stream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        return bitmapImage;
    }

    public static Bitmap? ToBitmap(this BitmapSource? bitmapSource)
    {
        if (bitmapSource == null)
        {
            return null;
        }
        var memoryStream = new MemoryStream();
        var bitmapEncoder = new BmpBitmapEncoder();
        bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        bitmapEncoder.Save(memoryStream);
        return new Bitmap(memoryStream);
    }
    public static string AddBinaryToFileName(this string path)
    {
        var splitedPath = path.Split('.').ToList();
        splitedPath.Insert(splitedPath.Count - 2, "bin");
        return string.Join(".", splitedPath);
    }
    public static Path Path(this Path path, Geometry geo, Brush stroke, double strokeThickness)
    {
        path.Data = geo;
        path.Stroke = stroke;
        path.StrokeThickness = strokeThickness;
        return path;
    }
    public static Canvas FindCanvas(this DependencyObject parent)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is Canvas canvas)
            {
                return canvas;
            }

            var foundCanvas = child.FindCanvas();
            if (foundCanvas != null)
            {
                return foundCanvas;
            }
        }

        return null;
    }
    public static ItemsControl FindItemsControll(this DependencyObject parent)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is ItemsControl canvas)
            {
                return canvas;
            }

            var foundCanvas = child.FindItemsControll();
            if (foundCanvas != null)
            {
                return foundCanvas;
            }
        }

        return null;
    }
}
