using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    public enum GameState
    {
        Wait,
        Playing,
        Result
    }

    public static class StateManager
    {
        public static int ChangeState(int state)
        {
            switch(state)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                default:
                    return -1;
            }
        }
    }
}
