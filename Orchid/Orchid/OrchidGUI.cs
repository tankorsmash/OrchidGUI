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

        //draws all the gui buttons. Loops over all the gui elements, so it might get slow later
        public static void DrawGUIButtons(List<GuiElement> elemList, GameTime gameTime)
        {
            //loop over each element in the list of GuiElements and Draw them all
            foreach (GuiElement elem in elemList)
            {
                if (elem is Button)
                {
                    elem.Draw(gameTime);
                }
               
            }
        }


        //draws all the messageBoxes. Loops over all the gui elements, so it might get slow later
        public static void DrawGUIMessageBoxes(List<GuiElement> elemList, GameTime gameTime)
        {
            //loop over each element in the list of GuiElements and Draw them all
            foreach (GuiElement elem in elemList)
            {
                if (elem is MessageBox)
                {
                    MessageBox castedElem = (MessageBox)elem;
                    castedElem.Draw();
                }
            }
        }




    }
}
