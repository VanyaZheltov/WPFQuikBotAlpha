using QuikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFQuikBotTest.QuikSharpConnect.Data
{
   public class DataQuik
    {
        public delegate void ConnectHandler(string message);

        #region события 
        public event ConnectHandler ConnectNotify;
        public event ConnectHandler LastPriceNotify;
        #endregion

        public Tool tool;
        public Quik _quik;

        bool isServerConnected = false;
        string classCode = "";
        string clientCode;
        #region TimeOut
        private int _TimeOut = 1000;
        public int TimeOut { get => _TimeOut; set => _TimeOut = value; }
        #endregion  
        #region Sec Code
        private string secCode;
        public string SecCode { get => secCode; set => secCode = value; }

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
        #region LastPrice
        private string _LastPrice = "----";
        public string LastPrice
        {
            get => _LastPrice;

            set
            {
                _LastPrice = value;
                LastPriceNotify?.Invoke(value);
            }
        }
        #endregion

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
}
