using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintWPF;


public enum DrawingType
{
    Custom = 0,
    Line = 1,
    Rectangle = 2,
    Circle = 3,
    Triangle = 4,
    Select = 5,
    Move = 6,
    Scale = 7,
}