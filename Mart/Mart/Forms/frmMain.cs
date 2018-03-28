using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mart.UserControls;
using Mart.InstanceClasses;
using Mart.Forms;

using System.IO;

namespace Mart
{
    public partial class frmMain : Form
    {
        List<Panel> listPanel = new List<Panel>();
        Point mouseLocation;
        readonly int MINIMUM_WIDTH = 1000;
        readonly int MINIMUM_HEIGHT = 700;
        readonly int WIDTH_NO_TASKBAR = Screen.PrimaryScreen.WorkingArea.Width;
        readonly int HEIGHT_NO_TASKBAR = Screen.PrimaryScreen.WorkingArea.Height;
        private readonly Color ButtonBackGround = Color.FromArgb(255, 205, 65);        

        private Employee empLogin;

        /* This Constructor will be Called when Login is Successful */
        public frmMain(Employee empLogin):this()
        {
            if (empLogin == null) throw new ArgumentNullException();
            this.empLogin = empLogin;           
        }

        public frmMain()
        {
            InitializeComponent();
            SetSizeLocation();
            RegisterEventControll();            
        }

        private void SetSizeLocation()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            Width = MINIMUM_WIDTH;
            Height = MINIMUM_HEIGHT;
            this.CenterToScreen();
        }

        private void RegisterEventControll()
        {
            /* After Form Load Event*/
            this.Shown += frmMain_Shown;

            /*Picture Account Event*/
            pbAccountImage.MouseHover += pbAccountImage_MouseHover;
            pbAccountImage.Click += pbAccountImage_Click;

            /* Set Opacity Event to Main Form for First Load */
            this.Opacity = 0.1;
            timerMain.Interval = 20;
            timerMain.Tick += timerMain_Tick;

            /* Set Event Banner Panel for Moving Main Form */
            pBanner.MouseDown += pBanner_MouseDown;           
            pBanner.MouseMove += pBanner_MouseMove;
            pBanner.MouseUp += pBanner_MouseUp;

            /* Set Click Event to TitleBar Button */
            pbExit.Click +=DoClick;
            pbMinimize.Click += DoClick;
            pbResize.Click += DoClick;

            /*Set Hover Event to TitleBar Button*/
            pbExit.MouseHover += DoHover;
            pbMinimize.MouseHover += DoHover;
            pbResize.MouseHover += DoHover;

            /* Register Manu Buttons */
            btnUser.Click +=btnUser_Click;
            btnStock.Click +=btnStock_Click;
            btnSold.Click +=btnSold_Click;
            btnSetting.Click +=btnSetting_Click;
            btnReport.Click +=btnReport_Click;
            btnProduct.Click +=btnProduct_Click;
            btnExit.Click +=btnExit_Click;
            btnBin.Click +=btnBin_Click;
        }

        void pBanner_MouseUp(object sender, MouseEventArgs e)
        {
            /*Check Main Form Location*/          
            if (this.Location.Y <= 0)
            {
                Left = Top = 0;
                Width = WIDTH_NO_TASKBAR;
                Height = HEIGHT_NO_TASKBAR;
            }
        }

        void pbAccountImage_Click(object sender, EventArgs e)
        {
            if (empLogin != null)
            {
                frmProfile profile = new frmProfile(empLogin);
                profile.FormClosed += profile_FormClosed;
                profile.ShowDialog();
            }
        }

        void profile_FormClosed(object sender, FormClosedEventArgs e)
        {
            lblUsername.Text = empLogin.LastName + " " + empLogin.FirstName ;
            pbAccountImage.Image = Image.FromStream(new MemoryStream(empLogin.Photo));
            lblRole.Text = empLogin.Roles.Name;
        }

        void pbAccountImage_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip((PictureBox)sender,"Account Settings");
        }

        void frmMain_Shown(object sender, EventArgs e)
        {
            /* Define role to All Users */
            if (empLogin != null)
            {
                if (empLogin.Roles.Name.CompareTo("Admin") != 0)
                {
                    btnUser.Enabled = false;
                }
                lblRole.Text = empLogin.Roles.Name;
                lblUsername.Text = empLogin.LastName +" "+empLogin.FirstName;

                pbAccountImage.Image = Image.FromStream(new MemoryStream(empLogin.Photo));     
            }            
        }

        void timerMain_Tick(object sender, EventArgs e)
        {
            if (this.Opacity <= 1.0)
            {
                this.Opacity += 0.025;
            }
            else
            {
                this.timerMain.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            /* Start Opacity to Form */
            this.timerMain.Start();
        }

        private void DoHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            if (sender == pbMinimize)
            {
                toolTip.SetToolTip((PictureBox)sender,"Minimize");
            }
            else if (sender == pbResize)
            {
                toolTip.SetToolTip((PictureBox)sender, "Resize");            
            }
            else if (sender == pbExit)
            {
                toolTip.SetToolTip((PictureBox)sender, "Exit");                   
            }
        }

        void pBanner_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {               
                /* IF main Form is match the whole screen => we set it to Minimum size */                
                if (Top == 0 && Left == 0 && Width == WIDTH_NO_TASKBAR && Height == HEIGHT_NO_TASKBAR)
                {
                    Width = MINIMUM_WIDTH;
                    Height = MINIMUM_HEIGHT;
                    this.CenterToScreen();    
                }                
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;                               
            }
        }        

        void pBanner_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X,-e.Y);
        }

        private void DoClick(object sender, EventArgs e)
        {
            if (sender == pbMinimize)
            {
                WindowState = FormWindowState.Minimized;
            }else if(sender == pbResize){
                if(Width == MINIMUM_WIDTH && Height == MINIMUM_HEIGHT){
                    Left = Top = 0;
                    Width = WIDTH_NO_TASKBAR;
                    Height = HEIGHT_NO_TASKBAR;                   
                }
                else
                {                       
                    Width = MINIMUM_WIDTH;
                    Height = MINIMUM_HEIGHT;
                    this.CenterToScreen();
                }                
            }
            else if(sender == pbExit)
            {
                Environment.Exit(0);
            }                
        }      

        private void btnExit_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            Environment.Exit(0);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            btnUser.BackColor = ButtonBackGround;       

            if (!mainPanel.Controls.Contains(UEmployee.Instance))
            {
                mainPanel.Controls.Add(UEmployee.Instance);
                UEmployee.Instance.Dock = DockStyle.Fill;
                UEmployee.Instance.BringToFront();
            }
            else
            {
                UEmployee.Instance.BringToFront();
            }                 
        }

        private void ClearMenuButtonColor()
        {
            btnUser.BackColor = Color.Transparent;
            btnStock.BackColor = Color.Transparent;
            btnSold.BackColor = Color.Transparent;
            btnSetting.BackColor = Color.Transparent;
            btnReport.BackColor = Color.Transparent;
            btnProduct.BackColor = Color.Transparent;
            btnExit.BackColor = Color.Transparent;
            btnBin.BackColor = Color.Transparent;            
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            btnSetting.BackColor = ButtonBackGround;       

            if (!mainPanel.Controls.Contains(USetting.Instance))
            {
                mainPanel.Controls.Add(USetting.Instance);
                USetting.Instance.Dock = DockStyle.Fill;
                USetting.Instance.BringToFront();
            }
            else
            {
                USetting.Instance.BringToFront();
            }
        }

        private void btnBin_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            btnBin.BackColor = ButtonBackGround;      

            if (!mainPanel.Controls.Contains(UBin.Instance))
            {
                mainPanel.Controls.Add(UBin.Instance);
                UBin.Instance.Dock = DockStyle.Fill;
                UBin.Instance.BringToFront();
            }
            else
            {
                UBin.Instance.BringToFront();
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            btnProduct.BackColor = ButtonBackGround;      

            if (!mainPanel.Controls.Contains(UProduct.Instance))
            {
                mainPanel.Controls.Add(UProduct.Instance);
                UProduct.Instance.Dock = DockStyle.Fill;
                UProduct.Instance.BringToFront();
            }
            else
            {
                UProduct.Instance.BringToFront();
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            btnReport.BackColor = ButtonBackGround;       

            if (!mainPanel.Controls.Contains(UReport.Instance))
            {
                mainPanel.Controls.Add(UReport.Instance);
                UReport.Instance.Dock = DockStyle.Fill;
                UReport.Instance.BringToFront();
            }
            else
            {
                UReport.Instance.BringToFront();
            }
        }

        private void btnSold_Click(object sender, EventArgs e)
        {
            ClearMenuButtonColor();
            btnSold.BackColor = ButtonBackGround;       

            if (!mainPanel.Controls.Contains(USold.Instance))
            {
                mainPanel.Controls.Add(USold.Instance);
                USold.Instance.Dock = DockStyle.Fill;
                USold.Instance.BringToFront();
            }
            else
            {
                USold.Instance.BringToFront();
            }
        }

        private void btnStock_Click(object sender, EventArgs e)
        {            
            ClearMenuButtonColor();
            btnStock.BackColor = ButtonBackGround;       

            if (!mainPanel.Controls.Contains(UImportStock.Instance))
            {
                mainPanel.Controls.Add(UImportStock.Instance);
                UImportStock.Instance.Dock = DockStyle.Fill;
                UImportStock.Instance.BringToFront();
            }
            else
            {
                UImportStock.Instance.BringToFront();
            }
        }

        private void showFormInPanel(Form form)
        {
            mainPanel.Controls.Clear();
            form.TopLevel = false;
            form.AutoScroll = true;
            mainPanel.Controls.Add(form);

            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }
        
    }
}
