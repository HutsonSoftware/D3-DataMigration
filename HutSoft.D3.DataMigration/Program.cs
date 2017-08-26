using System;
using System.Windows.Forms;

namespace HutSoft.D3.DataMigration
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Migration());
        }
    }
}
