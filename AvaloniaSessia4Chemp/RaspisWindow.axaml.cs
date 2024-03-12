using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Newtonsoft.Json;

namespace AvaloniaSessia4Chemp;

public partial class RaspisWindow : Window
{
    private int _page = 1;
    public RaspisWindow()
    {
        InitializeComponent();
        LoagFilters();
    }

    private List<DoctorsFilter> _doc;
    private List<NapravlenFilter> _naprav;
    private async void LoagFilters()
    {
        using (var client = new HttpClient())
        {
            HttpResponseMessage responseMessage = await client.GetAsync("http://localhost:5298/api/Doctors/GetDoctors");
            string context = await responseMessage.Content.ReadAsStringAsync();
            _doc = JsonConvert.DeserializeObject<List<DoctorsFilter>>(context)!.ToList();
            ComboBoxDoctor.ItemsSource = _doc.Select(x => new
            {
                NameDoctor = x.Nameuser
            }).ToList();
            if (responseMessage.IsSuccessStatusCode)
            {
                using (var clientNaprav = new HttpClient())
                {
                    HttpResponseMessage responseMessageNaprav = await clientNaprav.GetAsync("http://localhost:5298/api/Napravlen/GetNaprav");
                    string contextNaprav = await responseMessageNaprav.Content.ReadAsStringAsync();
                    _naprav = JsonConvert.DeserializeObject<List<NapravlenFilter>>(contextNaprav)!.ToList();
                    ComboBoxNaprav.ItemsSource = _naprav.Select(x => new
                    {
                        NameNaprav = x.Namenaprav
                    }).ToList();
                }
            }
        }
        
    }

    private List<RaspisanieModel> _rasp = new List<RaspisanieModel>();
    private async void LoadRaspisanie()
    {
        using (var client = new HttpClient())
        {
            if (ComboBoxTipeRaspis.SelectedIndex != -1)
            {
                HttpResponseMessage responseMessage = await client
                    .GetAsync($"http://localhost:5298/api/Raspisanie/GetRaspisanie?page={_page}&tipe={ComboBoxTipeRaspis.SelectedIndex}");
                string context = await responseMessage.Content.ReadAsStringAsync();
                _rasp = JsonConvert.DeserializeObject<List<RaspisanieModel>>(context)!.ToList();
                if (ComboBoxNaprav.SelectedIndex != -1)
                {
                    _rasp = _rasp.Where(x => x.Napravlenie == _naprav[ComboBoxNaprav.SelectedIndex].Id).ToList();
                }
                if (ComboBoxDoctor.SelectedIndex != -1)
                {
                    _rasp = _rasp.Where(x => x.Doctorid == _doc[ComboBoxDoctor.SelectedIndex].Id).ToList();
                }
                if (!string.IsNullOrEmpty(DataStart.Text) && !string.IsNullOrEmpty(DataEnd.Text))
                {
                    try
                    {
                        DateTime start = Convert.ToDateTime(DataStart.Text);
                        DateTime end = Convert.ToDateTime(DataEnd.Text);
                        if (start < end)
                        {
                            _rasp = _rasp.Where(x => x.Datapriema >= start && x.Datapriema <= end).ToList();
                        }
                    }
                    catch (Exception e)
                    {
                        DataStart.Background = Brushes.Red;
                        DataEnd.Background = Brushes.Red;
                    }
                }
                ListBoxRaspisanie.ItemsSource = _rasp.Select(x => new
                {
                    NapravlenieText = x.Napravlenie,
                    TitleText = x.Title,
                    Color = string.IsNullOrEmpty(x.Title) ? Brushes.Pink : Brushes.DarkSeaGreen,
                    DoctoridText = x.Doctorid,
                    DatapriemaText = x.Datapriema,
                    IdText = x.Id
                }).ToList();
            }
        }
    }

    private void SearchButton(object? sender, RoutedEventArgs e)
    {
        LoadRaspisanie();
    }

    private void TipeRasp(object? sender, SelectionChangedEventArgs e)
    {
        LoadRaspisanie();
    }

    private void MinesButton(object? sender, RoutedEventArgs e)
    {
        if (_page > 1)
        {
            _page--;
            BlockPage.Text = _page.ToString();
            LoadRaspisanie();
        }
    }

    private void PlasButton(object? sender, RoutedEventArgs e)
    {
        _page++;
        BlockPage.Text = _page.ToString();
        LoadRaspisanie();
    }

    private void NapravBox(object? sender, SelectionChangedEventArgs e)
    {
        LoadRaspisanie();
    }

    private void DoctorBox(object? sender, SelectionChangedEventArgs e)
    {
        LoadRaspisanie();
    }
}

public class NapravlenFilter
{
    public int Id { get; set; }

    public string Namenaprav { get; set; }
}

public class DoctorsFilter
{
    public int Id { get; set; }

    public string Nameuser { get; set; }
}

public class RaspisanieModel
{
    public int Id { get; set; }

    public DateTime Datapriema { get; set; }

    public int? Doctorid { get; set; }

    public string? Title { get; set; }

    public int? Napravlenie { get; set; }
}