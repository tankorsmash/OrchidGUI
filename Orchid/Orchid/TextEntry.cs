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
    class TextEntry : MessageBox
    {
        //the list of all the recently typed keys.
        public List<string> typed = new List<string>();

        //constructor
        public TextEntry(Game1 game, GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch, Rectangle rect, Color colorBG, 
            List<string> msgList,Color textColor )
            : base(game, graphicsDevice, spriteBatch, rect, colorBG, msgList,textColor, false)

        {

            //adds this instance to the globabl GUI element list
            Orchid.masterGuiElementList.Add(this);
        }



        public override void OnMouseDown()
        {
            this.msgList.Clear();

            
        }

        public override void DrawMessages()
        {

            this.msgList.Clear();
            //takes all the strings in the typed list and then joins em all up and passes
            // them as a line to msgList.
            string toAdd = String.Join(String.Empty, typed.ToArray());
            this.msgList.Add(toAdd);

            base.DrawMessages();

        }

    }
}
