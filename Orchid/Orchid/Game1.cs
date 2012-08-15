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

        public static  SpriteFont defaultFont;

        //input
        InputHandler inputHandler;
        MouseState currentMouseState = Mouse.GetState();
        MouseState lastMouseState = Mouse.GetState();

        //list of gui elements
        public List<GuiElement> masterGuiElementList = new List<GuiElement>();


        public Game1()
        {



            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.inputHandler = new InputHandler(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Rectangle qwe = new Rectangle(500, 0, 210, 110);
            this.button = new Button(qwe, "NEW BUTTON", this);
            this.button.Initialize();

            button2 = new Button(new Rectangle(0, 0, 155, 122), "second", this );
            button2.Initialize();


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


            inputHandler.CheckInput(masterGuiElementList);

            // TODO: Add your update logic here
            //button.borderRectangle.Location = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            //button.BuildInnerRect();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.button.Draw(gameTime);
            this.button2.Draw(gameTime);

            
            base.Draw(gameTime);
        }
    }
        public class InputHandler
        {


            //the main Game instance
            Game1 theGame;

            //GuiElements cache
            GuiElement activeElement;
            GuiElement emptyElement ;
            GuiElement hoveredElement;

            //MouseStates cache
            MouseState currentMouseState = Mouse.GetState();
            MouseState lastMouseState = Mouse.GetState();

            public  InputHandler(Game1 game)
            {
                this.theGame = game;

                //create empty element
                emptyElement = new GuiElement(this.theGame);
                hoveredElement = new GuiElement(this.theGame);
            }
            


            public void CheckInput(List<GuiElement> guiElementList)
            {
                Point mousePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                currentMouseState = Mouse.GetState();

                //if mouse1 is pressed
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    foreach (GuiElement elem in guiElementList)
                    {
                        if (elem is Button)
                        {

                            Button elem2 = elem as Button;
                            //elem = elem as Button;
                            if (elem2.borderRectangle.Contains(mousePos))
                            {
                                activeElement = elem;
                                elem2.OnMouseDown();

                            }
                        }
                    }
                }

                //if mouse1 is clicked
                if (currentMouseState.LeftButton == ButtonState.Released &&
                       lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    //Console.WriteLine("trying for a mouse up");
                    activeElement.OnMouseUp();

                    activeElement = emptyElement;

                    
                }

                //if the mouse is just chilling, hovering
                if (currentMouseState.LeftButton == ButtonState.Released &&
                    currentMouseState.RightButton == ButtonState.Released)
                {

                    if (!hoveredElement.borderRectangle.Contains(mousePos))
                    {
                        hoveredElement.OffMouseHover();
                    }

                    foreach (GuiElement elem in guiElementList)
                    {
                        if (elem.borderRectangle.Contains(mousePos))
                        {
                            elem.OnMouseHover();
                            hoveredElement = elem;
                        }
                    }
                }



                 lastMouseState = currentMouseState;
            }
        }
  

    

    
}
