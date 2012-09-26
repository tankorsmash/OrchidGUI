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
        public SpriteBatch spriteBatch;
        public Rectangle rect;


        //name of the button
        //public string _name = "GuiElement";
        protected string _name;
        protected string name { get { return _name; } set { _name = value; } }

        //protected string name = "DefaultUnchangedGUIELEMET";



        public GuiElement(Game1 game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            this.name= this.GetType().Name;
            Console.WriteLine("GUIElement name: {0}", this.name);
        }

        //All guielements should have a tooltip
        public  virtual void CreateTooltip()
        {
            Rectangle tool_size = new Rectangle(100, 100, 275, 50);
            new Tooltip(this.game, GraphicsDevice, this.spriteBatch, tool_size, Color.White,
                new List<string>(new string[] { "Tooltip testing" }));
        }


        public virtual void OffMouseHover()
        {
            //Console.WriteLine("{0} lost mouse over", this);
        }
        public virtual void OnMouseHover()
        {
            Console.WriteLine("{0}:{1} received mouse over", this, this.GetHashCode());

        }

        public virtual void OnMouseDown()
        {
            //Console.WriteLine("{0} received mouse down", this);
        }

        public virtual void OnMouseUp()
        {
            //Console.WriteLine("{0} received mouse up", this);
        }


        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// Temporrary class that can be used until a proper guielement can be used
    /// </summary>
    public class DefaultElement : GuiElement
    {
        public  DefaultElement(Game1 game): base(game, game.spriteBatch)
            
        {        }

        
        public override string ToString()
        {
            return "EMPTY GUI ELEMENT";
        }
    }


    public class Button : GuiElement
    {

        //texture
        //SpriteBatch spriteBatch;
        Texture2D dummyTexture;

        //rect of inner rectangle, with the buttoncolor 
        public Rectangle innerRectangle;

        //what the button will say on it
        string text;


        //a delegate for commands
        //public Func<int> ADelegate();
        public Func<int> command;
        //public Action command;

        //colors
        Color borderColor;
        Color innerColor;
        Vector2 textPos;

        //SpriteBatch spriteBatch;

        //default button colors
        Color buttonColor = new Color(18, 64, 171);
        Color hiliteButtonColor = new Color(108, 140, 213);
        Color clickedButtonColor = new Color(42, 68, 128);
        Color inactiveButtonColor = new Color(70, 113, 213);

        Color defaultTextColor = Color.White;


        /// <summary>
        /// The constructor for the button class
        /// </summary>
        /// <param name="buttonSize">the size of the button, x y w h</param>
        /// <param name="text">the string that will be drawn on the button</param>
        /// <param name="game"></param>
        /// <param name="command">the function that will be called when the button gets clicked</param>
        /// <param name="innerColor">the inner button color</param>
        /// <param name="textColor"></param>
        public Button(Rectangle buttonSize, string text, Game1 game, SpriteBatch spriteBatch, Func<int> command = null, Color innerColor = new Color(), Color? textColor = null)
            //: base(buttonSize, game)
            : base(game, spriteBatch)
        {
            //determines when the element is drawn. a higher number means it'll be drawn laster
            DrawOrder = 1000;
            //the rect of the entire button
            this.rect = buttonSize;
            this.BuildInnerRect();

            //this.spriteBatch = spriteBatch;
 
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
            //base.name = this.text + base.name;
            //base.name = text + base._name;
            this.name = text + name;

            //set the msgArea color to default or use given color
            if (textColor == null)
            {
                textColor = defaultTextColor;
            }

            if (command != null) { this.command = command; }
            else { this.command = (defaultCommand); }


            //add this new button to the list of gui elements. Only buttons for now
            Orchid.masterGuiElementList.Add(this);

            this.Initialize();

        }

        public int defaultCommand()
        {
            string text = string.Format("{0} received mouse down", this);
            Console.WriteLine(text);
            this.game.msgList.Add(text);

            return 4;
            
        }

        //public override string ToString()
        //{
        //    return name;
        //}

        public override void OnMouseHover()
        {
            Console.WriteLine("{0} !!!received mouse over", this);

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

        //this is mouse down, not click. 
        public override void OnMouseDown() 
        {
            this.command();

            this.innerColor = clickedButtonColor;


        }

        public override void OnMouseUp()
        {
            this.innerColor = buttonColor;
            string text = string.Format("{0} received mouse up", this);
            this.game.msgList.Add(text);
        }

        /// <summary>
        /// Builds a Rect out of the size rect. It's uneven looking right now because 
        /// if one axis (x or y) is longer than the other, the borders will be different sizes
        /// You'll see what I mean when you make your own button instance
        /// 
        /// To solve it we'd just have to calculate a borderwidth somehow, and then 
        /// substract for the width and height at the same time, rather than having
        /// two different values
        /// </summary>
        public void BuildInnerRect()
        {
            //the inner portion will be 90 percent the rect of the button, leaving a 10 percent border
            //double innerW = rect.Width * .9;
            //double innerH = rect.Height * .9;
            //double innerX = rect.Center.X - (innerW / 2);
            //double innerY = rect.Center.Y - (innerH / 2);

            double innerDiffW = rect.Width * .1;
            double innerDiffH = rect.Height * .1;
            //it'll be set to having the same center point as the borderRect            
            //this.innerRectangle = new Rectangle((int)innerX, (int)innerY, (int)innerW, (int)innerH);
            this.innerRectangle = this.rect;
            this.innerRectangle.Inflate(-(int)innerDiffH, -(int)innerDiffH);
            
            //this.innerRectangle = innerRectangle;
        }

        //loads content. Really don't know what it's for though.
        protected override void LoadContent()
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();
            Color newColor = new Color(borderColor.R ,borderColor.G,  borderColor.B );

            spriteBatch.Draw(dummyTexture, rect, newColor);
            spriteBatch.Draw(dummyTexture, innerRectangle, innerColor);

            //find where to put the text. Midleft along the center of the button. '12' is half the rect of the font, which is 24
            textPos = new Vector2(rect.Center.X - (innerRectangle.Width / 2), rect.Center.Y - 12);
            spriteBatch.DrawString(Game1.defaultFont, this.text, textPos, Color.Black);
            
            //spriteBatch.End();

        }

        
    }

}
