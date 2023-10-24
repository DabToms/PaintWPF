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

namespace PaintWPF;
internal static class Extensions
{
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

            var foundCanvas = FindCanvas(child);
            if (foundCanvas != null)
            {
                return foundCanvas;
            }
        }
        return null;
    }
}
