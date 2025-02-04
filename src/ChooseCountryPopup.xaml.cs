using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using InternationalPhoneEntry.Models;
using PhoneNumbers;

namespace InternationalPhoneEntry;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ChooseCountryPopup : Popup, INotifyPropertyChanged, IResourcesProvider
{
    #region Fields


    private CountryModel _selectedCountry;
    private List<CountryModel>? _visibleCountries;
    #endregion Fields

    #region Constructors



    public ChooseCountryPopup(CountryModel selectedCountry)
    {
        LoadCountries();
        SelectedCountry = selectedCountry;
        UserStoppedTypingCommand = new Command(UserStoppedTyping);
        InitializeComponent();
        if (PopupFrameStyle is null)
        {
            PopupFrameStyle = this.GetResource<Style>("PopupFrameStyle");
        }
        if (ListViewCountriesItemTemplate is null)
        {
            ListViewCountriesItemTemplate = this.GetResource<DataTemplate>("ListViewCountriesDefaultItemTemplate");
        }
        if (CancelButtonStyle is null)
        {
            CancelButtonStyle = this.GetResource<Style>("CancelButtonDefaultStyle");
        }
        if (DoneButtonStyle is null)
        {
            DoneButtonStyle = this.GetResource<Style>("DoneButtonDefaultStyle");
        }

    }

    #endregion Constructors

    #region Properties
    public bool CloseOnSelect {get;set;}
    public ICommand CountrySelectedCommand { get; set; }
    public ICommand UserStoppedTypingCommand { get; private set; }

    public List<CountryModel>? Countries { get; private set; }

    public List<CountryModel>? VisibleCountries
    {
        get => _visibleCountries; private set
        {
            _visibleCountries = value; OnPropertyChanged(nameof(VisibleCountries));
        }
    }
    public CountryModel? SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            _selectedCountry = value;
            OnPropertyChanged(nameof(SelectedCountry));
        }
    }



    public static readonly BindableProperty CancelButtonTextProperty = BindableProperty.Create(
        nameof(CancelButtonText),
        returnType: typeof(string),
        declaringType: typeof(ChooseCountryPopup),
        "Cancel",
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(CancelButtonText)));

    public string? CancelButtonText
    {
        get => (string?)GetValue(CancelButtonTextProperty);
        set => SetValue(CancelButtonTextProperty, value);
    }

    public static readonly BindableProperty DoneButtonTextProperty = BindableProperty.Create(
        nameof(DoneButtonText),
        returnType: typeof(string),
        declaringType: typeof(ChooseCountryPopup),
        "Done",
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(DoneButtonText)));


    public string? DoneButtonText
    {
        get => (string?)GetValue(DoneButtonTextProperty);
        set => SetValue(DoneButtonTextProperty, value);
    }

    public static readonly BindableProperty SelectedLabelTextProperty = BindableProperty.Create(
        nameof(SelectedLabelText),
        returnType: typeof(string),
        declaringType: typeof(ChooseCountryPopup),
        "Selected",
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(SelectedLabelText)));


    public string? SelectedLabelText
    {
        get => (string?)GetValue(SelectedLabelTextProperty);
        set => SetValue(SelectedLabelTextProperty, value);
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        returnType: typeof(string),
        declaringType: typeof(ChooseCountryPopup),
        "Select your country",
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(Title)));


    public string? Title
    {
        get => (string?)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty SearchPlaceholderProperty = BindableProperty.Create(
        nameof(SearchPlaceholder),
        returnType: typeof(string),
        declaringType: typeof(ChooseCountryPopup),
        "Search for country",
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(SearchPlaceholder)));


    public string? SearchPlaceholder
    {
        get => (string?)GetValue(SearchPlaceholderProperty);
        set => SetValue(SearchPlaceholderProperty, value);
    }



    public static readonly BindableProperty CountriesListLabelTextProperty = BindableProperty.Create(
        nameof(CountriesListLabelText),
        returnType: typeof(string),
        declaringType: typeof(ChooseCountryPopup),
        "Countries",
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(CountriesListLabelText)));


    public string? CountriesListLabelText
    {
        get => (string?)GetValue(CountriesListLabelTextProperty);
        set => SetValue(CountriesListLabelTextProperty, value);
    }

    public static readonly BindableProperty CancelButtonStyleProperty = BindableProperty.Create(
        nameof(CancelButtonStyle),
        returnType: typeof(Style),
        declaringType: typeof(ChooseCountryPopup),
        null,
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(CancelButtonStyle)));


    public Style? CancelButtonStyle
    {
        get => (Style?)GetValue(CancelButtonStyleProperty);
        set => SetValue(CancelButtonStyleProperty, value);
    }
    public static readonly BindableProperty DoneButtonStyleProperty = BindableProperty.Create(
        nameof(DoneButtonStyle),
        returnType: typeof(Style),
        declaringType: typeof(ChooseCountryPopup),
        null,
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(DoneButtonStyle)));


    public Style? DoneButtonStyle
    {
        get => (Style?)GetValue(DoneButtonStyleProperty);
        set => SetValue(DoneButtonStyleProperty, value);
    }

    public static readonly BindableProperty SearchBarStyleProperty = BindableProperty.Create(
        nameof(SearchBarStyle),
        returnType: typeof(Style),
        declaringType: typeof(ChooseCountryPopup),
        null,
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(SearchBarStyle)));


    public Style? SearchBarStyle
    {
        get => (Style?)GetValue(SearchBarStyleProperty);
        set => SetValue(SearchBarStyleProperty, value);
    }


    public static readonly BindableProperty PopupFrameStyleProperty = BindableProperty.Create(
        nameof(PopupFrameStyle),
        returnType: typeof(Style),
        declaringType: typeof(ChooseCountryPopup),
        null,
        BindingMode.OneWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(PopupFrameStyle)));


    public Style? PopupFrameStyle
    {
        get => (Style?)GetValue(PopupFrameStyleProperty);
        set => SetValue(PopupFrameStyleProperty, value);
    }


    public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(
        nameof(SeparatorColor),
        returnType: typeof(Color),
      declaringType: typeof(ChooseCountryPopup),
        Colors.Gray,
        BindingMode.TwoWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(SeparatorColor)));

    public Color? SeparatorColor
    {
        get => (Color?)GetValue(SeparatorColorProperty);
        set => SetValue(SeparatorColorProperty, value);
    }

    public static readonly BindableProperty ListViewCountriesItemTemplateProperty = BindableProperty.Create(
        nameof(ListViewCountriesItemTemplate),
        returnType: typeof(DataTemplate),
        declaringType: typeof(ChooseCountryPopup),
        null,
        BindingMode.TwoWay,
        propertyChanged: (b, o, n) => (b as ChooseCountryPopup)?.OnPropertyChanged(nameof(ListViewCountriesItemTemplate)));


    public DataTemplate? ListViewCountriesItemTemplate
    {
        get => (DataTemplate?)GetValue(ListViewCountriesItemTemplateProperty);
        set => SetValue(ListViewCountriesItemTemplateProperty, value);
    }


    public string? Search { get; set; }
    #endregion Properties

    #region Private Methods

    private void CloseBtn_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void ConfirmBtn_Clicked(object sender, EventArgs e)
    {
        CountrySelectedCommand?.Execute(SelectedCountry);
        Close();
    }

    private void LoadCountries()
    {
        //this is not Task, because it's really fast
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var isoCountries = CountryUtils.GetCountriesByIso3166();
        Countries =
        [
            ..isoCountries.Select(c => new CountryModel(

                countryCode : phoneNumberUtil.GetCountryCodeForRegion(c.TwoLetterISORegionName),
                countryName : c.EnglishName,
                flagUrl : $"https://hatscripts.github.io/circle-flags/flags/{c.TwoLetterISORegionName.ToLower()}.svg",
                twoLetterISORegionName : c.TwoLetterISORegionName

                ))
        ];
        VisibleCountries = [.. Countries];
    }

    private void UserStoppedTyping()
    {
        VisibleCountries =
        [
            ..string.IsNullOrWhiteSpace(Search)
                ? Countries
                : Countries.Where(country =>
                    country.CountryName.Contains(Search, StringComparison.InvariantCultureIgnoreCase))
        ];
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        SelectedCountry = e.Item as CountryModel;
        if (CloseOnSelect)
        {
            CountrySelectedCommand?.Execute(SelectedCountry);
            Close();
        }
    }
    #endregion Private Methods


}