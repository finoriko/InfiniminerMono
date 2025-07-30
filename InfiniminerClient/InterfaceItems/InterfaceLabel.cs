
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InterfaceItems
{
    class InterfaceLabel : InterfaceElement
    {
        public InterfaceLabel()
        {

        }
        public InterfaceLabel(InfiniminerMono.InfiniminerGame gameInstance)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
        }
        public InterfaceLabel(InfiniminerMono.InfiniminerGame gameInstance, InfiniminerMono.PropertyBag pb)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
            _P = pb;
        }
        public override void Render(GraphicsDevice graphicsDevice)
        {
            if (visible && text != "")
            {
                SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                spriteBatch.Begin();
                spriteBatch.DrawString(uiFont, text, new Vector2(size.X, size.Y), Color.White);
                spriteBatch.End();
            }
        }
    }
}