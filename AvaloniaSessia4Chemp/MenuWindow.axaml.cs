using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaSessia4Chemp;

public partial class MenuWindow : Window
{
    public MenuWindow()
    {
        InitializeComponent();
        switch (UsdUser.LoginUser[0])
        {
            case 2:
                ButtonRaspis.IsVisible = true;
                ButtonReg.IsVisible = true;
                ButtonNaprav.IsVisible = true;
                ButtonResept.IsVisible = true;
                break;
            case 1:
                ButtonNaprav.IsVisible = true;
                ButtonResept.IsVisible = true;
                break;
            case 4:
                ButtonReg.IsVisible = true;
                ButtonNaprav.IsVisible = true;
                break;
        }
    }

    private void RaspisButton(object? sender, RoutedEventArgs e)
    {
        RaspisWindow raspisWindow = new RaspisWindow();
        raspisWindow.Show();
        Close();
    }

    private void RegButton(object? sender, RoutedEventArgs e)
    {
        RegWindow regWindow = new RegWindow();
        regWindow.Show();
        Close();
    }

    private void NapravlenButton(object? sender, RoutedEventArgs e)
    {
        NapravlenWindow napravlenWindow = new NapravlenWindow();
        napravlenWindow.Show();
        Close();
    }

    private void ReseptsButton(object? sender, RoutedEventArgs e)
    {
        ReseptsWindow reseptsWindow = new ReseptsWindow();
        reseptsWindow.Show();
        Close();
    }
}