using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace ActionRPG
{
    public partial class Form1 : Form
    {
        public Brush brush1 = new SolidBrush(Color.Blue);
        public Pen pen1 = new Pen(Color.Black);
        public static Image testimage = Image.FromFile("data/testimage.png");
        public static Image testimage2 = Image.FromFile("data/mc.png");
        public Image backgroundimage1 = Image.FromFile("data/background1.jpg");
        int PosX = -1500;
        int PosY = -1500; 
        static float angle = 0;
        Boolean keyLeftpressed = false;
        Boolean keyRightpressed = false;
        Boolean keyUppressed = false;
        Boolean keyDownpressed = false;
        Boolean MCHits = false;
        int moveOffset = 0;
        static int enemyNumber = 1;


        public enum GameState { Loading, Paused, ActiveGame };
        GameState gameState = GameState.ActiveGame;
        Enemy[] enemy = new Enemy[enemyNumber];
        MC mc = new MC();


        public class MC
        {
            public int PosX;
            public int PosY;
            public MC()
            {
                
            }
        }

        public class Enemy
        {
            public int PosX,PosY;
            public int centerPosX, centerPosY;
            public int LineOfSight = 250;
            public int ChaseDistance = 350;
            public int enemyType = 0;
            public int bodyRange;
            public int movementSpeed = 15; //the lower the faster
            public Boolean EnemySpotted = false;
            public Boolean lives = false;
            public Boolean charges = false;

            public Enemy(int PosX, int PosY, int enemyType)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                centerPosX = PosX + testimage.Width /2;
                centerPosY = PosY + testimage.Height /2;
                bodyRange = (testimage.Height + testimage.Width) / 4;
                this.enemyType = enemyType;
                lives = true;
            }
            
        }

        public Form1()
        {
            InitializeComponent();
        }

        private Image RotateImage(Image inputImg, double degreeAngle)
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
               // g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
               // g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //Transformation matrix
                Matrix m = new Matrix();
                m.RotateAt((float)degreeAngle, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f));
                //m.Translate(-bounds.Left, -bounds.Top, MatrixOrder.Append); //изместване за да компенсира ротацията

                g.Transform = m;
                g.DrawImage(inputImg, 0, 0);
            }
            return (Image)rotatedBitmap;
        }

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

        protected override void OnKeyDown(KeyEventArgs keyEvent)
        {
            if ((keyEvent.KeyCode == Keys.Left) && (!keyLeftpressed))
            {
                keyLeftpressed = true;
            }
            if ((keyEvent.KeyCode == Keys.Right) && (!keyRightpressed))
            {
                keyRightpressed = true;
            }
            if ((keyEvent.KeyCode == Keys.Up) && (!keyUppressed))
            {
                keyUppressed = true;
            }
            if ((keyEvent.KeyCode == Keys.Down) && (!keyDownpressed))
            {
                keyDownpressed = true;
            }
            if (keyEvent.KeyCode == Keys.A)
            {
                MCHits = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs keyEvent)
        {
            if (keyEvent.KeyCode == Keys.Left)
            {
                keyLeftpressed = false;
            }

            if (keyEvent.KeyCode == Keys.Right)
            {
                keyRightpressed = false;
            }
            if (keyEvent.KeyCode == Keys.Up)
            {
                keyUppressed = false;
            }
            if (keyEvent.KeyCode == Keys.Down)
            {
                keyDownpressed = false;
            }
        }


        int hitRange = 70;

        public void MovementModificators()
        {

            if (keyUppressed)
            {
                moveOffset = 30;
            }
            else if (keyDownpressed)
            {
                moveOffset = -10;
            }

            if (keyLeftpressed)
            {
                angle -= 12.00F;
            }
            else if (keyRightpressed)
            {
                angle += 12.00F;
            }

            if (moveOffset != 0)
            {
                int x = 0;
                int y = 0;

                x = (int)(Math.Sin(PointMath.DegreeToRadian(angle)) * moveOffset);
                y = (int)(Math.Cos(PointMath.DegreeToRadian(angle)) * moveOffset);
                moveOffset = 0;

                PosX -= x;
                PosY += y;
                mc.PosX += x;
                mc.PosY -= y;

            }
        }

        int distanceX, distanceY, distanceToEnemy, distanceToHit;
        double enemyangle;


        public void enemyHandler(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < enemyNumber; i++)
            {
                int enlargement = 0;

                distanceX = mc.PosX - enemy[i].centerPosX;
                distanceY = mc.PosY - enemy[i].centerPosY;
                distanceToEnemy = (int) (Math.Sqrt(distanceX * distanceX + distanceY * distanceY));

                if (enemy[i].EnemySpotted == false)
                {

                    if (distanceToEnemy < enemy[i].LineOfSight)
                    {
                        enlargement += 10;
                        //enemy[i].EnemySpotted = true;
                        enemy[i].charges = true;
                    }

                    if (distanceToEnemy > enemy[i].ChaseDistance)
                    {
                        enlargement -= 10;
                        //enemy[i].EnemySpotted = true;
                        enemy[i].charges = false;
                    }


                }

                if (enemy[i].charges)
                {
                    if (distanceToEnemy > 60)
                    {
                        enemy[i].centerPosX += distanceX / enemy[i].movementSpeed;
                        enemy[i].PosX += distanceX / enemy[i].movementSpeed;

                        enemy[i].centerPosY += distanceY / enemy[i].movementSpeed;
                        enemy[i].PosY += distanceY / enemy[i].movementSpeed;
                    }

                }

                
                if (distanceY == 0)
                {
                    //enemyangle = 0;
                }
                 
                else
                {
                    enemyangle = Math.Atan2(distanceY, distanceX);
                    enemyangle = PointMath.RadianToDegree(enemyangle) + 90;
                }


                if ((MCHits) && (distanceToEnemy < (hitRange + enemy[i].bodyRange)))
                {
                    int distanceToHitX, distanceToHitY;
                    int hitPointX, hitPointY;

                    hitPointX = mc.PosX - (int)((Math.Sin(PointMath.DegreeToRadian(-angle))) * hitRange);
                    hitPointY = mc.PosY - (int)((Math.Cos(PointMath.DegreeToRadian(-angle))) * hitRange);
                    distanceToHitX = hitPointX - enemy[i].centerPosX;
                    distanceToHitY = hitPointY - enemy[i].centerPosY;
                    distanceToHit = (int)(Math.Sqrt(distanceToHitX * distanceToHitX + distanceToHitY * distanceToHitY));
                    if (distanceToHit < enemy[i].bodyRange)
                    {
                        enemy[i].lives = false;
                    }
                }

                if (enemy[i].lives)
                {
                    g.DrawImage(RotateImage(testimage, enemyangle), enemy[i].PosX + PosX - enlargement / 2, enemy[i].PosY + PosY - enlargement / 2, testimage.Width + enlargement, testimage.Height + enlargement);
                    //g.DrawImage(RotateImage(testimage, enemyangle), enemy[i].PosX + PosX - enlargement / 2, enemy[i].PosY + PosY - enlargement / 2);
                }
                Font font2 = new Font("Courier New", 15, FontStyle.Bold);
                g.DrawString("enemyangle      " + enemyangle.ToString(), font2, brush1, 0, 20);
            }
        }

        private void AnimationWindow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
             
            switch (gameState)
            {
                case GameState.ActiveGame:
                    {
                        g.DrawImage(backgroundimage1, PosX, PosY);
                        Font font2 = new Font("Courier New", 15, FontStyle.Bold);
                        g.DrawString("MC X,Y          " + mc.PosX.ToString() + " , " + mc.PosY.ToString(), font2, brush1, 0, 0);
                        g.DrawString("angle           " + angle.ToString(), font2, brush1, 0, 80);
                       
                        g.DrawString("distanceToHit   " + distanceToHit.ToString(), font2, brush1, 0, 40);
                        

                        
                        MovementModificators();

                        enemyHandler(e);
                        if (MCHits)
                        {
                            g.FillEllipse(brush1, (AnimationWindow.Width / 2) - (int)(Math.Sin(PointMath.DegreeToRadian(-angle)) * hitRange), (AnimationWindow.Height / 2) - (int)(Math.Cos(PointMath.DegreeToRadian(-angle)) * hitRange), 10, 10);
                            MCHits = false;
                        }
                        g.DrawImage(RotateImage(testimage2, angle), (AnimationWindow.Width / 2) - (testimage2.Width / 2), (AnimationWindow.Height / 2) - (testimage2.Height / 2));

                        
                        break;
                    }
                case GameState.Paused:
                    {
                        break;
                    }
                case GameState.Loading:
                    {
                        break;
                    }
                default:
                    {
                        gameState = GameState.ActiveGame;
                        break;
                    }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AnimationWindow.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            for (int i = 0; i < enemyNumber; i++)
            {
                enemy[i] = new Enemy(+ 1500 + 500 * i,  + 1500 + 500 * i, 1);
            }

            mc.PosX = (AnimationWindow.Width / 2) - PosX;
            mc.PosY = (AnimationWindow.Height / 2) - PosY;
        }
    }
}