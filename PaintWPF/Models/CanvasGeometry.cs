using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PaintWPF.Models;
public class CanvasGeometry
{
    public CanvasGeometry(Geometry geometry, Brush stroke, double thickness)
    {
        Data = geometry;
        Stroke = stroke;
        StrokeThickness = thickness;
    }
    public Animatable Data { get; set; }
    public Brush Stroke { get; set; }
    public double StrokeThickness { get; set; }
}
