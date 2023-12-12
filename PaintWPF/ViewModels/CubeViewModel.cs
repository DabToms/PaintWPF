using PaintWPF.Commands;
using PaintWPF.Providers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PaintWPF.ViewModels;
public class CubeViewModel : ViewModelBase
{
    public ICommand NavigatePaintCommand { get; }

    public CubeViewModel(INavigationService paintNavigationService)
    {
        NavigatePaintCommand = new NavigateCommand(paintNavigationService);
    }
}
