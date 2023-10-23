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

namespace PaintWPF;
internal class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CanvasGeometry> ShapesCollection { get; set; } = new ObservableCollection<CanvasGeometry>();
    public CanvasGeometry selectedGeometry { get; set; }
    public CanvasGeometry SelectedGeometry
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
    public ICommand SaveCanvasCommand { get; set; }

    public MainViewModel()
    {
        SaveCanvasCommand = new RelayCommand(SaveCanvas);
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
                    ShapesCollection.FirstOrDefault(x => x == SelectedGeometry).Geometry.Transform = new ScaleTransform(Math.Abs(start.X - end.X) / 100, Math.Abs(start.Y - end.Y) / 100);
                }
                break;
            case DrawingType.Move:
                if (SelectedGeometry != null)
                {
                    ShapesCollection.FirstOrDefault(x => x == SelectedGeometry).Geometry.Transform = new TranslateTransform(end.X, end.Y);
                }
                break;

            case DrawingType.Select:
                foreach (var i in ShapesCollection)
                {
                    if (i.Geometry.Bounds.Contains(end.X, end.Y))
                    {
                        SelectedGeometry = i;
                        break;
                    }
                }
                break;

            case DrawingType.Rectangle:
                var rect = new RectangleGeometry(new Rect(start, end)); // { Width = Math.Abs(start.X - end.X), Height = Math.Abs(start.Y - end.Y), Fill = Brushes.Silver, Stroke = System.Windows.Media.Brushes.LightSteelBlue };
                var shr = new CanvasGeometry(rect, Brushes.Black, 3);
                ShapesCollection.Add(shr);
                break;

            case DrawingType.Triangle:
                var tri = new StreamGeometry(); // { Width = Math.Abs(start.X - end.X), Height = Math.Abs(start.Y - end.Y), Fill = Brushes.Silver, Stroke = System.Windows.Media.Brushes.LightSteelBlue };
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
                var sht = new CanvasGeometry(tri, Brushes.Black, 3);
                ShapesCollection.Add(sht);
                break;

            case DrawingType.Circle:
                var circle1 = new EllipseGeometry(new Rect(start, end)); // { Width = Math.Abs(start.X - end.X), Height = Math.Abs(start.Y - end.Y), Fill = Brushes.Silver, Stroke = System.Windows.Media.Brushes.LightSteelBlue };
                var shc = new CanvasGeometry(circle1, Brushes.Black, 3);
                ShapesCollection.Add(shc);
                break;

            case DrawingType.Custom:
                var line = new PathGeometry(new List<PathFigure> { new PathFigure(start, path.Select(x => new LineSegment(x, true)), false) });
                var shl = new CanvasGeometry(line, Brushes.Black, 3);
                ShapesCollection.Add(shl);
                break;

            case DrawingType.Line:
                var line2 = new LineGeometry(start, end);// { X1 = start.X, X2 = end.X, Y1 = start.Y, Y2 = end.Y, StrokeThickness = 2, Stroke = System.Windows.Media.Brushes.LightSteelBlue };
                var sh2 = new CanvasGeometry(line2, Brushes.Black, 3);
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

    private void SaveCanvas(object parameter)
    {
        if (ShapesCollection.Count == 0)
        {
            return;
        }

        var canvas = new Canvas
        {
            Background = Brushes.Transparent,
            Width = Math.Abs(ShapesCollection.Max(x => x.Geometry.Bounds.X) - ShapesCollection.Min(x => x.Geometry.Bounds.X)),
            Height = Math.Abs(ShapesCollection.Max(x => x.Geometry.Bounds.Y) - ShapesCollection.Min(x => x.Geometry.Bounds.Y))
        };

        foreach (var i in ShapesCollection)
        {
            var path = new System.Windows.Shapes.Path
            {
                Data = i.Geometry,
                Stroke = i.Stroke,
                StrokeThickness = i.StrokeThickness
            };
            path.Width = i.Geometry.Bounds.Width;
            path.Height = i.Geometry.Bounds.Height;
            Canvas.SetLeft(path, i.Geometry.Bounds.Left);
            Canvas.SetTop(path, i.Geometry.Bounds.Top);
            canvas.Children.Add(path);
        }

        Size size = new Size(canvas.Width, canvas.Height);
        canvas.Measure(size);
        canvas.Arrange(new Rect(size));

        RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
            (int)size.Width,
            (int)size.Height,
            96d,
            96d,
            PixelFormats.Default);

        renderBitmap.Render(canvas);

        using (FileStream outStream = new FileStream("save.png", FileMode.Create))
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(outStream);
        }
    }

    public void SaveCanvas(Canvas canvas, string filename)
    {
        // Get the size of canvas
        int width = (int)canvas.Width;
        int height = (int)canvas.Height;

        // Create a render bitmap and push the surface to it
        RenderTargetBitmap renderBitmap =
            new RenderTargetBitmap(
                width,
                height,
                96d,
                96d,
                PixelFormats.Pbgra32);
        renderBitmap.Render(canvas);

        // Create a file stream for saving image
        using (FileStream outStream = new FileStream(filename, FileMode.Create))
        {
            // Use png encoder for our data
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            // Push the rendered bitmap to it
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            // Save the data to the stream
            encoder.Save(outStream);
        }
    }
}