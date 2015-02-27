using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace NYP.EVoting
{
    public partial class VoteConfirmation : Form
    {
        public VoteConfirmation()
        {
            InitializeComponent();
        }

        private void RecordVote_Load(object sender, EventArgs e)
        {
            
            timer1.Interval = (3*1000); 
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
                  
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Hide();
            LoginForm loginform = new LoginForm();
            loginform.ShowDialog();
        }
     
    }
}
