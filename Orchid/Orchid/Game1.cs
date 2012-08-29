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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Button button;
        Button button2;

        //message boxes. testArea's for testing.
        public MessageBox messageArea;
        public MessageBox testArea;

        //font
        public static SpriteFont defaultFont;
        public static SpriteFont boldFont; 
        public static SpriteFont italicFont; 

        //input
        InputHandler inputHandler;
        MouseState currentMouseState = Mouse.GetState();
        MouseState lastMouseState = Mouse.GetState();

        
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
            string[] temp = { "blank1", "blank2", "blank3", "blank4", "blank5", "blank6", "blank7", "blank8" };
            foreach (string item in temp)
            {
                msgList.Add(item);
            }

            //adds the first proper string  to the msgList
            msgList.Add("game init");


        }

        // TODO: fix this up so that it accepts a bounding box rectangle, 
        // and allow the function to draw to a given surface, because right now it
        // just reuses the save RT as it was given, blindly.
        public void TextFormatter()
        {

            //test html string
            // <b> for bold, <i> for italics,  <Color.a_color> ie <Color.Red> for red.
            // combine Color.a_color with either .bold or .italics for those tags
            // ie <Color.Red.bold> or <Color.Red.italic> . No need for both bold and italic yet.
            string html = @"unformatted text<b> Bolded text</b> regular text<color.Beige> italic text</color.Beige> normal text";

            //creates an HTMLDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);


            //all the font heights, into a list
            List<float> fontHeights = new List<float>();
            //height of the regular font
            float regularFontHeight = (float)defaultFont.MeasureString("ABCabc").Y;
            fontHeights.Add(regularFontHeight);
            //height of the bold font and the difference between this and regular
            float boldFontHeight = (float)boldFont.MeasureString("ABCabc").Y;
            fontHeights.Add(boldFontHeight);
            //height of the italic font and the difference between this and regular
            float italicFontHeight = (float)italicFont.MeasureString("ABCabc").Y;
            fontHeights.Add(italicFontHeight);

            //find the tallest fontsize, then make it so the smaller fonts get drawn 
            //a bit lower than that
            float largestFontHeight = fontHeights.Max();  
            
            float regularFontDifference = largestFontHeight - regularFontHeight;
            float boldDifference = largestFontHeight - boldFontHeight;
            float italicDifference = largestFontHeight - italicFontHeight;

            HtmlNodeCollection nodes = doc.DocumentNode.ChildNodes;

            //where to start the line printing
            Vector2 lineStart = new Vector2(200, 200);

            //testing starting point
            //spriteBatch.DrawString(defaultFont, "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT", lineStart, Color.Beige);
            
            //the starting position for the text
            Vector2 position = lineStart;
            //loop over all the nodes, and draw them in the right spot
            foreach (HtmlNode node in nodes)
            {
                //declare local variables
                SpriteFont font;
                float difference;
                Color fontColor = Color.Black;

                //check for formatting
                if (node.Name.StartsWith("color."))
                {
                    string textColor = node.Name.Split('.')[1];
                    //fontColor = Color.Red;
                    System.Drawing.Color tempColor = System.Drawing.Color.FromName(textColor);
                    fontColor = new Color(tempColor.R, tempColor.G, tempColor.B, tempColor.A);
                    //fontColor = (Color)typeof(Color).GetField(node.Name).GetValue(null);
                    Console.WriteLine(fontColor);
                }
                if (node.Name == "b" | node.Name.Contains(".bold"))
                {
                    font = boldFont;
                    difference = boldDifference;
                       
                }

                else if (node.Name == "i" | node.Name.Contains(".italic"))
                {
                    font = italicFont;
                    difference = italicDifference;
                }

                else
                {
                    font = defaultFont;
                    difference = regularFontDifference;
                }

                //adjust for height, but it doesn't work
                position.Y += difference ;

                spriteBatch.DrawString(font, node.InnerText, position, fontColor);
                //moves the position over to the end of current node's text
                position.X += defaultFont.MeasureString(node.InnerText).X;

                //spriteBatch.DrawString(defaultFont, "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT", new Vector2(position.X, 247), Color.Beige);
                //resets the height of drawn text.
                position.Y -= difference ;
            }


       

        }
        
        
        

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {


            //Where to spawn the XNA game screen
            //see here http://stackoverflow.com/questions/9726667/how-can-i-control-where-xna-positions-the-game-window-at-startup
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.
                FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(800, 100);
            //Where to spawn the Console
            //Console.SetWindowPosition(100, 0); //Doesnt work need to manually do with
            // with the console properties and defaults
            
            
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
            boldFont = Content.Load<SpriteFont>("BoldFont");
            italicFont = Content.Load<SpriteFont>("ItalicFont");


            //create a message area at the bottom of the scree, 1/4 of the screen.
            int areaH = height / 4;
            Rectangle size = new Rectangle(0, height - areaH, width, areaH);

            //test message area
            messageArea = new MessageBox(this, GraphicsDevice, spriteBatch, size,
                                Color.RoyalBlue, defaultFont, this.defaultBG, msgList);

            //test smallArea
            List<string> smallList = new List<string>(new string[] {"Small MsgBox"});
            Rectangle smallRect = new Rectangle(500,500, 125, 125);
            MessageBox smallBox = new MessageBox(this, GraphicsDevice, spriteBatch, smallRect, 
                Color.Red,
                defaultFont, this.defaultBG, smallList);

            //test testArea out a bit
            //fill a blank list to use inside testarea
            List<string> testList = new List<string>();
            string[] temp = { "blank1", "blank2", "blank3", "blank4", "blank5", "blank6", "blank7", "blank8" };
            foreach (string item in temp)
            {
                testList.Add(item);
            }

            testList.Clear();
            testList.Add("This is testBox");
            testArea = new MessageBox(this, GraphicsDevice, spriteBatch,
                            new Rectangle(100, 100, 255,255), Color.AliceBlue, 
                            defaultFont, this.defaultBG, testList);

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
            inputHandler.CheckMouseAgainstElements(Orchid.masterGuiElementList);
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
            Orchid.UpdateGUIMessageBoxes(Orchid.masterGuiElementList, gameTime);

            //reset the RenderTarget to the backbuffer
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(defaultBG);

            ///draw the rendertargets to the backbuffer
            spriteBatch.Begin();
            //draw the MessageBoxes.
            Orchid.DrawGUIMessageBoxes(Orchid.masterGuiElementList, gameTime);

            TextFormatter();
            //text formatting fooling

            //draw the MBs to the backbuffer, and draw that.
            spriteBatch.End();

            //draw the buttons after, so they're on top, as well as after End()
            //because otherwise the MSB would be draw on top, because Buttons don't need
            //to use Spritebatches here.
            Orchid.DrawGUIButtons(Orchid.masterGuiElementList, gameTime);


            base.Draw(gameTime);
        }


    }

  


  

    

    
}
