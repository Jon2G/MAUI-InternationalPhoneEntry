using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using InternationalPhoneEntry.Models;
using Microsoft.Maui.Controls;
using System.Globalization;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InternationalPhoneEntry
{
    // All the code in this file is included in all platforms.
    public static class InternationalPhoneEntry
    {
        public static CountryModel DefaultCountry
        {
            get
            {
                try
                {
                    var region = new RegionInfo(CultureInfo.CurrentCulture.Name);
                    return CountryModel.GetCountryModelByTwoLetterISORegionName(region.TwoLetterISORegionName);
                }
                catch
                {
                    return CountryModel.GetCountryModelByThreeLetterISORegionName("US");
                }
            }
        }

        public static bool IsInitialized { get; private set; }
        public static MauiAppBuilder UseInternationalPhoneEntry(this MauiAppBuilder builder)
        {
            if (IsInitialized)
            {
                return builder;
            }
            IsInitialized = true;
            return builder.UseMauiCommunityToolkit().UseFFImageLoading();

        }

        public static void VerifyIsInited()
        {
            if (InternationalPhoneEntry.IsInitialized == false)
            {
                throw new InvalidOperationException("InternationalPhoneEntry should be initialized before using ChooseCountryPopup call builder.UseInternationalPhoneEntry();");
            }
        }
    }
}
