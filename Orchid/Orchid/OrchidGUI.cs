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
    public static class Orchid
    {
        //list of gui elements
        public static List<GuiElement> masterGuiElementList = new List<GuiElement>();

        public static void DrawGUI(List<GuiElement> elemList, GameTime gameTime)
        {
            //loop over each element in the list of GuiElements and Draw them all
            foreach (GuiElement elem in elemList)
            {
                elem.Draw(gameTime);
            }
        }


    }
}
