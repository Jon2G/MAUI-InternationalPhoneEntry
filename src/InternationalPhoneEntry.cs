using CommunityToolkit.Maui;
using FFImageLoading.Maui;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InternationalPhoneEntry
{
    // All the code in this file is included in all platforms.
    public static class InternationalPhoneEntry
    {
        public static MauiAppBuilder UseInternationalPhoneEntry(this MauiAppBuilder builder)
        => builder.UseMauiCommunityToolkit().UseFFImageLoading();



    }
}
