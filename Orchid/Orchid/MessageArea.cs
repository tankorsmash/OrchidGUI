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


    public class Surface : GuiElement
    {
        protected GraphicsDevice graphicsDevice;
        protected SpriteBatch spriteBatch;

        //base color is the color without alpha
        public Color baseColor;
        public Color backgroundColor;

        public RenderTarget2D surface;
        //public Rectangle rect;

        //fontsizes
        public float regularFontHeight;
        public float boldFontHeight;
        public float italicFontHeight;
        public float largestFontHeight;

        public float regularFontDifference;
        public float boldDifference;
        public float italicDifference;

        //alpha percentage. 1.0 = Alpha 255
        private float _alpha = 1F;
        public float alpha
        {
            get { return _alpha ; }
            set {
                if (value < 0)
                {
                    _alpha = 0;
                }
                else { _alpha = value; }
            }
        }
        //public float alpha = 1f;

        public Surface(Game1 game, GraphicsDevice graphicsDevice, 
            SpriteBatch spriteBatch, Rectangle rect, Color colorBG) :base(game)
        {

            this.baseColor = colorBG;

            this.CalculateBackgroundColor();



            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.surface = new RenderTarget2D(graphicsDevice, rect.Width, rect.Height);
            this.rect = rect;
            Console.WriteLine("msgbox x {0}, y {1}, w {2} h {3} ", this.rect.X,
                    this.rect.Y, this.rect.Width, this.rect.Height);
        }

        public void CalculateBackgroundColor()
        {
            //set the color for the Surface.
            int R = (int)(this.baseColor.R * this.alpha);
            int G = (int)(this.baseColor.G * this.alpha);
            int B = (int)(this.baseColor.B * this.alpha);

            this.backgroundColor = new Color(R, G, B);

        }


        public void FadeOut(int ticks)
        {
            this.alpha = this.alpha - .1f;
            Console.WriteLine("This is the alpha channel: {0}\n\tand _alpha: {1}", this.alpha, this._alpha);
        }

        public virtual void Update()
        {
            this.FadeOut(1);
            this.CalculateBackgroundColor();
        }

        public virtual  void UpdateSurface()
        {

            //change the renderTarger (pygame surface)
            graphicsDevice.SetRenderTarget(surface);   
            //clear it, like normal  
            graphicsDevice.Clear(Color.Thistle);   
             
            //make some SB draws
            spriteBatch.Begin(); 

            spriteBatch.End(); 

            //then reset the drawing surface to null, backbuffer.
            graphicsDevice.SetRenderTarget(null);

            //graphicsDevice.Clear(Color.CornflowerBlue);

            //return surface;

        }

        public virtual void Draw()
        { 
            //spriteBatch.Begin();


            //this.UpdateSurface();

            spriteBatch.Draw(this.surface, this.rect, Color.White);
            //spriteBatch.End();
        }

        // TODO:
        // allow the function to draw to a given surface, because right now it
        // just reuses the save RT as it was given, blindly.
        public float TextFormatter(String html, Rectangle textAreaSize)
        {

            ///test html string
            /// <b> for bold, <i> for italics,  <Color.a_color> ie <Color.Red> for red.
            /// combine Color.a_color with either .bold or .italics for those tags
            /// ie <Color.Red.bold> or <Color.Red.italic> . No need for both bold and italic yet.
            //string html = @"this is a real mother fucking paragraph yo, I don't even give a <Color.Green.bold>fuck</Color.Green.bold><Color.Turquoise> how long you thought</Color.Turquoise>  the last nice was, this thing is getting <i>real motherfucker</i>. I am <b>angry</b>, but also using a lot of words, so maybe I'm not as <Color.Red>angry</Color.Red> as you though eh? You're an asshole.";

            //creates an HTMLDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.ChildNodes;

            //where to start the line printing
            Vector2 lineStart = new Vector2(textAreaSize.X, textAreaSize.Y);

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
                string nodeText = node.InnerText;


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
                    font = Game1.boldFont;
                    difference = boldDifference;

                }

                else if (node.Name == "i" | node.Name.Contains(".italic"))
                {
                    font = Game1.italicFont;
                    difference = italicDifference;
                }

                else
                {
                    font = Game1.defaultFont;
                    difference = regularFontDifference;
                }

                //adjust for height, but it doesn't work
                position.Y += difference;

                //loop over all the words in nodeText, split on spaces and 
                // then make sure that their width isnt too long, instead of an entire lines
                // width

                foreach (string word in nodeText.Split(' '))
                {
                    String word_with_space_appended = String.Format("{0} ", word);
                    //split into new line if the next set of text is too wide
                    //position.x is where the 'cursor' is, linestart.x is the leftmost side of the text
                    // box. 
                    if ((position.X - lineStart.X) + font.MeasureString(word_with_space_appended).X >= textAreaSize.Width)
                    {
                        //if the string is too wide, go down a line,
                        position.Y += largestFontHeight;
                        //reset the cursor to the leftmost postion
                        position.X = lineStart.X;
                    }

                    //draw the word_with_space_appended
                    spriteBatch.DrawString(font, word_with_space_appended, position, fontColor);
                    //moves the position over to the end of current node's text
                    position.X += font.MeasureString(word_with_space_appended).X;

                    //spriteBatch.DrawString(defaultFont, "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT", new Vector2(position.X, 247), Color.Beige);

                }
                //resets the height of drawn text.
                position.Y -= difference;
            }
            //returns the last place the line was drawn to
            return position.Y;




        }
    }




    public class MessageBox : Surface
    {

        //public MessageWriter writer;
        public SpriteFont defaultFont;
        //the color of the widget's background
        public Color gameBG;
        //the list that holds all the messages
        public List<string> msgList;
        //the amount of messages to be drawn.
        public int messageLimit = 7;
        //the current index messages to draw
        public int[] activeMessages;

        //Whether or not it is being dragged or not
        public bool isbBeingDragged = false;
        //Whether is can be moved or not
        public bool isMoveLocked = false;

        //whether or not the message area display is showing the current messages or not
        public bool realtimeMsgs = true;

        ////fontsizes
        //float regularFontHeight;
        //float boldFontHeight;
        //float italicFontHeight;
        //float largestFontHeight;

        //float regularFontDifference;
        //float boldDifference;
        //float italicDifference;


        /// <summary>
        /// Constructor class
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="rect">The rectangle that represents the size of the msgbox</param>
        /// <param name="colorBG">color of the background</param>
        /// 
        /// 
        /// <param name="msgList"> the list of strings that the messagebox will deal with</param>
        /// <param name="moveLocked">whether or not the MB can get dragged or not</param>
        public MessageBox(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
            Rectangle rect, Color colorBG, List<string> msgList, bool moveLocked = false)
            : base(game, graphicsDevice, spriteBatch, rect, colorBG)
        {

            //this.writer = writer;
            this.msgList = msgList;

            //this.gameBG = gameBG;
            
            CalculateFontSizes();
            if (moveLocked )
            {
                //do nothing
            }
            else
            {
                Orchid.masterGuiElementList.Add(this);
                Console.WriteLine("added a msgbox to list");
            }

        }

        public void CalculateFontSizes()
        {           
            //all the font heights, into a list
            List<float> fontHeights = new List<float>();
            //height of the regular font
             regularFontHeight = (float)Game1.defaultFont.MeasureString("ABCabc").Y;
            fontHeights.Add(regularFontHeight);
            //height of the bold font and the difference between this and regular
             boldFontHeight = (float)Game1.boldFont.MeasureString("ABCabc").Y;
            fontHeights.Add(boldFontHeight);
            //height of the italic font and the difference between this and regular
             italicFontHeight = (float)Game1.italicFont.MeasureString("ABCabc").Y;
            fontHeights.Add(italicFontHeight);

            //find the tallest fontsize, then make it so the smaller fonts get drawn 
            //a bit lower than that
             largestFontHeight = fontHeights.Max();

             regularFontDifference = largestFontHeight - regularFontHeight;
             boldDifference = largestFontHeight - boldFontHeight;
             italicDifference = largestFontHeight - italicFontHeight;

        }

        /// <summary>
        /// Pretty self explanatory, subtracts the difference of current mouse pos 
        /// from the last mouse pos. Could probably switch it up to make it more clear
        /// </summary>
        public void Drag(MouseState currentMouseState, MouseState lastMouseState)
        {
            this.rect.X -= lastMouseState.X - currentMouseState.X;
            this.rect.Y -= lastMouseState.Y - currentMouseState.Y;
        }


        //draws the items of msgList to the surface... ActiveMessages is the list of
        //current messages dictated etiher in UpdatedActiveMessages or scrollMsgs
        public virtual void DrawMessages()
        {

            //make sure there's at least one item inside 
            if (this.msgList.Count >= 1)
            {
                //y is where the line gets drawn, relative to the RT
                int y = 0;
                //last line pos is where the last line was drawn
                float lastLinePos = 0;
                //activemesssages is a list of ints, index of which messages to be drawn
                foreach (int i in activeMessages)
                {
                    spriteBatch.Begin();

                    //draw a string that goes lower as the amount of lines get drawn ~BC~
                    Rectangle relativeSize = new Rectangle(0, y, 
                            this.rect.Width, this.rect.Height);
                    //format each string and draw it
                    lastLinePos =TextFormatter(msgList[i], relativeSize);
                    
                    spriteBatch.End();
                    //increment y so that the next string gets printed below current line
                    y = (int)lastLinePos +(int)largestFontHeight;
                }
            }
        }


        /// <summary>
        /// Only thing is does is draw the surface, a RenderTarget2d to the current buffer.
        /// Has to be the BackBuffer (setRenderTarget(null))
        /// </summary>
        public override void Draw() 
        {

            //draws the new surface stuff to the back buffer
            //Color newColor = new Color(this.backgroundColor.R, this.backgroundColor.G, this.backgroundColor.B, 100);
            spriteBatch.Draw(this.surface, this.rect, this.backgroundColor * this.alpha);

        }

        /// <summary>
        /// starts or stops the messages in the message area, live or paused.
        /// </summary>
        /// <param name="startRealtime">whether or not the messages are live or not</param>
        /// <returns>true is messages are in realtime</returns>
        public bool PauseMessageArea(bool startRealtime = false)
        {
            if (!startRealtime)
            {
                realtimeMsgs = false;

                Console.WriteLine("Messages are now paused.");

                return realtimeMsgs;
            }
            else
            {
                realtimeMsgs = true;

                Console.WriteLine("Messages are now live.");
                return realtimeMsgs;
            }
        }

        //updates the list to the last items in the message box, or leaves the MsgList alone
        //since somewhere else may have made it, such as scrollActiveMessages
        public void UpdateActiveMessages()
        {
            //new way, with a simple list, iterate over all the most recent Items and draw those
            int position;
            int COUNT = msgList.Count;

            
            //if there aren't enough items in the msgList, start at index 0
            if (COUNT < messageLimit)
            {
                position = 0;
            }
            //otherwise, start at the COUNT - limit
            else
            {
                position = COUNT - this.messageLimit;
            }
            
            //whether or not to update the messages on the fly or not
            if (realtimeMsgs)
            {
                //create an empty array the size of the messageLimit
                activeMessages = new int[messageLimit];

                //use the array to populate the messagearea item.
                int index = 0;
                for (int i = position; i < msgList.Count; i++)
                {
                    activeMessages[index] = i;
                    index++;
                }
            }
            //else: use activeMessages from elsewhere
            else
            {
                // use current activemessages, as if the stream is paused
            }
        }

        /// <summary>
        /// MessageBoxes only draw a number of items in a range of ints, so this changes
        /// the range, from the default last to -7 or so, to whatever distance is.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public int  ScrollMessageArea(int distance = -1)
        {
            if (!realtimeMsgs)
            {
                //pause realtimeMsgs
                realtimeMsgs = false;


                //copies the activeMsgs array to origArray, just in case
                int[] originalActive = new int[messageLimit];
                activeMessages.CopyTo(originalActive, 0);

                //make sure the distance isn't too big or small
                while (activeMessages[0] + distance < 0)
                {
                    distance += 1;
                }
                while (activeMessages[messageLimit - 1] + distance >= msgList.Count)
                {
                    distance -= 1;
                }

                //scroll the ints in activemessages by arg.distance or closest number.
                int index = 0;
                foreach (int item in originalActive)
                {
                    activeMessages[index] = item + distance;
                    index++;
                }
            }

            return 0;
        }



        //updates the messagearea surface, with the correct SetRenderTarget value
        //because you need to begin the spritebatch AFTER you've set the RT to what you want
        public override void UpdateSurface()
        {

            //change the renderTarger (pygame surface)
            graphicsDevice.SetRenderTarget(this.surface);
            //clear it, like normal
            graphicsDevice.Clear(Color.Thistle);

            //updates the int[] for the indexes of the messages to draw
            UpdateActiveMessages();

            //draw messages on surface
            DrawMessages();

            //then reset the drawing surface to null, backbuffer.
            //#### DON'T DRAW TO BackBuffer, it clears it.
            //graphicsDevice.SetRenderTarget(null);

            //graphicsDevice.Clear(gameBG);

            //return surface;
        } 
    }

    

}