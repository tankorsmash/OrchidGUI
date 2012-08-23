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
    public class InputHandler
    {


        //the main Game instance
        Game1 theGame;

        //GuiElements cache
        GuiElement activeElement;
        GuiElement emptyElement;
        GuiElement hoveredElement;

        //MouseStates cache
        MouseState currentMouseState = Mouse.GetState();
        MouseState lastMouseState = Mouse.GetState();

        public InputHandler(Game1 game)
        {
            //save a refernce to the main game, to easily access its attributes
            this.theGame = game;

            //create empty element
            emptyElement = new GuiElement(this.theGame);
            hoveredElement = new GuiElement(this.theGame);
            activeElement = new GuiElement(this.theGame);
        }



        public void CheckMouseAgainstElements(List<GuiElement> guiElementList)
        {
            Point mousePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            currentMouseState = Mouse.GetState();

            //if mouse1 is pressed
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                //loop over the elements in the gui list 
                foreach (GuiElement elem in guiElementList)
                {
                    if (elem is Button)
                    {

                        Button elem2 = elem as Button;
                        //elem = elem as Button;
                        if (elem2.borderRectangle.Contains(mousePos))
                        {
                            activeElement = elem;
                            elem2.OnMouseDown();

                        }
                    }
                }
            }

            //if mouse1 is clicked
            if (currentMouseState.LeftButton == ButtonState.Released &&
                   lastMouseState.LeftButton == ButtonState.Pressed)
            {
                //Console.WriteLine("trying for a mouse up");
                activeElement.OnMouseUp();

                activeElement = emptyElement;


            }

            //if the mouse is just chilling, hovering
            if (currentMouseState.LeftButton == ButtonState.Released &&
                currentMouseState.RightButton == ButtonState.Released)
            {

                if (!hoveredElement.borderRectangle.Contains(mousePos))
                {
                    hoveredElement.OffMouseHover();
                }

                foreach (GuiElement elem in guiElementList)
                {
                    if (elem.borderRectangle.Contains(mousePos))
                    {
                        //called hover method and set a element that is hovered over
                        elem.OnMouseHover();
                        hoveredElement = elem;
                    }
                }
            }


            //cache the mouse state so you have something to compare it against next frame.
            lastMouseState = currentMouseState;
        }
    }

}
