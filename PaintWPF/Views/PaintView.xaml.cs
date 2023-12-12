using Microsoft.Win32;

using PaintWPF.Providers;
using PaintWPF.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PaintWPF.Views;
/// <summary>
/// Interaction logic for PaintView.xaml
/// </summary>
public partial class PaintView : UserControl
{
    private Point? Start { get; set; }
    private Point? End { get; set; }
    private bool IsDrawing { get; set; }
    private List<Point> Path { get; set; } = new List<Point>();

    public PaintView()
    {
        InitializeComponent();
    }
    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        this.Start = e.MouseDevice.GetPosition(this.controll);
        IsDrawing = true;
    }

    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.End = e.MouseDevice.GetPosition(this.controll);

        if (this.DataContext is PaintViewModel vm)
        {
            if (this.Start is not null && this.End is not null)
            {
                vm.AddShape(this.Start.Value, this.End.Value, this.Path);
            }
        }
        this.Start = null;
        this.End = null;
        IsDrawing = false;
        Path.Clear();
    }

    private void Canvass_MouseMove(object sender, MouseEventArgs e)
    {
        if (IsDrawing)
        {
            var pos = e.GetPosition(this.controll);
            Path.Add(pos);
        }
    }

    private void SavePNGImage(object sender, RoutedEventArgs e)
    {
        // get canvas
        var canvas = this.FindCanvas();

        if (canvas == null)
        {
            MessageBox.Show("Could not find canvas.");
            return;
        }

        // render bitmap from canvas
        var renderBitmap = RenderBitmapFromCanvas(canvas);

        // file dialog
        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "PNG Image (*.png)|*.png";

        if (saveFileDialog.ShowDialog() == true)
        {
            // save image
            ImageSaver.SaveBitmapToPngFile(saveFileDialog, renderBitmap);
        }
    }

    private void SavePBMImage(object sender, RoutedEventArgs e)
    {
        var canvas = this.FindCanvas();

        if (canvas == null)
        {
            MessageBox.Show("Could not find canvas.");
            return;
        }

        var renderBitmap = RenderBitmapFromCanvas(canvas);

        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Pliki obrazów (*.pbm)|*.pbm";

        if (saveFileDialog.ShowDialog() == true)
        {
            ImageSaver.SaveBitmapToP1File(saveFileDialog, renderBitmap);
            ImageSaver.SaveBitmapToP4File(saveFileDialog, renderBitmap);
        }
    }

    private void SavePGMImage(object sender, RoutedEventArgs e)
    {
        var canvas = this.FindCanvas();

        if (canvas == null)
        {
            MessageBox.Show("Could not find canvas.");
            return;
        }
        var renderBitmap = RenderBitmapFromCanvas(canvas);

        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Pliki obrazów (*.pgm)|*.pgm";

        if (saveFileDialog.ShowDialog() == true)
        {
            ImageSaver.SaveBitmapToP2File(saveFileDialog, renderBitmap);
            ImageSaver.SaveBitmapToP5File(saveFileDialog, renderBitmap);
        }
    }

    private void SavePPMImage(object sender, RoutedEventArgs e)
    {
        var canvas = this.FindCanvas();

        if (canvas == null)
        {
            return;
        }
        var renderBitmap = RenderBitmapFromCanvas(canvas);

        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Pliki obrazów (*.ppm)|*.ppm";

        if (saveFileDialog.ShowDialog() == true)
        {
            ImageSaver.SaveBitmapToP3File(saveFileDialog, renderBitmap);
            ImageSaver.SaveBitmapToP6File(saveFileDialog, renderBitmap);
        }
    }

    private void LoadImage(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var imgLoadedWindow = new LoadedImageView(openFileDialog.FileName);
                imgLoadedWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                MessageBox.Show("Dupa :(\n" + ex.Message);
            }
        }
    }
    public RenderTargetBitmap RenderBitmapFromCanvas(Canvas canvas)
    {
        var renderBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        renderBitmap.Render(canvas);
        return renderBitmap;
    }
}
