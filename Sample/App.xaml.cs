using System;
using Sample.Helpers;
using Sample.Services;
using Sample.Views;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace Sample
{
    public sealed partial class App : Application
    {
        public string strCurrentLanguage = "en-us";

        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            if (ApplicationData.Current.LocalSettings.Values["strCurrentLanguage"] != null)
            {
                strCurrentLanguage = ApplicationData.Current.LocalSettings.Values["strCurrentLanguage"].ToString();
                if (strCurrentLanguage == "auto")
                {
                    ApplicationLanguages.PrimaryLanguageOverride = LanguageHelper.GetCurLanguage();
                }
                else
                    ApplicationLanguages.PrimaryLanguageOverride = strCurrentLanguage;
            }
            else
            {
                ApplicationLanguages.PrimaryLanguageOverride = strCurrentLanguage = LanguageHelper.GetCurLanguage();
                //ApplicationLanguages.PrimaryLanguageOverride = strCurrentLanguage = "en-us";
            }

            InitializeComponent();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
