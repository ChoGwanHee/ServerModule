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

        public static UserPrivateData hostUser = new UserPrivateData();
        public static UserPrivateData firstUser = new UserPrivateData();
        public static UserPrivateData secondUser = new UserPrivateData();
        public static UserPrivateData thirdUser = new UserPrivateData();

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
            InstanceValue.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
            InstanceValue.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
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

        public static void ClassifyUserList(string _nickname, int _id)
        {
        }

        public static string ParsePacketLogData()
        {
            return InstanceValue.Log;
        }

        public static void ParsePacket(int _length)
        {
            string message = Encoding.UTF8.GetString(InstanceValue.BufferSize, 2, _length - 2);
            string[] text = message.Split(':');
            InstanceValue.Log = message;
            /*
             * FALL
             * RESPAWN
             * CREATEROOM
             * JOINROOM
             * RECOVERY
             * ITEMBOX
             * READY
             * START
             * MOVE
             * ROTATE
             * BTNSTART
             * DISCONNECT
             * WEAPONCHANGE
             * HIT
             * WIN
             * LOSE
             * SCORE
             * PORTAL
             * HOST
             * GUEST
             * SCORE
             * ITEMBOX
             * ITEMBOXCREATE
             * ITEMBOXDELETE
            */
            switch (text[0])
            {
                case "CONNECT": // 접속 성공할 경우, 서버에서 패킷을 보내온다.
                    Send(string.Format("CONNECT:{0}:{1}", InstanceValue.Nickname, InstanceValue.ID));
                    break;
                case "INITIALIZE":
                    InstanceValue.Sequence = int.Parse(text[1]);
                    InstanceValue.Nickname = text[2];
                    InstanceValue.ID = int.Parse(text[3]);
                    InstanceValue.Room = text[4];
                    break;
                case "FULL":
                    Send(string.Format("FULL:{0}:{1}:{2}", InstanceValue.Nickname, InstanceValue.ID, InstanceValue.Room));
                    break;
                case "MOVE":
                    InstanceValue.X = int.Parse(text[3]);
                    InstanceValue.Z = int.Parse(text[4]);
                    break;
                case "SCORE":
                    ClassifyUserList(text[1], int.Parse(text[2]));
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
                case "JOINROOM":
                    break;
                case "DISCONNECT":
                    break;
                case "NICKNAME":
                    break;
                default:
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
