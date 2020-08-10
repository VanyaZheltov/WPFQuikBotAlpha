using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using WPFQuikBotTest.QuikSharpConnect.Data;
using WPFQuikBotTest.QuikSharpConnect.Indicator;

namespace WPFQuikBotTest.QuikSharpConnect
{
    class Test
    {
        public delegate void ConnectHandler(string message);
        public delegate void IndicatoHandler(double message);
        public delegate void IndicatorHandler();
        #region события
        public event ConnectHandler TimeNotify;
        public event ConnectHandler FullTimeNotify;

        public event ConnectHandler ConnectNotify;
        public event ConnectHandler LastPriceNotify;

        public event IndicatoHandler TotalNotify;


        public event ConnectHandler SMACheckNotify;

        public event IndicatoHandler LowNotify;
        public event IndicatoHandler HighNotify;
        public event IndicatoHandler DiffNotify;

        public event IndicatorHandler SMANotify;

        public event IndicatoHandler SMA1Notify;
        public event IndicatoHandler SMA2Notify;
        public event IndicatoHandler SMA3Notify;
        #endregion
        bool isServerConnected = false;
        public string path;
        string classCode = "";
        string clientCode;
        #region вызов классов
        public Tool tool;
        public Quik _quik;
        public Portfolio _Portfolio;
        //----------------------------------------------------------------------------------------//---------------------------------//
        public HighLow trends;
        public SMA sma;
        #endregion
        public List<double> Candles = new List<double>();

        #region CandlesTime
        private int _CandlesTime = 60;
        public int CandlesTime { get => _CandlesTime; set => _CandlesTime = value; }
        #endregion
        #region TimeOut
        private int _TimeOut = 1000;
        public int TimeOut { get => _TimeOut; set => _TimeOut = value; }
        #endregion  
        #region ip
        private string _ConnectIp = "127.0.0.1";
        public string ConnectIp { get => _ConnectIp; set => _ConnectIp = value; }
        #endregion
        #region Status
        private string _Status = "----";
        public string ConnectStatus
        {
            get => _Status;

            set
            {
                _Status = value;
                ConnectNotify?.Invoke(value);
                
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
        #region Low High
        #region Low
        private double _Low;
        public double Low
        {
            get => _Low;
            set
            {
                _Low = value;

                LowNotify?.Invoke(value);
            }
        }
        #endregion
        #region High
        private double _High;
        public double High
        {
            get => _High;
            set
            {
                _High = value;
                HighNotify?.Invoke(value);
            }
        }
        #endregion
        #region HDiff
        private double _Diff;
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
        #region последняя цена sma l/h 
        private string _LastPrice = "----";
        public string LastPrice
        {
            get => _LastPrice;
            set
            {
                
                _LastPrice = value;
                var valuetw = double.Parse(value, CultureInfo.InvariantCulture);
                double total_p = valuetw;


                double x = total_p;


                Candles.Add(total_p);

                if (Candles.Count == CandlesTime)
                {
                    double psma = Candles.Sum() / Candles.Count();
                    //------------------------------------------------------------------------
                    CandlesClose = Candles.Min();
                    CandlesMax = Candles.Max();
                    CandlesOpen = Candles[0];
                    CandlesClose = Candles.Last();
                    //----------------------------------------
                    sma.Price = psma;
                    SMA1 = sma.SmaTotal_1;
                    SMA2 = sma.SmaTotal_2;
                    SMA3 = sma.SmaTotal_3;
                    //--------------------------------------------------------------
                    
                    //---------------------------------------------------------------------------
                    SMACheck();
                    SMANotify?.Invoke();
                    Candles.Clear();

                }
                trends.Trand =  total_p;//
                Diff = trends.Trand;
                Low = trends.LowPrice;
                High = trends.HighPrice;

                LastPriceNotify?.Invoke(value);
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------------//
        #region Sec Code
        private string secCode;
        public string SecCode { get => secCode; set => secCode = value; }

        #endregion
        //-------------------------------------------------------------------------------------------------------------------//
        #region Tiker
        private string _Tiker;
        public string  Tiker { get => _Tiker; set => _Tiker = value; }

        #endregion
        #region time tiker
        private string _LastPriceTime;
        public string LastPriceTime { get => _LastPriceTime; set => _LastPriceTime = value; }


        List<string> CandlesTiks = new List<string>();

        private string _Time;
        public string TimeT
        {
            get => _Time.ToString();
            set
            {
                //int i = 1;
                //CandlesTiks.Add()
                int time = Convert.ToInt32(value.Substring(value.Length - 2));//последняя сикунда 
                if(time == 0)
                {
                    _Time = Convert.ToString(time);
                }
                if (Convert.ToInt32(_Time + 1 ) == time)// 
                {
                    LastPrice = LastPriceTime;
                    TimeNotify?.Invoke(time.ToString());
                }

                FullTimeNotify?.Invoke(value);
                _Time = time.ToString();
                

            }
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------------//
        #region




        #endregion
        //-------------------------------------------------------------------------------------------------------------------//
        #region проверка sma на пересечение 
        public void SMACheck()
            {
            var sma_x = SMA2 - SMA3;

            if (sma_x > 0.1 & sma_x < 0.2)      //расхождение вверх
            {
                SMACheckNotify?.Invoke("UP");
                
                if (!_Portfolio.StatusPosition)
                {
                    //открыть buy заменить на  true
                    _Portfolio.StatusPosition = true;
                    _Portfolio.StatusTransaction = "buy";
                    _Portfolio.PriceStart = _Portfolio.PriceStart = double.Parse(LastPriceTime, CultureInfo.InvariantCulture);

                }
                else
                {
                    if (_Portfolio.StatusTransaction == "buy")
                    {
                        // позицыя открыта buy
                    }
                    if (_Portfolio.StatusTransaction == "sell")
                    {
                        //закрыть buy  
                        _Portfolio.StatusTransaction = "buy";
                        _Portfolio.PriceStop = double.Parse(LastPriceTime, CultureInfo.InvariantCulture);
                        _Portfolio.TotalTransaction();
                        _Portfolio.PriceStart = double.Parse(LastPriceTime, CultureInfo.InvariantCulture);

                    }
                }
                 trends.HighPrice= Low;
                 trends.LowPrice = High;

            }
            if (sma_x < -0.1 & sma_x > -0.2)    //расхождение вниз
            {
                SMACheckNotify?.Invoke("Down");

                if (!_Portfolio.StatusPosition)
                {
                    _Portfolio.StatusPosition = true;
                    _Portfolio.StatusTransaction = "sell";
                    _Portfolio.PriceStart = _Portfolio.PriceStart = double.Parse(LastPriceTime, CultureInfo.InvariantCulture);
                }
                else
                {
                    if (_Portfolio.StatusTransaction == "sell")
                    {
                        // позицыя открыта sell
                    }
                    if (_Portfolio.StatusTransaction == "buy")
                    {
                        _Portfolio.StatusTransaction = "sell";
                        _Portfolio.PriceStop = double.Parse(LastPriceTime, CultureInfo.InvariantCulture);
                        _Portfolio.TotalTransaction();
                        _Portfolio.PriceStart = double.Parse(LastPriceTime, CultureInfo.InvariantCulture);
                    }
                }
                trends.HighPrice = Low;
                trends.LowPrice = High;
                TotalNotify?.Invoke(_Portfolio.TotalS());
            }
        }
        #endregion
        //-------------------------------
        public int XY { get; set; }
        //----------------------------------------------------------------------------------------------
        public void OpenFile()
        {
            trends = new HighLow();
            sma = new SMA();
            _Portfolio = new Portfolio();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.ShowDialog();   
                path = ofd.FileName;
                ConnectStatus = path;
            
           
        }
        public void Datat()
        {

            char x = ';';
            
            var data = ReadFile(x, path);
            DataFor(data);
        }
        public List<DataList> ReadFile(char separator, string pathx)
        {
            List<string> ClosesData = new List<string>();
            List<DataList> Data = new List<DataList>();
            using (StreamReader sr = new StreamReader(pathx, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(new char[] { separator });

                    //foreach (string[] s in words)
                    //{
                    //    Data.Add(new DataList() { Tiker = s[0]});
                    //}


                    string str = words[4].Replace(".", ",");
                    Data.Add(new DataList() { Tiker = words[0], Time = words[3], Close = words[7], Vol = words[8] }) ;


                }

            }
            
            return Data;
        }
        public void DataFor(List<DataList> x)
        {



            
            foreach (DataList s in x)
            {

                LastPriceTime = s.Close;
                TimeT = s.Time;
                Thread.Sleep(30);
               
            }



        }
        public async void StartDemo()
        {           
            await Task.Run(() => Datat());
        }
        //-------------------------------------------------------------------------------------------------------------------//
        public async void AsyncLastPrice()
        {
            await Task.Run(() => Start_Tick()); // вызов асинхронной LastPrice
        }
        public void Start_Tick()
        {
            while (true)
            {
                LastPrice = Convert.ToString(tool.LastPrice);
                
                System.Threading.Thread.Sleep(TimeOut);
            }

        }
        //-------------------------------------------------------------------------------------------------------------------//  
        public void RunQuik()
        {
            try
            {

                try
                {
                    classCode = _quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB", SecCode).Result;
                }
                catch
                {
                    //textBoxLogsWindow.AppendText("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно" + Environment.NewLine);
                }
                if (classCode != null && classCode != "")
                {
                    // textBoxClassCode.Text = classCode;

                    //textBoxLogsWindow.AppendText("Определяем код клиента..." + Environment.NewLine);
                    clientCode = _quik.Class.GetClientCode().Result;
                    //textBoxClientCode.Text = clientCode;
                    //textBoxLogsWindow.AppendText("Создаем экземпляр инструмента " + secCode + "|" + classCode + "..." + Environment.NewLine);
                    tool = new Tool(_quik, SecCode, classCode);
                    trends = new HighLow(); //запрос max min
                    sma = new SMA();
                    sma.SMAValues1 = 2;
                    sma.SMAValues2 = 9;
                    sma.SMAValues3 = 18;


                    if (tool != null && tool.Name != null && tool.Name != "")
                    {
                        LastPrice = Convert.ToString(tool.LastPrice);
                    }

                }
            }
            catch
            {
                //textBoxLogsWindow.AppendText("Ошибка получения данных по инструменту." + Environment.NewLine);
            }
            // AsyncLastPrice();
        }
        //---------------------------------------------------------------------------------------------------------------------------------//
        public void ConnectQuik()
        {
            try
            {
                _quik = new Quik(Quik.DefaultPort, new InMemoryStorage(), ConnectIp);    // инициализируем объект Quik с использованием удаленного IP-адреса терминала
                ConnectStatus = "Подключаемся к терминалу Quik...";
            }
            catch
            {
                ConnectStatus = "Подключаемся к терминалу Quik...";
            }

            if (_quik != null)
            {
                ConnectStatus = "Экземпляр Quik создан.";
                try
                {
                    ConnectStatus = "Получаем статус соединения с сервером....";
                    isServerConnected = _quik.Service.IsConnected().Result;
                    if (isServerConnected)
                    {
                        ConnectStatus = "Соединение с сервером установлено.";
                        //установить флаг готов к рвботе
                    }
                    else
                    {
                        ConnectStatus = "Соединение с сервером НЕ установлено.";
                    }
                }
                catch
                {
                    ConnectStatus = "Неудачная попытка получить статус соединения с сервером.";
                }
            }
        }
    }
    public class DataList
    {
        public object Tiker { get; set; }
        public string Time { get; set; }
        public string Data { get; set; }
        public string Low { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Close { get; set; }
        public string Vol { get; set; }


    }
}
