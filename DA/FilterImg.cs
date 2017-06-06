using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging.Filters;

namespace ModifyImg
{
    class FilterImg
    {

        public Bitmap ToSepia(Bitmap Im)
        {
            Sepia Img = new Sepia();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToYCbCrExtractChannel(Bitmap Im)
        {
            AForge.Imaging.Filters.YCbCrExtractChannel Img = new YCbCrExtractChannel();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToWaterWave(Bitmap Im)
        {
            AForge.Imaging.Filters.WaterWave Img = new WaterWave();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToSimplePosterization(Bitmap Im)
        {
            AForge.Imaging.Filters.SimplePosterization Img = new SimplePosterization();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToSharpen(Bitmap Im)
        {
            AForge.Imaging.Filters.Sharpen Img = new Sharpen();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToSaturationCorrection(Bitmap Im)
        {
            AForge.Imaging.Filters.SaturationCorrection Img = new SaturationCorrection();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToPixellate(Bitmap Im)
        {
            AForge.Imaging.Filters.Pixellate Img = new Pixellate();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToSaltAndPepperNoise(Bitmap Im)
        {
            AForge.Imaging.Filters.SaltAndPepperNoise Img = new SaltAndPepperNoise();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToRotateChannels(Bitmap Im)
        {
            AForge.Imaging.Filters.RotateChannels Img = new RotateChannels();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToOpening(Bitmap Im)
        {
            AForge.Imaging.Filters.Opening Img = new Opening();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToJitter(Bitmap Im)
        {
            AForge.Imaging.Filters.Jitter Img = new Jitter();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToOilPainting(Bitmap Im)
        {
            AForge.Imaging.Filters.OilPainting Img = new OilPainting();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToMedian(Bitmap Im)
        {
            AForge.Imaging.Filters.Median Img = new Median();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToInvert(Bitmap Im)
        {
            AForge.Imaging.Filters.Invert Img = new Invert();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToMean(Bitmap Im)
        {
            AForge.Imaging.Filters.Mean Img = new Mean();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToHueModifier(Bitmap Im)
        {
            AForge.Imaging.Filters.HueModifier Img = new HueModifier();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToHistogramEqualization(Bitmap Im)
        {
            AForge.Imaging.Filters.HistogramEqualization Img = new HistogramEqualization();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToHSLFiltering(Bitmap Im)
        {
            AForge.Imaging.Filters.HSLFiltering Img = new HSLFiltering();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToGrayscaleRMY(Bitmap Im)
        {
            AForge.Imaging.Filters.GrayscaleRMY Img = new GrayscaleRMY();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToFlatFieldCorrection(Bitmap Im)
        {
            AForge.Imaging.Filters.FlatFieldCorrection Img = new FlatFieldCorrection();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToGaussianBlur(Bitmap Im)
        {
            AForge.Imaging.Filters.GaussianBlur Img = new GaussianBlur();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToGammaCorrection(Bitmap Im)
        {
            AForge.Imaging.Filters.GammaCorrection Img = new GammaCorrection();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToExtractNormalizedRGBChannel(Bitmap Im)
        {
            AForge.Imaging.Filters.ExtractNormalizedRGBChannel Img = new ExtractNormalizedRGBChannel();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToExtractChannel(Bitmap Im)
        {
            AForge.Imaging.Filters.ExtractChannel Img = new ExtractChannel();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToExtractBiggestBlob(Bitmap Im)
        {
            AForge.Imaging.Filters.ExtractBiggestBlob Img = new ExtractBiggestBlob();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToDilatation(Bitmap Im)
        {
            AForge.Imaging.Filters.Dilatation Img = new Dilatation();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToEuclideanColorFiltering(Bitmap Im)
        {
            AForge.Imaging.Filters.EuclideanColorFiltering Img = new EuclideanColorFiltering();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToErosion(Bitmap Im)
        {
            AForge.Imaging.Filters.Erosion Img = new Erosion();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToContrastCorrection(Bitmap Im)
        {
            AForge.Imaging.Filters.ContrastCorrection Img = new ContrastCorrection();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToClosing(Bitmap Im)
        {
            AForge.Imaging.Filters.Closing Img = new Closing();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToBrightnessCorrection(Bitmap Im)
        {
            AForge.Imaging.Filters.BrightnessCorrection Img = new BrightnessCorrection();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToBlur(Bitmap Im)
        {
            AForge.Imaging.Filters.Blur Img = new Blur();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToBilateralSmoothing(Bitmap Im)
        {
            AForge.Imaging.Filters.BilateralSmoothing Img = new BilateralSmoothing();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToAdditiveNoise(Bitmap Im)
        {
            AForge.Imaging.Filters.AdditiveNoise Img = new AdditiveNoise();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToAdaptiveSmoothing(Bitmap Im)
        {
            AForge.Imaging.Filters.AdaptiveSmoothing Img = new AdaptiveSmoothing();
           Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ToGrayScale(Bitmap Im)
        {
            AForge.Imaging.Filters.GrayscaleY Img = new GrayscaleY();
            Bitmap bmImage = AForge.Imaging.Image.Clone(new Bitmap(Im), PixelFormat.Format24bppRgb);
            return Img.Apply(bmImage);
        }

        public Bitmap ChangeValue(Bitmap Im, int Value_Con, int Value_Brig, int Value_R, int Value_G, int Value_B)
        {
            float s = 1;
            float c = (float)Value_Con / 50;
            float b = (float)Value_Brig / 255;
            float red = (float)Value_R / 100;
            float green = (float)Value_G / 100;
            float blue = (float)Value_B / 100;
            float sr = (1 - s) * (float)0.3086;
            float sg = (1 - s) * (float)0.6094;
            float sb = (1 - s) * (float)0.082;
            float t = (1 - c) / 2;
            Bitmap NewBitmap = new Bitmap(Im.Width, Im.Height);
            Graphics NewGraphic = Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix = new float[][]{
                        new float[] {c*(sr+s)+red, c*sr, c*sr, 0, 0},
                        new float[] {c*sg, c*(sg+s)+green, c*sg, 0, 0},
                        new float[] {c*sb, c*sb, c*(sb+s)+blue, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {t+b, t+b, t+b, 0, 1}
                        };
            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphic.DrawImage(Im, new Rectangle(0, 0, Im.Width, Im.Height), 0, 0, Im.Width, Im.Height, GraphicsUnit.Pixel, Attributes);
            return NewBitmap;
        }


    }
}
