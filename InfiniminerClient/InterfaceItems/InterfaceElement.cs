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
    class InterfaceElement
    {
        public bool visible = false;
        public bool enabled = false;
        public string text = "";
        //public Vector2 position = Vector2.Zero;
        public Rectangle size = Rectangle.Empty;
        public SpriteFont uiFont;
        public InfiniminerMono.PropertyBag _P;

        public InterfaceElement()
        {
        }

        public InterfaceElement(InfiniminerMono.InfiniminerGame gameInstance, InfiniminerMono.PropertyBag pb)
        {
            uiFont = gameInstance.Content.Load<SpriteFont>("font_04b08");
            _P = pb;
        }

        public virtual void OnCharEntered(EventInput.CharacterEventArgs e)
        {
        }

        public virtual void OnKeyDown(Keys key)
        {
        }

        public virtual void OnKeyUp(Keys key)
        {
        }

        public virtual void OnMouseDown(MouseButton button, int x, int y)
        {
        }

        public virtual void OnMouseUp(MouseButton button, int x, int y)
        {
        }

        public virtual void OnMouseScroll(int scrollWheelValue)
        {
        }

        public virtual void Render(GraphicsDevice graphicsDevice)
        {

        }
    }
}
