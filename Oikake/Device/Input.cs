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

        //ゲームパッド
        private static List<PlayerIndex> playerIndex = new List<PlayerIndex>()
        {
            PlayerIndex.One,PlayerIndex.Two,PlayerIndex.Three,PlayerIndex.Four
        };
        private static Dictionary<PlayerIndex, GamePadState> currentGamePads = new Dictionary<PlayerIndex, GamePadState>()
        {
            {PlayerIndex.One,GamePad.GetState(PlayerIndex.One)},
            {PlayerIndex.Two,GamePad.GetState(PlayerIndex.Two)},
            {PlayerIndex.Three,GamePad.GetState(PlayerIndex.Three)},
            {PlayerIndex.Four,GamePad.GetState(PlayerIndex.Four)}
        };

        private static Dictionary<PlayerIndex, GamePadState> previousGamePads = new Dictionary<PlayerIndex, GamePadState>()
        {
            {PlayerIndex.One,GamePad.GetState(PlayerIndex.One)},
             {PlayerIndex.Two,GamePad.GetState(PlayerIndex.Two)},
              {PlayerIndex.Three,GamePad.GetState(PlayerIndex.Three)},
               {PlayerIndex.Four,GamePad.GetState(PlayerIndex.Four)},
        };

        public static void Update()
        {
            //キーボード
            previousKey = currentKey;
            currentKey = Keyboard.GetState();
            //マウス
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            //パッド
            for(int i=0;i<currentGamePads.Count;i++)
            {
                if (currentGamePads[playerIndex[i]].IsConnected==false)
                {
                    continue;
                }
                previousGamePads[playerIndex[i]] = currentGamePads[playerIndex[i]];
                currentGamePads[playerIndex[i]] = GamePad.GetState(playerIndex[i]);
            }
            //更新
            UpdateVelocity();
            UpdatePadVelocity();
        }
        #region キーボード関連
        public static Vector2 Velocity()
        {
            return velocity;
        }

       
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
        #endregion　キーボード関連

        #region マウス関連
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
        #endregion マウス関連

        #region ゲームパッド関連

        /// <summary>
        /// キーが押された瞬間か？
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        /// <returns>現在キーが押されていて1フレーム前に押されていなければtrue</returns>
        public static bool IskeyPadDown(PlayerIndex index,Buttons button)
        {
            //つながってなければ
            if(currentGamePads[index].IsConnected==false)
            {
                return false;
            }
            return currentGamePads[index].IsButtonDown(button) && !previousGamePads[index].IsButtonDown(button);
        }

        private static void UpdatePadVelocity()
        {
            velocity = Vector2.Zero;
            if (GetkeyState(PlayerIndex.One,Buttons.LeftThumbstickRight))
            {
                velocity.X += 1;
            }
            if (GetkeyState(PlayerIndex.One,Buttons.LeftThumbstickLeft))
            { velocity.X -= 1; }
            if (GetkeyState(PlayerIndex.One,Buttons.LeftThumbstickUp))
            { velocity.Y -= 1; }
            if (GetkeyState(PlayerIndex.One,Buttons.LeftThumbstickDown))
            { velocity.Y += 1; }
            if (velocity.Length() != 0)
            { velocity.Normalize(); }
        }
        /// <summary>
        /// キーが押された瞬間か
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        /// <returns>押された瞬間ならtrue</returns>
        public static bool GetKeyTrigger(PlayerIndex index,Buttons button)
        {
            //つながっていなければfalseを返す
            if(currentGamePads[index].IsConnected==false)
            {
                return false; 
            }
            return IskeyPadDown(index, button);
        }

        /// <summary>
        /// キーが押されているか
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        /// <returns>キーが押されていたら</returns>
        public static bool GetkeyState(PlayerIndex index,Buttons button)
        {
            //つながっていなければfalseを返す
            if(currentGamePads[index].IsConnected==false)
            {
                return false;
            }
            return currentGamePads[index].IsButtonDown(button);
        }

        /// <summary>
        /// キーが押されているか
        /// </summary>
        /// <param name="index"></param>
        /// <returns>キーが押されていたら</returns>
        public static Vector2 Velocity(PlayerIndex index)
        {
            //つながっていなければ0を返す
            if(currentGamePads[index].IsConnected==false)
            {
                return Vector2.Zero;
            }

            //毎ループ初期化
            padVelocity = Vector2.Zero;

            //右
            if(currentGamePads[index].IsButtonDown(Buttons.DPadRight))
            {
                padVelocity.X += 1.0f;
            }
            //左
            if(currentGamePads[index].IsButtonDown(Buttons.DPadLeft))
            {
                padVelocity.X -= 1.0f;
            }
            //上
            if(currentGamePads[index].IsButtonDown(Buttons.DPadUp))
            {
               // padVelocity.Y -= 1.0f;
            }
            //下
            if(currentGamePads[index].IsButtonDown(Buttons.DPadDown))
            {
              //  padVelocity.Y += 1.0f;
            }

            //正規化
            if(padVelocity.Length()!=0)
            {
                padVelocity.Normalize();
            }
            return padVelocity;
        }
        #endregion　ゲームパッド関連

    }
}
