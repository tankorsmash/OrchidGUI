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
        

        public  event CommandHandler _command;
        
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
                    Rectangle rect, Color colorBG, List<string> msgList, Menu parent, List<Menu> subMenus, CommandHandler command, Color textColor)
            : base(game, graphicsDevice, spriteBatch, rect, colorBG, msgList, false, textColor:textColor)
        {

            this.parent = parent;
            this.subMenus = subMenus;
            this._command += command;

        }


        public override void OnMouseUp()
        {
            //base.OnMouseUp();
            try
            {
                this._command.Invoke();
                this.CreateSubMenus(null, new List<string>(new string[] {"test 3"}));
            }

            catch (NullReferenceException ex)
            {
           
                Console.WriteLine("Caught null for menu clicking");
            }

        }

        /// <summary>
        /// creates a submenu with the given command and text.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="msgList"></param>
        public  void CreateSubMenus(CommandHandler command, List<String> msgList)
        {
            //create a rect lower than the current
            Rectangle new_rect = this.rect;
            new_rect.Offset(this.rect.Width, 0);


            Orchid.CreateMenuItem(new_rect, msgList, command, parent: this, colorBG: this.backgroundColor);
        }
    }


}
