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
        public Menu parent;
        public List<Menu> subMenus;
        public Delegate command;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="rect"></param>
        /// <param name="colorBG"></param>
        /// <param name="msgList"></param>
        /// <param name="parent"></param>
        /// <param name="subMenus"></param>
        /// <param name="command"></param>
        /// <param name="textColor">If left empty, it'll be black</param>
        public Menu(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
                    Rectangle rect, Color colorBG, List<string> msgList, Menu parent, List<Menu> subMenus, Delegate command, Color textColor)
            : base(game, graphicsDevice, spriteBatch, rect, colorBG, msgList, false, textColor:textColor)
        {

            this.parent = parent;
            this.subMenus = subMenus;
            this.command = command;

        }


        public override void OnMouseUp()
        {
            //base.OnMouseUp();

        }
    }


}
