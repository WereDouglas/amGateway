using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace amLibrary.Helpers
{
    public class Messenger
    {
        private static GsmCommMain comm;
        private static GsmCommMain comm2;
        private delegate void SetTextCallback(string text);
        private static string cmbCOM;
        private static Message _message;
        private static CommNetwork _cm;
        private static Network _network;
        private static string state;
        public static string portModem;
        public static string modemDetails;

        public static void Send(DBObject parent, string message, string number)
        {
            try
            {

                SmsSubmitPdu pdu;
                byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha7BitDefault;
                pdu = new SmsSubmitPdu(message, Convert.ToString(number), dcs);
                int times = 1;
                for (int i = 0; i < times; i++)
                {
                    comm.SendMessage(pdu);
                }


            }
            catch
            {


            }
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
        static string neter;
        public static bool Sender(DBObject parent, string id, string message, string number, string network)
        {


            foreach (CommNetwork coms in primes)
            {
                if (network == coms.Names)
                {
                    try
                    {

                        SmsSubmitPdu pdu;
                        byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha7BitDefault;
                        pdu = new SmsSubmitPdu(message, Convert.ToString(number), dcs);
                        int times = 1;
                        for (int i = 0; i < times; i++)
                        {
                            coms.Comms.SendMessage(pdu);
                            _message = new Message(parent);
                            _message.Update(id, "T");

                            sent = true;
                        }
                        sent = true;

                    }
                    catch (Exception r)
                    {
                        sent = false;
                        _network.UpdateStatus(network, "disconnected");
                        Console.WriteLine("comm is already open");
                    }


                }

            }

            _network = new Network(null);


            return sent;
        }



        static bool sent;
        public static bool SendUpdate(DBObject parent, string id, string message, string number, string network, string cb)
        {
            _network = new Network(null);
            comm = new GsmCommMain(cb, 9600, 150);
            try
            {

                SmsSubmitPdu pdu;
                byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha7BitDefault;
                pdu = new SmsSubmitPdu(message, Convert.ToString(number), dcs);
                int times = 1;
                for (int i = 0; i < times; i++)
                {

                    if (comm.IsConnected())
                    {
                        Console.WriteLine("comm is already open");
                        Console.WriteLine("modem  connected:" + network + "to:" + comm);
                        _network.UpdateStatus(network, "connected");
                    }
                    else
                    {
                        comm.Open();
                        _network.UpdateStatus(network, "connected");
                    }

                    comm.SendMessage(pdu);
                    _message = new Message(parent);
                    _message.Update(id, "T");

                    sent = true;
                }
                sent = true;

            }
            catch (Exception r)
            {
                sent = false;
                _network.UpdateStatus(network, "disconnected");
                Console.WriteLine("comm is already open");
            }
            return sent;
        }
        static bool ports = false;


        public static bool okay = false;
        public static string cmbCOMS;
        private static string netName;

        static List<CommNetwork> primes = new List<CommNetwork>();
        public static bool connected(string network, string comms)
        {
            netName = network;

            comm = new GsmCommMain(comms, 9600, 150);

            try
            {
                if (comm.IsConnected())
                {

                    Console.WriteLine(netName + "comm is already open");
                    okay = true;
                    if (!comm2.IsConnected())
                    {
                        comm2 = new GsmCommMain(comms, 9600, 150);

                        comm2.Open();
                        CommNetwork _cm = new CommNetwork() { Names = netName, Comms = comm2 };
                        primes.Add(_cm);
                    }
                    else
                    {

                        Console.WriteLine(netName + "all comms are open comm is already open");
                    }

                }
                else
                {
                    comm.Open();
                    CommNetwork _cm = new CommNetwork() { Names = netName, Comms = comm };
                    primes.Add(_cm);
                    Console.WriteLine(netName + "comm has connected");
                    okay = true;


                }

            }
            catch (Exception r)
            {
                Console.WriteLine("comm has failed");
                okay = false;

            }

            return okay;

        }


        public static bool disconnected()
        {
            _network = new Network(null);
            foreach (CommNetwork coms in primes)
            {
                try
                {
                    coms.Comms.Close();
                    Console.WriteLine("disconnected");
                    _network.UpdateStatus(coms.Names, "disconnected");
                    connect = true;
                }
                catch (Exception r)
                {
                    Console.WriteLine("comm has failed to disconnect");
                    connect = false;
                }
            }
            return connect;
        }
        static bool connect;
        public static bool connecter()
        {
          
            _network = new Network(null);

            foreach (CommNetwork coms in primes)
            {
                try
                {
                    coms.Comms.Open();
                    _network.UpdateStatus(coms.Names, "connected");
                    connect=true;
                }
                catch (Exception r)
                {
                    Console.WriteLine("comm has failed to disconnect");
                    connect = false;

                }
            }
            return connect;
        }

    }
    public class CommNetwork
    {
        public string Names { get; set; }
        public GsmCommMain Comms { get; set; }



    }
}
/***
 * 
 * 
 * 
 *  public static string connect()
        {
            state = "false";
            int d = 0;
            do
            {
                d++;
                cmbCOM = "COM" + d.ToString();
                comm = new GsmCommMain(cmbCOM, 9600, 150);
                Console.WriteLine(cmbCOM);
                try
                {
                    if (comm.IsConnected())
                    {
                        Console.WriteLine("comm is already open");
                        ports = true;
                        state = "true";
                        portModem = cmbCOM;
                        modemDetails = comm.GetCurrentOperator().ToString();
                        break;

                    }
                    else
                    {
                        Console.WriteLine("comm is not open");
                        comm.Open();
                        ports = true;
                        state = "true";
                        portModem = cmbCOM;
                        modemDetails = comm.GetCurrentOperator().ToString();

                    }

                }
                catch (Exception)
                {
                    ports = false;
                    state = "true";

                }

            }
            while (!comm.IsConnected() && d < 30);


            Console.WriteLine(cmbCOM);
            return state;

        }
***/