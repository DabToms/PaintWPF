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
using System.Windows.Media.Media3D;

using PaintWPF.Commands;
using PaintWPF.Models;
using PaintWPF.Providers;

using Path = System.Windows.Shapes.Path;

namespace PaintWPF.ViewModels;
internal partial class PaintViewModel : ViewModelBase
{
    public ICommand SetCustomDrawing { get; set; }
    public ICommand SetLineDrawing { get; set; }
    public ICommand SetCircleDrawing { get; set; }
    public ICommand SetRectangleDrawing { get; set; }
    public ICommand SetTriangleDrawing { get; set; }
    public ICommand SelectDrawing { get; set; }
    public ICommand MoveDrawing { get; set; }
    public ICommand ScaleDrawing { get; set; }

    public void InitializeCommands()
    {
        SetCircleDrawing = new Command(() => this.SetDrawingType(DrawingType.Circle));
        SetRectangleDrawing = new Command(() => this.SetDrawingType(DrawingType.Rectangle));
        SetTriangleDrawing = new Command(() => this.SetDrawingType(DrawingType.Triangle));
        SetLineDrawing = new Command(() => this.SetDrawingType(DrawingType.Line));
        SelectDrawing = new Command(() => this.SetDrawingType(DrawingType.Select));
        MoveDrawing = new Command(() => this.SetDrawingType(DrawingType.Move));
        ScaleDrawing = new Command(() => this.SetDrawingType(DrawingType.Scale));
        SetCustomDrawing = new Command(() => this.SetDrawingType(DrawingType.Custom));
    }
    private void SetDrawingType(DrawingType type) => CurrentDrawingType = type;
}