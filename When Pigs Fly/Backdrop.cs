using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Added System to access the random type
using System;

namespace When_Pigs_Fly
{
    class Backdrop
    {
        private Game1 m_main;

        //Variables for the backdrop position
        public Vector2 m_backdropPos = new Vector2(832, 335);
        public Vector2 m_backdropPos2 = new Vector2(832, 585);
        private int m_backdropOffsetX = 832;
        public int m_backdropStartPos = 2496;
        public int m_backdropStartPos2 = -832;
        public int m_backdropStartPos3 = -832;
        public int m_backdropStartPos4 = 1664;
        private float m_backdropFrontSpeed = 10f;
        private float m_backdropBackSpeed = 5f;

        //Variable for the terrain position
        public Vector2 m_terrainPos = new Vector2(832, 1036);
        public int m_terrainStartPos2 = -832;
        private int m_terrainOffsetX = 832;
        private int m_terrainStartPos = 2488;
        private float m_terrainSpeed = 8f;

        //Variables for the cloud position
        public Vector2 m_cloudPos = new Vector2(0, 250);
        public int m_cloudStartPos2 = 400;
        public int m_cloudStartPos3 = 800;
        public int m_cloudStartPos4 = 1200;
        public int m_cloudStartPos5 = 1600;
        private int m_cloudOffsetX = 150;
        public int m_cloudStartPos = 1814;
        private float m_cloudSpeed = 3f;

        //Setting up a random variable so we can randomise the clouds Y value, and booleans so we can prevent the randomisation from looping
        private Random m_rand = new Random();
        private int m_randCloudYPos;
        public bool m_isRandomised = false;
        public bool m_hasReset = false;

        public Backdrop()
        {

        }

        public Backdrop(Game1 main)
        {
            SetMainRef(main);
        }

        private void SetMainRef(Game1 main)
        {
            m_main = main;
        }

        //Method for moving the clouds
        public void CloudMove()
        {
            m_cloudPos.X -= m_cloudSpeed;

            //Checking what the game state is so the clouds will randomise different Y values accordingly
            if (m_main.GetGameState() == GameState.Menu || m_main.GetGameState() == GameState.Fail)
                m_randCloudYPos = m_rand.Next(75, 800);
            if (m_main.GetGameState() == GameState.Game)
                m_randCloudYPos = m_rand.Next(75, 250);

            //If isRandomised is false, the Y co-ordinate is randomised, then isRandomised is set to true to stop it looping
            if (m_isRandomised == false)
            {
                m_cloudPos.Y = m_randCloudYPos;
                m_isRandomised = true;
            }

            //If the cloud goes too far off the left side of the screen, resets the X position, and changes isRandomised to false so that the Y value can be
            //randomised again
            if (m_cloudPos.X <= -m_cloudOffsetX)
            {
                m_cloudPos.X = m_cloudStartPos;
                m_isRandomised = false;
            }
        }

        //Method for moving the terrain
        public void TerrainMove()
        {
            m_terrainPos.X -= m_terrainSpeed;
            if (m_terrainPos.X <= -m_terrainOffsetX)
                m_terrainPos.X = m_terrainStartPos;
        }

        //Method for moving the backdrop
        public void BackdropMove()
        {
            m_backdropPos.X -= m_backdropFrontSpeed;
            m_backdropPos2.X -= m_backdropBackSpeed;
            if (m_backdropPos.X <= -m_backdropOffsetX)
                m_backdropPos.X = m_backdropStartPos;
            if (m_backdropPos2.X <= -m_backdropOffsetX)
                m_backdropPos2.X = m_backdropStartPos;
        }

        //Method for resetting the clouds booleans which is used to determine what range their Y value should be randomised in
        public void CloudReset()
        {
            m_isRandomised = false;
            m_hasReset = false;
        }

        //Method for drawing the cloud
        public void DrawCloud(SpriteBatch _spriteBatch, Texture2D m_Cloud)
        {
            Vector2 cloudCenter = new Vector2(m_Cloud.Width / 2, m_Cloud.Height / 2);
            _spriteBatch.Draw(m_Cloud, m_cloudPos, null, Color.White, 0f, cloudCenter, 1f, SpriteEffects.None, 1);
        }

        //Method for drawing the terrain
        public void DrawTerrain(SpriteBatch _spriteBatch, Texture2D m_Terrain)
        {
            Vector2 terrainCenter = new Vector2(m_Terrain.Width / 2, m_Terrain.Height / 2);
            _spriteBatch.Draw(m_Terrain, m_terrainPos, null, Color.White, 0f, terrainCenter, 1f, SpriteEffects.None, 1);
        }

        //Method for drawing the Normal backdrop
        public void DrawBackdrop(SpriteBatch _spriteBatch, Texture2D m_Backdrop)
        {
            Vector2 backdropCenter = new Vector2(m_Backdrop.Width / 2, m_Backdrop.Height / 2);
            _spriteBatch.Draw(m_Backdrop, m_backdropPos, null, Color.White, 0f, backdropCenter, 0.7f, SpriteEffects.None, 1);
        }

        //Method for drawing the smaller backdrop in the background
        public void DrawSmallBackdrop(SpriteBatch _spriteBatch, Texture2D m_Backdrop)
        {
            Vector2 backdropCenter = new Vector2(m_Backdrop.Width / 2, m_Backdrop.Height / 2);
            _spriteBatch.Draw(m_Backdrop, m_backdropPos2, null, Color.White, 0f, backdropCenter, 0.3f, SpriteEffects.None, 1);
        }
    }
}
