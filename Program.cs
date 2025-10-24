namespace billing_system
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <remarks>
    /// This class contains the <see cref="Main"/> method, which starts the application.
    /// </remarks>
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// <remarks>
        /// This method initializes the application's configuration and starts the Windows Forms message loop.
        /// It begins by displaying the <see cref="Login"/> form, which serves as the initial user interface.
        /// The application will exit when the main form is closed.
        /// </remarks>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }
    }
}
