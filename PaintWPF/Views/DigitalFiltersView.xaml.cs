using PaintWPF.ViewModels;

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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintWPF.Views;
/// <summary>
/// Interaction logic for DigitalFiltersView.xaml
/// </summary>
public partial class DigitalFiltersView : UserControl
{
    public DigitalFiltersView()
    {
        InitializeComponent();
    }

    private void OnDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            if (this.DataContext is DigitalFiltersViewModel vm)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                vm.LoadImage(files[0]);
            }
        }
    }
}
