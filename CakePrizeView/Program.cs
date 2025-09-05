using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Services;
using CakePrizeView.Forms;
using CakePrizeView.Utils;

namespace CakePrizeView
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                
                // Test database connection before creating the form (optional)
                TestDatabaseConnection();
                
                // Create and run the login form
                Application.Run(new FrmLoginForm());
            }
            catch (Exception ex)
            {
                // Show detailed error information
                string errorMessage = $"Application startup error:\n\n" +
                                    $"Error Type: {ex.GetType().Name}\n" +
                                    $"Message: {ex.Message}\n\n" +
                                    $"Stack Trace:\n{ex.StackTrace}";
                
                MessageBox.Show(errorMessage, "Startup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Log to console if running from command line
                Console.WriteLine($"FATAL ERROR: {errorMessage}");
                try
                {
                    var logsService = new LogsService();
                    logsService.CreateLog(errorMessage, "Error", UserSession.GetCurrentUsername());
                }
                catch
                {
                    // If logging fails, just continue - we already showed the error to the user
                }
            }
        }
        
        /// <summary>
        /// Tests the database connection before starting the application
        /// </summary>
        private static void TestDatabaseConnection()
        {
            try
            {
                // Test the connection using centralized environment-aware connection management
                var connectionInfo = DatabaseConnectionManager.GetConnectionInfo();
                
                if (!connectionInfo.IsTestConnection)
                {
                    MessageBox.Show(
                        $"Database connection failed for environment '{connectionInfo.Environment}'.\n\n" +
                        $"Server: {connectionInfo.Server}\n" +
                        $"Database: {connectionInfo.Database}\n\n" +
                        "Please check:\n" +
                        "1. SQL Server is running\n" +
                        "2. Server is accessible\n" +
                        "3. Database exists\n" +
                        "4. Credentials are correct\n" +
                        "5. Environment configuration is correct\n\n" +
                        "The application will continue but database features may not work.",
                        "Database Connection Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Connection successful - no message needed
            }
            catch (Exception ex)
            {
                string errorMessage = $"Database connection test failed.\n\n" +
                                    $"Error Details: {ex.Message}\n\n" +
                                    "Please check your environment configuration and database setup.\n" +
                                    "The application will continue but database features may not work.";
                
                MessageBox.Show(errorMessage, "Database Connection Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}