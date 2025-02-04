using InternationalPhoneEntry.Models;

namespace InternationalPhoneEntry;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CountryCodeView : ContentView
{
    public static readonly BindableProperty CountryProperty = BindableProperty.Create(
        nameof(Country),
        returnType: typeof(CountryModel),
        declaringType: typeof(CountryCodeView),
        CountryPicker.DefaultCountry,
        BindingMode.TwoWay,
        propertyChanged: CountryPropertyChanged);

    private static void CountryPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is CountryCodeView countryCodeView)
        {
            countryCodeView.OnPropertyChanged(nameof(Country));
            countryCodeView.OnPropertyChanged(nameof(TwoLetterISORegionName));
        }
    }

    public string? TwoLetterISORegionName
    {
        get => Country?.TwoLetterISORegionName;
        set => SetValue(TwoLetterISORegionNameProperty, value);
    }

    public static readonly BindableProperty TwoLetterISORegionNameProperty = BindableProperty.Create(
        nameof(TwoLetterISORegionName),
        returnType: typeof(string),
        declaringType: typeof(CountryCodeView),
        null,
        BindingMode.TwoWay,
        propertyChanged: TwoLetterISORegionNamePropertyPropertyChanged);

    private static void TwoLetterISORegionNamePropertyPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (oldvalue?.ToString() != newvalue.ToString())
        {
            if (bindable is CountryCodeView countryCodeView)
            {
                countryCodeView.Country = CountryModel.GetCountryModelByTwoLetterISORegionName(newvalue.ToString());
            }
        }
    }

    public CountryModel? Country
    {
        get => (CountryModel?)GetValue(CountryProperty);
        set => SetValue(CountryProperty, value);
    }

    public CountryCodeView()
    {
        InitializeComponent();
    }
}