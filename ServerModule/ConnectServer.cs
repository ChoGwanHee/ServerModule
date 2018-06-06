﻿using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;

namespace ServerModule
{
    public class ConnectServer : ServerManager
    {
        public string _sequance;

        public override void SetConnect()
        {
            base.SetConnect();
            try
            {
                Connect("127.0.0.1", 2020);
                //multicastServer(UDPaddress, Port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Console.WriteLine("서버 접속 시도");
        }

        public override void Connect(string _address, int _port)
        {
            base.Connect(_address, _port);
            IPAddress serverIP = IPAddress.Parse(_address);
            int serverPort = Convert.ToInt32(_port);
            Instance.TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Instance.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 3000);
            Instance.TCP.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            Instance.TCP.Connect(new IPEndPoint(serverIP, serverPort));
            Console.WriteLine("Server Connect To Client (" + serverIP + ":" + serverPort + ")");
        }

        // UDP 클래스의 서버 주소 등록 함수
        private void multicastServer(string addr, int port)
        {
            // 상대편 좌표를 얻기 위한 멀티캐스트 접속을 위한 초기화
            // 블로킹모드로 상대방의 좌표값을 최신화시킨다.
            Instance.UDP = new UdpClient();
            int serverPort = Convert.ToInt32(port);
            IPAddress multicastIP = IPAddress.Parse(addr);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, serverPort);
            Instance.UDP.Client.Bind(localEP);
            Instance.UDP.JoinMulticastGroup(multicastIP);
            IPEndPoint _remote = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Multicast Initialize Setting ( " + addr + ":" + port + ":" + _remote + ")");
        }


        public override void StringSplitsWordsDelimiter(string _text)
        {
            // UPDT_DATE : 2018-05-29
            // UPDT_NAME : 조관희
            // REMK_TEXT : 정규표현식 Split 패킷 분할 작업
            base.StringSplitsWordsDelimiter(_text);
            char[] delimiterChars = { ':' };
            string text = _text;
            string[] words = text.Split(delimiterChars);
            Console.WriteLine(string.Format("{0} words in text", words.Length));
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
        }

        public override void ParsePacket(int _length)
        {
            base.ParsePacket(_length);
            string message = Encoding.UTF8.GetString(Instance.BufferSize, 2, _length - 2);
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

        public override void Send(string _text)
        {
            base.Send(_text);
            try
            {
                if (Instance.TCP != null && Instance.TCP.Connected)
                {
                    byte[] _buffer = new byte[4096];
                    Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(_text).Length + 2), 0, _buffer, 0, 2);
                    Buffer.BlockCopy(Encoding.UTF8.GetBytes(_text), 0, _buffer, 2, Encoding.UTF8.GetBytes(_text).Length);
                    Instance.TCP.Send(_buffer, Encoding.UTF8.GetBytes(_text).Length + 2, 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public override void Disconnect()
        {
            base.Disconnect();
        }

        void OnDestroy()
        {
            if (Instance.TCP != null && Instance.TCP.Connected)
            {
                Send(string.Format("DISCONNECT"));
                Thread.Sleep(500);
                Instance.TCP.Close();
            }
        }
    }
}
