﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarlordsRevenge.Classes;

namespace WarlordsRevenge.FarseerSamples
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        private readonly InputManager _inputManager;
        private bool _isInitialized;

        private readonly List<GameScreen> _screens;
        private readonly List<GameScreen> _screensToUpdate;

        private readonly List<RenderTarget2D> _transitions;

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game)
            : base(game)
        {
            Content = game.Content;
            Content.RootDirectory = "Content";
            //_input = new InputHelper(this);
            _inputManager = new InputManager();
            _inputManager.ShowCursor = true;

            _screens = new List<GameScreen>();
            _screensToUpdate = new List<GameScreen>();
            _transitions = new List<RenderTarget2D>();
        }

        public InputManager InputManager { get { return _inputManager; } }

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        //public LineBatch LineBatch { get; private set; }

        public ContentManager Content { get; private set; }

        public SpriteFonts Fonts { get; private set; }

        //public AssetCreator Assets { get; private set; }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            Fonts = new SpriteFonts(Content);
            base.Initialize();

            _isInitialized = true;
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            //LineBatch = new LineBatch(GraphicsDevice);
            //Assets = new AssetCreator(GraphicsDevice);
            //Assets.LoadContent(Content);
            _inputManager.LoadContent(Content, GraphicsDevice);

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in _screens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in _screens)
            {
                screen.UnloadContent();
            }
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            _inputManager.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _screensToUpdate.Clear();

            foreach (GameScreen screen in _screens)
            {
                _screensToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (_screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];

                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            int transitionCount = 0;
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.TransitionOff)
                {
                    ++transitionCount;
                    if (_transitions.Count < transitionCount)
                    {
                        PresentationParameters pp = GraphicsDevice.PresentationParameters;
                        _transitions.Add(new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents));
                    }
                    GraphicsDevice.SetRenderTarget(_transitions[transitionCount - 1]);
                    GraphicsDevice.Clear(Color.Transparent);
                    screen.Draw(gameTime);
                    GraphicsDevice.SetRenderTarget(null);
                }
            }

            GraphicsDevice.Clear(Color.Black);

            transitionCount = 0;
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.TransitionOff)
                {
                    SpriteBatch.Begin(0, BlendState.AlphaBlend);
                    SpriteBatch.Draw(_transitions[transitionCount], Vector2.Zero, Color.White*screen.TransitionAlpha);
                    SpriteBatch.End();

                    ++transitionCount;
                }
                else
                {
                    screen.Draw(gameTime);
                }
            }

            _inputManager.Draw(SpriteBatch);
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (_isInitialized)
            {
                screen.Initialize();
                screen.LoadContent();
            }

            _screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (_isInitialized)
                screen.UnloadContent();

            _screens.Remove(screen);
            _screensToUpdate.Remove(screen);
        }
    }
}