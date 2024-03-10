using Logic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using WebScraper;
using WebScraper.Extentions;
namespace SkinCalc.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        private double _KeyCost = 1.5d;
        public double KeyCost
        {
            get
            {
                return this._KeyCost;
            }
            set
            {
                this._KeyCost = value;
                base.OnPropertyChanged(nameof(this.KeyCost));
            }
        }

        public double _AvailableMoney;
        public double AvailableMoney
        {
            get => _AvailableMoney;
            set
            {
                _AvailableMoney = value;
                OnPropertyChanged(nameof(AvailableMoney));
                this.TriggerCalculation();
            }
        }

        private CalculationLogicResponse _CalcResponse;
        public CalculationLogicResponse CalcResponse
        {
            get
            {
                return this._CalcResponse;
            }
            set
            {
                this._CalcResponse = value;
                base.OnPropertyChanged(nameof(this.CalcResponse));
            }
        }

        private ObservableCollection<WebScraper.Model.SkinCase> _SkinCaseInfos;
        public ObservableCollection<WebScraper.Model.SkinCase> SkinCaseInfos
        {
            get
            {
                return this._SkinCaseInfos;
            }
            set
            {
                this._SkinCaseInfos = value;
                base.OnPropertyChanged(nameof(this.SkinCaseInfos));
            }
        }

        private WebScraper.Model.SkinCase _SelectedItem;
        public WebScraper.Model.SkinCase SelectedItem
        {
            get
            {
                return this._SelectedItem;
            }
            set
            {
                this._SelectedItem = value;
                base.OnPropertyChanged(nameof(this.SelectedItem));
                this.TriggerCalculation();
            }
        }

        private string _AvailableMoneyString;
        public string AvailableMoneyString
        {
            get
            {
                return this._AvailableMoneyString;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.Match(value, "^(?!,$)[\\d,.]+$").Success)
                {
                    return;
                }
                this._AvailableMoneyString = value;
                this.AvailableMoney = string.IsNullOrEmpty(this.AvailableMoneyString) ? 0d : double.Parse(this.AvailableMoneyString.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                base.OnPropertyChanged(nameof(this.AvailableMoneyString));
            }
        }

        #endregion

        #region Constructor
        public MainWindowViewModel()
        {

            Task.Run(async () =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await GetSkinCaseInfos();
                stopwatch.Stop();
                Debug.Print($"Download Skin Case infos from the Website ready in  {stopwatch.Elapsed:mm\\:ss\\:ffffff}");

            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves skin case information from a web scraper, stores it in an `ObservableCollection`,
        /// and initiates image downloads for the cases.
        /// </summary>
        public async Task GetSkinCaseInfos()
        {
            string myAppDataForImages = AppDatafileCheck();
            WebScraperClass webScraper = new();
            ObservableCollection<WebScraper.Model.SkinCase> observableCollectionSkinInfos = new(await webScraper.ExtractSkinCases());
            SkinCaseInfos = observableCollectionSkinInfos;
            SkincaseImageDownloadINAppDataFiles(SkinCaseInfos, myAppDataForImages);

        }

        /// <summary>
        /// Checks and creates required application data folders in the user's AppData directory.
        /// </summary>
        /// <returns>The path to the "SkinCalc/Images" folder.</returns>
        private static string AppDatafileCheck()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string myAppData = Path.Combine(localAppData, "SkinCalc");

            if (!Directory.Exists(myAppData))
            {
                Directory.CreateDirectory(myAppData);
            }
            string myAppDataForImages = Path.Combine(myAppData, "Images");

            if (!Directory.Exists(myAppDataForImages))
            {
                Directory.CreateDirectory(myAppDataForImages);
            }

            return myAppDataForImages;
        }

        /// <summary>
        /// Downloads and stores images for skin cases in the specified app data folder.
        /// </summary>
        /// <param name="SkinCaseInfos">The collection of skin case objects.</param>
        /// <param name="myAppDataForImages">The path to the "SkinCalc/Images" folder.</param>
        private static void SkincaseImageDownloadINAppDataFiles(ObservableCollection<WebScraper.Model.SkinCase> SkinCaseInfos, string myAppDataForImages)
        {

            foreach (var skinCaseInfo in SkinCaseInfos)
            {
                string SkincaseImagePathName = Path.Combine(myAppDataForImages, skinCaseInfo.Name.Replace(":", "_").Replace("&amp;", "&") + ".png");
                if (File.Exists(SkincaseImagePathName))
                {
                    skinCaseInfo.Image = File.ReadAllBytes(SkincaseImagePathName);
                }
                else
                {
                    skinCaseInfo.DownloadImage();
                    File.WriteAllBytes(SkincaseImagePathName, skinCaseInfo.Image);
                }
            }

        }

        /// <summary>
        /// Triggers a calculation using `LogicClass.CalcLogic()`, but only if the user has entered
        /// valid input for available money and a selected skin case.
        /// </summary>
        private void TriggerCalculation()
        {
            if (this.AvailableMoney == 0d || SelectedItem == default)
            {
                return;
            }
            this.CalcResponse = LogicClass.CalcLogic(AvailableMoney, KeyCost, (double)SelectedItem.Price);
        }

        #endregion
    }
}
