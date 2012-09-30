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
    public class Menu : MessageBox
    {

        //A list to hold all the menus that'll show up when you click on this one
        public List<Menu> subMenus; 


        public Menu(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
                    Rectangle rect, Color colorBG, List<string> msgList, List<Menu> subMenus )
            : base(game, graphicsDevice, spriteBatch, rect, colorBG, msgList)
        {

            this.subMenus = subMenus;

        }
    }
}
