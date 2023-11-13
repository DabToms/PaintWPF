using Microsoft.Extensions.DependencyInjection;

using PaintWPF.Providers;
using PaintWPF.ViewModels;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace PaintWPF;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<NavigationStore>();

        services.AddSingleton<INavigationService>(x => CreateNavigatePaintService(x));

        services.AddTransient<PaintViewModel>(x => new PaintViewModel(CreateNavigateCubeService(x)));

        services.AddTransient<CubeViewModel>(x => new CubeViewModel(CreateNavigatePaintService(x)));

        services.AddSingleton<MainViewModel>();

        services.AddSingleton<MainWindow>(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
        initialNavigationService.Navigate();

        MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }
    private INavigationService CreateNavigatePaintService(IServiceProvider serviceProvider)
    {
        return new NavigationService<PaintViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
               () => serviceProvider.GetRequiredService<PaintViewModel>());
    }
    private INavigationService CreateNavigateCubeService(IServiceProvider serviceProvider)
    {
        return new NavigationService<CubeViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
               () => serviceProvider.GetRequiredService<CubeViewModel>());
    }
}
