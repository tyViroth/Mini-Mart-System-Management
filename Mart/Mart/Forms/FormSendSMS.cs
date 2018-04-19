using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart.Forms
{
    public partial class FormSendSMS : Form
    {
        public FormSendSMS()
        {
            InitializeComponent();
            btnSend.Click += btnSend_Click;
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            using(System.Net.WebClient client = new System.Net.WebClient()){
                try
                {
                    string url = string.Format("http://smsc.vianett.no/v3/send.ashx?src={0}&dst={1}&msg={2}&username={3}&password={4}",
                        txtPhoneNumber.Text.Trim(),
                        txtPhoneNumber.Text.Trim(),
                        System.Web.HttpUtility.UrlDecode(txtText.Text.Trim(),System.Text.Encoding.GetEncoding("ISO-8859-1")),
                        System.Web.HttpUtility.UrlEncode(txtUsername.Text.Trim()),
                        System.Web.HttpUtility.UrlEncode(txtPassword.Text.Trim()));
                    string result = client.DownloadString(url);
                    if (result.Contains("OK"))
                    {
                        MessageBox.Show("Your text has been sent","Message");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Message send failure", "Message");
                }
            }
        }
    }
}
