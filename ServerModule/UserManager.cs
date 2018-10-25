using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    public static class UserManager
    {
        private static string _nickname;
        public static string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        private static int _id;
        public static int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private static bool _ready;
        public static bool Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }
    }
}