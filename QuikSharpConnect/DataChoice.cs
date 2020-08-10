using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFQuikBotTest.QuikSharpConnect.Data;

namespace WPFQuikBotTest.QuikSharpConnect
{
    public class DataChoice
    {
        public delegate void HandlerData(string message);
        
        public event HandlerData LastPriceNotifyData;
        public event HandlerData StatusNotifyData;


        DataDemoQuik dataDemoQuik;
        public DataQuik DataQuik;
        QuikSharpStart QuikSharp;



        #region Ip Quik
        private string _Ip_Quik;

        public string Ip
        {
            get => _Ip_Quik;
            set => _Ip_Quik = value;
        }
        #endregion
        #region Ip Quik Demo
        private string _Ip_Quik_Demo;

        public string Ip_Quik_Demo
        {
            get => _Ip_Quik_Demo;
            set => _Ip_Quik_Demo = value;
        }
        #endregion
        #region File data
        private string _FileName;

        public string FileName
        {
            get => _FileName;
            set => _FileName = value;
        }
        #endregion

        #region SecCode
        private string _SecCode;

        public string SecCode
        {
            get => _SecCode;
            set => _SecCode = value;
        }
        #endregion
        #region TimeOut
        private int _TimeOut;

        public int TimeOut
        {
            get => _TimeOut;
            set => _TimeOut = value;
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
                StatusNotifyData?.Invoke(value);
            }
        }
        #endregion

        #region Type Quik 
        private string _Type_Quik;

        public string Type_Quik
        {
            get => _Type_Quik;
            set => _Ip_Quik = value;
        }
        #endregion
        #region Last Price
        private string _LastPrice;

        public string LastPrice
        {
            get => _LastPrice;
            set
            {
                _LastPrice = value;
                LastPriceNotifyData?.Invoke(value);
            }

        }
        #endregion


        public DataChoice()
        {

        }
        public void Start()
        {
            if (Type_Quik == "File")
            {

            }
            if (Type_Quik == "Quik")
            {
                DataQuik = new DataQuik();
                DataQuik.TimeOut = TimeOut;
                DataQuik.SecCode = SecCode;
                DataQuik.ConnectNotify += ConnectQuikNotify;
                DataQuik.LastPriceNotify += LastPriceNotify;

                DataQuik.ConnectQuik();
                DataQuik.RunQuik();

            }
            if (Type_Quik == "QuikDemo")
            {

            }
        }
        public void LastPriceNotify(string message)
        {
            LastPrice = message;

        }
        public void ConnectQuikNotify(string message)
        {
            LastPrice = message;

        }
    }
}
