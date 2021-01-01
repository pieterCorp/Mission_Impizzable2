using System;

namespace Mission_Impizzable2
{
    class Program
    {
        static void Main(string[] args)
        {
            KassaSysteem kassa = new KassaSysteem();

            bool appRunning = true;

            while (appRunning)
            {
                appRunning = kassa.KassaMenu();
            }
        }
    }
}
