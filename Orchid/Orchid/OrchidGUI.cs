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
    /// <summary>
    /// A static class to hold a bunch of lists for now. What it's really for though is up
    /// in the air for me at this point.
    /// </summary>
    public static class Orchid
    {
        //list of gui elements
        public static List<GuiElement> masterGuiElementList = new List<GuiElement>();

        public static SpriteBatch spriteBatch;
        public static GraphicsDevice graphicsDevice;
        public static Game1 game;


        //helper funciton for creating a list<str> from one string.
        public static List<String> CreateMsgList(string line)
        {
            return new List<String>( new string[] {line});
        }

        //draws all the gui buttons. Loops over all the gui elements, so it might get slow later
        public static void DrawGUIButtons(List<GuiElement> elemList, GameTime gameTime)
        {
            //loop over each element in the list of GuiElements and Draw them all
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Button")
                {
                    elem.Draw(gameTime);
                }

            }
        }




        //updates all the surfaces of the MessageBoxes
        private static void UpdateGUIMessageBoxes(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "MessageBox")
                {
                    MessageBox castedElem = (MessageBox) elem;
                    castedElem.Update();
                    //castedElem.UpdateSurface();
                }
            }
        }

        //draws all the messageBoxes. Loops over all the gui elements, so it might get slow later
        private static void DrawGUIMessageBoxes(List<GuiElement> elemList, GameTime gameTime)
        {
            //loop over each element in the list of GuiElements and Draw them all
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "MessageBox")
                {
                    MessageBox castedElem = (MessageBox) elem;
                    castedElem.Draw();
                }
            }
        }


        public static void DrawGUISurfaces(List<GuiElement> elemList, GameTime gameTime)
        {

            //loop over each element in the list of GuiElements and Draw them all
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Surface")

                {
                    Surface castedElem = (Surface) elem;
                    castedElem.Draw();
                }
            }
        }

        private static void UpdateGUISurfaces(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Surface")
                {
                    Surface castedElem = (Surface) elem;
                    castedElem.UpdateSurface();
                }
            }
        }

        public static void UpdateGUI(List<GuiElement> elemList, GameTime gameTime)
        {
            UpdateGUIMessageBoxes(elemList, gameTime);
            UpdateGUIMenus(elemList, gameTime);
            UpdateGUITextEntrys(elemList, gameTime);
            UpdateGUISurfaces(elemList, gameTime);
            UpdateGUITooltips(elemList,gameTime);
            UpdateGUIMenuContainers(elemList,gameTime);
        }

        public static void CreateMenu(Rectangle rect, List<string> msgList, CommandHandler command, Menu parent, Color? colorBG)
        {
            if (rect == null)
            {

                 rect = new Rectangle(100, 100, 275, 50);
            }
                   //new List<string>(new string[] { "Menu 1" })
            Menu menu= new Menu(Orchid.game, Orchid.graphicsDevice, spriteBatch, rect, Color.GreenYellow,
                msgList, parent: parent, subMenus: null, command: command, textColor:Color.Green);

            //return menu;
        }

        public static void DrawGUI(List<GuiElement> elemList, GameTime gameTime)
        {

            DrawGUIMessageBoxes(elemList, gameTime);
            DrawGUIMenus(elemList, gameTime);
            DrawGUITextEntrys(elemList, gameTime);
            DrawGUISurfaces(elemList, gameTime);
            DrawGUIButtons(elemList, gameTime);
            DrawGUITextTooltips(elemList,gameTime);
            DrawGUIMenuContainers(elemList,gameTime);

        }


        private static void DrawGUIMenuContainers(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "MenuContainer")
                {
                    MenuContainer castedElem = (MenuContainer)elem;
                    if (!(castedElem.IsHidden))
                    {
                        castedElem.Draw();
                    }
                }
            }
        }

        private static void UpdateGUIMenuContainers(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "MenuContainer")
                {
                    MenuContainer castedElem = (MenuContainer)elem;
                    castedElem.Update();
                }
            }
        }

        private static void DrawGUITextTooltips(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Tooltip")
                {
                    Tooltip castedElem = (Tooltip)elem;
                    if (!(castedElem.IsHidden))
                    {
                        castedElem.Draw();
                    }
                }
            }
        }

        private static void UpdateGUITooltips(List<GuiElement> elemList,
                GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Tooltip")
                {
                    Tooltip castedElem = (Tooltip)elem;
                    castedElem.Update();
                }
            }
        }

        private static void DrawGUITextEntrys(List<GuiElement> elemList,
                GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "TextEntry")
                {
                    TextEntry castedElem = (TextEntry)elem;
                    castedElem.Draw();
                }
            }
        }
        private static void UpdateGUITextEntrys(List<GuiElement> elemList,
                GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "TextEntry")
                {
                    TextEntry castedElem = (TextEntry)elem;
                    castedElem.UpdateSurface();
                }
            }
        }
    private static void DrawGUIMenus(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Menu")
                {
                    Menu castedElem = (Menu)elem;
                    castedElem.Draw();
                }
            }
        }
        private static void UpdateGUIMenus(List<GuiElement> elemList, GameTime gameTime)
        {
            foreach (GuiElement elem in elemList)
            {
                if (elem.GetType().Name == "Menu")
                {
                    Menu castedElem = (Menu)elem;
                    castedElem.Update();
                    //castedElem.UpdateSurface();
                }
            }
        }
    }
}
