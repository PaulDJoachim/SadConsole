﻿using System;
using SadConsole;
using SadConsole.Consoles;
using Console = SadConsole.Consoles.Console;
using Microsoft.Xna.Framework;

namespace StarterProject.CustomConsoles
{
    class RandomScrollingConsole : Console
    {
        private TextSurface mainData;
        private TextSurface messageData;
        private bool initialized;
        private bool initializedStep2;
        private bool initializedStep3;

        public RandomScrollingConsole() : base(80, 25)
        {
            messageData = new TextSurface(10, 1, Engine.DefaultFont);
            IsVisible = false;

            
            KeyboardHandler = (cons, info) =>
            {

                if (info.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                    cons.DataViewport = new Rectangle(cons.DataViewport.X - 1, cons.DataViewport.Y, 80, 25);

                if (info.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                    cons.DataViewport = new Rectangle(cons.DataViewport.X + 1, cons.DataViewport.Y, 80, 25);

                if (info.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                    cons.DataViewport = new Rectangle(cons.DataViewport.X, cons.DataViewport.Y - 1, 80, 25);

                if (info.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                    cons.DataViewport = new Rectangle(cons.DataViewport.X, cons.DataViewport.Y + 1, 80, 25);

                return true;
            };

        }

        protected override void OnVisibleChanged()
        {
            if (IsVisible && !initialized)
            {
                // Write to the message layer
                _textSurface.Print(0, 0, "Generating random console data, please wait...");
                initialized = true;
            }
        }

        public override void Render()
        {
            // These 3 render calls are a hack to get the console data generated and display a message to the user
            // Should add in async calls that let us generate these in the background... That would be cool.
            if (IsVisible)
            {
                if (!initialized)
                    base.Render();

                else if (!initializedStep2)
                {
                    initializedStep2 = true;
                    base.Render();
                }
                else if (!initializedStep3)
                {
                    base.Render();

                    // Generate the content
                    _textSurface = new TextSurface(2000, 2000, Engine.DefaultFont); //500mb ?? why?
                    _textSurface.ViewArea = new Rectangle(0, 0, 80, 25);
                    //Data = new TextSurface(2000, 2000);
                    //DataViewport = new Rectangle(0, 0, 80, 25);

                    // Clear message data and make it transparent so that it acts as a layer
                    messageData.Fill(Color.White, Color.Transparent, 0, null);

                    // We need to set celldata to the big console data so we can use the FillWithRandom method.
                    FillWithRandomGarbage();
                    initializedStep3 = true;
                }

                else
                {
                    // Set message data information about where the viewport is located
                    messageData.Print(0, 0, $"{DataViewport.X} , {DataViewport.Y}            ", Color.White, Color.Black);

                    // Create a faux layering system.
                    base.Render();

                    Renderer.Render(messageData, new Point(0, 0));
                }
            }
        }
    }
}
