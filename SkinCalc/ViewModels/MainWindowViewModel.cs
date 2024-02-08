using Logic;
using SkinCalc.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
namespace SkinCalc.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {

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

        public static ObservableCollection<SkinsInfo> GenerateDummyData()
        {
            return new ObservableCollection<SkinsInfo>
            {
                new SkinsInfo { Name = "Dragon Lore", CaseCost = 1.00 },
                new SkinsInfo { Name = "Medusa", CaseCost = 1.5 },
                new SkinsInfo { Name = "The Emperor", CaseCost = 1.75 },
                new SkinsInfo { Name = "Asiimov", CaseCost = 1.4 },
                new SkinsInfo { Name = "Hyper Beast", CaseCost = 1.99 },
            };
        }



        private SkinsInfo _SelectedItem;
        public SkinsInfo SelectedItem
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


        public ObservableCollection<SkinsInfo> SkinsInfo { get; set; } = GenerateDummyData();

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

        public void TriggerCalculation()
        {
            if (this.AvailableMoney == 0d || SelectedItem == default)
            {
                return;
            }
            this.CalcResponse = LogicClass.CalcLogic(AvailableMoney, KeyCost, SelectedItem.CaseCost);
        }

    }
}
