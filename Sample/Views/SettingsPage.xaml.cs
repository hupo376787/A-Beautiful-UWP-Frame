using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Sample.Helpers;
using Sample.Services;

using Windows.ApplicationModel;
using Windows.Services.Store;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sample.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        //// TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
        //// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();

            comboBoxLanguage.Items.Add("auto Auto");
            comboBoxLanguage.Items.Add("en-us English");
            comboBoxLanguage.Items.Add("zh-cn 简体中文");

            string strCurrentLanguage = (Application.Current as App).strCurrentLanguage;
            string strTempAutoLang = strCurrentLanguage;
            if (strCurrentLanguage.ToLower().Equals("auto"))
                strCurrentLanguage = LanguageHelper.GetCurLanguage();

            comboBoxLanguage.SelectedIndex = 0;
            if (!strTempAutoLang.Equals("auto"))
            {
                for (int i = 0; i < comboBoxLanguage.Items.Count; i++)
                {
                    string temp = comboBoxLanguage.Items[i].ToString();
                    string[] tempArr = temp.Split(' ');
                    if (strCurrentLanguage == tempArr[0])
                        comboBoxLanguage.SelectedIndex = i;
                }
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void comboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string temp = comboBoxLanguage.SelectedItem.ToString();
            string[] tempArr = temp.Split(' ');
            ApplicationData.Current.LocalSettings.Values["strCurrentLanguage"] = tempArr[0];
        }
    }
}
