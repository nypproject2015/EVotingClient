using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NYP.EVoting.EvotingService;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.Configuration;
using System.Web.Services.Protocols;

namespace NYP.EVoting
{
    public partial class Voting : Form
    {
        private string clientCertifcateName = ConfigurationManager.AppSettings["CertificateName"];
        private string FilePath = ConfigurationManager.AppSettings["FilePath"];
        private string constituencyID;
        private bool btnclicked=false;
        private bool btnclicked2 = false;
        private bool btnclicked3 = false;
        private bool btnclicked4 = false;
        private bool selected_party1 = false;
        private bool selected_party2 = false;
        private bool selected_party3 = false;
        private bool selected_party4 = false;
        private int partyselection =0;
        public Voting(string fConstituencyID)
        {
            
            InitializeComponent();
            constituencyID = fConstituencyID;
            
        }
        public Voting()
        {

            InitializeComponent();
        }
        constituencyPartyMapping constituencyPartyMapping = new constituencyPartyMapping();
        //Party[] PartyDescription = new Party[4];
        //Party partyArray = new Party();
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            EvotingService.EVotingService evotingService = new EvotingService.EVotingService();
            
            try
            {
               WebServiceSecureCalleVoting(evotingService);
               ConstituencyPartyMappingBean ConstituencyPartyMappingBean = evotingService.fetchCandidates(constituencyID);
               constituencyPartyMapping.ConstituencyID = ConstituencyPartyMappingBean.constituencyId.ToString();
               constituencyPartyMapping.ConstituencyName = ConstituencyPartyMappingBean.constituencyName.ToString();
               constituencyName.Text = ConstituencyPartyMappingBean.constituencyName.ToString();
               int partyBeanCount = ConstituencyPartyMappingBean.partyBean.Count();
               Party[] PartyDescription = new Party[partyBeanCount];
               for (int pbCounter = 0; pbCounter < partyBeanCount; pbCounter++)
                 {
                    Party partyArray = new Party();
                    partyArray.ConstituencyPartyMappingID = ConstituencyPartyMappingBean.partyBean[pbCounter].constituencyPartyMappingid.ToString();
                    partyArray.DateOfInception = Convert.ToDateTime(ConstituencyPartyMappingBean.partyBean[pbCounter].dateOfInception);
                    partyArray.PartyID = ConstituencyPartyMappingBean.partyBean[pbCounter].partyId;
                    partyArray.PartyName = ConstituencyPartyMappingBean.partyBean[pbCounter].partyName;
                    partyArray.PartySymbol = ConstituencyPartyMappingBean.partyBean[pbCounter].partySymbol;
                    partyArray.Selected = Convert.ToBoolean(ConstituencyPartyMappingBean.partyBean[pbCounter].selected);
                    partyArray.Status = ConstituencyPartyMappingBean.partyBean[pbCounter].status;
                    PartyDescription[pbCounter] = partyArray;

                    byte[] imageBytes = ConstituencyPartyMappingBean.partyBean[pbCounter].partySymbol;
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    Image image = Image.FromStream(ms, true);
                              
                    Bitmap bmImage = new Bitmap(image);
                    bmImage.Save(FilePath + @"\Images\" + "008" + ConstituencyPartyMappingBean.partyBean[pbCounter].partyName.ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                
                    if (lblName1.Text == "Vote")
                    {
                        lblName1.Text = ConstituencyPartyMappingBean.partyBean[pbCounter].partyName.ToString();
                        pbparty1.Image = image;
                        string PartyID_1 = PartyDescription[pbCounter].PartyID;
                    }
                    else if (lblName2.Text == "Vote")
                    {
                        lblName2.Text = ConstituencyPartyMappingBean.partyBean[pbCounter].partyName.ToString();
                        pbparty2.Image = image;
                        string PartyID_2 = PartyDescription[pbCounter].PartyID;
                    }
                    else if (lblName3.Text == "Vote")
                    {
                        lblName3.Text = ConstituencyPartyMappingBean.partyBean[pbCounter].partyName.ToString();
                        pbparty3.Image = image;
                        string PartyID_3 = PartyDescription[pbCounter].PartyID;
                    }
                    else if (lblName4.Text == "Vote")
                    {
                        lblName4.Text = ConstituencyPartyMappingBean.partyBean[pbCounter].partyName.ToString();
                        pbparty4.Image = image;
                        string PartyID_4 = PartyDescription[pbCounter].PartyID;
                    }

                 }
               constituencyPartyMapping.PartyDescription = PartyDescription;
            }
            catch (Exception error)
            {
                returnLoginPageOnError(error.Message);
            }

                       
        }
        public void returnLoginPageOnError(string ErrorMessage)
        {
            label1.Text = ErrorMessage;
            LoginForm loginfrm = new LoginForm();
            loginfrm.ShowDialog();
            return;
        }
        //public Image EncodedBase64ToImage(string base64String)
        //{
        //    // Convert Base64 String to byte[]
        //    byte[] imageBytes = Convert.FromBase64String(base64String);
        //    MemoryStream ms = new MemoryStream(imageBytes, 0,imageBytes.Length);
        //    // Convert byte[] to Image
        //    ms.Write(imageBytes, 0, imageBytes.Length);
        //    Image image = Image.FromStream(ms, true);
        //    return image;
        //}

        private void WebServiceSecureCalleVoting(EvotingService.EVotingService evotingService)
        {
            //Validate the Server certificate
            ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificateCallback;

            //Read the client certificate from the certificate store
            X509Certificate2 clientCertificate = new X509Certificate2();
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection clientCertificates = store.Certificates.Find(X509FindType.FindBySubjectName, clientCertifcateName, false);

            if (clientCertificates.Count > 0)
            {
                clientCertificate = clientCertificates[0];
            };
            evotingService.ClientCertificates.Add(clientCertificate);
            store.Close();

        }
        public static bool TrustAllCertificateCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {
            Boolean TrustServerCertificate = Convert.ToBoolean(ConfigurationManager.AppSettings["TruseServerSSL"]);
            return TrustServerCertificate;
        }

        //private void WebServiceSecureCalleVoting(EvotingService.EVotingService evotingService)
        //{
        //    //Validate the Server certificate
        //    ServicePointManager.ServerCertificateValidationCallback = TrustServerCertificateCallback;
        //    //Read the client certificate from the certificate store
        //    X509Certificate2 clientCertificate = new X509Certificate2();
        //    X509Store store = new X509Store(StoreLocation.CurrentUser);
        //    store.Open(OpenFlags.ReadOnly);
        //    X509Certificate2Collection clientCertificates = store.Certificates.Find(X509FindType.FindBySubjectName, clientCertifcateName, false);

        //    if (clientCertificates.Count > 0)
        //    {
        //        clientCertificate = clientCertificates[0];
        //    };
        //    evotingService.ClientCertificates.Add(clientCertificate);
        //    store.Close();

        //}
  
        //public static bool TrustServerCertificateCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        //{
        //    bool b1 = false; ;
        //    foreach (X509ChainElement e in chain.ChainElements)
        //        if (e.Certificate.Subject == ConfigurationManager.AppSettings["ServerSSLSubject"] 
        //            && e.Certificate.GetPublicKeyString() == ConfigurationManager.AppSettings["ServerPublicKey"])
                    
        //            b1 =true;
            
        //    return b1;
        //}
    
       private void btnParty1_Click(object sender, EventArgs e)
        {

            if (btnclicked)
            {
                
                btnclicked = false; selected_party1 = false;partyselection = partyselection - 1;
                pb1.Visible = false;
                btnParty1.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\voteforme.jpg");
                constituencyPartyMapping.PartyDescription[0].Selected = false;
             }
           else
            {
                btnclicked = true; selected_party1 = true; partyselection = partyselection + 1;
                pb1.Visible = true;
                btnParty1.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\Novote.png");
                constituencyPartyMapping.PartyDescription[0].Selected = true;
            }
        } 
        private void btnParty2_Click(object sender, EventArgs e)
        {
            if (btnclicked2)
            {
                 btnclicked2 = false; selected_party2 = false; partyselection = partyselection - 1;
                 pb2.Visible = false;
                 btnParty2.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\voteforme.jpg");
                 constituencyPartyMapping.PartyDescription[1].Selected = false;
            }
            else
            {
                
                btnclicked2 = true; selected_party2 = true; partyselection = partyselection + 1;
                btnParty2.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\Novote.png");
                pb2.Visible = true;
                constituencyPartyMapping.PartyDescription[1].Selected = true;
            }
            
          }
        private void btnParty3_Click(object sender, EventArgs e)
        {

            if (btnclicked3)
            {
                
                 btnclicked3 = false; selected_party3 = false; partyselection = partyselection - 1;
                 btnParty3.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\voteforme.jpg");
                 pb3.Visible = false;
                 constituencyPartyMapping.PartyDescription[2].Selected = false;
            }
            else
            {
                              
                btnclicked3 = true; selected_party3 = true; partyselection = partyselection + 1;
                btnParty3.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\Novote.png");
                pb3.Visible = true;
                constituencyPartyMapping.PartyDescription[2].Selected = true;
            }

        }
        private void btnParty4_Click(object sender, EventArgs e)
        {

            if (btnclicked4)
            {
                
                btnclicked4 = false; selected_party4 = false; partyselection = partyselection - 1;
                pb4.Visible = false;
                btnParty4.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\voteforme.jpg");
                constituencyPartyMapping.PartyDescription[3].Selected = false;
            }
            else
            {
               
                btnclicked4 = true; selected_party4 = true; partyselection = partyselection + 1;
                pb4.Visible = true;
                btnParty4.BackgroundImage = Image.FromFile(FilePath + @"\\Images\\Novote.png");
                constituencyPartyMapping.PartyDescription[3].Selected = true;
            }
         }
        private void btnVote_Click(object sender, EventArgs e)
        {
            //EvotingService.EVotingService evotingService = new EvotingService.EVotingService();
            //constituencyPartyMapping constituencyPartyMapping = new constituencyPartyMapping();
            //WebServiceSecureCalleVoting(evotingService);
            //ConstituencyPartyMappingBean ConstituencyPartyMappingBean = evotingService.fetchCandidates(constituencyID);

            DialogResult MultiPartySelect;
            DialogResult SinglePartySelect;
            MultiPartySelect = DialogResult.None;
            SinglePartySelect = DialogResult.None;

            using (var evotingService = new EvotingService.EVotingService())
            {

                try
                {
                    ConstituencyPartyMappingBean constituencyPartyMappingBean = new ConstituencyPartyMappingBean();
                    PartyBean[] partyBeanList = new PartyBean[4];
                    PartyBean partyBeanItem = new PartyBean();

                    for (int pmCounter = 0; pmCounter < constituencyPartyMapping.PartyDescription.Count(); pmCounter++)
                    {
                        constituencyPartyMappingBean.constituencyId = Convert.ToString(constituencyPartyMapping.ConstituencyID);
                        constituencyPartyMappingBean.constituencyName = constituencyPartyMapping.ConstituencyName.ToString();
                        PartyBean partyBean = new PartyBean();
                        partyBean.constituencyPartyMappingid = Convert.ToInt32(constituencyPartyMapping.PartyDescription[pmCounter].ConstituencyPartyMappingID);
                        partyBean.dateOfInception = constituencyPartyMapping.PartyDescription[pmCounter].DateOfInception;
                        partyBean.partyId = constituencyPartyMapping.PartyDescription[pmCounter].PartyID;
                        partyBean.partyName = constituencyPartyMapping.PartyDescription[pmCounter].PartyName;
                        partyBean.partySymbol = constituencyPartyMapping.PartyDescription[pmCounter].PartySymbol;
                        partyBean.selected = constituencyPartyMapping.PartyDescription[pmCounter].Selected;
                        partyBean.status = constituencyPartyMapping.PartyDescription[pmCounter].Status;
                        partyBeanList[pmCounter] = partyBean;
                    }
                   constituencyPartyMappingBean.partyBean = partyBeanList;
                                        
                   if ((selected_party1) || (selected_party2) || (selected_party3) || (selected_party4))
                    {
                        if (partyselection > 1)
                        {
                            MultiPartySelect = MessageBox.Show("More than 1 Party selected. Do you confirm your vote?", "Voting", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            SinglePartySelect = MessageBox.Show("Do you confirm your vote?", "VotingConfirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        }
                        if ((MultiPartySelect == DialogResult.Yes) || (SinglePartySelect == DialogResult.Yes))
                        {
                            WebServiceSecureCalleVoting(evotingService);
                            evotingService.recordVote(constituencyPartyMappingBean);
                            this.Hide();
                            VoteConfirmation VoteConfirmationform = new VoteConfirmation();
                            VoteConfirmationform.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select atleast one party!", "Voting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    }
                catch (Exception error)
                {

                    returnLoginPageOnError(error.Message);
                }
            }
        }

       private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateAndTime = DateTime.Now;
            lbltimer.Text = dateAndTime.ToString("dd-MM-yyyy hh:mm:ss tt");
        }
      }
    }
    
