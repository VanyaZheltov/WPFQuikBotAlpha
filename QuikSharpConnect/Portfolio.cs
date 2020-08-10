using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFQuikBotTest.QuikSharpConnect
{
    class Portfolio
    {
        List<double> Transaction = new List<double>();


        #region _StatusPosition
        private bool _StatusPosition = false;
        public bool StatusPosition
        {
            get => _StatusPosition;

            set
            {
                _StatusPosition = value;
            }
        }
        #endregion
        #region _StatusTransaction
        private string _StatusTransaction;
        public string StatusTransaction
        {
            get => _StatusTransaction;

            set
            {
                _StatusTransaction = value;
            }
        }
        #endregion
        #region PriceStart
        private double _PriceStart;
        public double PriceStart
        {
            get => _PriceStart;

            set
            {
                _PriceStart = value;
            }
        }
        #endregion
        #region Total
        private double _Total;
        public double Total
        {
            get => _Total;

            set
            {
                _Total = value;
            }
        }
        #endregion
        #region PriceStop
        private double _PriceStop;
        public double PriceStop
        {
            get => _PriceStop;

            set
            {
                _PriceStop = value;
            }
        }
        #endregion
        public void TotalTransaction()
        {
            if(StatusTransaction == "buy")
            {
                var total = _PriceStop - _PriceStart;
                Transaction.Add(total);
            }
            if(StatusTransaction == "sell")
            {
                var total = _PriceStart - _PriceStop;
                Transaction.Add(total);
            }
        }
        public double TotalS()
        {
            return Transaction.Sum();
        }

    }
}
