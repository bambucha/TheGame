using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ActionRPG
{
    //========================================STATIC VARIABLES=============================================
    public static class cStaticVar
    {
        public static Image imgEnemy = Image.FromFile("data/testimage.png");
        public static Image imgCharacter = Image.FromFile("data/mc.png");
    }

    //==========================================STATIC METODS====================================================
    public static class cStaticDef
    {
        public static Image mRotateImage(Image inputImg, double degreeAngle)
        {
            //ъглите на image
            PointF[] rotationPoints = { new PointF(0, 0),
                                        new PointF(inputImg.Width, 0),
                                        new PointF(0, inputImg.Height),
                                        new PointF(inputImg.Width, inputImg.Height)};

            //роатция на ъглите
            PointMath.RotatePoints(rotationPoints, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f), degreeAngle);

            //Получаваме новите стойности след ротацията на ъглите

            Rectangle bounds = PointMath.GetBounds(rotationPoints);

            //Празен bitmap за получения завъртян образ
            Bitmap rotatedBitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                //Transformation matrix
                Matrix m = new Matrix();
                m.RotateAt((float)degreeAngle, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f));

                g.Transform = m;
                g.DrawImage(inputImg, 0, 0);
            }
            return (Image)rotatedBitmap;
        }
    }

    //=========================================STATIC CLASSES================================================
    public static class PointMath
    {
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * 180.0 / Math.PI;
        }

        public static PointF RotatePoint(PointF pnt, double degreeAngle)
        {
            return RotatePoint(pnt, new PointF(0, 0), degreeAngle);
        }

        public static PointF RotatePoint(PointF pnt, PointF origin, double degreeAngle)
        {
            double radAngle = DegreeToRadian(degreeAngle);

            PointF newPoint = new PointF();

            double deltaX = pnt.X - origin.X;
            double deltaY = pnt.Y - origin.Y;

            newPoint.X = (float)(origin.X + (Math.Cos(radAngle) * deltaX - Math.Sin(radAngle) * deltaY));
            newPoint.Y = (float)(origin.Y + (Math.Sin(radAngle) * deltaX + Math.Cos(radAngle) * deltaY));

            return newPoint;
        }

        public static void RotatePoints(PointF[] pnts, double degreeAngle)
        {
            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] = RotatePoint(pnts[i], degreeAngle);
            }
        }

        public static void RotatePoints(PointF[] pnts, PointF origin, double degreeAngle)
        {
            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] = RotatePoint(pnts[i], origin, degreeAngle);
            }
        }

        public static Rectangle GetBounds(PointF[] pnts)
        {
            RectangleF boundsF = GetBoundsF(pnts);
            return new Rectangle((int)Math.Round(boundsF.Left),
                                (int)Math.Round(boundsF.Top),
                                (int)Math.Round(boundsF.Width),
                                (int)Math.Round(boundsF.Height));
        }

        public static RectangleF GetBoundsF(PointF[] pnts)
        {
            float left = pnts[0].X;
            float right = pnts[0].X;
            float top = pnts[0].Y;
            float bottom = pnts[0].Y;

            for (int i = 1; i < pnts.Length; i++)
            {
                if (pnts[i].X < left)
                    left = pnts[i].X;
                else if (pnts[i].X > right)
                    right = pnts[i].X;

                if (pnts[i].Y < top)
                    top = pnts[i].Y;
                else if (pnts[i].Y > bottom)
                    bottom = pnts[i].Y;
            }

            return new RectangleF(left,
                                top,
                                (float)Math.Abs(right - left),
                                (float)Math.Abs(bottom - top));
        }
    }
}
