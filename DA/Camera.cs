using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;

namespace ModifyImg
{
    public partial class Camera : Form
    {
        FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice Cmr;
        public Bitmap bmp;
        public bool Ok;
        private bool KT_loi;
        public Camera()
        {
            InitializeComponent();
            Ok = false;
            KT_loi = false;
        }

        private void Camera_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (KT_loi == false) Cmr.Stop();
        }

        private void Camera_Load(object sender, EventArgs e)
        {

            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
            {
                comboBox.Items.Add(Device.Name);
            }
            if (comboBox.Items.Count == 0)
            {
                MessageBox.Show("Not find any camera device, Check again !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Ok = false;
                KT_loi = true;
                btnCapture.Enabled = false;
            }
            else
            {
                comboBox.SelectedIndex = 0;
                Cmr = new VideoCaptureDevice(CaptureDevice[comboBox.SelectedIndex].MonikerString);
                Cmr.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
                Cmr.Start();
            }
        }

        void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            cmrShow.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void bntCapture_Click(object sender, EventArgs e)
        {
            capturePic.Image = cmrShow.Image;
            btnOK.Enabled = true;
        }

        private void bntCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (KT_loi == false) Cmr.Stop();
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)capturePic.Image;
            Ok = true;
            this.Hide();
            Cmr.Stop();
        }
    }
}
