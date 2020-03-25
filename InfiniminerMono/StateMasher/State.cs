using InfiniminerShared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMasher
{
    public class State
    {
        public StateMachine _SM = null;
        public Infiniminer.PropertyBag _P = null;

        public virtual void OnEnter(string oldState)
        {
        }

        public virtual void OnLeave(string newState)
        {
        }

        public virtual string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            return null;
        }

        public virtual void OnRenderAtEnter(GraphicsDevice graphicsDevice)
        {
        }

        public virtual void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
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

        //public virtual void OnStatusChange(NetConnectionStatus status)
        //{
        //}

        //public virtual void OnPacket(NetBuffer buffer, NetMessageType type)
        //{
        //}
    }
}
