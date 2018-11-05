using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;

// http://treeofimaginary.tistory.com/96 .NET exception 버그 참고
// http://littles.egloos.com/tag/mono/page/1 .NET 동적 로드 기법
// Unity\Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win32_development_mono\Data\Managed
namespace ServerModule
{
    public static class ServerManager
    {
        private static int _people;
        public static int People
        {
            get { return _people; }
            set { _people = value; }
        }
        public static string _sequance;

        private static string[] _text;
        public static string[] text
        {
            get { return _text; }
            set { _text = value; }
        }

        private static string _log;
        public static string Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public static void Initialized()
        {
            // server argument
            InstanceValue.TCP = null;
            InstanceValue.UDP = null;
            InstanceValue.Address = null;
            InstanceValue.Port = 0;
            InstanceValue.BufferSize = new byte[4096];
            InstanceValue.Receive = 0;
            // lobby argument
            InstanceValue.Connected = false;
            InstanceValue.State = (int)PeerValue.Disconnected;
            InstanceValue.ID = 0;
            InstanceValue.Version = null;
            InstanceValue.Nickname = null;
        }

        public static void TCPConnect(string _address, int _port)
        {
            IPAddress serverIP = IPAddress.Parse(_address);
            int serverPort = Convert.ToInt32(_port);
            InstanceValue.TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            InstanceValue.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 500);
            InstanceValue.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 500);
            InstanceValue.TCP.Connect(new IPEndPoint(serverIP, serverPort));
            Console.WriteLine("Server Connect To Client (" + serverIP + ":" + serverPort + ")");
        }

        public static void UDPConnect(string _address, int _port)
        {
            InstanceValue.UDP = new UdpClient();
            int serverPort = Convert.ToInt32(_port);
            IPAddress multicastIP = IPAddress.Parse(_address);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, serverPort);
            InstanceValue.UDP.Client.Bind(localEP);
            InstanceValue.UDP.JoinMulticastGroup(multicastIP);
            IPEndPoint _remote = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Multicast Initialize Setting ( " + _address + ":" + _port + ":" + _remote + ")");
        }

        public static void Disconnect()
        {
            if (InstanceValue.TCP != null && InstanceValue.TCP.Connected)
            {
                Send(string.Format("DISCONNECT"));
                Thread.Sleep(500);
                InstanceValue.TCP.Close();
            }
        }

        public static void StringSplitsWordsDelimiter(string _text)
        {
            char[] delimiterChars = { ':' };
            string text = _text;
            string[] words = text.Split(delimiterChars);
            Console.WriteLine(string.Format("{0} words in text", words.Length));
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
        }

        public static string ParsePacketLogData()
        {
            return Log;
        }

        public static void ParsePacket(int _length)
        {
            string message = Encoding.UTF8.GetString(InstanceValue.BufferSize, 2, _length - 2);
            //string[] text = message.Split(':');
            text = message.Split(':');
            Log = message;
            /*
             * FALL
             * RESPAWN
             * CREATEROOM
             * RECOVERY
             * ITEMBOX
             * READY
             * START
             * MOVE
             * ROTATE
             * BTNSTART
             * DISCONNECT
             * WEAPONCHANGE
             * JOINGAME
             * HIT
             * WIN
             * LOSE
             * SCORE
             * PORTAL
             * HOST
             * GUEST
            */
            switch (text[0])
            {
                case "CONNECT": // 접속 성공할 경우, 서버에서 패킷을 보내온다.
                    Send(string.Format("CONNECT:{0}:{1}", InstanceValue.Nickname, InstanceValue.ID));
                    Console.WriteLine("Client Server Connected : " + text[0]);
                    break;
                case "INITIALIZE":
                    Console.WriteLine("Play to game set data : " + text[0]);
                    _sequance = text[1];
                    People++;
                    Send(string.Format("GAMESTART"));
                    break;
                case "MOVE":
                    InstanceValue.X = int.Parse(text[3]);
                    InstanceValue.Z = int.Parse(text[4]);
                    break;
                case "ROTATE":
                    InstanceValue.Rotate = float.Parse(text[1]);
                    break;
                case "BTNSTART":
                    break;
                case "JOINGAME":
                    break;
                case "CREATEROOM":
                    InstanceValue.Room = text[3];
                    break;
                case "DISCONNECT":
                    Console.WriteLine("Client Server Disconnected : " + text[0]);
                    break;
                case "NICKNAME":
                    Console.WriteLine("Client NickName : " + message);
                    break;
                default:
                    Console.WriteLine("Data does not exist");
                    break;
            }
        }

        public static void Send(string _text)
        {
            try
            {
                if (InstanceValue.TCP != null && InstanceValue.TCP.Connected)
                {
                    byte[] _buffer = new byte[4096];
                    Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(_text).Length + 2), 0, _buffer, 0, 2);
                    Buffer.BlockCopy(Encoding.UTF8.GetBytes(_text), 0, _buffer, 2, Encoding.UTF8.GetBytes(_text).Length);
                    InstanceValue.TCP.Send(_buffer, Encoding.UTF8.GetBytes(_text).Length + 2, 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public static void WriteLine(string _text)
        {
            Console.WriteLine(_text);
        }

        public static byte[] ShortToByte(int _value)
        {
            byte[] temp = new byte[2];
            temp[1] = (byte)((_value & 0x0000ff00) >> 8);
            temp[0] = (byte)((_value & 0x000000ff));
            return temp;
        }
    }
}
