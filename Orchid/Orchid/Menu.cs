﻿using System;
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
    public class MenuContainer : GuiElement
    {
        //list to hold the menu items on this menu level, a list of tuples that have
        // a msgList and command in them
        public List<Tuple<List<string>, CommandHandler>> MenuItems;
        public string MenuName;

        public Menu parent;

        public event CommandHandler _command;

        //object to build all the menu items once the MenuContainer is told to begin building
        public MenuContainer(Menu parent,
                             Game1 game,
                             GraphicsDevice
                                 graphicsDevice,
                             SpriteBatch spriteBatch,
                             string name = "Default MenuContainer")
            : base(game, spriteBatch)
        {
            this.MenuName = name;
            this.parent = parent;
        }

        public void AddMenuItem(CommandHandler command, List<String> msgList)
        {

            //create a tuple with a stringlist and a command in it to append the list of menu items
            Tuple<List<string>, CommandHandler> pair =
                new Tuple<List<string>, CommandHandler>(msgList, command);

            //add that tuple to the menuitem list
            MenuItems.Add(pair);
        }

        public  void CreateVisibleMenu()
        {
            //loop over all the menu items and create a surface to draw to screen
            for (int i = 0; i < MenuItems.Count; i++)
            {
                
            }
        }
    }


    public class Menu : MessageBox
    {
        //A list to hold all the menus that'll show up when you click on this one
        public Menu parent;
        public List<Menu> subMenus;

        //list to hold the menu items on this menu level, a list of tuples that have
        // a msgList and command in them
        public List<Tuple<List<string>, CommandHandler>> MenuItems;

        public event CommandHandler command;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textColor">If left empty, it'll be black</param>
        public Menu(Game1 game,
                    GraphicsDevice graphicsDevice,
                    SpriteBatch spriteBatch,
                    Rectangle rect,
                    Color colorBG,
                    List<string> msgList,
                    Menu parent,
                    List<Menu> subMenus,
                    CommandHandler command,
                    Color textColor)
            : base(
                game, graphicsDevice, spriteBatch, rect, colorBG, msgList, false,
                textColor: textColor)
        {
            this.parent = parent;
            this.NotifyParent();

            if (subMenus == null)
            {
                this.subMenus = new List<Menu>();
            }
            else if (subMenus != null)
            {
                this.subMenus = subMenus;
            }

            if (command != null)
            {
                this.command += command;
            }
            else
            {
                this.command +=
                    () =>
                    this.CreateSubMenu(null, 
                                new List<string>(new string[]
                                {"test default"}));
            }
        }


        public override void OnMouseUp()
        {
            //base.OnMouseUp();
            try
            {
                this.command.Invoke();
                //this.CreateSubMenu(null, new List<string>(new string[] {"test 3"}));
            }

            catch (NullReferenceException ex)
            {
                Console.WriteLine("Caught null for menu clicking");
            }
        }

        /// <summary>
        /// creates a submenu with the given command and text.
        /// </summary>
        public void CreateSubMenu(CommandHandler command, List<String> msgList)
        {

            //make sure the submenu doesn't already exist. 
            // in the future, make sure that the command is exactly the same
            // before getting upset that it doesn't work, because I know I will
            // make that mistake
            for (int i = 0; i < this.subMenus.Count; i++)
            {
                Menu sub = this.subMenus[i];
                CommandHandler sub_cmd = sub.command;
                List<String> sub_msg = sub.msgList;


                if (msgList[0] == sub_msg[0])
                {
                    Console.WriteLine("Menu Name already exists, please choose another name.");
                    return;
                }
            }

            //create a rect lower than the current
                Rectangle new_rect = this.rect;
                new_rect.Offset(this.rect.Width + 10, 0);
                //create the child menu
                Orchid.CreateMenu(new_rect, msgList, command, parent: this,
                                  colorBG: this.backgroundColor);
            
        }

        /// <summary>
        /// give a command and message and it will create a menu entry for it inside itself.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="msgList"></param>
        public void CreateMenuItem(CommandHandler command, List<string> msgList)
        {
            //create a tuple with a stringlist and a command in it to append the list of menu items
            Tuple<List<string>, CommandHandler> pair =
                new Tuple<List<string>, CommandHandler>(msgList, command);

            //add that tuple to the menuitem list
            MenuItems.Add(pair);
        }

        //This adds the instance to the parent's list
        // it will eventually send more complex messages, like open and close.
        public void NotifyParent()
        {
            if (this.parent != null)
            {
                this.parent.subMenus.Add(this);
                Console.WriteLine("Added to parent");
            }
        }
    }
}
