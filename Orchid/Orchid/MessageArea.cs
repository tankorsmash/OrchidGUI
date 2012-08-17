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
    public class MessageWriter : TextWriter
    {

        public List<String> _output;

        public MessageWriter(List<String> output)
        {

            this._output = output;
        }



        public override void Write(char value)
        {
            base.Write(value);
            this._output.Add(value.ToString());

        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

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

        public virtual RenderTarget2D UpdateSurface()
        {

            //change the renderTarger (pygame surface)
            graphicsDevice.SetRenderTarget(surface);
            //clear it, like normal
            graphicsDevice.Clear(Color.Thistle);


            //then reset the drawing surface to null, backbuffer.
            graphicsDevice.SetRenderTarget(null);

            //graphicsDevice.Clear(Color.CornflowerBlue);

            return surface;

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

        public MessageWriter writer;
        public SpriteFont defaultFont;

        public MessageArea(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Rectangle rect, Color colorBG, MessageWriter writer, SpriteFont defaultFont)
            : base(graphicsDevice, spriteBatch, rect, colorBG)
        {

            this.writer = writer;
            this.defaultFont = defaultFont;

        }


        public void DrawMessages()
        {
            spriteBatch.Begin();
            int y = 0;
            int x = 0;
            int count = 1;
            for (int i = 0; i < writer._output.Count; i++)
            {
                if (writer._output[i] == "\n")
                {
                    y += 20;
                    x = 200;
                    count = 0; //not 1, because it gets ++'d at the end of the loop.
                }
                spriteBatch.DrawString(defaultFont, writer._output[i], new Vector2((x + (12 * count)), y), Color.White);
                count++;
            }

            spriteBatch.End();
        }

        public override void Draw()
        {
            //draw border
            //base.Draw();
            UpdateSurface();

        }

        public override RenderTarget2D UpdateSurface()
        {

            //change the renderTarger (pygame surface)
            graphicsDevice.SetRenderTarget(surface);
            //clear it, like normal
            graphicsDevice.Clear(Color.Thistle);

            //draw messages on surface
            DrawMessages();

            //then reset the drawing surface to null, backbuffer.
            graphicsDevice.SetRenderTarget(null);

            //graphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(this.surface, this.rect, this.backgroundColor);
            spriteBatch.End();
            return surface;
        } 
    }

}