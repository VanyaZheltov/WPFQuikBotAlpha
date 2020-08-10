using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFQuikBotTest.QuikSharpConnect.Indicator;

namespace WPFQuikBotTest.QuikSharpConnect
{
   public class QuikSharpStart
    {

        public delegate void ConnectHandler(string message);
        public delegate void IndicatoHandler(double message);

        public event ConnectHandler LastPriceNotify;

        public event IndicatoHandler LowNotify;
        public event IndicatoHandler HighNotify;
        public event IndicatoHandler DiffNotify;

        public event IndicatoHandler SMA1Notify;
        public event IndicatoHandler SMA2Notify;
        public event IndicatoHandler SMA3Notify;

        public event ConnectHandler StatusNotify;

        public Trends trends;
        public SMA sma;

        public DataChoice DataChoice;

        public List<double> Candles = new List<double>();

        #region HighLow
        private double _Diff;
        private double _Low;
        private double _High;
         

        public double Low
        {
            get => _Low;
            set
            {
                _Low = value;
                LowNotify?.Invoke(value);
            }
        }
        public double High
        {
            get => _High;
            set
            {
                _High = value;
                HighNotify?.Invoke(value);
            }
        }
        public double Diff
        {
            get => _Diff;
            set
            {
                _Diff = value;
                DiffNotify?.Invoke(value);
            }
        }

        #endregion

        #region SMA
        private double _SMA1;
        private double _SMA2;
        private double _SMA3;
        //-------------------------------------------//-----------------------------------------------//
        public double SMA1
        {
            get => _SMA1;
            set
            {
                _SMA1 = value;
                SMA1Notify?.Invoke(value);
            }
        }
        public double SMA2
        {
            get => _SMA2;
            set
            {
                _SMA2 = value;
                SMA2Notify?.Invoke(value);
            }
        }
        public double SMA3
        {
            get => _SMA3;
            set
            {
                _SMA3 = value;
                SMA3Notify?.Invoke(value);
            }
        }
        #endregion
        #region Candles
        private double _CandlesMin;
        private double _CandlesMax;
        private double _CandlesOpen;
        private double _CandlesClose;

        public double CandlesMin
        {
            get => _CandlesMin;
            set
            {
                _CandlesMin = value;
                
            }
        }
        public double CandlesMax
        {
            get => _CandlesMax;
            set
            {
                _CandlesMax = value;
                
            }
        }
        public double CandlesOpen
        {
            get => _CandlesOpen;
            set
            {
                _CandlesOpen = value;
                
            }
        }
        public double CandlesClose
        {
            get => _CandlesClose;
            set
            {
                _CandlesClose = value;

            }
        }
        #endregion

        #region Status
        private string _Status;

        public string Status
        {
            get => _Status;
            set
            {
                _Status = value;
                StatusNotify?.Invoke(value);
            }
        }
        #endregion


        #region CandlesTime
        private int _CandlesTime = 60;
        public int CandlesTime { get => _CandlesTime; set => _CandlesTime = value; }
        #endregion
        



        private string _LastPrice = "----";
        public string LastPrice
        {
            get => _LastPrice;

            set
            {
                _LastPrice = value;
                var total_p = Convert.ToDouble(value);
                LastPriceNotify?.Invoke(value);
                Candles.Add(total_p);

                if (Candles.Count == CandlesTime)
                {
                    double psma = Candles.Count / Candles.Sum();

                    CandlesClose = Candles.Min();
                    CandlesMax = Candles.Max();
                    CandlesOpen = Candles[0];
                    CandlesClose = Candles.Last();



                    sma.Price = psma;
                    SMA1 = sma.SmaTotal_1;
                    SMA2 = sma.SmaTotal_2;
                    SMA3 = sma.SmaTotal_3;

                    Candles.Clear();

                }
                trends.Trand = total_p;
                Diff = trends.Trand;
                Low = trends.LowPrice;
                High = trends.HighPrice;
            }
        }

        public void RobotStart()
        {
            DataChoice = new DataChoice();
            DataChoice.Type_Quik = "Quik";
            DataChoice.TimeOut = 1000;
            DataChoice.SecCode = "MMU0";
            DataChoice.StatusNotifyData += StatusData;
            DataChoice.LastPriceNotifyData += LastPriceData;
            DataChoice.Start();
        }
        public void LastPriceData(string mes)
        {
            LastPrice = mes;
        }
        public void StatusData(string mes)
        {
            Status = mes;
        }




    }
}
