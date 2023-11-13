using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using PaintWPF.Commands;
using PaintWPF.Models;
using PaintWPF.Providers;
using Path = System.Windows.Shapes.Path;

namespace PaintWPF.ViewModels;
internal class PaintViewModel : ViewModelBase
{
    public ICommand NavigateCubeCommand { get; }
    public ObservableCollection<Path> ShapesCollection { get; set; } = new ObservableCollection<Path>();
    public ObservableCollection<GeometryModel3D> GeometriesCollection { get; set; } = new ObservableCollection<GeometryModel3D>();
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
    public float _CMYK_C_Value;
    public float CMYK_C_Value
    {
        get => _CMYK_C_Value;
        set
        {
            _CMYK_C_Value = value;
            OnPropertyChanged(nameof(CMYK_C_Value));

            RGB_R_Value = Convert.ToInt32(255 * (1 - value) * (1 - _CMYK_K_Value));

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)RGB_G_Value, (byte)RGB_B_Value));
        }
    }
    public float _CMYK_M_Value;
    public float CMYK_M_Value
    {
        get => _CMYK_M_Value;
        set
        {
            _CMYK_M_Value = value;
            OnPropertyChanged(nameof(CMYK_M_Value));

            RGB_G_Value = Convert.ToInt32(255 * (1 - value) * (1 - _CMYK_K_Value));

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)RGB_G_Value, (byte)RGB_B_Value));
        }
    }
    public float _CMYK_Y_Value;
    public float CMYK_Y_Value
    {
        get => _CMYK_Y_Value;
        set
        {
            _CMYK_Y_Value = value;
            OnPropertyChanged(nameof(CMYK_Y_Value));

            RGB_B_Value = Convert.ToInt32(255 * (1 - value) * (1 - _CMYK_K_Value));

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)RGB_G_Value, (byte)RGB_B_Value));
        }
    }
    public float _CMYK_K_Value;
    public float CMYK_K_Value
    {
        get => _CMYK_K_Value;
        set
        {
            _CMYK_K_Value = value;
            OnPropertyChanged(nameof(CMYK_K_Value));


            RGB_R_Value = Convert.ToInt32(255 * (1 - _CMYK_C_Value) * (1 - _CMYK_K_Value));
            RGB_G_Value = Convert.ToInt32(255 * (1 - _CMYK_M_Value) * (1 - _CMYK_K_Value));
            RGB_B_Value = Convert.ToInt32(255 * (1 - _CMYK_Y_Value) * (1 - _CMYK_K_Value));
            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));
        }
    }
    public int _RGB_R_Value;
    public int RGB_R_Value
    {
        get => _RGB_R_Value;
        set
        {
            _RGB_R_Value = value;
            OnPropertyChanged(nameof(RGB_R_Value));

            _CMYK_K_Value = Math.Min(Math.Min(1f - value / 255f, 1 - _RGB_G_Value / 255f), 1 - _RGB_B_Value / 255f);
            _CMYK_C_Value = (1 - value / 255f - _CMYK_K_Value) / (1 - _CMYK_K_Value);
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            var hsv = Providers.ColorConverter.RGBToHSV(RGB_R_Value, RGB_G_Value, RGB_B_Value);

            _HSV_H_Value = hsv.H;
            _HSV_S_Value = hsv.S;
            _HSV_V_Value = hsv.V;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));


            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));

        }
    }
    public int _RGB_G_Value;
    public int RGB_G_Value
    {
        get => _RGB_G_Value;
        set
        {
            _RGB_G_Value = value;
            OnPropertyChanged(nameof(RGB_G_Value));

            _CMYK_K_Value = Math.Min(Math.Min(1f - value / 255f, 1 - _RGB_R_Value / 255f), 1 - _RGB_B_Value / 255f);
            _CMYK_M_Value = (1 - value / 255f - _CMYK_K_Value) / (1 - _CMYK_K_Value);
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            var hsv = Providers.ColorConverter.RGBToHSV(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _HSV_H_Value = hsv.H;
            _HSV_S_Value = hsv.S;
            _HSV_V_Value = hsv.V;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));
        }
    }
    public int _RGB_B_Value;
    public int RGB_B_Value
    {
        get => _RGB_B_Value;
        set
        {
            _RGB_B_Value = value;
            OnPropertyChanged(nameof(RGB_B_Value));

            _CMYK_K_Value = Math.Min(Math.Min(1f - value / 255f, 1 - _RGB_G_Value / 255f), 1 - _RGB_R_Value / 255f);
            _CMYK_Y_Value = (1 - value / 255f - _CMYK_K_Value) / (1 - _CMYK_K_Value);
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            var hsv = Providers.ColorConverter.RGBToHSV(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _HSV_H_Value = hsv.H;
            _HSV_S_Value = hsv.S;
            _HSV_V_Value = hsv.V;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));
        }
    }
    public float _HSV_H_Value;
    public float HSV_H_Value
    {
        get => _HSV_H_Value;
        set
        {
            _HSV_H_Value = value;
            OnPropertyChanged(nameof(HSV_H_Value));

            var rgb = Providers.ColorConverter.HSVToRGB(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            RGB_R_Value = rgb.R;
            RGB_G_Value = rgb.G;
            RGB_B_Value = rgb.B;

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));
        }
    }
    public float _HSV_S_Value;
    public float HSV_S_Value
    {
        get => _HSV_S_Value;
        set
        {
            _HSV_S_Value = value;
            OnPropertyChanged(nameof(HSV_S_Value));

            var rgb = Providers.ColorConverter.HSVToRGB(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            RGB_R_Value = rgb.R;
            RGB_G_Value = rgb.G;
            RGB_B_Value = rgb.B;

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));
        }
    }
    public float _HSV_V_Value;
    public float HSV_V_Value
    {
        get => _HSV_V_Value;
        set
        {
            _HSV_V_Value = value;
            OnPropertyChanged(nameof(HSV_V_Value));

            var rgb = Providers.ColorConverter.HSVToRGB(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            RGB_R_Value = rgb.R;
            RGB_G_Value = rgb.G;
            RGB_B_Value = rgb.B;

            SelectedBrush = new SolidColorBrush(Color.FromRgb((byte)_RGB_R_Value, (byte)_RGB_G_Value, (byte)_RGB_B_Value));

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
    public ICommand SetCustomDrawing { get; set; }
    public ICommand SetLineDrawing { get; set; }
    public ICommand SetCircleDrawing { get; set; }
    public ICommand SetRectangleDrawing { get; set; }
    public ICommand SetTriangleDrawing { get; set; }
    public ICommand SelectDrawing { get; set; }
    public ICommand MoveDrawing { get; set; }
    public ICommand ScaleDrawing { get; set; }

    public PaintViewModel(INavigationService cubeNavigationService)
    {

        NavigateCubeCommand = new NavigateCommand(cubeNavigationService);


        SetCustomDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Custom;
        });
        SetCircleDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Circle;
        });
        SetRectangleDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Rectangle;
        });
        SetTriangleDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Triangle;
        });
        SetLineDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Line;
        });
        SelectDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Select;
        });
        MoveDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Move;
        });
        ScaleDrawing = new RelayCommand(() =>
        {
            CurrentDrawingType = DrawingType.Scale;
        });
    }

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
            case DrawingType.Cube:
                // Define the cube's vertices and indices
                Point3D p0 = new Point3D(0, 0, 0);
                Point3D p1 = new Point3D(1, 0, 0);
                Point3D p2 = new Point3D(1, 0, 1);
                Point3D p3 = new Point3D(0, 0, 1);
                Point3D p4 = new Point3D(0, 1, 0);
                Point3D p5 = new Point3D(1, 1, 0);
                Point3D p6 = new Point3D(1, 1, 1);
                Point3D p7 = new Point3D(0, 1, 1);

                // Create a mesh builder
                MeshGeometry3D mesh = new MeshGeometry3D();
                mesh.Positions.Add(p0);
                mesh.Positions.Add(p1);
                mesh.Positions.Add(p2);
                mesh.Positions.Add(p3);
                mesh.Positions.Add(p4);
                mesh.Positions.Add(p5);
                mesh.Positions.Add(p6);
                mesh.Positions.Add(p7);

                // Front face
                mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(1); mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(2); mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(0);

                // Right face
                mesh.TriangleIndices.Add(1); mesh.TriangleIndices.Add(5); mesh.TriangleIndices.Add(6);
                mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(2); mesh.TriangleIndices.Add(1);

                // Back face
                mesh.TriangleIndices.Add(5); mesh.TriangleIndices.Add(4); mesh.TriangleIndices.Add(7);
                mesh.TriangleIndices.Add(7); mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(5);

                // Left face
                mesh.TriangleIndices.Add(4); mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(3);
                mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(7); mesh.TriangleIndices.Add(4);

                // Top face
                mesh.TriangleIndices.Add(4); mesh.TriangleIndices.Add(5); mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(1); mesh.TriangleIndices.Add(0); mesh.TriangleIndices.Add(4);

                // Bottom face
                mesh.TriangleIndices.Add(3); mesh.TriangleIndices.Add(2); mesh.TriangleIndices.Add(6);
                mesh.TriangleIndices.Add(6); mesh.TriangleIndices.Add(7); mesh.TriangleIndices.Add(3);

                // Create a GeometryModel3D
                GeometryModel3D model = new GeometryModel3D(mesh, new DiffuseMaterial(SelectedBrush));

                GeometriesCollection.Add(model);
                break;

            default:
                break;
        }
    }
}