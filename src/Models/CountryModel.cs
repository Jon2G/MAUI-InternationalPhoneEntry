using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;

namespace InternationalPhoneEntry.Models
{
    public class CountryModel : INotifyPropertyChanged
    {
        public const int DefaultGroupEach = 2;
        public const string DefaultGroupSeparator = "-";

        #region Fields
        private bool _AddCountryCode;
        private string _flagUrl;
        private string _countryName;
        private int _countryCode;
        private string _twoLetterISORegionName;
        private int _GroupEach;
        private string? _GroupSeparator;

        #endregion Fields

        #region Properties
        public bool AddCountryCode
        {
            get => _AddCountryCode;
            set => SetProperty(ref _AddCountryCode, value);
        }
        public string GroupSeparator
        {
            get => _GroupSeparator ?? DefaultGroupSeparator;
            set => SetProperty(ref _GroupSeparator, value);
        }
        public int GroupEach
        {
            get => _GroupEach;
            set => SetProperty(ref _GroupEach, value);
        }

        public string FlagUrl
        {
            get => _flagUrl;
            set => SetProperty(ref _flagUrl, value);
        }

        public string CountryName
        {
            get => _countryName;
            set => SetProperty(ref _countryName, value);
        }

        public int CountryCode
        {
            get => _countryCode;
            set => SetProperty(ref _countryCode, value);
        }

        public string TwoLetterISORegionName
        {
            get => _twoLetterISORegionName;
            set => SetProperty(ref _twoLetterISORegionName, value);
        }

        #endregion Properties

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return this?.TwoLetterISORegionName ?? "??";
        }

        #endregion

        public CountryModel(
        string flagUrl,
        string countryName,
        int countryCode,
        string twoLetterISORegionName,
        int? groupEach = null,
        bool addCountryCode = true
            )
        {
            FlagUrl = flagUrl;
            CountryName = countryName;
            CountryCode = countryCode;
            TwoLetterISORegionName = twoLetterISORegionName;
            GroupEach = groupEach ?? DefaultGroupEach;
            AddCountryCode = addCountryCode;
        }

        /// <summary>
        /// Get Country Model by Country Name
        /// </summary>
        /// <param name="threeLettersISORegionName">Three Letters ISO Region Name of Country</param>
        /// <returns>Complete Country Model with Region, Flag, Name and Code</returns>
        public static CountryModel GetCountryModelByThreeLetterISORegionName(string threeLettersISORegionName)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var isoCountries = CountryUtils.GetCountriesByIso3166();
            var regionInfo = isoCountries.FirstOrDefault(c => c.ThreeLetterISORegionName == threeLettersISORegionName);
            return regionInfo != null
                ? new CountryModel(

                    countryCode: phoneNumberUtil.GetCountryCodeForRegion(regionInfo.TwoLetterISORegionName),
                    countryName: regionInfo.EnglishName,
                    flagUrl: $"https://hatscripts.github.io/circle-flags/flags/{regionInfo.TwoLetterISORegionName.ToLower()}.svg",
                    twoLetterISORegionName: regionInfo.TwoLetterISORegionName


                    )
                : throw new KeyNotFoundException("Three letters iso region not found");
        }
        /// <summary>
        /// Get Country Model by Country Name
        /// </summary>
        /// <param name="twoLettersISORegionName">Two Letters ISO Region Name of Country</param>
        /// <returns>Complete Country Model with Region, Flag, Name and Code</returns>
        public static CountryModel GetCountryModelByTwoLetterISORegionName(string twoLettersISORegionName)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var isoCountries = CountryUtils.GetCountriesByIso3166();
            var regionInfo = isoCountries.FirstOrDefault(c => c.TwoLetterISORegionName == twoLettersISORegionName);
            return regionInfo != null
                ? new CountryModel(
                    countryCode: phoneNumberUtil.GetCountryCodeForRegion(regionInfo.TwoLetterISORegionName),
                    countryName: regionInfo.EnglishName,
                    flagUrl: $"https://hatscripts.github.io/circle-flags/flags/{regionInfo.TwoLetterISORegionName.ToLower()}.svg",
                    twoLetterISORegionName: regionInfo.TwoLetterISORegionName
                    )
                : throw new KeyNotFoundException("Two letters iso region not found");
        }
        /// <summary>
        /// Get Country Model by Country Name
        /// </summary>
        /// <param name="englishName">English Name of Country</param>
        /// <returns>Complete Country Model with Region, Flag, Name and Code</returns>
        public static CountryModel GetCountryModelByEnglishName(string englishName)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var isoCountries = CountryUtils.GetCountriesByIso3166();
            var regionInfo = isoCountries.FirstOrDefault(c => c.EnglishName == englishName);
            return regionInfo != null
                ? new CountryModel(
                    countryCode: phoneNumberUtil.GetCountryCodeForRegion(regionInfo.TwoLetterISORegionName),
                    countryName: regionInfo.EnglishName,
                    flagUrl: $"https://hatscripts.github.io/circle-flags/flags/{regionInfo.TwoLetterISORegionName.ToLower()}.svg",
                    twoLetterISORegionName: regionInfo.TwoLetterISORegionName
                    )
                : throw new KeyNotFoundException("Country not found");
        }
    }
}
