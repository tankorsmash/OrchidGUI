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
    /// <summary>
    /// A tooltip surface. Small, but autosizes on text entry,
    /// fades in an out. Text cannot change.
    /// </summary>
    public class Tooltip : MessageBox
    {
        //whether or not to draw the tooltip
        public bool IsHidden = true;

        public Tooltip(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
            Rectangle rect, Color colorBG, List<string> msgList) : base(game, graphicsDevice, spriteBatch, rect, colorBG, msgList)
        {

            this.textColor = Color.Black;

        }

    }
}
