using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Added Microsoft.Xna.Framework.Media so we can use sound files
using Microsoft.Xna.Framework.Media;

namespace When_Pigs_Fly
{
    public class PigPlayer
    {
        private Game1 m_main;

        //Variables for the pig position and size
        private Vector2 m_pigStartPos = new Vector2(200, 500);
        private Vector2 m_pigCurrentPos = new Vector2(200, 500);
        private Vector2 m_pigSize = new Vector2(100, 73);

        //Variables for handling the collider
        private Rectangle m_pigCollider;
        private Vector2 m_colliderOffset = new Vector2(45, 25);

        //Variables for handling the falling and jumping
        private float m_fallSpeed = 4.0f;
        private float m_jumpSpeed = 20.0f;
        private float m_fallDelay = 0.0f;
        private float m_fallDelayLimit = 25.0f;
        private bool m_isFalling = true;

        public PigPlayer()
        {
            //Defines positions for the pig collision, and makes it slightly smaller, so that detection is a little more forgiving on the player
            m_pigCollider = new Rectangle((int)m_pigCurrentPos.X, (int)m_pigCurrentPos.Y, (int)m_pigSize.X - 15, (int)m_pigSize.Y - 15);
        }

        public PigPlayer(Game1 main)
        {
            SetMainRef(main);

            //Defines positions for the pig collision, and makes it slightly smaller, so that detection is a little more forgiving on the player
            m_pigCollider = new Rectangle((int)m_pigCurrentPos.X, (int)m_pigCurrentPos.Y, (int)m_pigSize.X - 15, (int)m_pigSize.Y - 15);
        }

        private void SetMainRef(Game1 main)
        {
            m_main = main;
        }

        public Rectangle GetCollider()
        {
            return m_pigCollider;
        }

        public void PlayerJump(GameTime gameTime)
        {
            //Sets the pig collider position to match it's image position and offsets it's value due to the collider not having an origin point
            m_pigCollider.X = (int)m_pigCurrentPos.X - (int)m_colliderOffset.X;
            m_pigCollider.Y = (int)m_pigCurrentPos.Y - (int)m_colliderOffset.Y;

            //https://gamedev.stackexchange.com/questions/12903/execute-code-at-specific-intervals-only-once
            m_fallDelay += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Automatically make the pig sprite fall
            m_pigCurrentPos.Y += m_fallSpeed;

            //If space is pressed, sets fallSpeed to 0 to stop the pig sprite falling, and sets isFalling to false, so it can be used as a conditional
            if (m_main.GetPrevKeyboardState().IsKeyUp(Keys.Space) && m_main.GetCurrentKeyboardState().IsKeyDown(Keys.Space) && m_isFalling == true)
            {
                m_fallSpeed = 0f;
                m_isFalling = false;
                m_main.PlayFlapSFX();
            }

            //Whilst isFalling is false, uses jumpSpeed to move the pig sprite up and slowly decreases jumpSpeed until it reaches zero
            if (m_isFalling == false && m_jumpSpeed > 0)
            {
                m_pigCurrentPos.Y -= m_jumpSpeed;
                m_jumpSpeed -= 1f;

                //Once jumpSpeed reaches zero, resets it to initial value, then sets isFalling to true so that the pig sprite can begin falling again
                if (m_jumpSpeed <= 0)
                {
                    m_jumpSpeed = 20f;
                    m_isFalling = true;
                }
            }

            //Checks if isFalling is true, and if fallSpeed is less than 4, and if so, slowly increases the fallSpeed until it reaches 4
            //A delay is used to make this happen at a slower rate
            if (m_isFalling == true && m_fallSpeed < 4 && m_fallDelay > m_fallDelayLimit)
            {
                m_fallSpeed += 0.1f;
                m_fallDelay -= m_fallDelayLimit;
            }
        }

        //Method for resetting the variables so that the game can be replayed without restarting the application
        public void Reset()
        {
            m_pigCurrentPos = m_pigStartPos;
            m_fallSpeed = 4f;
            m_jumpSpeed = 20f;
            m_fallDelay = 0;
            m_isFalling = true;
        }

        //Method for drawing the pig which is called in the Draw method of the Game1 class
        public void Draw(SpriteBatch _spriteBatch, Texture2D m_Pig, Texture2D m_PigFlap)
        {
            Vector2 pigCenter = new Vector2(m_Pig.Width / 2, m_Pig.Height / 2);
            if(m_isFalling == false)
            _spriteBatch.Draw(m_PigFlap, m_pigCurrentPos, null, Color.White, 0f, pigCenter, 1f, SpriteEffects.None, 1);
            else
            _spriteBatch.Draw(m_Pig, m_pigCurrentPos, null, Color.White, 0f, pigCenter, 1f, SpriteEffects.None, 1);

        }
    }
}
