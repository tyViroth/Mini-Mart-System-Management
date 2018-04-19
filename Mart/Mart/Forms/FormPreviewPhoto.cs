using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Mart.Forms
{
    public partial class FormPreviewPhoto : Form
    {
        byte[] image = null;
        public FormPreviewPhoto(byte[] image):this()
        {           
            this.image = image;            
        }

        public FormPreviewPhoto()
        {
            InitializeComponent();          
            this.Shown += frmPreviewPhoto_Shown;
        }


        void frmPreviewPhoto_Shown(object sender, EventArgs e)
        {
            if (image != null)
            {
                pbPreviewImage.Image = Image.FromStream(new MemoryStream(image));
            }   
        }

    }
}
