using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFQuikBotTest.QuikSharpConnect.Indicator
{
    public delegate void SMAHandler();
    public class IndicatorQuik
    {
       





    }
    public class SMA
    {
        #region События
        public event SMAHandler SMANotify;
        #endregion
        private double _Price;

        #region SMAValues1
        private int _SMAValues1 = 2;
        public int SMAValues1
        {
            get => _SMAValues1;
            set
            {
                _SMAValues1 = value;
            }
        }
        #endregion

        #region SMAValues2
        private int _SMAValues2 = 9;
        public int SMAValues2
        {
            get => _SMAValues2;
            set
            {
                _SMAValues2 = value;
            }
        }
        #endregion

        #region SMAValues3
        private int _SMAValues3 = 16;
        public int SMAValues3
        {
            get => _SMAValues3;
            set
            {
                _SMAValues3 = value;
            }
        }
        #endregion

        #region PriceSMA
        

        public List<double> Sma_1 = new List<double>();
        public List<double> Sma_2 = new List<double>();
        public List<double> Sma_3 = new List<double>();

        private double SMA_total_1;
        private double SMA_total_2;
        private double SMA_total_3;


        public double SmaTotal_1 { get => SMA_total_1; set => SMA_total_1 = value; }
        public double SmaTotal_2 { get => SMA_total_2; set => SMA_total_2 = value; }
        public double SmaTotal_3 { get => SMA_total_3; set => SMA_total_3 = value; }



        public double Price
        {
            get => _Price;
            set
            {
                _Price = value;
                Sma_1.Add(_Price);
                if (Sma_1.Count == SMAValues1)
                {
                    SmaTotal_1 = Sma_1.Sum() / Sma_1.Count;
                    Sma_1.RemoveAt(0);

                }
                Sma_2.Add(_Price);
                if (Sma_2.Count == SMAValues2)
                {
                    SmaTotal_2 = Sma_2.Sum() / Sma_2.Count;
                    Sma_2.RemoveAt(0);

                }
                Sma_3.Add(_Price);
                if (Sma_3.Count == SMAValues3)
                {
                    SmaTotal_3 = Sma_3.Sum() / Sma_3.Count;
                    Sma_3.RemoveAt(0);
                    

                }
            }
        }

        
        #endregion
        public void SMAIndicator()
        {
            
        }
    }
}
