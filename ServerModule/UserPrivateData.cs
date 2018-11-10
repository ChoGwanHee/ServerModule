using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    public class UserPrivateData
    {
        public int id;
        public string nickname;
        public int score;
        public string room;
        public float x;
        public float z;
        public float rotate;
        public bool ready;
        public int sequence;

        public UserPrivateData()
        {
            id = 0;
            nickname = null;
            score = 0;
            room = null;
            x = 0;
            z = 0;
            rotate = 0;
            ready = false;
            sequence = 0;
        }
    }
}
