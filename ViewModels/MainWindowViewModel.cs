using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFQuikBotTest.QuikSharpConnect;
using WPFQuikBotTest.ViewModels.Base;
using WPFQuikBotTest.Infrastructure.Commands.Base;
using WPFQuikBotTest.Infrastructure.Commands;
using WPFQuikBotTest.Model;


namespace WPFQuikBotTest.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public double pointY = 0;

        #region Title
        private string _Title = "WPFBot";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region ConnectIP
        private string _ConnectIP = "127.0.0.1";

        public string ConnectIP
        {
            get => _ConnectIP;
            set => Set(ref _ConnectIP, value);
        }
        #endregion

        #region Status
        private string _Status = "Не подключено";

        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion

        #region TestDataPoints : IEnumerable<DataPoint>

        /// <summary>
        ///  Тестовый набор данных для графиков
        /// </summary>
        private IEnumerable<DataPoint> _TestDataPoints;

        /// <summary>
        ///  Тестовый набор данных для графиков
        /// </summary>
        public IEnumerable<DataPoint> TestDataPoint { get => _TestDataPoints; set => Set(ref _TestDataPoints, value); }

        #endregion
        #region SMA Check
        private string _SMACheck;

        public string SMACheck
        {
            get => _SMACheck;
            set => Set(ref _SMACheck, value);
        }
        #endregion
        #region SMA1
        private string _SMA1;

        public string SMA1
        {
            get => _SMA1;
            set => Set(ref _SMA1, value);
        }
        #endregion
        #region SMA2
        private string _SMA2;

        public string SMA2
        {
            get => _SMA2;
            set => Set(ref _SMA2, value);
        }
        #endregion
        #region SMA3
        private string _SMA3;

        public string SMA3
        {
            get => _SMA3;
            set => Set(ref _SMA3, value);
        }
        #endregion

        #region Low
        private string _Low;

        public string Low
        {
            get => _Low;
            set => Set(ref _Low, value);
        }
        #endregion
        #region High
        private string _High;

        public string High
        {
            get => _High;
            set => Set(ref _High, value);
        }
        #endregion
        #region Diff
        private string _Diff;

        public string Diff
        {
            get => _Diff;
            set => Set(ref _Diff, value);
        }
        #endregion
        #region SecCode
        private string _SecCode;

        public string SecCode
        {
            get => _SecCode;
            set => Set(ref _SecCode, value);
        }
        #endregion
        #region Tiker
        private string _Tiker;

        public string Tiker
        {
            get => _Tiker;
            set => Set(ref _Tiker, value);
        }
        #endregion
        #region Time
        private string _Time;

        public string Time
        {
            get => _Time;
            set => Set(ref _Time, value);
        }
        private string _FullTime;

        public string FullTime
        {
            get => _FullTime;
            set => Set(ref _FullTime, value);
        }
        #endregion
        #region LastPrice
        private string _LastPrice;

        public string LastPrice
        {
            get => _LastPrice;
            set => Set(ref _LastPrice, value);
        }
        #endregion
        #region LastPrice2
        private string _LastPrice2;

        public string LastPrice2
        {
            get => _LastPrice2;
            set => Set(ref _LastPrice2, value);
        }
        #endregion
        #region Total
        private string _Total = "";

        public string Total
        {
            get => _Total;
            set => Set(ref _Total, value);
        }
        #endregion
        #region ClientCode
        private string _ClientCode;

        public string ClientCode
        {
            get => _ClientCode;
            set => Set(ref _ClientCode, value);
        }
        #endregion

        #region Команды

        #region ConnectCommand

        public ICommand ConnectCommand { get; }

        private void OnConnectCommandExecuted(object p)
        {
            Start();
        }
        private bool CanConnectCommandExecuted(object p) => true;

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            ConnectCommand = new LambdaCommand(OnConnectCommandExecuted, CanConnectCommandExecuted);

            #endregion

            //var data_points = new List<DataPoint>((int)(360 / 0.1));
            //for (var x = 0d; x <= 360; x += 0.1)
            //{
            //    const double to_rad = Math.PI / 180;
            //    var y = Math.Sin(x * to_rad);

            //    data_points.Add(new DataPoint { XValue = x, YValue = y });
            //}
            //TestDataPoint = data_points;



        }

        public void Start()
        {
            Test testc = new Test();
            Test testc2 = new Test();

            testc.SecCode = "MMU0";
            testc2.SecCode = "RIU0";

            testc.ConnectNotify += Mes;
            testc2.LastPriceNotify += MesLastPrice2;
            //--------------------------------------------------------------
            testc.OpenFile();
            //--------------------------------------------------------------
            //testc2.ConnectQuik();
            //testc2.RunQuik();
            //testc2.AsyncLastPrice();
            //---------------------------------------------------------------------------------------------

            testc.LastPriceNotify += MesLastPrice;
            //---------------------------------------------------------------------------------------------
            testc.LowNotify += MesLow;
            testc.HighNotify += MesHigh;
            testc.LowNotify += MesLow;
            testc.HighNotify += MesHigh;
            testc.DiffNotify += MesDiff;

            testc.TimeNotify += MesTime;
            testc.FullTimeNotify += MesFullTime;

            testc.TotalNotify += MesTotal;

            testc.SMA1Notify += MesSMA1;
            testc.SMA2Notify += MesSMA2;
            testc.SMA3Notify += MesSMA3;

            testc.SMACheckNotify += MesSMACheck;

            testc.StartDemo();


            //testc.ConnectQuik();
            //testc.RunQuik();
            //testc.AsyncLastPrice();
        }
        public void Mes(string message)
        {
            Status = message;
        }
        public void MesLastPrice(string message)
        {
            LastPrice = message;
            

        }
        public void MesTotal(double message)
        {
            Total = message.ToString();
        }
        //-------------------------------------------------------------------------------------
        public void MesTime(string message)
        {
            Time = message;
        }
        public void MesFullTime(string message)
        {
            FullTime = message;
        }

        //------------------------------------------------------------------------------
        public void MesTiker(string message)
        {
            Tiker = message;
        }
        //------------------------------------------------------------------------------
        public void MesLastPrice2(string message)
        {
            LastPrice2 = message;
        }


        //------------------------------------------- indicator
        public void MesLow(double message)
        {
            Low = message.ToString();
        }
        public void MesHigh(double message)
        {
            High = message.ToString();
        }
        public void MesDiff(double message)
        {
            Diff = message.ToString();
        }
        //------------------------------------------- indicatorSMA
        public void MesSMA1(double mes)
        {
            SMA1 = mes.ToString();
        }
        public void MesSMA2(double message)
        {
            SMA2 = message.ToString();
        }
        public void MesSMA3(double message)
        {
            SMA3 = message.ToString();
        }
        public void MesSMACheck(string mes)
        {
            SMACheck = mes.ToString();
        }
    }
}
