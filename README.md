# InternationalPhoneEntry-MAUI
A customised Entry to input international phone number along with country code.

[![NuGet version (InternationalPhoneEntry-MAUI)](https://img.shields.io/nuget/v/InternationalPhoneEntry-MAUI.svg)](https://www.nuget.org/packages/InternationalPhoneEntry-MAUI/)

This nuget can be used to make customised text field to take phone number input for any country along with an option to choose country code from a dropdown.

![Sample](https://raw.githubusercontent.com/Jon2G/MAUI-InternationalPhoneEntry/main/sample.gif)

Usage:
```xml
 <VerticalStackLayout Padding="16" Spacing="25">

     <VerticalStackLayout Spacing="0">
         <Label Style="{StaticResource LabelStyle}" Text="Country code view only:" />
         <ipe:CountryCodeView TwoLetterISORegionName="MX" />

     </VerticalStackLayout>
     <BoxView BackgroundColor="Gray" HeightRequest="1" />
     <VerticalStackLayout Spacing="0">
         <Label Style="{StaticResource LabelStyle}" Text="Country code picker:" />

         <ipe:CountryPicker CloseOnSelect="True" />

     </VerticalStackLayout>
     <BoxView BackgroundColor="Gray" HeightRequest="1" />
     <VerticalStackLayout Spacing="0">
         <Label Style="{StaticResource LabelStyle}" Text="Country code phone editor:" />
         <ipe:PhoneEntry CloseOnSelect="True" />
     </VerticalStackLayout>

 </VerticalStackLayout>
```
