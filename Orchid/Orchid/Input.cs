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
        //KeyboardStates cache
        KeyboardState currentKeyState = Keyboard.GetState();
        KeyboardState lastKeyState = Keyboard.GetState();

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

            //this.theGame.testArea.Drag(currentMouseState, lastMouseState);

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
                        if (elem2.rect.Contains(mousePos))
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


            if  (lastMouseState.RightButton == ButtonState.Pressed)
            {
                //Console.WriteLine("trying for a mouse up");
                //activeElement.OnMouseUp();
                //activeElement = emptyElement;

                foreach (GuiElement elem in Orchid.masterGuiElementList)
                {
                    if (elem.rect.Contains(mousePos))
                    {
                        if (elem is MessageBox)
                        {
                            MessageBox msgbox = (MessageBox)elem;
                            msgbox.Drag(currentMouseState, lastMouseState);
                            Console.WriteLine("dragiging");
                        }

                        else { Console.WriteLine("is not message box"); }
                    }
                }

            }


            //if the mouse is just chilling, hovering
            if (currentMouseState.LeftButton == ButtonState.Released &&
                currentMouseState.RightButton == ButtonState.Released)
            {

                if (!hoveredElement.rect.Contains(mousePos))
                {
                    hoveredElement.OffMouseHover();
                }

                foreach (GuiElement elem in guiElementList)
                {
                    if (elem.rect.Contains(mousePos))
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

        public void HandleKeys(Game1 game)
        {
            //update current key state
            currentKeyState = Keyboard.GetState();

            if (KeyPressed(Keys.S))
            {

            }

            //if you hit espace exit 
            if (KeyPressed(Keys.Escape))
            {
                game.Exit();
            }

            if (KeyPressed(Keys.P))
            {
                game.messageArea.PauseMessageArea();
            }
            if (KeyPressed(Keys.U))
            {
                game.messageArea.PauseMessageArea(true);
            }

            //scroll messages back  5 lines
            if (KeyPressed(Keys.W))
            {
                game.messageArea.ScrollMessageArea(-5);
            }
            //scroll messages forward 5 lines
            if (KeyPressed(Keys.S))
            {
                game.messageArea.ScrollMessageArea(5);
            }

            //save the current keystate as last keystate for next loop
            lastKeyState = currentKeyState;
        }

        public bool KeyPressed(Keys key)
        {
            if (currentKeyState.IsKeyUp(key) && lastKeyState.IsKeyDown(key))
            {
                return true;
            }
            else { return false; }
        }
    }

}
