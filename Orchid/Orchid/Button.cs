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

namespace Orchid
{
    public class GuiElement : DrawableGameComponent
    {
        public Game1 game;
        public Rectangle borderRectangle;



        public GuiElement(Game1 game)
            : base(game)
        {
            this.game = game;
        }

        public GuiElement(Rectangle size, Game1 game): base(game)
        {
            this.game = game;
        }

        public virtual void OffMouseHover()
        {
            Console.WriteLine("{0} lost mouse over", this);
        }
        public virtual void OnMouseHover()
        {
            Console.WriteLine("{0} received mouse over", this);
        }

        public virtual void OnMouseDown()
        {
            Console.WriteLine("{0} received mouse down", this);
        }

        public virtual void OnMouseUp()
        {
            Console.WriteLine("{0} received mouse up", this);
        }
    }


    public class DefaultElement : GuiElement
    {
        public  DefaultElement(Game1 game): base(game)
            
        {

        }

        public override string ToString()
        {
            return "EMPTY GUI ELEMENT";
        }
    }


    public class Button : GuiElement
    {

        //texture
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;

        //rect of inner rectangle, with the buttoncolor 
        public Rectangle innerRectangle;

        //what the button will say on it
        string text;


        //a delegate for commands
        delegate void ADelegate();

        //colors
        Color borderColor;
        Color innerColor;
        Vector2 textPos;

        //default button colors
        Color buttonColor = new Color(18, 64, 171);
        Color hiliteButtonColor = new Color(108, 140, 213);
        Color clickedButtonColor = new Color(42, 68, 128);
        Color inactiveButtonColor = new Color(70, 113, 213);

        Color defaultTextColor = Color.White;

        //name of the button
        string _name = "_Button";
        string name { get { return _name; } set { _name = value; } }

        public Button(Rectangle buttonSize, string text, Game1 game,
               Color innerColor = new Color(), Color? textColor= null)
            : base(buttonSize, game)
        {
            //determines when the element is drawn. a higher number means it'll be drawn laster
            DrawOrder = 1000;
            //the rect of the entire button
            this.borderRectangle = buttonSize;
            this.BuildInnerRect();
 
            //colors
            if (innerColor == new Color())
            {
                this.innerColor = buttonColor;
            }
            else
            {
                this.innerColor = innerColor;
            }
            this.borderColor = Color.DarkBlue;

            //sets the name of the button to text plus whatever the _name is
            this.text = text;
            this.name = text + _name;

            //set the msgArea color to default or use given color
            if (textColor == null)
            {
                textColor = defaultTextColor;
            }


            //add this new button to the list of gui elements. Only buttons for now
            game.masterGuiElementList.Add(this);

            this.Initialize();

        }

        public override string ToString()
        {
            return name;
        }

        public override void OnMouseHover()
        {
            Console.WriteLine("{0} received mouse over", this);

            ////Randomize the color of the button when it's hovered
            //Random rnd = new Random();
            //int r = rnd.Next(0, 255);
            //int b = rnd.Next(0, 255);
            //int g = rnd.Next(0, 255);
            //int a = rnd.Next(0, 255);
            //Color clr = new Color(r, g, b, a);
            //this.innerColor = clr;

            this.innerColor = hiliteButtonColor;
        }

        public override void OffMouseHover()
        {
            this.innerColor = buttonColor;
        }

        public override void OnMouseDown() 
        {
            this.innerColor = clickedButtonColor;

            string text = string.Format("{0} received mouse down", this);
            Console.WriteLine(text);
            this.game.msgList.Add(text);
            
        }

        public override void OnMouseUp()
        {
            this.innerColor = buttonColor;
            string text = string.Format("{0} received mouse up", this);
            this.game.msgList.Add(text);
        }

        public void BuildInnerRect()
        {
            //the inner portion will be 90 percent the rect of the button, leaving a 10 percent border
            double innerW = borderRectangle.Width * .9;
            double innerH = borderRectangle.Height * .9;
            double innerX = borderRectangle.Center.X - (innerW / 2);
            double innerY = borderRectangle.Center.Y - (innerH / 2);
            //it'll be set to having the same center point as the borderRect            
            this.innerRectangle = new Rectangle((int)innerX, (int)innerY, (int)innerW, (int)innerH);

            //this.innerRectangle = innerRectangle;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(dummyTexture, borderRectangle, borderColor);
            spriteBatch.Draw(dummyTexture, innerRectangle, innerColor);

            

            //find where to put the text. Midleft along the center of the button. '12' is half the rect of the font, which is 24
            textPos = new Vector2(borderRectangle.Center.X - (innerRectangle.Width / 2), borderRectangle.Center.Y - 12);
            spriteBatch.DrawString(Game1.defaultFont, this.text, textPos, Color.Black);
            
            spriteBatch.End();

        }

        
    }

}
