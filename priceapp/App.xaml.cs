using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp;

public partial class App
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        if (activationState != null)
            return new Window(new AppShell(activationState.Context.Services.GetService<IServiceProvider>()));
        return new Window(new ConnectionErrorPage(new ConnectionErrorArgs
            { Message = "Відсутнє зʼєднання з сервером", StatusCode = 404, Success = false }));
    }
}