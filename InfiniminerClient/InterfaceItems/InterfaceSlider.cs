﻿using InfiniminerShared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceItems
{
    class InterfaceSlider : InterfaceElement
    {
        public float minVal = 0f;
        public float maxVal = 1f;
        private bool sliding = false;
        public float value = 0;
        public bool integers = false;
        public InterfaceSlider()
        {

        }
        public InterfaceSlider(InfiniminerMono.InfiniminerGame gameInstance)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
        }
        public InterfaceSlider(InfiniminerMono.InfiniminerGame gameInstance, InfiniminerMono.PropertyBag pb)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
            _P = pb;
        }
        public void setValue(float newVal)
        {
            if (integers)
                value = (int)Math.Round((double)newVal);
            else
                value = newVal;
        }
        public float getPercent()
        {
            return (value - minVal) / (maxVal - minVal);
        }

        public override void OnMouseDown(MouseButton button, int x, int y)
        {
            if (size.Contains(x, y))
            {
                sliding = true;
                Update(x, y);
            }
        }
        public void Update()
        {
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
                Update(ms.X, ms.Y);
            else
                sliding = false;
        }
        public void Update(int x, int y)
        {
            if (sliding)
            {
                MouseState ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Released)
                    sliding = false;
                else
                {
                    if (x < size.X + size.Height)
                        value = minVal;
                    else if (x > size.X + size.Width - size.Height)
                        value = maxVal;
                    else
                    {
                        int xMouse = x - size.X - size.Height;
                        int xMax = size.Width - 2 * size.Height;
                        float sliderPercent = (float)xMouse / (float)xMax;
                        if (integers)
                            value = (int)Math.Round((sliderPercent * (maxVal - minVal)) + minVal);
                        else
                            value = sliderPercent * (maxVal - minVal) + minVal;
                        if (value < minVal)
                            value = minVal;
                        else if (value > maxVal)
                            value = maxVal;
                    }
                }
            }
        }

        public override void Render(GraphicsDevice graphicsDevice)
        {
            Update();

            if (visible && size.Width > 0 && size.Height > 0)
            {
                Color drawColour = new Color(1f, 1f, 1f);
                if (!enabled)
                {
                    drawColour = new Color(.5f, .5f, .5f);
                }
                //Generate 1px white texture
                Texture2D shade = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
                shade.SetData(new Color[] { Color.White });
                //Draw end boxes

                SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(shade, new Rectangle(size.X, size.Y, size.Height, size.Height), drawColour);
                spriteBatch.Draw(shade, new Rectangle(size.X + size.Width - size.Height, size.Y, size.Height, size.Height), drawColour);

                //Draw line

                float sliderPercent = getPercent();
                int sliderPartialWidth = size.Height / 4;
                int midHeight = (int)(size.Height / 2) - 1;
                int actualWidth = size.Width - 2 * size.Height;
                int actualPosition = (int)(sliderPercent * actualWidth);
                spriteBatch.Draw(shade, new Rectangle(size.X, size.Y + midHeight, size.Width, 1), drawColour);
                //Draw slider
                spriteBatch.Draw(shade, new Rectangle(size.X + size.Height + actualPosition - sliderPartialWidth, size.Y + midHeight - sliderPartialWidth, size.Height / 2, size.Height / 2), drawColour);
                if (text != "")
                {
                    //Draw text
                    spriteBatch.DrawString(uiFont, text, new Vector2(size.X, size.Y - 36), drawColour);
                }
                //Draw amount
                spriteBatch.DrawString(uiFont, (((float)(int)(value * 10)) / 10).ToString(), new Vector2(size.X, size.Y - 20), drawColour);
                spriteBatch.End();
                shade.Dispose();
            }
        }
    }
}
