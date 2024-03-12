using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Newtonsoft.Json;

namespace AvaloniaSessia4Chemp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void LoginButton(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(BoxLogin.Text))
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync($"http://localhost:5298/api/Loginuser/GetRoleUser?login={BoxLogin.Text}");
                string content = await responseMessage.Content.ReadAsStringAsync();
                UsdUser.LoginUser = JsonConvert.DeserializeObject<List<int>>(content)!.ToList();
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (UsdUser.LoginUser.Count == 1)
                    {
                        MenuWindow menuWindow = new MenuWindow();
                        menuWindow.Show();
                        Close();
                    }
                }
            }
        }
    }
}