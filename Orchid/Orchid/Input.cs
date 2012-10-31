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
    /// a gamestate representing where to send the input.
    /// </summary>
    public  class GameState
    {

        public string name;

        public GameState(string stateName)
        {
            this.name = stateName;
        }
    }

    public class InputHandler
    {


        //the main Game instance
        Game1 theGame;



        //GuiElements cache
        public GuiElement activeElement;
        public GuiElement emptyElement;
        public GuiElement hoveredElement;

        //which toplevel menu is currently active
        public Menu activeMenu;
        public Menu emptyMenu;

        //MouseStates cache
        MouseState currentMouseState = Mouse.GetState();
        MouseState lastMouseState = Mouse.GetState();
        //KeyboardStates cache
        KeyboardState currentKeyState = Keyboard.GetState();
        KeyboardState lastKeyState = Keyboard.GetState();


        //bool guiFound = false;


        public InputHandler(Game1 game)
        {
            //save a refernce to the main game, to easily access its attributes
            this.theGame = game;
 
            //create empty element
            emptyElement = new GuiElement(this.theGame, this.theGame.spriteBatch);
            hoveredElement = new GuiElement(this.theGame, this.theGame.spriteBatch);
            activeElement = new GuiElement(this.theGame, this.theGame.spriteBatch);

            emptyMenu = new Menu(this.theGame, this.theGame.GraphicsDevice,
                    this.theGame.spriteBatch, new Rectangle(10,10,10,10),
                    Color.Black, Orchid.CreateMsgList("empty menu"), null, null,
                    null, Color.White);
            //so the emptyMenu is never seen
            emptyMenu.IsHidden = true;
            //so the other menus know that it's not normal
            emptyMenu.name = "empty menu not a real one";
        }



        public void CheckMouseAgainstElements(List<GuiElement> guiElementList)
        {
            currentMouseState = Mouse.GetState();
            Point mousePos = new Point(currentMouseState.X, currentMouseState.Y);

            //this.theGame.testArea.Drag(currentMouseState, lastMouseState);

            //if mouse1 is pressed, but not dragging
            if (currentMouseState.LeftButton == ButtonState.Pressed &
                lastMouseState.LeftButton  != ButtonState.Pressed)
            {

                //for making sure an element is found
                bool elem_found = false;

                //loop over the elements in the gui list 
                foreach (GuiElement elem in guiElementList)
                {
                    //if it's a Button or a child
                    if (elem is Button)
                    {

                        Button castedElem = elem as Button;
                        //elem = elem as Button;
                        if (castedElem.rect.Contains(mousePos))
                        {
                            activeElement = elem;
                            castedElem.OnMouseDown();

                            elem_found = true;
                        }
                    }

                    //if it's a child of Surface
                    else if (elem is Surface)
                    {
                        //if it's gotten clicked
                        if (elem.rect.Contains(mousePos))
                        {
                            Surface castedElem = elem as Surface;
                            string text = String.Format("{0}, the Surface, has mouse down", castedElem);
                            theGame.msgList.Add(text);

                            //activate the pressed element
                            activeElement = castedElem;

                            //set active menu if you've clicked a menu otherwise
                            //reset it
                            if (elem is Menu)
                            {
                                activeMenu = (Menu)elem;
                            }
                            else { activeMenu = emptyMenu;}

                            elem_found = true;
                        }
                    }

                }


            //if no element was found to be clicked on reset em all to default
            //empty
            if (! elem_found)
            {
                activeElement = emptyElement;

                activeMenu = emptyMenu;

            }
            }


            
            //if the mouse1 button is being dragged
            if (currentMouseState.LeftButton == ButtonState.Pressed &
                lastMouseState.LeftButton == ButtonState.Pressed)
            {                             //if mousepos is along the left side of the box
                // make sure it's not too high

                if (activeElement is Surface)
                {
                    Surface castedElem = (Surface)activeElement;
                    if ((castedElem.rect.Top < mousePos.Y &
                        mousePos.Y < castedElem.rect.Bottom) )
                        //&
                        //// make sure its not too far the the left or right.
                        //(castedElem.rect.Left - (castedElem.rect.Width * .1) < mousePos.X) &
                        //(castedElem.rect.Left + (castedElem.rect.Width * .1) > mousePos.X))
                    {
                        string text2 = String.Format("{0}, the Surface, has mouse in", castedElem);
                        theGame.msgList.Add(text2);
                        //Console.WriteLine("RESIZING");
                        castedElem.Resize(currentMouseState, lastMouseState);


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

                foreach (GuiElement elem in guiElementList)
                {
                    if (elem is TextEntry)
                    {
                        if (elem.rect.Contains(mousePos))
                        {
                            TextEntry castedElem = (TextEntry)elem;
                            castedElem.OnMouseDown();

                            activeElement = elem;

                            theGame.currentGameState = theGame.typingGameState;

                        }
                    }
                }

               


            }

            //if mouse2 is pressed
            if  (lastMouseState.RightButton == ButtonState.Pressed)
            {
                //Console.WriteLine("trying for a mouse up");
                //activeElement.OnMouseUp();
                //activeElement = emptyElement;

                bool guiFound = false;
                foreach (GuiElement elem in Orchid.masterGuiElementList)
                {
                    if (elem.rect.Contains(mousePos) && !guiFound)
                    {
                        if (elem is MessageBox)
                        {
                            MessageBox msgbox = (MessageBox)elem;
                            msgbox.Drag(currentMouseState, lastMouseState);
                            Console.WriteLine("dragging something");
                            guiFound = true;
                        }

                        else { Console.WriteLine("is not message box"); }
                    }
                }

            }


            //if the mouse is just chilling, hovering
            if (currentMouseState.LeftButton == ButtonState.Released &&
                currentMouseState.RightButton == ButtonState.Released)
            {

                //TODO: will need to make sure that hoveredElement becomes null or default
                // because it just keeps on calling OffMouseHover until a new element comes along
                if (!hoveredElement.rect.Contains(mousePos))
                {
                    hoveredElement.OffMouseHover();

                    //reset hoveredElement
                    hoveredElement = emptyElement;
                }

                List<GuiElement> orig_elemlist = new List<GuiElement>(guiElementList);
                foreach (GuiElement elem in orig_elemlist)
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

            //if the gameState is playing, then do the following stuff, 
            if (game.currentGameState == game.playingGameState)
            {
                if (KeyPressed(Keys.T))
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

                if (KeyPressed(Keys.F))
                {
                    game.messageArea.msgList.Add("<Color.Magenta.bold>This is bold and magenta text </Color.Magenta.bold> <i> Press F do do this again</i>");
                }


            }
            else if (game.currentGameState == game.typingGameState)
            {
                //check is a key was released
                foreach (Keys key in lastKeyState.GetPressedKeys())
                {
                    if (KeyPressed(key))
                    {
                        TextEntry castedElem = (TextEntry)activeElement;

                        // TODO: BUG: if you backspace when there aren't any chars
                        // left to backspace away, it'll crash.

                        //if the key was backspace, remove last item in typed
                        if (key == Keys.Back)
                        {
                            castedElem.typed.RemoveAt(castedElem.typed.Count - 1);
                        }

                        else if (key == Keys.Escape)
                        {
                            theGame.currentGameState = theGame.playingGameState;
                        }
                        else
                        {
                            castedElem.typed.Add(key.ToString());
                        }

                    }

                }

                //save the current keystate as last keystate for next loop

            }
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
