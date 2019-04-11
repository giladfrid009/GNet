using System;

namespace PNet.GlobalVars
{
    public static class Globals
    {
        public static Random Rnd { get; private set; }

        static Globals()
        {
            Rnd = new Random();
        }
    }
}
