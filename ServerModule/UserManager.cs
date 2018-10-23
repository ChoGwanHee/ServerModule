using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    class UserManager
    {
        private static int _nickname;
        public static int Nickname
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

        private static string _weapon;
        public static string Weapon
        {
            get { return _weapon; }
            set { _weapon = value; }
        }
    }
}