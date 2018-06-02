namespace FillClient
{
    public class InitialExecution
    {
        static bool isInitialized;

        public static void Init()
        {
            if (isInitialized) { return; }

            new FillBootstrap();

            isInitialized = true;
        }
    }
}


