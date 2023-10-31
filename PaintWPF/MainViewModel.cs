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
using Path = System.Windows.Shapes.Path;

namespace PaintWPF;
internal class MainViewModel : INotifyPropertyChanged
{
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
    public float _CMYK_C_Value;
    public float CMYK_C_Value
    {
        get => _CMYK_C_Value;
        set
        {
            _CMYK_C_Value = value;
            OnPropertyChanged(nameof(CMYK_C_Value));

            this._RGB_R_Value = Convert.ToInt32(255 * (1 - value) * (1 - this._CMYK_C_Value));
            OnPropertyChanged(nameof(RGB_R_Value));
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

            this._RGB_G_Value = Convert.ToInt32(255 * (1 - value) * (1 - this._CMYK_C_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
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

            this._RGB_B_Value = Convert.ToInt32(255 * (1 - value) * (1 - this._CMYK_C_Value));
            OnPropertyChanged(nameof(RGB_B_Value));
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


            this._RGB_R_Value = Convert.ToInt32(255 * (1 - this._CMYK_C_Value) * (1 - this._CMYK_C_Value));
            this._RGB_G_Value = Convert.ToInt32(255 * (1 - this._CMYK_M_Value) * (1 - this._CMYK_C_Value));
            this._RGB_B_Value = Convert.ToInt32(255 * (1 - this._CMYK_Y_Value) * (1 - this._CMYK_C_Value));
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));
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

            this._CMYK_K_Value = (float)Math.Min(Math.Min(255-value, 255-this._RGB_G_Value), 255-this.RGB_B_Value)/255;
            this._CMYK_C_Value = (1-(float)(value/255)-this._CMYK_K_Value)/(1-this._CMYK_K_Value);
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

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

            this._CMYK_K_Value = (float)Math.Min(Math.Min(255-value, 255-this._RGB_R_Value), 255-this.RGB_B_Value)/255;
            this._CMYK_M_Value = (1-(float)(value/255)-this._CMYK_K_Value)/(1-this._CMYK_K_Value);
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));
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

            this._CMYK_K_Value = (float)Math.Min(Math.Min(255-value, 255-this._RGB_G_Value), 255-this.RGB_R_Value)/255;
            this._CMYK_Y_Value = (1-(float)(value/255)-this._CMYK_K_Value)/(1-this._CMYK_K_Value);
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));
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
        var brush = new SolidColorBrush(Color.FromRgb((byte)this._RGB_R_Value, (byte)this.RGB_G_Value, (byte)this.RGB_B_Value));
        switch (this.CurrentDrawingType)
        {
            case DrawingType.Scale:
                if (SelectedGeometry != null)
                {
                    SelectedGeometry.Data.Transform = new ScaleTransform(Math.Abs(start.X - end.X)/100, Math.Abs(start.Y - end.Y)/100, -SelectedGeometry.Data.Bounds.Left+ SelectedGeometry.Data.Bounds.Right, -SelectedGeometry.Data.Bounds.Top+ SelectedGeometry.Data.Bounds.Bottom);
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
                    ctx.LineTo(new Point(start.X + 2 * (Math.Abs(start.X - end.X)), start.Y + 2 * (Math.Abs(start.Y - end.Y))), true, false);
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
    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}