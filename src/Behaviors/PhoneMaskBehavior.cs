using CommunityToolkit.Maui.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternationalPhoneEntry.Models;
using PhoneNumbers;
using PhoneNumberBuilder = PhoneNumbers.PhoneNumber.Builder;
using System.Text.RegularExpressions;
namespace InternationalPhoneEntry.Behaviors
{
    struct PhoneFormat
    {
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
    /// <summary>
    /// The MaskedBehavior is a behavior that allows the user to define an input mask for data entry. Adding this behavior to an <see cref="InputView"/> (i.e. <see cref="Entry"/>) control will force the user to only input values matching a given mask. Examples of its usage include input of a credit card number or a phone number.
    /// </summary>
    public class PhoneMaskBehavior : BaseBehavior<InputView>, IDisposable
    {
        private readonly PhoneNumberUtil PhoneNumberUtil = PhoneNumberUtil.GetInstance();

        public static readonly BindableProperty CountryProperty = BindableProperty.Create(
            nameof(Country),
            returnType: typeof(CountryModel),
            declaringType: typeof(PhoneMaskBehavior),
            CountryPicker.DefaultCountry,
            BindingMode.TwoWay,
            propertyChanged: CountryPropertyChanged);

        private static async void CountryPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PhoneMaskBehavior countryCodeView)
            {
                if (countryCodeView.View is not null)
                {
                    countryCodeView.View.Text = string.Empty;
                }
                countryCodeView.OnPropertyChanged(nameof(Country));

                countryCodeView.CountryCodeRegex = GetRegexForCountryCode(countryCodeView.Country);

                var maskedBehavior = (PhoneMaskBehavior)bindable;
                await maskedBehavior.OnMaskChanged(CancellationToken.None).ConfigureAwait(false);


            }
        }

        private static Regex GetRegexForCountryCode(CountryModel country)
        {
            return new Regex(@"\(?\+" + country.CountryCode + @"\)?", RegexOptions.Compiled);
        }

        public CountryModel Country
        {
            get => (CountryModel?)GetValue(CountryProperty) ?? CountryPicker.DefaultCountry;
            set => SetValue(CountryProperty, value);
        }

        readonly SemaphoreSlim applyMaskSemaphoreSlim = new(1, 1);

        bool isDisposed;

        private Regex? CountryCodeRegex;

        /// <summary>
        /// Finalizer
        /// </summary>
        ~PhoneMaskBehavior() => Dispose(false);

        public PhoneMaskBehavior()
        {

        }




        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                applyMaskSemaphoreSlim.Dispose();
            }

            isDisposed = true;
        }

        /// <inheritdoc />
        protected override async void OnViewPropertyChanged(InputView sender, PropertyChangedEventArgs e)
        {
            base.OnViewPropertyChanged(sender, e);

            if (e.PropertyName == InputView.TextProperty.PropertyName)
            {
                await OnTextPropertyChanged(CancellationToken.None);
            }
        }



        Task OnTextPropertyChanged(CancellationToken token)
        {
            // Android does not play well when we update the Text inside the TextChanged event. 
            // Therefore if we dispatch the mechanism of updating the Text property it solves the issue of the caret position being updated incorrectly.
            // https://github.com/CommunityToolkit/Maui/issues/460
            return View?.Dispatcher.DispatchAsync(() => ApplyMask(View?.Text, token)) ?? Task.CompletedTask;
        }


        async ValueTask OnMaskChanged(CancellationToken token)
        {
            //if (mask is null)
            //{
            //    maskPositions = null;
            //    return;
            //}

            var originalText = RemoveMask(View?.Text);

            await ApplyMask(originalText, token);
        }

        [return: NotNullIfNotNull(nameof(text))]
        string? RemoveMask(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return string.Join(string.Empty, text.Split());
        }
        private PhoneFormat ParsePhoneFormat(string text)
        {
            CountryCodeRegex ??= GetRegexForCountryCode(Country);
            text = CountryCodeRegex.Replace(text, string.Empty).Trim();
            text = text.Replace(this.Country.GroupSeparator,string.Empty);

            if (!string.IsNullOrEmpty(text) && text.Length > this.Country.GroupEach)
            {
                var joined = string.Join(this.Country.GroupSeparator, text.Chunk(this.Country.GroupEach)
                    .Select(x => new string(x))
                    .ToArray());

                text = joined;
            }

            return new PhoneFormat
            {
                CountryCode = $"({Country.CountryCode})",
                PhoneNumber = text
            };
        }

        async Task ApplyMask(string? text, CancellationToken token)
        {
            await applyMaskSemaphoreSlim.WaitAsync(token);

            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var phoneFormat = ParsePhoneFormat(text);
                    text = Country.AddCountryCode ? $"+{Country.CountryCode} {phoneFormat.PhoneNumber}" : phoneFormat.PhoneNumber;

                    if (View is not null)
                    {
                        View.Text = text;
                    }
                }
            }
            finally
            {
                applyMaskSemaphoreSlim.Release();
            }
        }
    }
}
