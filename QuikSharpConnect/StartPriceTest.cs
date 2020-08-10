using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace WPFQuikBotTest.QuikSharpConnect
{
    public delegate void TrendsHandler(string message);

    public class StartPriceTest
    {
       

       





    }

    public class Trends
    {
       

        #region События
        public event TrendsHandler TrenndsNotify;
        #endregion

        #region Trand

        #region Prices
        private double _LowPrice = 2937;
        private double _HighPrice = 2352;

        public double LowPrice
        {

            get => _LowPrice;
            set => _LowPrice = value;

        }

        public double HighPrice
        {

            get => _HighPrice;
            set => _HighPrice = value;

        }
        #endregion

        private double _Diff;

        private double _Trand;
        public double Trand
        {
            get => _Diff;
            set
            {                
                if(_LowPrice > value)
                {
                    _LowPrice = value;
                    // Тут должно быть событие //
                }
                
                if(_HighPrice < value)
                {
                    _HighPrice = value;
                    // Тут должно быть событие //
                }

                _Diff = _HighPrice - _LowPrice;

            }
        }
        #endregion

        #region Price



        #endregion

    }

}
