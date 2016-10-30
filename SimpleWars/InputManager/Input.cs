﻿namespace SimpleWars.InputManager
{
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The input.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static Input instance;

        /// <summary>
        /// The key state.
        /// </summary>
        private KeyboardState keyState;

        /// <summary>
        /// The previous key state.
        /// </summary>
        private KeyboardState previousKeyState;

        /// <summary>
        /// The mouse state.
        /// </summary>
        private MouseState mouseState;

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        private MouseState previousMouseState;

        /// <summary>
        /// Prevents a default instance of the <see cref="Input"/> class from being created.
        /// </summary>
        private Input()
        {
            this.keyState = Keyboard.GetState();
            this.mouseState = Mouse.GetState();
        }

        /// <summary>
        /// The instance.
        /// </summary>
        public static Input Instance = instance ?? (instance = new Input());



        public void Update()
        {
            this.previousKeyState = this.keyState;
            this.previousMouseState = this.mouseState;

            this.keyState = Keyboard.GetState();
            this.mouseState = Mouse.GetState();
        }

        public bool KeyPressed(params Keys[] keys)
        {
            return keys.Any(key => this.keyState.IsKeyDown(key) && this.previousKeyState.IsKeyUp(key));
        }

        public bool KeyReleased(params Keys[] keys)
        {
            return keys.Any(key => this.keyState.IsKeyUp(key) && this.previousKeyState.IsKeyDown(key));
        }

        public bool KeyDown(params Keys[] keys)
        {
            return keys.Any(key => this.keyState.IsKeyDown(key));
        }

        public bool RightMouseClick()
        {
            return this.mouseState.RightButton == ButtonState.Pressed
                   && this.previousMouseState.RightButton == ButtonState.Released;
        }

        public bool RightMouseHold()
        {
            return this.mouseState.RightButton == ButtonState.Pressed;
        }

        public bool RightMouseRelease()
        {
            return this.mouseState.RightButton == ButtonState.Released;
        }

        public bool LeftMouseClick()
        {
            return this.mouseState.LeftButton == ButtonState.Pressed
                   && this.previousMouseState.LeftButton == ButtonState.Released;
        }

        public bool LeftMouseHold()
        {
            return this.mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool LeftMouseRelease()
        {
            return this.mouseState.LeftButton == ButtonState.Released;
        }
    }
}