using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsmComm.GsmCommunication;
using System.IO.Ports;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;

namespace EneaWPF
{
    class HandlePhone
    {
        private static string _COMPORT;
        public static string comPort
        {
            get { return _COMPORT; }
            set { if (value == null) throw new ArgumentNullException(); _COMPORT = value; }
        }

        protected static GsmCommMain _MYPHONE;
        public static GsmCommMain myPhone
        {
            get { return _MYPHONE; }
            set { if (value == null) throw new ArgumentNullException(); _MYPHONE = value; }
        }

        public HandlePhone() { }
        public HandlePhone(string COMPort, GsmCommMain MyPhone)
        {
            comPort = COMPort;
            myPhone = MyPhone;
        }


        public bool COMCheck()
        {
            bool DeviceFound = false;
            string[] AvailableCOMs = SerialPort.GetPortNames();


            foreach (string Port in AvailableCOMs)
            {
                GsmCommMain TestPort = new GsmCommMain(Port, 9600);
                TestPort.Open();

                if (TestPort.IsConnected())
                {
                    TestPort.Close();
                    _COMPORT = TestPort.PortName;
                    _MYPHONE = new GsmCommMain(_COMPORT, 9600);
                    _MYPHONE.Open();
                    DeviceFound = true;
                    break;
                }
                TestPort.Close();
            }


            return DeviceFound;
        }


        public string ReturnComPortName()
        { return _COMPORT; }

        public int ReturnDeviceBaudRate()
        { return _MYPHONE.BaudRate; }


        private string _TEXTMESSAGE;
        public string textMesssage
        {
            get { return _TEXTMESSAGE; }
            set { if (value == null) throw new ArgumentNullException(); _TEXTMESSAGE = value; }
        }

        private List<string> _PHONENUMBERS;
        public List<string> phoneNumbers
        {
            get { return _PHONENUMBERS; }
            set { if (value == null) throw new ArgumentNullException(); _PHONENUMBERS = value; }
        }

        public static IEnumerable<string> SplitByLength(string s, int length)
        {
            for (int i = 0; i < s.Length; i += length)
            {
                if (i + length <= s.Length)
                {
                    yield return s.Substring(i, length);
                }
                else
                {
                    yield return s.Substring(i);
                }
            }
        }

        public void SendSMS(string textMessage, string number)
        {
            IEnumerable<string> NewDividedMessage = SplitByLength(textMessage, 70);

            foreach (string VARIABLE in NewDividedMessage)
            {
                byte DCS = (byte)DataCodingScheme.GeneralCoding.Alpha16Bit;
                SmsSubmitPdu SMS = new SmsSubmitPdu(VARIABLE, number, DCS);

                if (!_MYPHONE.IsOpen()) _MYPHONE.Open();

                _MYPHONE.SendMessage(SMS);
                _MYPHONE.Close();
            }
        }
    }
}

