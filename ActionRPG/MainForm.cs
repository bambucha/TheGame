using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ActionRPG
{
    public partial class MainForm : Form
    {
        public Brush brush1 = new SolidBrush(Color.Blue);
        public Pen pen1 = new Pen(Color.Black);
        public Image backgroundimage1 = Image.FromFile("data/background1.jpg");
        int PosX = -1500;
        int PosY = -1500; 
        static float angle = 0;
       
        
        sKeyPressed movKey = new sKeyPressed(false, false, false, false);

        Boolean MCHits = false;
        int moveOffset = 0;
        static int enemyNumber = 1;

        GameState gameState = GameState.ActiveGame;
        Enemy[] enemy = new Enemy[enemyNumber];
        MC mc = new MC();


        

        public MainForm()
        {
            InitializeComponent();
        }



        protected override void OnKeyDown(KeyEventArgs keyEvent)
        {
            if ((keyEvent.KeyCode == Keys.Left) && (!movKey.sLeft))
            {
                movKey.sLeft = true;
            }
            if ((keyEvent.KeyCode == Keys.Right) && (!movKey.sRight))
            {
                movKey.sRight = true;
            }
            if ((keyEvent.KeyCode == Keys.Up) && (!movKey.sUp))
            {
                movKey.sUp = true;
            }
            if ((keyEvent.KeyCode == Keys.Down) && (!movKey.sDown))
            {
                movKey.sDown = true;
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
                movKey.sLeft = false;
            }
            if (keyEvent.KeyCode == Keys.Right)
            {
                movKey.sRight = false;
            }
            if (keyEvent.KeyCode == Keys.Up)
            {
                movKey.sUp = false;
            }
            if (keyEvent.KeyCode == Keys.Down)
            {
                movKey.sDown = false;
            }
        }


        int hitRange = 70;

        public void MovementModificators()
        {

            if (movKey.sUp)
            {
                moveOffset = 30;
            }
            else if (movKey.sDown)
            {
                moveOffset = -10;
            }

            if (movKey.sLeft)
            {
                angle -= 12.00F;
            }
            else if (movKey.sRight)
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
                        enemy[i].charges = true;
                    }

                    if (distanceToEnemy > enemy[i].ChaseDistance)
                    {
                        enlargement -= 10;
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

                
                if (distanceY != 0)
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
                    g.DrawImage(cStaticDef.mRotateImage(cStaticVar.imgEnemy, enemyangle), enemy[i].PosX + PosX - enlargement / 2, enemy[i].PosY + PosY - enlargement / 2, cStaticVar.imgEnemy.Width + enlargement, cStaticVar.imgEnemy.Height + enlargement);
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
                        g.DrawImage(cStaticDef.mRotateImage(cStaticVar.imgCharacter, angle), (AnimationWindow.Width / 2) - (cStaticVar.imgCharacter.Width / 2), (AnimationWindow.Height / 2) - (cStaticVar.imgCharacter.Height / 2));

                        
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