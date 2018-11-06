using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    public static class StatManager
    {
        public static void IStat(bool _ready, bool _live, bool _fall)
        {
            Ready = _ready;
            Live = _live;
            Fall = _fall;
        }

        /// <summary>
        /// 준비 상태
        /// </summary>
        private static bool _ready;
        public static bool Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }

        /// <summary>
        /// 생존 상태
        /// </summary>
        private static bool _live;
        public static bool Live
        {
            get { return _live; }
            set { _live = value; }
        }

        /// <summary>
        /// 낙사 상태
        /// </summary>
        private static bool _fall;
        public static bool Fall
        {
            get { return _fall; }
            set { _fall = value; }
        }
    }
}
