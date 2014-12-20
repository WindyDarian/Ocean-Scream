using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTCOS.MainGame
{
    public static class GameState
    {
        public enum State { Menu, AIVsAI, PlayerVsAI, Video ,Exit};
        public static State currentGameState;
    }
}
