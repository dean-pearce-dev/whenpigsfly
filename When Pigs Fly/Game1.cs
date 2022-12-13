using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Added Microsoft.Xna.Framework.Media and Microsoft.Xna.Framework.Audio so we can use sound files
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace When_Pigs_Fly
{
    public enum GameState { Menu, Game, Fail }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Window Size
        private const int windowW = 1664;
        private const int windowH = 936;

        //Texture Variables
        private Texture2D m_pig;
        private Texture2D m_pigFlap;
        private Texture2D m_net;
        private Texture2D m_cloud;
        private Texture2D m_backdrop;
        private Texture2D m_terrain;
        private Texture2D m_startScreen;
        private Texture2D m_failScreen;
        private Texture2D m_howToPlay;
        private Texture2D m_startPrompt;
        private Texture2D m_howToPrompt;
        private Texture2D m_toMenuPrompt;
        private Texture2D m_tryAgainPrompt;
        private Texture2D m_exitPrompt;
        private SpriteFont m_font;

        //Variables for playing sound on score increase, and player jump
        private SoundEffect m_ding;
        private SoundEffect m_click;
        private SoundEffect m_flap;
        private Song m_backgroundMusic;

        //Variables for NetObstacle instances
        private NetObstacle m_netObst1;
        private NetObstacle m_netObst2;
        private NetObstacle m_netObst3;

        //Variable for the PigPlayer instance
        private PigPlayer m_player;

        //Variables for Cloud instances from Backdrop Class
        private Backdrop m_cloud1;
        private Backdrop m_cloud2;
        private Backdrop m_cloud3;
        private Backdrop m_cloud4;
        private Backdrop m_cloud5;

        //Variables for Terrain instances from Backdrop Class
        private Backdrop m_terrain1;
        private Backdrop m_terrain2;

        //Variables for Building instances from Backdrop Class
        private Backdrop m_backdrop1;
        private Backdrop m_backdrop2;
        private Backdrop m_backdrop3;
        private Backdrop m_backdrop4;

        //Bool for checking if the backdrop has been set to correct positions
        private bool m_cloudPosSet = false;

        //Bools for activating multiple nets when they're required instead of instantly
        private bool m_net2IsActive = false;
        private bool m_net3IsActive = false;
        private int m_netTrigger = 555;

        //Variables for tracking the score
        private int m_scoreCount = 0;
        private Vector2 m_scorePos = new Vector2(50, 50);
        private Vector2 m_youScoredPos = new Vector2(832, 375);
        private string m_youScoredString;

        //Variables for making the prompts pulse
        private float m_promptScale = 0.5f;
        private float m_promptScaleMin = 0.4f;
        private float m_promptScaleMax = 0.5f;
        private bool m_promptPulse = true;
        private float m_promptScaleSpeed = 0.005f;

        //Variables for menu prompt positions
        private Vector2 m_startPromptPos = new Vector2(832, 800);
        private Vector2 m_howToPromptPos = new Vector2(832, 350);
        private Vector2 m_toMenuPromptPos = new Vector2(832, 860);
        private Vector2 m_tryAgainPromptPos = new Vector2(400, 800);
        private Vector2 m_exitPromptPos = new Vector2(1250, 800);

        //Variable for images that need to use the entire screen
        private Vector2 m_fullScreenImgPos = new Vector2(0, 0);
        private bool m_howToScreen = false;

        //Enum for cycling through game states
        private GameState m_gameState;

        private KeyboardState m_keyPrevState;
        private KeyboardState m_keyCurrentState;

        private VolumeControl m_volumeControl;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = windowW;
            _graphics.PreferredBackBufferHeight = windowH;
            _graphics.ApplyChanges();

            //Setting initial value for the game state enum
            m_gameState = GameState.Menu;

            m_volumeControl = new VolumeControl(this);

            //Setting volume of the media player and setting it to loop
            MediaPlayer.Volume = m_volumeControl.GetMusicVolume();
            MediaPlayer.IsRepeating = true;

            base.Initialize();

            CreateClouds();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            m_pig = Content.Load<Texture2D>("Sprites/pig");
            m_pigFlap = Content.Load<Texture2D>("Sprites/pigflap");
            m_net = Content.Load<Texture2D>("Sprites/net");
            m_cloud = Content.Load<Texture2D>("Sprites/cloud");
            m_terrain = Content.Load<Texture2D>("Sprites/terrain");
            m_backdrop = Content.Load<Texture2D>("Sprites/backdrop");
            m_startPrompt = Content.Load<Texture2D>("Sprites/startprompt");
            m_howToPrompt = Content.Load<Texture2D>("Sprites/howtoprompt");
            m_toMenuPrompt = Content.Load<Texture2D>("Sprites/tomenuprompt");
            m_tryAgainPrompt = Content.Load<Texture2D>("Sprites/tryagainprompt");
            m_exitPrompt = Content.Load<Texture2D>("Sprites/exitprompt");
            m_startScreen = Content.Load<Texture2D>("Screens/menu");
            m_failScreen = Content.Load<Texture2D>("Screens/fail");
            m_howToPlay = Content.Load<Texture2D>("Screens/howto");
            m_ding = Content.Load<SoundEffect>("Sounds/ding");
            m_flap = Content.Load<SoundEffect>("Sounds/flap");
            m_click = Content.Load<SoundEffect>("Sounds/click");
            m_backgroundMusic = Content.Load<Song>("Sounds/bensound-happyrock");
            m_font = Content.Load<SpriteFont>("font");
            MediaPlayer.Play(m_backgroundMusic);
        }

        //Creating instances of the NetObstacle class to use as our nets
        private void CreateNets()
        {
            m_netObst1 = new NetObstacle();
            m_netObst2 = new NetObstacle();
            m_netObst3 = new NetObstacle();
        }

        //Creating an instance of the player
        private void CreatePlayer()
        {
            m_player = new PigPlayer(this);
        }

        private void CreateClouds()
        {
            m_cloud1 = new Backdrop(this);
            m_cloud2 = new Backdrop(this);
            m_cloud3 = new Backdrop(this);
            m_cloud4 = new Backdrop(this);
            m_cloud5 = new Backdrop(this);
        }

        private void CreateTerrain()
        {
            m_terrain1 = new Backdrop(this);
            m_terrain2 = new Backdrop(this);
        }

        private void CreateBackdrop()
        {
            m_backdrop1 = new Backdrop(this);
            m_backdrop2 = new Backdrop(this);
            m_backdrop3 = new Backdrop(this);
            m_backdrop4 = new Backdrop(this);
        }

        private void SetCloudPos()
        {
            m_cloud2.m_cloudPos.X = m_cloud2.m_cloudStartPos2;
            m_cloud3.m_cloudPos.X = m_cloud2.m_cloudStartPos3;
            m_cloud4.m_cloudPos.X = m_cloud2.m_cloudStartPos4;
            m_cloud5.m_cloudPos.X = m_cloud2.m_cloudStartPos5;
            m_cloudPosSet = true;
        }

        private void SetTerrainPos()
        {
            m_terrain2.m_terrainPos.X = m_terrain2.m_terrainStartPos2;
        }

        private void SetBackdropPos()
        {
            m_backdrop2.m_backdropPos.X = m_backdrop2.m_backdropStartPos2;
            m_backdrop3.m_backdropPos.X = m_backdrop3.m_backdropStartPos3;
            m_backdrop4.m_backdropPos2.X = m_backdrop4.m_backdropStartPos4;
        }

        public void PlayFlapSFX()
        {
            m_flap.Play();
        }

        public GameState GetGameState()
        {
            return m_gameState;
        }

        public KeyboardState GetCurrentKeyboardState()
        {
            return m_keyCurrentState;
        }

        public KeyboardState GetPrevKeyboardState()
        {
            return m_keyPrevState;
        }

        private void StartGame()
        {
            m_gameState = GameState.Game;
            m_cloud1.CloudReset();
            m_cloud2.CloudReset();
            m_cloud3.CloudReset();
            m_cloud4.CloudReset();
            m_cloud5.CloudReset();

            CreateTerrain();
            CreateBackdrop();
            SetTerrainPos();
            SetBackdropPos();

            CreatePlayer();

            CreateNets();
        }

        protected override void Update(GameTime gameTime)
        {            
            m_keyCurrentState = Keyboard.GetState();
            m_volumeControl.VolumeMenu();            

            //Setting the score string so we can measure the length later and display it in the center of the screen
            m_youScoredString = "You scored " + m_scoreCount + "!";

            if (m_cloudPosSet == false)
                SetCloudPos();


            //Moving the clouds
            m_cloud1.CloudMove();
            m_cloud2.CloudMove();
            m_cloud3.CloudMove();
            m_cloud4.CloudMove();
            m_cloud5.CloudMove();

            //Code to run if game state is set to menu
            if (m_gameState == GameState.Menu)
            {
                //Press Space to start game
                if (m_howToScreen == false && m_keyCurrentState.IsKeyDown(Keys.Space))
                {
                    m_click.Play();
                    StartGame();
                }
                else if (m_howToScreen == false && m_keyCurrentState.IsKeyDown(Keys.E))
                {
                    m_click.Play();
                    m_howToScreen = true;
                }
                else if (m_howToScreen == true && m_keyCurrentState.IsKeyDown(Keys.Enter))
                {
                    m_click.Play();
                    m_howToScreen = false;
                }
            }

            //Code to run if game state is set to game
            else if (m_gameState == GameState.Game)
            {
                m_backdrop1.BackdropMove();
                m_backdrop2.BackdropMove();
                m_backdrop3.BackdropMove();
                m_backdrop4.BackdropMove();
                m_terrain1.TerrainMove();
                m_terrain2.TerrainMove();

                m_player.PlayerJump(gameTime);

                //Calling the NetMove function for each instance of the NetObstacle class, starts moving m_Net1 straight away, then moves the next one
                //once the previous net has reached a specific point
                m_netObst1.NetMove();
                if (m_netObst1.GetTopNetPos().X <= m_netTrigger || m_net2IsActive == true)
                {
                    m_netObst2.NetMove();
                    m_net2IsActive = true;
                }
                if (m_netObst2.GetTopNetPos().X <= m_netTrigger || m_net3IsActive == true)
                {
                    m_netObst3.NetMove();
                    m_net3IsActive = true;
                }

                //Checking if the player has passed a collision detection for the nets, adds points to the score, and plays a sound
                if (m_player.GetCollider().Intersects(m_netObst1.GetScoreCollider()) && m_netObst1.HasCollided() == false)
                {
                    m_ding.Play();
                    m_scoreCount++;
                    m_netObst1.SetCollided(true);
                }
                if (m_player.GetCollider().Intersects(m_netObst2.GetScoreCollider()) && m_netObst2.HasCollided() == false)
                {
                    m_ding.Play();
                    m_scoreCount++;
                    m_netObst2.SetCollided(true);
                }
                if (m_player.GetCollider().Intersects(m_netObst3.GetScoreCollider()) && m_netObst3.HasCollided() == false)
                {
                    m_ding.Play();
                    m_scoreCount++;
                    m_netObst3.SetCollided(true);
                }

                //Checking if the player has collided with the nets or window boundaries and runs the fail state if so
                if (m_player.GetCollider().Intersects(m_netObst1.GetTopNetCollider()) || m_player.GetCollider().Intersects(m_netObst1.GetBottomNetCollider()))
                    m_gameState = GameState.Fail;
                if (m_player.GetCollider().Intersects(m_netObst2.GetTopNetCollider()) || m_player.GetCollider().Intersects(m_netObst2.GetBottomNetCollider()))
                    m_gameState = GameState.Fail;
                if (m_player.GetCollider().Intersects(m_netObst3.GetTopNetCollider()) || m_player.GetCollider().Intersects(m_netObst3.GetBottomNetCollider()))
                    m_gameState = GameState.Fail;
                if (m_player.GetCollider().Y >= windowH || m_player.GetCollider().Y <= 0)
                    m_gameState = GameState.Fail;
            }

            //Code to run if the game state is set to fail
            else if (m_gameState == GameState.Fail)
            {
                //Resetting values for the clouds to assist in randomising their Y values depending on the game state
                if (m_cloud1.m_hasReset == false || m_cloud2.m_hasReset == false || m_cloud3.m_hasReset == false || m_cloud4.m_hasReset == false || m_cloud5.m_hasReset == false)
                {
                    m_cloud1.m_isRandomised = false;
                    m_cloud1.m_hasReset = true;
                    m_cloud2.m_isRandomised = false;
                    m_cloud2.m_hasReset = true;
                    m_cloud3.m_isRandomised = false;
                    m_cloud3.m_hasReset = true;
                    m_cloud4.m_isRandomised = false;
                    m_cloud4.m_hasReset = true;
                    m_cloud5.m_isRandomised = false;
                    m_cloud5.m_hasReset = true;

                }
                //Resets the game values if 'E' is pressed, so that the game can begin again without having to be restarted
                if (m_keyCurrentState.IsKeyDown(Keys.E))
                {
                    m_click.Play();
                    m_gameState = GameState.Game;
                    m_net2IsActive = false;
                    m_net3IsActive = false;
                    m_player.Reset();
                    m_netObst1.Reset();
                    m_netObst2.Reset();
                    m_netObst3.Reset();
                    m_scoreCount = 0;
                    m_cloud1.CloudReset();
                    m_cloud2.CloudReset();
                    m_cloud3.CloudReset();
                    m_cloud4.CloudReset();
                    m_cloud5.CloudReset();
                }
            }

            //Making the prompts increase and decrease in size to give them a pulsing effect
            if (m_gameState == GameState.Menu || m_gameState == GameState.Fail)
            {
                if (m_promptPulse == true)
                    m_promptScale -= m_promptScaleSpeed;
                if (m_promptPulse == false)
                    m_promptScale += m_promptScaleSpeed;
                if (m_promptScale <= m_promptScaleMin || m_promptScale >= m_promptScaleMax)
                    m_promptPulse = !m_promptPulse;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || m_keyCurrentState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            m_keyPrevState = m_keyCurrentState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            _spriteBatch.Begin();
            m_cloud1.DrawCloud(_spriteBatch, m_cloud);
            m_cloud2.DrawCloud(_spriteBatch, m_cloud);
            m_cloud3.DrawCloud(_spriteBatch, m_cloud);
            m_cloud4.DrawCloud(_spriteBatch, m_cloud);
            m_cloud5.DrawCloud(_spriteBatch, m_cloud);

            //Drawing the start screen and start prompt if the game is in the menu state
            Vector2 startPromptCenter = new Vector2(m_startPrompt.Width / 2, m_startPrompt.Height / 2);
            Vector2 howToPromptCenter = new Vector2(m_howToPrompt.Width / 2, m_howToPrompt.Height / 2);
            Vector2 toMenuPromptCenter = new Vector2(m_toMenuPrompt.Width / 2, m_toMenuPrompt.Height / 2);
            Vector2 tryAgainPromptCenter = new Vector2(m_tryAgainPrompt.Width / 2, m_tryAgainPrompt.Height / 2);
            Vector2 exitPromptCenter = new Vector2(m_exitPrompt.Width / 2, m_exitPrompt.Height / 2);
            if (m_gameState == GameState.Menu)
            {
                if (m_howToScreen == false)
                {
                    _spriteBatch.Draw(m_startScreen, m_fullScreenImgPos, Color.White);
                    _spriteBatch.Draw(m_startPrompt, m_startPromptPos, null, Color.White, 0f, startPromptCenter, m_promptScale, SpriteEffects.None, 1);
                    _spriteBatch.Draw(m_howToPrompt, m_howToPromptPos, null, Color.White, 0f, howToPromptCenter, m_promptScale, SpriteEffects.None, 1);
                }
                if (m_howToScreen == true)
                {
                    _spriteBatch.Draw(m_howToPlay, m_fullScreenImgPos, Color.White);
                    _spriteBatch.Draw(m_toMenuPrompt, m_toMenuPromptPos, null, Color.White, 0f, toMenuPromptCenter, m_promptScale, SpriteEffects.None, 1);
                }
            }

            //If the game is in the game state, then draws the backdrop objects, the player, net objects, and a string which displays the score
            if (m_gameState == GameState.Game)
            {
                m_backdrop4.DrawSmallBackdrop(_spriteBatch, m_backdrop);
                m_backdrop2.DrawSmallBackdrop(_spriteBatch, m_backdrop);
                m_backdrop1.DrawBackdrop(_spriteBatch, m_backdrop);
                m_backdrop3.DrawBackdrop(_spriteBatch, m_backdrop);
                m_terrain1.DrawTerrain(_spriteBatch, m_terrain);
                m_terrain2.DrawTerrain(_spriteBatch, m_terrain);
                m_player.Draw(_spriteBatch, m_pig, m_pigFlap);
                m_netObst1.Draw(_spriteBatch, gameTime, m_net);
                m_netObst2.Draw(_spriteBatch, gameTime, m_net);
                m_netObst3.Draw(_spriteBatch, gameTime, m_net);
                _spriteBatch.DrawString(m_font, "Score: " + m_scoreCount, m_scorePos, Color.Black);
            }

            //Draws the fail screen and the score that was achieved if the game state is set to fail
            if (m_gameState == GameState.Fail)
            {
                //Setting variables to center the string for displaying the score achieved after a fail state
                Vector2 scoredStringSize = m_font.MeasureString(m_youScoredString);
                Vector2 youScoredCenter = new Vector2(scoredStringSize.X/2, scoredStringSize.Y/2);

                _spriteBatch.Draw(m_failScreen, m_fullScreenImgPos, Color.White);
                _spriteBatch.Draw(m_tryAgainPrompt, m_tryAgainPromptPos, null, Color.White, 0f, tryAgainPromptCenter, m_promptScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(m_exitPrompt, m_exitPromptPos, null, Color.White, 0f, exitPromptCenter, m_promptScale, SpriteEffects.None, 1);
                _spriteBatch.DrawString(m_font, m_youScoredString, m_youScoredPos - youScoredCenter, Color.Black);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
