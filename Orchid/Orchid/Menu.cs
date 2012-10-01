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

        public Menu(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
                    Rectangle rect, Color colorBG, List<string> msgList, Menu parent, List<Menu> subMenus, Delegate command)
            : base(game, graphicsDevice, spriteBatch, rect, colorBG, msgList)
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

    //public class SubMenu
    //{
    //    public Menu parent;
    //    public string menu_string;
    //    public Delegate command;


    //    /// <summary>
    //    /// This is used as a child for the menu. It will share all the characteristics from the parent menu.
    //    /// </summary>
    //    /// <param name="menu_string"></param>
    //    /// <param name="command"></param>
    //    public SubMenu(Menu parent, string menu_string, Delegate command)
    //    {
    //        this.command = command;
    //        this.menu_string = menu_string;
    //        this.parent = parent;


    //    }
    //}
}
