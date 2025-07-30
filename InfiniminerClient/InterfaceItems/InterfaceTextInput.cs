using InfiniminerShared;
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
    class InterfaceTextInput : InterfaceElement
    {
        public string value = "";
        private bool partialInFocus = false;
        private bool inFocus = false;
        //Infiniminer.KeyMap keyMap;
        public InterfaceTextInput()
        {
            //keyMap = new Infiniminer.KeyMap();
        }
        public InterfaceTextInput(InfiniminerMono.InfiniminerGame gameInstance)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
            //keyMap = new Infiniminer.KeyMap();
        }

        public InterfaceTextInput(InfiniminerMono.InfiniminerGame gameInstance, InfiniminerMono.PropertyBag pb)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
            _P = pb;
            //keyMap = new Infiniminer.KeyMap();
        }

        public override void OnMouseDown(MouseButton button, int x, int y)
        {
            if (enabled && size.Contains(x, y))
                partialInFocus = true;
            else
                inFocus = false;
        }

        public override void OnMouseUp(MouseButton button, int x, int y)
        {
            if (enabled && partialInFocus && size.Contains(x, y))
            {
                inFocus = true;
                _P.PlaySound(InfiniminerShared.InfiniminerSound.ClickLow);
            }
            partialInFocus = false;
        }

        public override void OnCharEntered(EventInput.CharacterEventArgs e)
        {
            base.OnCharEntered(e);
            if ((int)e.Character < 32 || (int)e.Character > 126) //From space to tilde
                return; //Do nothing
            if (inFocus)
            {
                value += e.Character;
            }
        }

        public override void OnKeyDown(Keys key)
        {
            base.OnKeyDown(key);
            if (inFocus)
            {
                if (key == Keys.Enter)
                {
                    inFocus = false;
                    _P.PlaySound(InfiniminerShared.InfiniminerSound.ClickHigh);
                }
                else if (key == Keys.Back && value.Length > 0)
                    value = value.Substring(0, value.Length - 1);
                /*else if (keyMap.IsKeyMapped(key))
                {
                    value += keyMap.TranslateKey(key, Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift));

                }*/
            }
        }

        public override void Render(GraphicsDevice graphicsDevice)
        {
            if (visible && size.Width > 0 && size.Height > 0)
            {
                Color drawColour = new Color(1f, 1f, 1f);

                if (!enabled)
                    drawColour = new Color(.7f, .7f, .7f);

                else if (!inFocus)
                    drawColour = new Color(.85f, .85f, .85f);

                //Generate 1px white texture
                Texture2D shade = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
                shade.SetData(new Color[] { Color.White });

                //Draw base background
                SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                spriteBatch.Begin();
                spriteBatch.Draw(shade, size, drawColour);

                spriteBatch.DrawString(uiFont, value, new Vector2(size.X + size.Width / 2 - uiFont.MeasureString(value).X / 2, size.Y + size.Height / 2 - 8), Color.Black);

                if (text != "")
                {
                    //Draw text
                    spriteBatch.DrawString(uiFont, text, new Vector2(size.X, size.Y - 20), enabled ? Color.White : new Color(.7f, .7f, .7f));//drawColour);
                }

                /*spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
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
                //Draw amount
                spriteBatch.DrawString(uiFont, (((float)(int)(value * 10)) / 10).ToString(), new Vector2(size.X, size.Y - 20), drawColour);
                */
                spriteBatch.End();
                shade.Dispose();
            }
        }
    }
}