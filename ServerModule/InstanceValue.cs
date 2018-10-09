using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerModule
{
    public enum PeerValue
    {
        Disconnected,
        Connected,
        Disconnecting,
        Connecting,
        RoomIn,
        RoomOut,
        Ready,
        Win,
        Lose
    }

    public static class InstanceValue
    {
        /// <summary>
        /// server argument
        /// </summary>
        private static Socket _tcp;
        public static Socket TCP
        {
            get { return _tcp; }
            set { _tcp = value; }
        }

        private static UdpClient _udp;
        public static UdpClient UDP
        {
            get { return _udp; }
            set { _udp = value; }
        }

        private static string _address;
        public static string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private static int _port;
        public static int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private static byte[] _buffersize;
        public static byte[] BufferSize
        {
            get { return _buffersize; }
            set { _buffersize = value; }
        }

        private static int _receive;
        public static int Receive
        {
            get { return _receive; }
            set { _receive = value; }
        }

        /// <summary>
        /// lobby argument
        /// </summary>
        private static bool _connected;
        public static bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        }

        private static int _id;
        public static int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private static int _state;
        public static int State
        {
            get { return _state; }
            set { _state = value; }
        }

        private static string _version;
        public static string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private static string _nickname;
        public static string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        private static int _min;
        public static int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        private static int _max;
        public static int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        readonly static int RoomMaxCount = 4;

        private static int _roomid;
        public static int Roomid
        {
            get { return _roomid; }
            set { _roomid = value; }
        }

        private static bool _ready;
        public static bool Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }
    }
}
