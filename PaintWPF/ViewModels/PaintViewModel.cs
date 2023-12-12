using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using PaintWPF.Commands;
using PaintWPF.Models;
using PaintWPF.Providers;

using Path = System.Windows.Shapes.Path;

namespace PaintWPF.ViewModels;
internal partial class PaintViewModel : ViewModelBase
{
    public ICommand NavigateCubeCommand { get; }
    public ICommand NavigateFiltersCommand { get; }
    public ObservableCollection<Path> ShapesCollection { get; set; } = new ObservableCollection<Path>();
    public Path _selectedGeometry;
    public Path SelectedGeometry
    {
        get => _selectedGeometry;
        set
        {
            _selectedGeometry = value;
            OnPropertyChanged(nameof(SelectedGeometry));
        }
    }

    public Brush _SelectedColour;
    public Brush SelectedBrush
    {
        get => _SelectedColour;
        set
        {
            _SelectedColour = value;
            OnPropertyChanged(nameof(SelectedBrush));
        }
    }

    private DrawingType CurrentDrawingType { get; set; }

    public PaintViewModel(INavigationService cubeNavigationService,INavigationService navigateFiltersService)
    {
        NavigateCubeCommand = new NavigateCommand(cubeNavigationService);
        NavigateFiltersCommand = new NavigateCommand(navigateFiltersService);
        this.InitializeCommands();
        this.ReloadBruchFromRgb();
    }

    private void ReloadBruchFromRgb() => SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));

    public void AddShape(Point start, Point end, List<Point> path)
    {
        var brush = SelectedBrush;
        switch (CurrentDrawingType)
        {
            case DrawingType.Scale:
                if (SelectedGeometry != null)
                {
                    SelectedGeometry.Data.Transform = new ScaleTransform(Math.Abs(start.X - end.X) / 100, Math.Abs(start.Y - end.Y) / 100, -SelectedGeometry.Data.Bounds.Left + SelectedGeometry.Data.Bounds.Right, -SelectedGeometry.Data.Bounds.Top + SelectedGeometry.Data.Bounds.Bottom);
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
                var shr = new Path().Path(rect, brush, 3);
                ShapesCollection.Add(shr);
                break;

            case DrawingType.Triangle:
                var tri = new StreamGeometry();
                tri.FillRule = FillRule.EvenOdd;
                using (var ctx = tri.Open())
                {
                    ctx.BeginFigure(start, true, true);
                    ctx.LineTo(end, true, false);
                    ctx.LineTo(new Point(start.X + 2 * Math.Abs(start.X - end.X), start.Y + 2 * Math.Abs(start.Y - end.Y)), true, false);
                }

                // Freeze the geometry (make it unmodifiable)
                // for additional performance benefits.
                tri.Freeze();
                var sht = new Path().Path(tri, brush, 3);
                ShapesCollection.Add(sht);
                break;

            case DrawingType.Circle:
                var circle1 = new EllipseGeometry(new Rect(start, end));
                var shc = new Path().Path(new EllipseGeometry(new Rect(start, end)), brush, 3);
                ShapesCollection.Add(shc);
                break;

            case DrawingType.Custom:
                var line = new PathGeometry(new List<PathFigure> { new PathFigure(start, path.Select(x => new LineSegment(x, true)), false) });
                var shl = new Path().Path(line, brush, 3);
                ShapesCollection.Add(shl);
                break;

            case DrawingType.Line:
                var line2 = new LineGeometry(start, end);
                var sh2 = new Path().Path(line2, brush, 3);
                ShapesCollection.Add(sh2);
                break;

            default:
                break;
        }
    }
}