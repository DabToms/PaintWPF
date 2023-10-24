using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintWPF;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Point? Start { get; set; }
    private Point? End { get; set; }
    private bool IsDrawing { get; set; }
    private List<Point> Path { get; set; } = new List<Point>();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        this.Start = e.MouseDevice.GetPosition(this.controll);
        IsDrawing = true;
    }

    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.End = e.MouseDevice.GetPosition(this.controll);

        var vm = this.DataContext as MainViewModel;
        if (vm is not null)
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

    private void SaveImage(object sender, RoutedEventArgs e)
    {
        var canvas = this.FindCanvas();

        if (canvas == null)
        {
            return;
        }

        var renderBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        renderBitmap.Render(canvas);
        var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
        saveFileDialog.Filter = "PNG Image (*.png)|*.png";
        if (saveFileDialog.ShowDialog() == true)
        {
            using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(stream);
            }
        }
    }
}