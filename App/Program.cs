using System;
using App.Windows;

namespace App
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Application is started");

            using var mainWindow = new MainWindow(500, 500);
    
            mainWindow.Run();

            Console.ReadKey();
        }
    }
}