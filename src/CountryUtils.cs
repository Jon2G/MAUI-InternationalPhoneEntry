﻿using System.Globalization;

namespace InternationalPhoneEntry;

public static class CountryUtils
{
    /// <summary>
    /// Gets the list of countries based on ISO 3166-1
    /// </summary>
    /// <returns>Returns the list of countries based on ISO 3166-1</returns>
    public static List<RegionInfo> GetCountriesByIso3166()
    {
        var countries = new List<RegionInfo>();
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            var info = new RegionInfo(culture.Name);
            if (countries.All(p => p.Name != info.Name))
                countries.Add(info);
        }
        return countries.OrderBy(p => p.EnglishName).ToList();
    }


}
