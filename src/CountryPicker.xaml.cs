using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CommunityToolkit.Maui.Views;
using InternationalPhoneEntry.Models;

namespace InternationalPhoneEntry;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CountryPicker : ContentView
{
    #region Properties
    public static readonly CountryModel DefaultCountry = CountryModel.GetCountryModelByTwoLetterISORegionName("US");
    public static readonly BindableProperty SelectedCountryProperty = BindableProperty.Create(
        nameof(SelectedCountry),
        returnType: typeof(CountryModel),
        declaringType: typeof(CountryPicker),
        DefaultCountry,
    BindingMode.TwoWay,
        propertyChanged: SelectedCountryPropertyChanged);

    private static void SelectedCountryPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        (bindable as CountryPicker)?.OnPropertyChanged(nameof(SelectedCountry));
    }

    public CountryModel? SelectedCountry
    {
        get => GetValue(SelectedCountryProperty) as CountryModel;
        set => SetValue(SelectedCountryProperty, value);
    }

    public static readonly BindableProperty CloseOnSelectProperty = BindableProperty.Create(
        nameof(CloseOnSelect),
        returnType: typeof(bool),
        declaringType: typeof(CountryPicker),
        false,
        BindingMode.TwoWay,
        propertyChanged: CloseOnSelectPropertyChanged);

    private static void CloseOnSelectPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        (bindable as CountryPicker)?.OnPropertyChanged(nameof(CloseOnSelect));
    }

    public bool CloseOnSelect
    {
        get => (GetValue(CloseOnSelectProperty) as bool?)??false;
        set => SetValue(CloseOnSelectProperty, value);
    }

    

    #endregion Properties

    #region Commands

    public ICommand ShowPopupCommand { get; }
    public ICommand CountrySelectedCommand { get; }
    private ChooseCountryPopup? ChooseCountryPopup;

    #endregion Commands

    public CountryPicker()
    {
        ShowPopupCommand = new AsyncCommand<CountryModel>(ExecuteShowPopupCommand);
        CountrySelectedCommand = new Command<CountryModel>(ExecuteCountrySelectedCommand);
        InitializeComponent();
    }

    #region Private Methods

    private Task ExecuteShowPopupCommand(CountryModel? selectedCountry)
    {
        ChooseCountryPopup = new ChooseCountryPopup(selectedCountry ?? CountryPicker.DefaultCountry)
        {
            CountrySelectedCommand = CountrySelectedCommand
        };
        return Application.Current!.Windows!.FirstOrDefault()!.Page!.ShowPopupAsync(ChooseCountryPopup);
    }

    private void ExecuteCountrySelectedCommand(CountryModel country)
    {
        SelectedCountry = country;
        if (CloseOnSelect)
        {
            ChooseCountryPopup?.Close(SelectedCountry);
        }
    }

    #endregion Private Methods


}