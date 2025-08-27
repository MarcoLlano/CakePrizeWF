using CakePrizeView.Forms;
using System;
using System.Windows.Forms;

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
            }
        }
        
        /// <summary>
        /// Tests the database connection before starting the application
        /// </summary>
        private static void TestDatabaseConnection()
        {
            try
            {
                // First, try to get the connection string to see if configuration is loaded
                string connectionString = CakePrizeCore.libs.DBUtils.DBUtils.GetConnectionString();
                
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show(
                        "Database configuration not found. Please check:\n" +
                        "1. App.config file exists and contains 'CakePrize' connection string\n" +
                        "2. App.config is copied to the output directory\n" +
                        "3. Connection string format is correct\n\n" +
                        "The application will continue but database features may not work.",
                        "Configuration Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Test the connection
                using (var connection = CakePrizeCore.libs.DBUtils.DBUtils.CreateConnection())
                {
                    connection.Open();
                    connection.Close();
                }
                
                // Connection successful - no message needed
            }
            catch (Exception ex)
            {
                string errorMessage = $"Database connection failed. Please check:\n\n" +
                                    $"1. SQL Server is running\n" +
                                    $"2. Server 'MARCOLLANO' is accessible\n" +
                                    $"3. Database 'CakePrize' exists\n" +
                                    $"4. Credentials are correct\n\n" +
                                    $"Error Details: {ex.Message}\n\n" +
                                    $"The application will continue but database features may not work.";
                
                MessageBox.Show(errorMessage, "Database Connection Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}