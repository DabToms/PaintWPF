using System;
using System.Collections.Generic;
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
        this.Start = e.MouseDevice.GetPosition(this);
        this.Start.Value.Offset(((Canvas)sender).Width, ((Canvas)sender).Height);
        IsDrawing = true;
    }

    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.End = e.MouseDevice.GetPosition(this);
        this.End.Value.Offset((sender as Canvas).Width, (sender as Canvas).Height);

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
            Path.Add(e.GetPosition(this));
        }
    }
}
