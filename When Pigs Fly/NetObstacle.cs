using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Added System to access the random type
using System;

namespace When_Pigs_Fly
{
    public class NetObstacle
{
        //Variables for handling the net size, speed, and position
        private Vector2 m_netSize = new Vector2(150, 600);
        private int m_topNetStartXPos = 1764;
        private int m_bottomNetStartXPos = 1764;
        private Vector2 m_topNetCurrentPos;
        private Vector2 m_bottomNetCurrentPos;
        private float m_netSpeed = 8f;

        //Variables for handling the colliders
        private Vector2 m_colliderOffset = new Vector2(75, 300);
        private int m_scoreColliderOffsetX = 100;
        private Rectangle m_topNetCollider;
        private Rectangle m_bottomNetCollider;
        private Rectangle m_scoreCollider;
        private bool m_hasCollided = false;

        //Setting up a random variable so we can randomise the nets Y value
        private Random m_rand = new Random();       

        //Method for tracking the position of the top and bottom net, and it's relevant colliders
        public NetObstacle()
        {
            //Random is used here to randomise the y coordinates so that the nets can be different heights on each pass
            int randomTopNetPos = m_rand.Next(0, 75);
            int randomBottomNetPos = m_rand.Next(936, 1001);
            m_topNetCurrentPos.X = m_topNetStartXPos;
            m_topNetCurrentPos.Y = randomTopNetPos;
            m_bottomNetCurrentPos.X = m_bottomNetStartXPos;
            m_bottomNetCurrentPos.Y = randomBottomNetPos;

            //Setting the colliders to match their respective counterparts
            m_topNetCollider = new Rectangle((int)m_topNetCurrentPos.X, (int)m_topNetCurrentPos.Y, (int)m_netSize.X, (int)m_netSize.Y);
            m_bottomNetCollider = new Rectangle((int)m_bottomNetCurrentPos.X, (int)m_bottomNetCurrentPos.Y, (int)m_netSize.X, (int)m_netSize.Y);
            m_scoreCollider = new Rectangle((int)m_topNetCurrentPos.X, 0, 1, 936);
        }

        public Vector2 GetTopNetPos()
        {
            return m_topNetCurrentPos;
        }

        public Vector2 GetBottomNetPos()
        {
            return m_bottomNetCurrentPos;
        }

        public Rectangle GetScoreCollider()
        {
            return m_scoreCollider;
        }

        public Rectangle GetTopNetCollider()
        {
            return m_topNetCollider;
        }

        public Rectangle GetBottomNetCollider()
        {
            return m_bottomNetCollider;
        }

        public bool HasCollided()
        {
            return m_hasCollided;
        }

        public void SetCollided(bool hasCollided)
        {
            m_hasCollided = hasCollided;
        }

        public void NetMove()
        {
            //Using random again, because the first use was for setting the initial Y position
            int randomTopNetPos = m_rand.Next(0, 75);
            int randomBottomNetPos = m_rand.Next(936, 1001);

            //Moving the nets X position by how fast we want them to move
            m_topNetCurrentPos.X -= m_netSpeed;
            m_bottomNetCurrentPos.X -= m_netSpeed;

            //If the net goes off the left side of the screen, it's X position is reset
            if (m_topNetCurrentPos.X < -100 && m_bottomNetCurrentPos.X < -100)
            {
                m_topNetCurrentPos.X = m_topNetStartXPos;
                m_topNetCurrentPos.Y = randomTopNetPos;
                m_bottomNetCurrentPos.X = m_bottomNetStartXPos;
                m_bottomNetCurrentPos.Y = randomBottomNetPos;
                m_hasCollided = false;
            }

            //Setting the colliders to match the nets, also using offsets because they don't have origin points
            m_scoreCollider.X = (int)m_topNetCurrentPos.X + m_scoreColliderOffsetX;
            m_topNetCollider.X = (int)m_topNetCurrentPos.X - (int)m_colliderOffset.X;
            m_topNetCollider.Y = (int)m_topNetCurrentPos.Y - (int)m_colliderOffset.Y;
            m_bottomNetCollider.X = (int)m_bottomNetCurrentPos.X - (int)m_colliderOffset.X;
            m_bottomNetCollider.Y = (int)m_bottomNetCurrentPos.Y - (int)m_colliderOffset.Y;
        }

        //Method for resetting the variables so that the game can be replayed without restarting the application
        public void Reset()
        {
            m_topNetCurrentPos.X = m_topNetStartXPos;
            m_bottomNetCurrentPos.X = m_bottomNetStartXPos;
            m_scoreCollider.X = m_topNetStartXPos;
            m_topNetCollider.X = m_topNetStartXPos;
            m_bottomNetCollider.X = m_topNetStartXPos;
            m_hasCollided = false;
        }

        //Method for drawing the nets which is called in the Draw method of the Game1 class
        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, Texture2D m_Net)
        {
            Vector2 netCenter = new Vector2(m_Net.Width / 2, m_Net.Height / 2);
            _spriteBatch.Draw(m_Net, m_topNetCurrentPos, null, Color.White, 0f, netCenter, 1.5f, SpriteEffects.None, 1);
            _spriteBatch.Draw(m_Net, m_bottomNetCurrentPos, null, Color.White, 0f, netCenter, 1.5f, SpriteEffects.None, 1);
        }
    }
}
