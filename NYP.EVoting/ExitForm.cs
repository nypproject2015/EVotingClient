using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace NYP.EVoting
{
    public partial class ExitForm : Form
    {
        public ExitForm()
        {
            InitializeComponent();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {

            if (txtPassword.Text == ConfigurationManager.AppSettings["StopProgram"])
            {
                label3.Text = "";
                DialogResult closeVoting = MessageBox.Show("Are you sure, you want to stop the voting application", "Stop Voting", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (closeVoting == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else 
                {
                    LoginForm loginform = new LoginForm();
                    loginform.ShowDialog();
                }
            }
            else
            {
                label3.Visible = true;
                label3.Text = "Invalid Password";
            }
        }
    }
}
