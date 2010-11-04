using System;

namespace ActionRPG
{
    //=======================================ENUMS==========================================
    public enum GameState 
    { 
        Loading, 
        Paused, 
        ActiveGame 
    };


    //========================================CLASSES=======================================
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
        public int PosX, PosY;
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
            centerPosX = PosX + cStaticVar.imgEnemy.Width / 2;
            centerPosY = PosY + cStaticVar.imgEnemy.Height / 2;
            bodyRange = (cStaticVar.imgEnemy.Height + cStaticVar.imgEnemy.Width) / 4;
            this.enemyType = enemyType;
            lives = true;
        }
    }

    public struct sKeyPressed
    {
        public bool sLeft;
        public bool sRight;
        public bool sUp;
        public bool sDown;

        public sKeyPressed(bool pL, bool pR, bool pU, bool pD)
        {
            sLeft = pL;
            sRight = pR;
            sUp = pU;
            sDown = pD;
        }
    }

}
