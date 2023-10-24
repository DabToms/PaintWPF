using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Path = System.Windows.Shapes.Path;

namespace PaintWPF;
internal class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Path> ShapesCollection { get; set; } = new ObservableCollection<Path>();
    public Path selectedGeometry { get; set; }
    public Path SelectedGeometry
    {
        get => selectedGeometry;
        set
        {
            selectedGeometry = value;
            OnPropertyChanged();
        }
    }

    private DrawingType CurrentDrawingType { get; set; }
    public ICommand SetCustomDrawing { get; set; }
    public ICommand SetLineDrawing { get; set; }
    public ICommand SetCircleDrawing { get; set; }
    public ICommand SetRectangleDrawing { get; set; }
    public ICommand SetTriangleDrawing { get; set; }
    public ICommand SelectDrawing { get; set; }
    public ICommand MoveDrawing { get; set; }
    public ICommand ScaleDrawing { get; set; }

    public MainViewModel()
    {
        SetCustomDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Custom;
        });
        SetCircleDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Circle;
        });
        SetRectangleDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Rectangle;
        });
        SetTriangleDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Triangle;
        });
        SetLineDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Line;
        });
        SelectDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Select;
        });
        MoveDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Move;
        });
        ScaleDrawing = new RelayCommand(x =>
        {
            CurrentDrawingType = DrawingType.Scale;
        });
    }


    public event PropertyChangedEventHandler PropertyChanged;

    public void AddShape(Point start, Point end, List<Point> path)
    {
        switch (this.CurrentDrawingType)
        {
            case DrawingType.Scale:
                if (SelectedGeometry != null)
                {
                    SelectedGeometry.Data.Transform = new ScaleTransform(Math.Abs(start.X - end.X) / 100, Math.Abs(start.Y - end.Y) / 100);
                }
                break;
            case DrawingType.Move:
                if (SelectedGeometry != null)
                {
                    SelectedGeometry.Data.Transform = new TranslateTransform(end.X, end.Y);
                }
                break;

            case DrawingType.Select:
                foreach (var i in ShapesCollection)
                {
                    if (i.Data.Bounds.Contains(end.X, end.Y))
                    {
                        SelectedGeometry = i;
                        break;
                    }
                    SelectedGeometry = null;
                }
                break;

            case DrawingType.Rectangle:
                var rect = new RectangleGeometry(new Rect(start, end));
                var shr = new Path().Path(rect, Brushes.Black, 3);
                ShapesCollection.Add(shr);
                break;

            case DrawingType.Triangle:
                var tri = new StreamGeometry(); 
                tri.FillRule = FillRule.EvenOdd;
                using (var ctx = tri.Open())
                {
                    ctx.BeginFigure(start, true, true);
                    ctx.LineTo(end, true, false);
                    ctx.LineTo(new Point(start.X + 2 * (Math.Abs(start.X - end.X)), start.Y + 2 * (Math.Abs(start.Y - end.Y))), true, false);
                }

                // Freeze the geometry (make it unmodifiable)
                // for additional performance benefits.
                tri.Freeze();
                var sht = new Path().Path(tri, Brushes.Black, 3);
                ShapesCollection.Add(sht);
                break;

            case DrawingType.Circle:
                var circle1 = new EllipseGeometry(new Rect(start, end));
                var shc = new Path().Path(new EllipseGeometry(new Rect(start, end)), Brushes.Black, 3);
                ShapesCollection.Add(shc);
                break;

            case DrawingType.Custom:
                var line = new PathGeometry(new List<PathFigure> { new PathFigure(start, path.Select(x => new LineSegment(x, true)), false) });
                var shl = new Path().Path(line, Brushes.Black, 3);
                ShapesCollection.Add(shl);
                break;

            case DrawingType.Line:
                var line2 = new LineGeometry(start, end);
                var sh2 = new Path().Path(line2, Brushes.Black, 3);
                ShapesCollection.Add(sh2);
                break;

            default:
                break;
        }
    }
    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}