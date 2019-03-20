using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            const int stackSize = 1024 * 1024 * 64;
             //thread untuk menjalankan fungsi NewMain dengan stackSize sebagai ukuran stack maksimum
            Thread t = new Thread(NewMain, stackSize);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }


        // fungsi yang akan dijalankan thread t di main 
        static void NewMain()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
