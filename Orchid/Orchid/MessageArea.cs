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
    //public class MessageWriter : TextWriter
    //{

    //    public List<String> _output;

    //    public MessageWriter(List<String> output)
    //    {

    //        this._output = output;
    //    }



    //    public override void Write(char value)
    //    {
    //        base.Write(value);
    //        this._output.Add(value.ToString());

    //    }

    //    public override Encoding Encoding
    //    {
    //        get { return System.Text.Encoding.UTF8; }
    //    }
    //}

    public class Surface
    {
        protected GraphicsDevice graphicsDevice;
        protected SpriteBatch spriteBatch;

        public Color backgroundColor;

        public RenderTarget2D surface;
        public Rectangle rect;

        public Surface(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Rectangle rect, Color colorBG)
        {
            this.backgroundColor = colorBG;
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.surface = new RenderTarget2D(graphicsDevice, rect.Width, rect.Height);
            this.rect = rect;
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
            spriteBatch.Begin();


            this.UpdateSurface();

            spriteBatch.Draw(this.surface, this.rect, this.backgroundColor);
            spriteBatch.End();
        }
    }




    public class MessageArea : Surface
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

        //whether or not the message area display is showing the current messages or not
        public bool realtimeMsgs = true;

        public MessageArea(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
            Rectangle rect, Color colorBG,  SpriteFont defaultFont, Color gameBG, List<string> msgList)
            : base(graphicsDevice, spriteBatch, rect, colorBG)
        {

            //this.writer = writer;
            this.msgList = msgList;

            this.defaultFont = defaultFont;
            this.gameBG = gameBG;

        }


        public void DrawMessages()
        {

            int y = 0;
            foreach (int i in activeMessages)
            {
                spriteBatch.Begin();

                //draw a string that goes lower as the amount of lines get drawn ~BC~
                spriteBatch.DrawString(defaultFont, msgList[i], new Vector2(0, (y * 24)), Color.Black);
                spriteBatch.End();
                //increment y so that the next string gets printed below current line
                y++;
            }
        }

        public override void Draw()
        {
            //draw border
            //base.Draw();
            
            //updates the messagearea surface, with the correct SetRenderTarget value
            //because you need to begin the spritebatch AFTER you've set the RT to what you want
            UpdateSurface();

            //draws the new surface stuff to the back buffer
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            //spriteBatch.Begin();
            spriteBatch.Draw(this.surface, this.rect, this.backgroundColor);
            spriteBatch.End();

        }

        /// <summary>
        /// starts or stops the messages in the message area, live for pause
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

        public override void UpdateSurface()
        {

            //change the renderTarger (pygame surface)
            graphicsDevice.SetRenderTarget(surface);
            //clear it, like normal
            graphicsDevice.Clear(Color.Thistle);

            //updates the int[] for the indexes of the messages to draw
            UpdateActiveMessages();

            //draw messages on surface
            DrawMessages();

            //then reset the drawing surface to null, backbuffer.
            graphicsDevice.SetRenderTarget(null);

            graphicsDevice.Clear(gameBG);

            //return surface;
        } 
    }

}