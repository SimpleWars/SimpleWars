namespace SimpleWars.InputManagement
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The input.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// The key state.
        /// </summary>
        private static KeyboardState keyState = Keyboard.GetState();

        /// <summary>
        /// The previous key state.
        /// </summary>
        private static KeyboardState previousKeyState;

        /// <summary>
        /// The mouse state.
        /// </summary>
        private static MouseState mouseState = Mouse.GetState();

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        private static MouseState previousMouseState;

        public static void Update()
        {
            previousKeyState = keyState;
            previousMouseState = mouseState;

            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public static IEnumerable<Keys> GetKeysPressed()
        {
            return keyState.GetPressedKeys();
        }

        public static bool KeyPressed(params Keys[] keys)
        {
            return keys.Any(key => keyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key));
        }

        public static bool KeyReleased(params Keys[] keys)
        {
            return keys.Any(key => keyState.IsKeyUp(key) && previousKeyState.IsKeyDown(key));
        }

        public static bool KeyDown(params Keys[] keys)
        {
            return keys.Any(key => keyState.IsKeyDown(key));
        }

        public static bool RightMouseClick()
        {
            return mouseState.RightButton == ButtonState.Pressed
                   && previousMouseState.RightButton == ButtonState.Released;
        }

        public static bool RightMouseHold()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }

        public static bool RightMouseRelease()
        {
            return mouseState.RightButton == ButtonState.Released;
        }

        public static bool LeftMouseClick()
        {
            return mouseState.LeftButton == ButtonState.Pressed
                   && previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool LeftMouseHold()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool LeftMouseRelease()
        {
            return mouseState.LeftButton == ButtonState.Released;
        }

        public static bool MiddleButtonHold()
        {
            return mouseState.MiddleButton == ButtonState.Pressed;
        }

        public static Vector2 MousePos => mouseState.Position.ToVector2();

        public static Vector2 PrevMountPos => previousMouseState.Position.ToVector2();

        public static int MouseScroll
        {
            get
            {
                if (mouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
                {
                    return -1;
                }
                else if (mouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}