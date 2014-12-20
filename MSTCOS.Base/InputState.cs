using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace MSTCOS.Base
{
    /// <summary>
    /// 获取玩家输入的类（静态）
    /// </summary>
    public static class InputState
    {
        public static Texture2D MouseTexture;
        public static Texture2D SelectMouseTexture;
        public static Texture2D AttackMouseTexture;
        public static CursorState CursorState = Base.CursorState.Normal;
        

        private static Vector2 currentMousePosition;
        /// <summary>
        /// 当前鼠标位置
        /// </summary>
        public static Vector2 CurrentMousePosition
        {
            get { return currentMousePosition; }
        }

        public static KeyboardState previousKeyboardState= Keyboard.GetState();
        public static KeyboardState currentKeyboardState = Keyboard.GetState();
        public static MouseState previousMouseState = Mouse.GetState();
        public static MouseState currentMouseState = Mouse.GetState();
        public static KeyboardState KeyboardState
        {
            get
            {
                return currentKeyboardState;
            }
        }
        /// <summary>
        /// 在Game中Update函数最前面执行这个方法！
        /// </summary>
        /// <param name="game"></param>
        public static void UpdateInput(Game game)
        {
            CursorState = Base.CursorState.Normal;
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            //if (previousKeyboardState == null)
            //{
            //    previousKeyboardState = Keyboard.GetState();
            //}
            if (game.IsActive)
            {

                currentKeyboardState = Keyboard.GetState();
                currentMouseState = Mouse.GetState();
            }

            currentMousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            
        }
        /// <summary>
        /// 是否按了某个键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyPressed(Keys key)
        {
            
            if (previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否按住某个键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
        /// <summary>
        /// 是否按了某个鼠标键
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            ButtonState cbs = GetCurrentButtonState(button);
            ButtonState pbs = GetPreviousButtonState(button);
  
            if (cbs == ButtonState.Pressed && pbs == ButtonState.Released)
            {
                return true;
            }
            
            return false;
        }
        /// <summary>
        /// 是否按住某个鼠标键
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsMouseButtonDown(MouseButton button)
        {
            ButtonState cbs = GetCurrentButtonState(button);

            if (cbs == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }
        public static ButtonState GetCurrentButtonState(MouseButton button)
        {
            ButtonState cbs = ButtonState.Released ;
            switch (button)
            {
                case MouseButton.LeftButton:
                    cbs = currentMouseState.LeftButton;
                    break;
                case MouseButton.MiddleButton:
                    cbs = currentMouseState.MiddleButton;
                    break;
                case MouseButton.RightButton:
                    cbs = currentMouseState.RightButton;
                    break;
                case MouseButton.XButton1:
                    cbs = currentMouseState.XButton1;
                    break;
                case MouseButton.XButton2:
                    cbs = currentMouseState.XButton2;
                    break;
                default:
                    break;
            }
            return cbs;
        }
        public static ButtonState GetPreviousButtonState(MouseButton button)
        {
            ButtonState pbs = ButtonState.Released;
            switch (button)
            {
                case MouseButton.LeftButton:
                    pbs = previousMouseState.LeftButton;
                    break;
                case MouseButton.MiddleButton:
                    pbs = previousMouseState.MiddleButton;
                    break;
                case MouseButton.RightButton:
                    pbs = previousMouseState.RightButton;
                    break;
                case MouseButton.XButton1:
                    pbs = previousMouseState.XButton1;
                    break;
                case MouseButton.XButton2:
                    pbs = previousMouseState.XButton2;
                    break;
                default:
                    break;
            }
            return pbs;
        }

        public static void DrawMouse(SpriteBatch spriteBatch)
        {
            Texture2D t = null;
            spriteBatch.Begin();
            switch (InputState.CursorState)
            {
                case Base.CursorState.Normal:
                    t = MouseTexture;
                    break;
                case Base.CursorState.Attack:
                    t = AttackMouseTexture;
                    break;
                case Base.CursorState.Select:
                    t = SelectMouseTexture;
                    break;
            }
            if (t != null) spriteBatch.Draw(t, InputState.CurrentMousePosition - new Vector2(4, 0), Color.White);

            spriteBatch.End();
        }
    }
    /// <summary>
    /// 鼠标按键的枚举，由大地无敌在2009年9月29日建立
    /// </summary>
    public enum MouseButton
    {
        LeftButton,
        MiddleButton,
        RightButton,
        XButton1,
        XButton2,
    }

    public enum CursorState
    {
        Normal,
        Select,
        Attack,
    }
}
