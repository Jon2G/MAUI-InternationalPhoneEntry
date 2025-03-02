using AsyncAwaitBestPractices.MVVM;
using CommunityToolkit.Maui.Views;
using InternationalPhoneEntry.Models;
using PhoneNumbers;
using System.Windows.Input;

namespace InternationalPhoneEntry;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PhoneEntry : ContentView
{
    public static readonly BindableProperty CountryProperty = BindableProperty.Create(
        nameof(Country),
        returnType: typeof(CountryModel),
        declaringType: typeof(PhoneEntry),
        null,
        BindingMode.TwoWay,
        propertyChanged: CountryPropertyChanged);

    private static void CountryPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is PhoneEntry countryCodeView)
        {
            //countryCodeView.Phone = string.Empty;
            countryCodeView.OnPropertyChanged(nameof(Country));
        }
    }
    public CountryModel Country
    {
        get => (CountryModel?)GetValue(CountryProperty) ?? InternationalPhoneEntry.DefaultCountry;
        set => SetValue(CountryProperty, value);
    }

    public static readonly BindableProperty PhoneProperty = BindableProperty.Create(
        nameof(Phone),
        returnType: typeof(string),
        declaringType: typeof(PhoneEntry),
        string.Empty,
        BindingMode.TwoWay);

    public string? Phone
    {
        get => (string?)GetValue(PhoneProperty);
        set => SetValue(PhoneProperty, value);
    }

    public static readonly BindableProperty CloseOnSelectProperty = BindableProperty.Create(
        nameof(CloseOnSelect),
        returnType: typeof(bool),
        declaringType: typeof(PhoneEntry),
        false,
        BindingMode.TwoWay,
        propertyChanged: CloseOnSelectPropertyChanged);

    private static void CloseOnSelectPropertyChanged(BindableObject bindable, object oldvalue, object? newvalue)
    {
        if ((bindable is PhoneEntry pe))
        {
            pe.OnPropertyChanged(nameof(CloseOnSelect));
        }
    }

    public bool CloseOnSelect
    {
        get => (GetValue(CloseOnSelectProperty) as bool?) ?? false;
        set => SetValue(CloseOnSelectProperty, value);
    }


    public static readonly BindableProperty AllowCountrySelectionProperty = BindableProperty.Create(
        nameof(AllowCountrySelection),
        returnType: typeof(bool),
        declaringType: typeof(PhoneEntry),
        true,
        BindingMode.TwoWay,
        propertyChanged: AllowCountrySelectionPropertyChanged);

    private static void AllowCountrySelectionPropertyChanged(BindableObject bindable, object oldvalue, object? newvalue)
    {
        if ((bindable is PhoneEntry pe))
        {
            pe.OnPropertyChanged(nameof(AllowCountrySelection));
        }
    }

    public bool AllowCountrySelection
    {
        get => (GetValue(AllowCountrySelectionProperty) as bool?) ?? true;
        set => SetValue(AllowCountrySelectionProperty, value);
    }

    public ICommand ShowPopupCommand { get; }
    public ICommand CountrySelectedCommand { get; }
    private ChooseCountryPopup? ChooseCountryPopup;
    public PhoneEntry()
    {
        ShowPopupCommand = new AsyncCommand<CountryModel>(ExecuteShowPopupCommand, (b) =>
        {
            return AllowCountrySelection;
        });
        CountrySelectedCommand = new Command<CountryModel>(ExecuteCountrySelectedCommand);
        InitializeComponent();
    }
    private Task ExecuteShowPopupCommand(CountryModel? selectedCountry)
    {
        ChooseCountryPopup = new ChooseCountryPopup(selectedCountry ?? InternationalPhoneEntry.DefaultCountry)
        {
            CountrySelectedCommand = CountrySelectedCommand,
            CloseOnSelect = CloseOnSelect
        };
        return Application.Current!.Windows!.FirstOrDefault()!.Page!.ShowPopupAsync(ChooseCountryPopup);
    }
    private void ExecuteCountrySelectedCommand(CountryModel country)
    {
        Country = country;
    }
}