using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.IO;
using System.Text;
using System.Net;


namespace Orchid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Button button;
        Button button2;

        public MessageArea messageArea;
        public MessageArea testArea;

        public static  SpriteFont defaultFont;

        //input
        InputHandler inputHandler;
        MouseState currentMouseState = Mouse.GetState();
        MouseState lastMouseState = Mouse.GetState();

        //list of gui elements
        public List<GuiElement> masterGuiElementList = new List<GuiElement>();
        
        //screensize
        public int width = 1024;
        public int height = 768;

        //colors
        Color defaultBG;

        //message list
        public List<string> msgList = new List<string>();
        //public MessageWriter writer;

        public Game1()
        {
            //set the stdout stream wrapper
            //writer = new MessageWriter(msgList);

            //set the default background color
            defaultBG = Color.RoyalBlue;

            //make the mouse visible
            this.IsMouseVisible = true;

            //create a graphicsdevice manage and set the default screen size
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;


            Content.RootDirectory = "Content";
            
            //create an input handler
            this.inputHandler = new InputHandler(this);

            string[] temp = { "blank1", "blank2", "blank3", "blank4", "blank5", "blank6", "blank7", "blank8" };
            foreach (string item in temp)
            {
                msgList.Add(item);
            }

            msgList.Add("game init");


        }



        //public int Qwe() { return 1; }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            
            //send console stdout to writer
            //Console.SetOut(writer);

            // TODO: Add your initialization logic here

            //create a button
            Rectangle qwe = new Rectangle(500, 0, 210, 110);
            this.button = new Button(qwe, "NEW BUTTON", this);
            //and another button
            this.button2 = new Button(new Rectangle(0, 0, 155, 122), "second", this);


            //message area buttons, up and down message scroll
            Button msgAreaScrlUp = new Button(new Rectangle(this.width - 100, this.height - 150,
                                                25, 25), "up", this);
            Button msgAreaScrlDown = new Button(new Rectangle(this.width - 100, this.height - 50,
                                                25, 25), "down", this);
            //binding the commands using lambdas.
            msgAreaScrlUp.command = () => messageArea.ScrollMessageArea(-5);
            msgAreaScrlDown.command = () => messageArea.ScrollMessageArea(5);
             

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load font
            defaultFont = Content.Load<SpriteFont>("DefaultFont");


            //create a message area at the bottom of the scree, 1/4 of the screen.
            int areaH = height / 4;
            Rectangle size = new Rectangle(0, height - areaH, width, areaH);

            //test message area
            messageArea = new MessageArea(GraphicsDevice, spriteBatch, size,
                                Color.RoyalBlue, defaultFont, this.defaultBG, msgList);
            List<string> testList = new List<string>();
            string[] temp = { "blank1", "blank2", "blank3", "blank4", "blank5", "blank6", "blank7", "blank8" };
            foreach (string item in temp)
            {
                testList.Add(item);
            }
            testArea = new MessageArea(GraphicsDevice, spriteBatch, new Rectangle(100, 100, 255,255), Color.AliceBlue, defaultFont, this.defaultBG, testList);

            // TODO: use this.Content to load your game content here


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            //test the input against all the elements of the gui
            inputHandler.CheckMouseAgainstElements(masterGuiElementList);
            inputHandler.HandleKeys(this);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Green);
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(defaultBG);
            // TODO: Add your drawing code here


            //draw the message area textures,
            //messageArea.Draw();
            messageArea.UpdateSurface();
            //draw the testArea
            //testArea.Draw();
            testArea.UpdateSurface();

            //reset the RenderTarget to the backbuffer
            GraphicsDevice.SetRenderTarget(null);


            //draw the rendertargets to the backbuffer
            spriteBatch.Begin();
            messageArea.Draw();
            testArea.Draw();
            spriteBatch.End();

            //GraphicsDevice.Clear(Color.Green);

            //draw the gui.
            //Orchid.DrawGUI(masterGuiElementList, gameTime);


            base.Draw(gameTime);
        }


    }

  


  

    

    
}
