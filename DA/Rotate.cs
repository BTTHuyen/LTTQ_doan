using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ModifyImg
{
    class RotateImage
    {
        private Bitmap Bm;
        private int Dx;
        public RotateImage(Bitmap Img, int DoXoay)
        {
            Bm = new Bitmap(Img);
            Dx = DoXoay;
        }

        public Bitmap Rotate()
        {
            Bitmap rotatedBmp = new Bitmap(Bm.Width, Bm.Height);
            rotatedBmp.SetResolution(Bm.HorizontalResolution, Bm.VerticalResolution);
            Graphics g = Graphics.FromImage(rotatedBmp);
            PointF offset = new PointF((float)Bm.Width / 2, (float)Bm.Height / 2);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(Dx);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(Bm, new PointF(0, 0));
            return rotatedBmp;
        }
    }
}
