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
using HtmlAgilityPack;



namespace Orchid
{

    public delegate void CommandHandler();

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        Button button;
        Button button2;

        //gamestates
        public  GameState playingGameState = new GameState("default GameState");
        public  GameState typingGameState = new GameState("typing GameState");
        public  GameState currentGameState ;

        //message boxes. testArea's for testing.
        public MessageBox messageArea;
        public MessageBox testArea;
        public Surface resizingSurface;

        //font
        public static SpriteFont defaultFont;
        public static SpriteFont boldFont; 
        public static SpriteFont italicFont; 

        //input
        public InputHandler inputHandler;
        MouseState currentMouseState = Mouse.GetState();
        MouseState lastMouseState = Mouse.GetState();

        //screensize
        public int width = 1280;
        public int height = 1024;

        //colors
        Color defaultBG;

        //message list
        public List<string> msgList = new List<string>();
        //public MessageWriter writer;

        public Game1()
        {
            //frame limit. change the last number to the frames per second
            int limit = 1000 / 60;
            TargetElapsedTime = TimeSpan.FromMilliseconds(limit);

            //set the default background color
            defaultBG = Color.RoyalBlue;

            //make the mouse visible
            this.IsMouseVisible = true;

            //create a graphicsdevice manage and set the default screen size
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;

            //this better be a default XNA thing, because IDK what its for, beyond the obvios
            Content.RootDirectory = "Content";
            
            //create an input handler
            this.inputHandler = new InputHandler(this);

            //a temp string list to make sure shit doesnt crash
            string[] temp = { @"<b>this is my united states of whatever </b>", @"<b>blank2</b>", @"<b>blank3</b>", "blank4", "blank5", "blank6", "blank7", "blank8" };
            foreach (string item in temp)
            {
                msgList.Add(item);
            }

            //adds the first proper string  to the msgList
            msgList.Add("game init");


        }

        /// <summary>
        /// A wrapper for loading images into the game
        /// </summary>
        /// <param name="content_name">The name for the content you want to load into the game</param>
        /// 
        /// <returns>returns a Surface, sized to Size rect, ready to use.</returns>
        public Surface createImageSurface(string content_name, Rectangle size_arg= new Rectangle() )
        {
            Rectangle size2;

            if (size_arg == new Rectangle())
            {
                size2 = new Rectangle(300, 300, 500, 200);
            }

            else { 
                size2 = size_arg;
            }

            resizingSurface = new Surface(this, GraphicsDevice,
                spriteBatch, size2, Color.White, Color.OliveDrab);
            Texture2D imgTexture = Content.Load<Texture2D>(content_name);
            //Changed to use a filepath, rather than a Content name
            //Texture2D imgTexture = Texture2D.FromStream(GraphicsDevice,  File.OpenRead(content_name));
            resizingSurface.image = imgTexture;

            return resizingSurface;

        }
        

        /// <summary>
        /// Allows the game to perform any initialization it needs to before
        /// starting to run.  This is where it can query for any required
        /// services and load any non-graphic related content.  Calling
        /// base.Initialize will enumerate through any components and initialize
        /// them as well.
        /// </summary>
        protected override void Initialize()
        {

            //set window title.
            Window.Title = "Orchid GUI Test";

            //Where to spawn the XNA game screen
            //see here http://stackoverflow.com/questions/9726667/how-can-i-control-where-xna-positions-the-game-window-at-startup
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.
                FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(630, 15);
            //Where to spawn the Console
            //Console.SetWindowPosition(100, 0); //Doesnt work need to manually do with
            // with the console properties and defaults
            
            
            // TODO: Add your initialization logic here
            this.currentGameState = this.playingGameState;

            Components.Add(new FrameRateCounter(this, new Vector2(25, 25), Color.White, Color.Black));


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
            boldFont = Content.Load<SpriteFont>("BoldFont");
            italicFont = Content.Load<SpriteFont>("ItalicFont");

            //set some Orchid variables up
            Orchid.spriteBatch = spriteBatch;
            Orchid.graphicsDevice = GraphicsDevice;
            Orchid.game = this;



            //create a message area at the bottom of the scree, 1/4 of the screen.
            int areaH = height / 4;
            Rectangle size = new Rectangle(0, height - areaH, width, areaH);
            messageArea = new MessageBox(this, GraphicsDevice, spriteBatch, size, Color.Green, msgList, Color.Black, false);
           
            ////create a button
            //Rectangle qwe = new Rectangle(500, 0, 210, 110);
            //this.button = new Button(qwe, "NEW BUTTON", this);
            ////and another button
            //this.button2 = new Button(new Rectangle(0, 0, 155, 122), "second", this);


            //message area buttons, up and down message scroll
            Button msgAreaScrlUp = new Button(new Rectangle(this.width - 100, this.height - 150,
                                                25, 25), "up", this, command: null, spriteBatch: spriteBatch);
            Button msgAreaScrlDown = new Button(new Rectangle(this.width - 100, this.height - 50,
                                                25, 25), "down", this, command: null, spriteBatch: spriteBatch);
            //binding the commands using lambdas.
            msgAreaScrlUp.command = () => messageArea.ScrollMessageArea(-5);
            msgAreaScrlDown.command = () => messageArea.ScrollMessageArea(5);

            ////test smallArea
            //List<string> smallList = new List<string>(new string[] {"Small MsgBox"});
            //Rectangle smallRect = new Rectangle(500,500, 125, 125);
            //MessageBox smallBox = new MessageBox(this, GraphicsDevice, spriteBatch, smallRect, Color.Red, smallList);

            //test testArea out a bit
            //fill a blank list to use inside testarea
            //List<string> testList = new List<string>();
            //string[] temp = { "blank1", "blank2", "blank3", "blank4", "blank5", "blank6", "blank7", "blank8" };
            //foreach (string item in temp)
            //{
            //    testList.Add(item);
            //}

            //testList.Clear();
            //testList.Add("This is testBox");
            //testArea = new MessageBox(this, GraphicsDevice, spriteBatch, 
            //    new Rectangle(100, 100, 255, 255), Color.Gold, testList,
            //    textColor: Color.Black);

            //Rectangle textRect = new Rectangle(400, 200, 100, 25);
            //TextEntry textEntry = new TextEntry(this, GraphicsDevice, spriteBatch,
            //    textRect, Color.DarkBlue, new List<string>(new string[]{"asd"}), Color.Red);

            //new surface, for resizing
            //Rectangle size2 = new Rectangle(300, 300, 200, 200);
            //createImageSurface("smiley", size2);
            ////createImageSurface(@"C:\Users\Mark\Documents\Visual Studio 2010\Projects\Orchid\Orchid\OrchidContent\smiley.png", size2); 

            //Rectangle size3 = new Rectangle(100, 100, 200, 200);
            //createImageSurface("inverted", size3);
            ////createImageSurface(@"C:\Users\Mark\Documents\Visual Studio 2010\Projects\Orchid\Orchid\OrchidContent\inverted.png", size3); 

            ////Tooltip testing. Won't show now, I think because it's not attached to anything.
            //Rectangle tool_size = new Rectangle(100, 100, 275, 50);
            //new Tooltip(this, GraphicsDevice, spriteBatch, tool_size, Color.White,
            //    new List<string>(new string[] { "Tooltip testing" }));

            //menu testing

            Rectangle cmd_size = new Rectangle(300, 400, 275, 50);
            //CommandHandler cmd = new CommandHandler(() => Console.WriteLine("Clicked the menu"));


            //CommandHandler cmd =
            //    new CommandHandler( () => Orchid.CreateMenu(cmd_size, 
            //                        new List<string>(new string[] {"Menu 2"}),
            //                        command:() => Exit(),
            //                        parent: null, colorBG: null));
            Rectangle menu_size = new Rectangle(100, 100, 275, 50);
            // Orchid.CreateMenu(menu_size, new List<string>(new string[] { "Menu 1" }),null, null, null);

            MenuContainer cont = new MenuContainer(null, this, GraphicsDevice,
                    spriteBatch, menu_size, colorBG:Color.Red);



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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                                                ButtonState.Pressed)
                this.Exit();
            //draw the message area textures,
            Orchid.UpdateGUI(Orchid.masterGuiElementList, gameTime);

            //resizingSurface.Update();


            //test the input against all the elements of the gui
            inputHandler.CheckMouseAgainstElements(Orchid.masterGuiElementList);
            //inputHandler.CheckMouseAgainstElements(new List<GuiElement>{(GuiElement)this.resizingSurface});
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

            // TODO: Add your drawing code here

            //reset the RenderTarget to the backbuffer
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(defaultBG);

            ///draw the rendertargets to the backbuffer
            spriteBatch.Begin();
            Orchid.DrawGUI(Orchid.masterGuiElementList, gameTime);


            spriteBatch.End();



            base.Draw(gameTime);
        }


    }

  


  

    

    
}
