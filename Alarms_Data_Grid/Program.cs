using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATAGRID_NATIVE_ATTEMPT_1
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form hostForm = new Form();
            hostForm.Text = "UserControl Test";
            hostForm.Size = new System.Drawing.Size(1400, 780);
            hostForm.FormBorderStyle = FormBorderStyle.FixedDialog;  

            Form1 userControl = new Form1();  
            userControl.Dock = DockStyle.Fill;

            hostForm.Controls.Add(userControl);

            Application.Run(hostForm);

        }
    }
}
