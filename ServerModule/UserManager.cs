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

        private static float _score;
        public static float Score
        {
            get { return _score; }
            set { _score = value; }
        }
    }
}