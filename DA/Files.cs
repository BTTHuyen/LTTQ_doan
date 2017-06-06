using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace ModifyImg
{
    class Files
    {
        public bool Opened;
        public bool Saved;
        public string strPathOpen;
        public string strPathSave;
        public double zoom;
        public int DoXoay;
        private Bitmap bmImage;

        public Files()
        {
            this.Opened = false;
            this.Saved = true;
            strPathSave = "";
            strPathOpen = "";
            zoom = 1;
            DoXoay = 0;
        }

        public Bitmap img
        {
            get { return bmImage; }
            set { bmImage =  AForge.Imaging.Image.Clone(value,PixelFormat.Format24bppRgb); }
        }

        public void OpenFile()
        {
            OpenFileDialog openf = new OpenFileDialog();
            openf.Filter = "All Image files|*.bmp;*.gif;*.jpg;*.ico;*.png;" +
                "*.emf;,*.wmf|Bitmap Files(*.bmp;*.gif;*.jpg;" +
                "*.ico)|*.bmp;*.gif;*.jpg;*.ico|" +
                "Meta Files(*.emf;*.wmf;*.png)|*.emf;*.wmf;*.png";
            openf.Multiselect = false;
            openf.Title = "Choose Image";
            if (openf.ShowDialog() == DialogResult.OK)
            {
                this.strPathOpen = openf.FileName;
                bmImage = AForge.Imaging.Image.Clone(new Bitmap(this.strPathOpen),PixelFormat.Format24bppRgb);
                Opened = true;
                Saved = true;
            }
        }

        public void SaveFile()
        {
            strPathSave = "";
            if (this.Saved == false)
            {

                SaveFileDialog savef = new SaveFileDialog();
                savef.Title = "Save Image";
                savef.DefaultExt = "jpg";
                savef.Filter = "PNG File(*.png)|*.png" +
                "Bitmap File(*.bmp)|*.bmp|" +
                "JPEG File(*.jpg)|*.jpg|" +
                "Gif File(*.gif)|*.gif|";
                if (savef.ShowDialog() == DialogResult.OK)
                {
                    string fileName = savef.FileName;
                    string strFilExtn = fileName.Remove(0, fileName.Length - 3);
                    switch (strFilExtn)
                    {
                        case "bmp":
                            bmImage.Save(fileName, ImageFormat.Bmp);
                            break;
                        case "jpg":
                            bmImage.Save(fileName, ImageFormat.Jpeg);
                            break;
                        case "gif":
                            bmImage.Save(fileName, ImageFormat.Gif);
                            break;
                        case "tif":
                            bmImage.Save(fileName, ImageFormat.Tiff);
                            break;
                        case "png":
                            bmImage.Save(fileName, ImageFormat.Png);
                            break;
                        default:
                            break;
                    }
                    this.strPathSave = savef.FileName;
                    this.Saved = true;

                }
            }
        }

        public void GoFullscreen()
        {
            Form frmViewfull = new Form();
            frmViewfull.WindowState = FormWindowState.Normal;
            frmViewfull.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frmViewfull.Bounds = Screen.PrimaryScreen.Bounds;
            PictureBox picFull = new PictureBox();
            picFull.Dock = DockStyle.Fill;
            picFull.Image = bmImage;
            picFull.SizeMode = PictureBoxSizeMode.Zoom;
            frmViewfull.Controls.Add(picFull);
            frmViewfull.BackColor = Color.Black;
            frmViewfull.KeyDown += new KeyEventHandler(keypressed);
            frmViewfull.Show();
        }

        private void keypressed(Object o, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Form f = (Form)o;
                f.Close();
                f.Dispose();
            }
        }

        public void OpenwithPaint()
        {
            System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
            procInfo.FileName = ("mspaint.exe");
            if (this.strPathSave == "")
                procInfo.Arguments = string.Format("\"{0}\"", this.strPathOpen);
            else
            {
                if (this.Saved == false)
                    this.SaveFile();
                procInfo.Arguments = string.Format("\"{0}\"", this.strPathSave);
            }
            System.Diagnostics.Process.Start(procInfo);
        }

        public void SetBackgrounDesktop()
        {
            if (this.Opened)
            {
                if (Saved == false)
                {
                    MessageBox.Show("Please Save image!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SaveFile();
                }
                else
                    if (strPathSave != "") Wallpaper.SetDesktopWallpaper(strPathSave);
                    else
                        Wallpaper.SetDesktopWallpaper(strPathOpen);
            }
        }

        private void printImg_event(Object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmImage, 0, 0);
        }

        public void PrintImg()
        {
            PrintDocument PrintDoc = new PrintDocument();
            PrintDialog PrintDia = new PrintDialog();
            PrintDoc.PrintPage += new PrintPageEventHandler(printImg_event);
            PrintDia.Document = PrintDoc;
            if (PrintDia.ShowDialog() == DialogResult.OK)
            {
                PrintDoc.Print();
            }
        }

        public void OpenCmr()
        {
            Camera Cmr = new Camera();
            try
            {
                
            }
            catch (Exception e)
            {

            }
            Cmr.ShowDialog();
            if (Cmr.Ok)
            {
                Opened = true;
                Saved = false;
                strPathSave = "";
                strPathOpen = "";
                bmImage = Cmr.bmp;
            }
            Cmr.Dispose();
            Cmr.Close();
        }
    }

    class Wallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        const uint SPI_SETDESKWALLPAPER = 20;
        const uint SPIF_SENDWININICHANGE = 0x1;
        public static void SetDesktopWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_SENDWININICHANGE);
        }
    }
}
