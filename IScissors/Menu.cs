using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace IScissors
{
    public class Menu : GameComponent
    {
        private Control root;
        private MenuStrip menu;

        public Menu(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            root = Control.FromHandle(Game.Window.Handle);

            //menu = new MenuStrip();
            //menu.Size = new Size(800, 20);

            var text1 = new TextBox();
            text1.Location = new System.Drawing.Point(40, 40);
            text1.BorderStyle = BorderStyle.None;
            text1.Multiline = true;
            text1.Size = new Size(400, 400);

            root.Controls.Add(text1);
        }
    }
}
