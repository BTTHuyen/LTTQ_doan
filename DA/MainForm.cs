using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace ModifyImg
{
    public partial class mainfrm : Form
    {
        private Files f;
        private bool CheckUpdate;
        public mainfrm()
        {
            InitializeComponent();
            f = new Files();
            CheckUpdate = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #region DoiTuongve
        Pen pen;
        // tạo đối tượng vẽ
        int numberDraw = 0; // vẽ
        int ChooseTypeDraw = 1; // loại hình vẽ
        int sizepen = 1; // cỡ bút vẽ
        Color colorDraw = Color.Black; // màu vẽ
        Point PointStart = new Point(0, 0);
        Point PointFinish = new Point(0, 0);
        Bitmap bm;
        // tạo đối tượng vẽ hình chữ nhật
        int PointX, PointY, width, height;
        // tạo đối tượng vẽ hình tam giác
        Point PointA, PointB, PointC = new Point(0, 0);
        Icon icon;
        List<Bitmap> UndoRedo = new List<Bitmap>();
        int indexUndoRedo;
        float scoreX, scoreY;
        #endregion

        #region menu
        private void filter_Click(object sender, EventArgs e)
        {
            if (f.Opened)
            {
                panelDraw.Hide();
                panelRotate.Hide();
                this.Enabled = false;
                trackBar_Blue.Value = 30;
                trackBar_Bright.Value = 30;
                trackBar_Contrast.Value = 30;
                trackBar_Green.Value = 30;
                trackBar_Red.Value = 30;
                Filter();
            }
        }


        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f.Opened)
            {
                panelDraw.Hide();
                panelFilter.Hide();
                CBXoay();
            }
        }

        private void draw_Click(object sender, EventArgs e)
        {
            if (f.Opened)
            {
                UndoRedo.Clear();
                panelFilter.Hide();
                panelRotate.Hide();
                panelDraw.Show();
                DrawOnImg();
            }
        }

        private void imageOpen_Click(object sender, EventArgs e)
        {
            if (f.Opened == false || (f.Opened == true) && (f.Saved == true))
                f.OpenFile();
            else
            {

                DialogResult dlr = MessageBox.Show("This image hasn't been saved. \n Do you want to save it ?", "!!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                    f.SaveFile();
                if (dlr == DialogResult.No)
                    f.OpenFile();
            }
            panelFilter.Hide();
            panelDraw.Hide();
            panelRotate.Hide();

            // hiển thị ảnh tại màn hình chính
            if (f.Opened)
            {
                mainPicture.Image = f.img;
                if (f.img.Height <= 626 && f.img.Width <= 1008)
                {
                    double a = 626.0 / f.img.Height;
                    double b = 1008.0 / f.img.Width;
                    double hs;
                    if (a < b) hs = a;
                    else hs = b;
                    mainPicture.Height = (int)hs * f.img.Height;
                    mainPicture.Width = (int)hs * f.img.Width;
                }
                else
                {
                    double a = f.img.Height / 626.0;
                    double b = f.img.Width / 1008.0;
                    double hs;
                    if (a > b) hs = a;
                    else hs = b;
                    mainPicture.Height = (int)(f.img.Height / hs);
                    mainPicture.Width = (int)(f.img.Width / hs);
                }
                mainPicture.Location = new Point((1008 - mainPicture.Width) / 2, (626 - mainPicture.Height) / 2 + 32);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (f.Saved == false)
            {
                f.SaveFile();
                mainPicture.Image = f.img;
            }
            panelDraw.Hide();
            panelFilter.Hide();
            panelRotate.Hide();
        }

        private void fullscreen_Click(object sender, EventArgs e)
        {
            if (f.Opened)
            {
                f.GoFullscreen();
                panelDraw.Hide();
                panelFilter.Hide();
                panelRotate.Hide();
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f.Saved == false)
            {
                DialogResult dlr = MessageBox.Show("This image hasn't been saved. \n Do you want to save it ?", "!!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                    f.SaveFile();
                if (dlr == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void setdesktopBackground_Click(object sender, EventArgs e)
        {
            panelDraw.Hide();
            panelFilter.Hide();
            panelRotate.Hide();
            f.SetBackgrounDesktop();
        }

        private void openWithpaint_Click(object sender, EventArgs e)
        {
            panelDraw.Hide();
            panelFilter.Hide();
            panelRotate.Hide();
            f.OpenwithPaint();
        }

        private void About_Click(object sender, EventArgs e)
        {
            panelDraw.Hide();
            panelFilter.Hide();
            panelRotate.Hide();
            About Ab = new About();
            Ab.Show();
        }

        private void print_Click(object sender, EventArgs e)
        {
            panelDraw.Hide();
            panelFilter.Hide();
            panelRotate.Hide();
            f.PrintImg();
        }

        private void OpenCamera_Click(object sender, EventArgs e)
        {
            f.OpenCmr();
            panelDraw.Hide();
            panelFilter.Hide();
            panelRotate.Hide();
            if (f.Opened) mainPicture.Image = f.img;
        }

        private void DrawOnImg()
        {
            pictureDraw.Image = mainPicture.Image;
            pictureDraw.Cursor = new Cursor("Cursor.cur");
            pictureDraw.SizeMode = PictureBoxSizeMode.StretchImage;
            scoreX = (float)pictureDraw.Image.Width / pictureDraw.Width;
            scoreY = (float)pictureDraw.Image.Height / pictureDraw.Height;
            bm = new Bitmap(pictureDraw.Image);

            Bitmap a = (Bitmap)bm.Clone();
            UndoRedo.Add(a);

            indexUndoRedo = 0;
        }

        private void mainfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        #endregion

        #region Draw
        private void pictureDraw_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                numberDraw = 1;
                switch (ChooseTypeDraw)
                {

                    case 1:
                    case 2:
                        PointStart.X = (int)(e.X * scoreX);
                        PointStart.Y = (int)(e.Y * scoreY);
                        break;
                    case 3:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                        PointX = (int)(e.X * scoreX);
                        PointY = (int)(e.Y * scoreY);
                        break;
                    case 4:
                        PointA.X = (int)(e.X * scoreX);
                        PointA.Y = (int)(e.Y * scoreY);
                        PointB.Y = (int)(e.Y * scoreY);
                        break;
                }
            }

        }

        private void pictureDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (numberDraw == 1)
            {
                switch (ChooseTypeDraw)
                {
                    #region Draw Shape
                    case 1: // vẽ đường tự do

                        PointFinish.X = (int)(e.X * scoreX);
                        PointFinish.Y = (int)(e.Y * scoreY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            g.DrawLine(pen, PointStart, PointFinish);
                        }
                        PointStart = PointFinish;
                        pictureDraw.Image = bm;
                        mainPicture.Image = bm;
                        break;

                    case 2:
                        PointFinish.X = (int)(e.X * scoreX);
                        PointFinish.Y = (int)(e.Y * scoreY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            g.DrawLine(pen, PointStart, PointFinish);
                        }

                        pictureDraw.Image = bm;
                        pictureDraw.Refresh();
                        pictureDraw.Invalidate();
                        bm = new Bitmap(mainPicture.Image);
                        break;
                    case 3: // vẽ hình chữ nhật

                        width = (int)Math.Abs(e.X * scoreX - PointX);
                        height = (int)Math.Abs(e.Y * scoreY - PointY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            Rectangle rc = new Rectangle(PointX, PointY, width, height);
                            g.DrawRectangle(pen, rc);
                        }
                        pictureDraw.Image = bm;
                        pictureDraw.Refresh();
                        pictureDraw.Invalidate();
                        bm = new Bitmap(mainPicture.Image);
                        break;

                    case 4: // vẽ hình tam giác

                        PointB.X = (int)(e.X * scoreX);
                        PointC.X = (int)(e.X * scoreX);
                        PointC.Y = (int)(e.Y * scoreY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            Point[] pt = { PointA, PointB, PointC };
                            g.DrawPolygon(pen, pt);
                        }

                        pictureDraw.Image = bm;
                        pictureDraw.Refresh();
                        pictureDraw.Invalidate();
                        bm = new Bitmap(mainPicture.Image);

                        break;

                    case 5: // vẽ hình ellipse
                        width = (int)Math.Abs(e.X * scoreX - PointX);
                        height = (int)Math.Abs(e.Y * scoreY - PointY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            Rectangle rc = new Rectangle(PointX, PointY, width, height);
                            g.DrawEllipse(pen, rc);
                        }
                        pictureDraw.Image = bm;
                        pictureDraw.Refresh();
                        pictureDraw.Invalidate();
                        bm = new Bitmap(mainPicture.Image);
                        break;

                    #endregion

                    #region Draw icon
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            width = (int)Math.Abs(e.X * scoreX - PointX);
                            height = (int)Math.Abs(e.Y * scoreY - PointY);
                            Rectangle rc = new Rectangle(PointX, PointY, width, height);
                            g.DrawIcon(icon, rc);
                        }
                        pictureDraw.Image = bm;
                        pictureDraw.Refresh();
                        pictureDraw.Invalidate();
                        bm = new Bitmap(mainPicture.Image);
                        break;
                    #endregion
                }
            }
        }

        private void pictureDraw_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (numberDraw == 1)
            {
                switch (ChooseTypeDraw)
                {
                    #region Draw Shape
                    case 1:
                        bm = new Bitmap(mainPicture.Image);
                        break;
                    case 2:

                        PointFinish.X = (int)(e.X * scoreX);
                        PointFinish.Y = (int)(e.Y * scoreY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            g.DrawLine(pen, PointStart, PointFinish);
                        }
                        break;
                    case 3:
                        width = (int)Math.Abs(e.X * scoreX - PointX);
                        height = (int)Math.Abs(e.Y * scoreY - PointY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            Rectangle rc = new Rectangle(PointX, PointY, width, height);
                            g.DrawRectangle(pen, rc);

                        }
                        break;
                    case 5:
                        width = (int)Math.Abs(e.X * scoreX - PointX);
                        height = (int)Math.Abs(e.Y * scoreY - PointY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            Rectangle rc = new Rectangle(PointX, PointY, width, height);
                            g.DrawEllipse(pen, rc);
                        }
                        break;
                    case 4:
                        PointB.X = (int)(e.X * scoreX);
                        PointC.X = (int)(e.X * scoreX);
                        PointC.Y = (int)(e.Y * scoreY);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            pen = new Pen(colorDraw, sizepen * scoreX);
                            Point[] pt = { PointA, PointB, PointC };
                            g.DrawPolygon(pen, pt);
                        }
                        break;
                    #endregion

                    #region Draw icon
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:

                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            width = (int)Math.Abs(e.X * scoreX - PointX);
                            height = (int)Math.Abs(e.Y * scoreY - PointY);
                            Rectangle rc = new Rectangle(PointX, PointY, width, height);
                            g.DrawIcon(icon, rc);
                        }
                        break;
                    #endregion
                }

                pictureDraw.Image = bm;
                mainPicture.Image = bm;
                f.img = bm;
                numberDraw = 0;

                // undoredo

                if (indexUndoRedo < (UndoRedo.Count - 1))
                {
                    UndoRedo.RemoveRange(indexUndoRedo + 1, UndoRedo.Count - indexUndoRedo - 1);
                }
                Bitmap a = (Bitmap)bm.Clone();
                UndoRedo.Add(a);
                f.Saved = false;
                indexUndoRedo++;
            }
        }
        #endregion

        #region ButtonDraw
        private void btnColor_Click(object sender, EventArgs e)
        {
            if (dalColorDraw.ShowDialog() == DialogResult.OK)
            {
                colorDraw = dalColorDraw.Color;
            }
        }

        private void trBSizePen_Scroll(object sender, EventArgs e)
        {
            sizepen = trBSizePen.Value;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ChooseTypeDraw = 1;
        }

        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 2;
        }

        private void btnDrawRectangle_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 3;
        }

        private void btnDrawTriangle_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 4;
        }

        private void btnDrawCircle_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 5;
        }
        #endregion

        #region Filter
        private void Filter()
        {
            Load_FilterPanel();
        }

        private void Load_FilterPanel()
        {
            pictureBox_Filter.Image = f.img;
            Resize_PanelZoom();
            f.zoom = 1;
            Load_all_default_filter1(f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter2, f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter3, f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter4, f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter5, f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter6, f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter7, f.img);
            ThreadPool.QueueUserWorkItem(Load_all_default_filter8, f.img);
            panelFilter.Show();
        }

        private void Resize_PanelZoom()
        {
            if (f.img.Height <= 578 && f.img.Width <= 650)
            {
                double a = 578.0 / f.img.Height;
                double b = 650.0 / f.img.Width;
                double hs;
                if (a < b) hs = a;
                else hs = b;
                panel_ZoomIMG.Height = (int)hs * f.img.Height;
                panel_ZoomIMG.Width = (int)hs * f.img.Width;
                pictureBox_Filter.Height = panel_ZoomIMG.Height;
                pictureBox_Filter.Width = panel_ZoomIMG.Width;
            }
            else
            {
                double a = f.img.Height / 578.0;
                double b = f.img.Width / 650.0;
                double hs;
                if (a > b) hs = a;
                else hs = b;
                CheckForIllegalCrossThreadCalls = false;
                panel_ZoomIMG.Height = (int)(f.img.Height / hs);
                panel_ZoomIMG.Width = (int)(f.img.Width / hs);
                pictureBox_Filter.Height = panel_ZoomIMG.Height;
                pictureBox_Filter.Width = panel_ZoomIMG.Width;
            }
            panel_ZoomIMG.Location = new Point(((670 - panel_ZoomIMG.Width) / 2), (600 - panel_ZoomIMG.Height) / 2);
        }

        private void Load_all_default_filter8(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox32.Image = (new FilterImg()).ToGrayscaleRMY(Im);
                pictureBox33.Image = (new FilterImg()).ToExtractChannel(Im);
                pictureBox34.Image = (new FilterImg()).ToExtractNormalizedRGBChannel(Im);
                pictureBox35.Image = (new FilterImg()).ToWaterWave(Im);
                this.Enabled = true;
                pictureBox36.Image = (new FilterImg()).ToFlatFieldCorrection(Im);
            }
        }

        private void Load_all_default_filter7(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox27.Image = (new FilterImg()).ToHistogramEqualization(Im);
                pictureBox28.Image = (new FilterImg()).ToInvert(Im);
                pictureBox29.Image = (new FilterImg()).ToEuclideanColorFiltering(Im);
                pictureBox30.Image = (new FilterImg()).ToPixellate(Im);
                pictureBox31.Image = (new FilterImg()).ToGrayScale(Im);
            }
        }

        private void Load_all_default_filter6(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox22.Image = (new FilterImg()).ToSharpen(Im);
                pictureBox23.Image = (new FilterImg()).ToOilPainting(Im);
                pictureBox24.Image = (new FilterImg()).ToOpening(Im);
                pictureBox25.Image = (new FilterImg()).ToRotateChannels(Im);
                pictureBox26.Image = (new FilterImg()).ToSepia(Im);
            }
        }

        private void Load_all_default_filter5(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox17.Image = (new FilterImg()).ToSaturationCorrection(Im);
                pictureBox18.Image = (new FilterImg()).ToSimplePosterization(Im);
                pictureBox19.Image = (new FilterImg()).ToSaltAndPepperNoise(Im);
                pictureBox20.Image = (new FilterImg()).ToJitter(Im);
                pictureBox21.Image = (new FilterImg()).ToMean(Im);
            }
        }

        private void Load_all_default_filter4(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox12.Image = (new FilterImg()).ToGaussianBlur(Im);
                pictureBox13.Image = (new FilterImg()).ToHSLFiltering(Im);
                pictureBox14.Image = (new FilterImg()).ToHueModifier(Im);
                pictureBox15.Image = (new FilterImg()).ToGammaCorrection(Im);
                pictureBox16.Image = (new FilterImg()).ToMedian(Im);
            }
        }

        private void Load_all_default_filter3(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox7.Image = (new FilterImg()).ToBrightnessCorrection(Im);
                pictureBox8.Image = (new FilterImg()).ToBlur(Im);
                pictureBox9.Image = (new FilterImg()).ToDilatation(Im);
                pictureBox10.Image = (new FilterImg()).ToExtractBiggestBlob(Im);
                pictureBox11.Image = (new FilterImg()).ToErosion(Im);
            }
        }

        private void Load_all_default_filter2(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox2.Image = (new FilterImg()).ToContrastCorrection(Im);
                pictureBox3.Image = (new FilterImg()).ToAdaptiveSmoothing(Im);
                pictureBox4.Image = (new FilterImg()).ToAdditiveNoise(Im);
                pictureBox5.Image = (new FilterImg()).ToBilateralSmoothing(Im);
                pictureBox6.Image = (new FilterImg()).ToClosing(Im);
            }
        }

        private void Load_all_default_filter1(Object threadContext)
        {
            lock (threadContext)
            {
                Bitmap Im = new Bitmap((Bitmap)threadContext);
                pictureBox1.Image = Im;
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            f.img = (Bitmap)pictureBox_Filter.Image;
            mainPicture.Image = f.img;
            f.Saved = false;
            panelFilter.Hide();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            panelFilter.Hide();
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox26.Image;
        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox35.Image;
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox18.Image;
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox22.Image;
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox17.Image;
        }

        private void pictureBox30_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox30.Image;
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox19.Image;
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox25.Image;
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox24.Image;
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox20.Image;
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox23.Image;
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox16.Image;
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox28.Image;
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox21.Image;
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox14.Image;
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox27.Image;
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox13.Image;
        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox32.Image;
        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox36.Image;
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox12.Image;
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox15.Image;
        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox34.Image;
        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox33.Image;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox10.Image;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox9.Image;
        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox29.Image;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox11.Image;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox2.Image;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox6.Image;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox7.Image;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox8.Image;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox5.Image;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox4.Image;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox3.Image;
        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox31.Image;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = pictureBox1.Image;
        }

        #endregion

        #region ButtonDrawicon
        private void button1_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 6;
            icon = new System.Drawing.Icon("icon/1.ico");
        }

        private void btn_Icon_2_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 7;
            icon = new System.Drawing.Icon("icon/2.ico");
        }

        private void btn_Icon_3_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 8;
            icon = new System.Drawing.Icon("icon/3.ico");
        }

        private void btn_Icon_4_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 9;
            icon = new System.Drawing.Icon("icon/4.ico");
        }

        private void btn_Icon_5_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 10;
            icon = new System.Drawing.Icon("icon/5.ico");
        }

        private void btn_Icon_6_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 11;
            icon = new System.Drawing.Icon("icon/6.ico");
        }

        private void btn_Icon_7_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 12;
            icon = new System.Drawing.Icon("icon/7.ico");
        }

        private void btn_Icon_8_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 13;
            icon = new System.Drawing.Icon("icon/8.ico");
        }

        private void btn_Icon_9_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 14;
            icon = new System.Drawing.Icon("icon/9.ico");
        }

        private void btn_Icon_10_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 15;
            icon = new System.Drawing.Icon("icon/10.ico");
        }

        private void btn_Icon_11_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 16;
            icon = new System.Drawing.Icon("icon/11.ico");
        }

        private void btn_Icon_12_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 17;
            icon = new System.Drawing.Icon("icon/12.ico");
        }

        private void btn_Icon_13_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 18;
            icon = new System.Drawing.Icon("icon/13.ico");
        }

        private void btn_Icon_14_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 19;
            icon = new System.Drawing.Icon("icon/14.ico");
        }

        private void btn_Icon_15_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 20;
            icon = new System.Drawing.Icon("icon/15.ico");
        }

        private void btn_Icon_16_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 21;
            icon = new System.Drawing.Icon("icon/16.ico");
        }

        private void btn_Icon_17_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 22;
            icon = new System.Drawing.Icon("icon/17.ico");
        }

        private void btn_Icon_18_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 23;
            icon = new System.Drawing.Icon("icon/18.ico");
        }

        private void btn_Icon_19_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 24;
            icon = new System.Drawing.Icon("icon/19.ico");
        }

        private void btn_Icon_20_Click(object sender, EventArgs e)
        {
            ChooseTypeDraw = 25;
            icon = new System.Drawing.Icon("icon/20.ico");
        }

        #endregion

        #region Undo
        private void btnUndo_Click(object sender, EventArgs e)
        {
            indexUndoRedo--;
            UnRedo();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            indexUndoRedo++;
            UnRedo();

        }

        public void UnRedo()
        {
            if (indexUndoRedo >= 0 && indexUndoRedo < UndoRedo.Count)
            {
                pictureDraw.Image = UndoRedo[indexUndoRedo];
                mainPicture.Image = UndoRedo[indexUndoRedo];
                f.img = UndoRedo[indexUndoRedo];
                numberDraw = 0;
                bm = (Bitmap)UndoRedo[indexUndoRedo].Clone();
            }
            else
            {
                if (indexUndoRedo < 0)
                    indexUndoRedo = 0;
                if (indexUndoRedo >= UndoRedo.Count)
                    indexUndoRedo = UndoRedo.Count - 1;
            }
        }
        #endregion

        #region Scroll_trackbar
        private void trackBar_Contrast_Scroll(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = (new FilterImg()).ChangeValue(f.img, trackBar_Contrast.Value, trackBar_Bright.Value, trackBar_Red.Value, trackBar_Green.Value, trackBar_Blue.Value);
        }

        private void trackBar_Bright_Scroll(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = (new FilterImg()).ChangeValue(f.img, trackBar_Contrast.Value, trackBar_Bright.Value, trackBar_Red.Value, trackBar_Green.Value, trackBar_Blue.Value);
        }

        private void trackBar_Red_Scroll(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = (new FilterImg()).ChangeValue(f.img, trackBar_Contrast.Value, trackBar_Bright.Value, trackBar_Red.Value, trackBar_Green.Value, trackBar_Blue.Value);
        }


        private void trackBar_Green_Scroll(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = (new FilterImg()).ChangeValue(f.img, trackBar_Contrast.Value, trackBar_Bright.Value, trackBar_Red.Value, trackBar_Green.Value, trackBar_Blue.Value);
        }

        private void trackBar_Blue_Scroll(object sender, EventArgs e)
        {
            pictureBox_Filter.Image = (new FilterImg()).ChangeValue(f.img, trackBar_Contrast.Value, trackBar_Bright.Value, trackBar_Red.Value, trackBar_Green.Value, trackBar_Blue.Value);
        }
        #endregion

        #region Zoom
        private void btn_ZoomOut_Click(object sender, EventArgs e)
        {
            if (f.zoom > 1)
            {
                f.zoom = f.zoom - 0.5;
                pictureBox_Filter.Width = (int)(panel_ZoomIMG.Width * f.zoom);
                pictureBox_Filter.Height = (int)(panel_ZoomIMG.Height * f.zoom);
            }
        }

        private void btn_ZoomIn_Click(object sender, EventArgs e)
        {
            if (f.zoom < 4)
            {
                f.zoom = f.zoom + 0.5;
                pictureBox_Filter.Width = (int)(panel_ZoomIMG.Width * f.zoom);
                pictureBox_Filter.Height = (int)(panel_ZoomIMG.Height * f.zoom);
            }
        }
        #endregion

        #region Rotate
        private void CBXoay()
        {
            pictureBoxRotate.Image = f.img;
            panelRotate.Show();
            if (f.img.Height <= 623 && f.img.Width <= 744)
            {
                double a = 623.0 / f.img.Height;
                double b = 744.0 / f.img.Width;
                double hs;
                if (a < b) hs = a;
                else hs = b;
                pictureBoxRotate.Height = (int)hs * f.img.Height;
                pictureBoxRotate.Width = (int)hs * f.img.Width;
            }
            else
            {
                double a = f.img.Height / 623.0;
                double b = f.img.Width / 744.0;
                double hs;
                if (a > b) hs = a;
                else hs = b;
                pictureBoxRotate.Height = (int)(f.img.Height / hs);
                pictureBoxRotate.Width = (int)(f.img.Width / hs);
            }
            pictureBoxRotate.Location = new Point((760 - pictureBoxRotate.Width) / 2, (620 - pictureBoxRotate.Height) / 2);
        }

        private void trackBar_DoXoay_Scroll(object sender, EventArgs e)
        {
            txt_DoXoay.Text = trackBar_DoXoay.Value.ToString() + "°";
            f.DoXoay = trackBar_DoXoay.Value;
            Rotate();
        }

        private void txt_DoXoay_Click(object sender, EventArgs e)
        {
            txt_DoXoay.Text = f.DoXoay.ToString();
            txt_DoXoay.SelectAll();
        }

        private void txt_DoXoay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                try
                {
                    string s = txt_DoXoay.Text.Replace("°", "");
                    int dx = int.Parse(s);
                    if (dx <= 360 && dx >= 0)
                    {
                        f.DoXoay = dx;
                        Rotate();
                        trackBar_DoXoay.Value = dx;
                        txt_DoXoay.Text = txt_DoXoay.Text + "°";
                    }
                    else
                        txt_DoXoay.Text = f.DoXoay.ToString() + "°";
                }
                catch (Exception) { txt_DoXoay.Text = f.DoXoay.ToString() + "°"; }
        }

        private void btn_Rotate90_Click(object sender, EventArgs e)
        {
            f.DoXoay = (f.DoXoay + 90) % 360;
            txt_DoXoay.Text = f.DoXoay.ToString() + "°";
            trackBar_DoXoay.Value = f.DoXoay;
            Rotate();
        }

        private void Rotate()
        {
            RotateImage R_img = new RotateImage(f.img, f.DoXoay);
            pictureBoxRotate.Image = R_img.Rotate();
        }


        private void btn_Flip1_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBoxRotate.Image);
            bm.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pictureBoxRotate.Image = bm;
        }

        private void btn_Flip2_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBoxRotate.Image);
            bm.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBoxRotate.Image = bm;
        }

        private void btn_UnDone_Click(object sender, EventArgs e)
        {
            panelRotate.Hide();
        }

        private void btn_Done_Click(object sender, EventArgs e)
        {
            f.img = (Bitmap)pictureBoxRotate.Image;
            mainPicture.Image = f.img;
            f.Saved = false;
            panelRotate.Hide();
        }
        #endregion

        #region Update
        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update Uform = new Update();
            Uform.Show();
            this.Enabled = false;
            StreamReader inputf = new StreamReader("version.txt");
            string filenameof_version_on_host = Path.GetTempPath()+"version.txt";
            string version = inputf.ReadLine();
            string path_download_checkversion = inputf.ReadLine();
            inputf.Close();
            string newVersion = TakeVerOnServer(path_download_checkversion, filenameof_version_on_host);
            Uform.Close();
            DialogResult dlr;
            if (version == newVersion)
            {
                dlr = MessageBox.Show("This is the last version(" + version + ")", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dlr = MessageBox.Show("This version: " + version + Environment.NewLine + "The last version: " + newVersion
                    + Environment.NewLine + "Do you want update ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (dlr == DialogResult.Yes)
            {
                inputf = new StreamReader(filenameof_version_on_host);
                version = inputf.ReadLine();
                string path_download_setup = inputf.ReadLine();
                string filename = Path.GetTempPath() + "/"+inputf.ReadLine();
                wait w = new wait();
                w.Show();
                WebClient client = new WebClient();
                if (File.Exists(filename)) File.Delete(filename);
                client.DownloadFile(path_download_setup, filename);
                w.Close();
                try
                {
                    Process.Start(filename);
                    Application.Exit();
                }
                catch (Exception) { }
                File.Delete(filenameof_version_on_host);
                this.Enabled = true;
            }
            File.Delete(filenameof_version_on_host);
            this.Enabled = true;
        }

        private string TakeVerOnServer(string path, string fname)
        {
            string newVer = "";
            WebClient client = new WebClient();
            client.DownloadFile(path, fname);
            StreamReader inputf = new StreamReader(fname);
            newVer = inputf.ReadLine();
            inputf.Close();
            return newVer;
        }
        #endregion
    }
}