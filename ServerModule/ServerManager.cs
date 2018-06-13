using System.Collections;
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
        public static string _sequance;
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
            InstanceValue.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 3000);
            InstanceValue.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
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

        public static void ParsePacket(int _length)
        {
            string message = Encoding.UTF8.GetString(InstanceValue.BufferSize, 2, _length - 2);
            string[] text = message.Split(':');

            switch (text[0])
            {
                case "CONNECT":
                    Send(string.Format("INITIALIZE"));
                    Console.WriteLine("Client Server Connected : " + text[0]);
                    break;
                case "INITIALIZE":
                    Console.WriteLine("Play to game set data : " + text[0]);
                    _sequance = text[1];
                    Send(string.Format("GAMESTART"));
                    break;
                case "DISCONNECT":
                    Console.WriteLine("Client Server Disconnected : " + text[0]);
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
