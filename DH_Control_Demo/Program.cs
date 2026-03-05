namespace DH_Control_Demo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmLoadCard());
            GVL.AxisActTask = Task.Factory.StartNew(() =>
            {
                if (GVL.AxisMontions != null)
                {
                    do
                    {
                        for (int i = 0; i < GVL.AxisMontions.Length; i++)
                        {
                            GVL.AxisMontions[i].AxisActTaskMethod();
                        }
                        Thread.Sleep(2);
                    } while (true);
                }
            }, TaskCreationOptions.LongRunning);
            Application.Run(new FrmMain());

        }
    }
}