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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
                       
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //if (txtConstituencyID.Text == ConfigurationManager.AppSettings["ConstituencyID"])
            //{
            //    Voting votingform = new Voting(txtConstituencyID.Text);
            //    this.Hide();
            //    votingform.ShowDialog();
            //}
            //else
            //{
                
            //    label3.Text = "Invalid constituencyID";
            //    label3.Visible = true;
            //}
           
                Voting votingform = new Voting(txtConstituencyID.Text);
                this.Hide();
                votingform.ShowDialog();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateAndTime = DateTime.Now;
            lbltimer.Text=dateAndTime.ToString("dd-MM-yyyy hh:mm:ss tt");

             
        }

        private void Login_Load(object sender, EventArgs e)
        {
            VoteConfirmation VoteConfirmationform = new VoteConfirmation();
            ExitForm exitvoting = new ExitForm();
            VoteConfirmationform.Hide();
            exitvoting.Hide();
            timer1.Start();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            ExitForm exitvotingform = new ExitForm();
            exitvotingform.ShowDialog();
        }
    }
}
