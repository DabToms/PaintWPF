using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintWPF;
public class CanvasGeometry
{
    public CanvasGeometry(Geometry geometry, Brush stroke, double thickness)
    {
        this.Geometry = geometry;
        this.Stroke = stroke;
        this.StrokeThickness= thickness;
    }
    public Geometry Geometry { get; set; }
    public Brush Stroke { get; set; }
    public double StrokeThickness { get; set; }
}
