using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Oikake.Device
{
    static class Input
    {
        private static Vector2 velocity = Vector2.Zero;
        //キーボード
        private static KeyboardState currentKey;
        private static KeyboardState previousKey;
        //マウス
        private static MouseState currentMouse;
        private static MouseState previousMouse;
       
        private static Vector2 padVelocity = Vector2.Zero;//パッド移動量

        public static void Update()
        {
            //キーボード
            previousKey = currentKey;
            currentKey = Keyboard.GetState();
            //マウス
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            //更新
            UpdateVelocity();
        }
        public static Vector2 Velocity()
        {
            return velocity;
        }
        /// <summary>
        /// キーボード関連
        /// </summary>
        private static void UpdateVelocity()
        {
            velocity = Vector2.Zero;
            if (currentKey.IsKeyDown(Keys.Right))
            { velocity.X += 1; }
            if (currentKey.IsKeyDown(Keys.Left))
            { velocity.X -= 1; }
            if (currentKey.IsKeyDown(Keys.Up))
            { velocity.Y -= 1; }
            if (currentKey.IsKeyDown(Keys.Down))
            { velocity.Y += 1; }
            if (velocity.Length() != 0)
            { velocity.Normalize(); }
        }
        public static bool IsKeyDown(Keys key)
        { return currentKey.IsKeyDown(key) && !previousKey.IsKeyDown(key); }
        public static bool GetKeyTrigger(Keys key)
        { return IsKeyDown(key); }
        public static bool GetKeyState(Keys key)
        { return currentKey.IsKeyDown(key); }
        /// <summary>
        /// マウス関連
        /// </summary>
        /// <returns></returns>
        public static bool IsMouseLButtonDown()
        {
            return currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;
        }
        public static bool IsMouseLButtonUp()
        {
            return currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed;
        }
        public static bool IsMouseLButton()
        {
            return currentMouse.LeftButton == ButtonState.Pressed;
        }
        public static bool IsMouseRButtonDown()
        {
            return currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released;
        }
        public static bool IsMouseRButtonUp()
        {
            return currentMouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed;
        }
        public static bool IsMouseRButton()
        {
            return currentMouse.RightButton == ButtonState.Pressed ;
        }
        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(currentMouse.X, currentMouse.Y);
            }
        }
        public static int GetMouseWheel()
        { return previousMouse.ScrollWheelValue - currentMouse.ScrollWheelValue; }
    }
}
