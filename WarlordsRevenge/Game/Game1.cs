using System;
using Microsoft.Xna.Framework;
using WarlordsRevenge.Classes;
using WarlordsRevenge.FarseerSamples;

namespace WarlordsRevenge
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
                {
                    PreferMultiSampling = true,
                    PreferredBackBufferWidth = 1600,
                    PreferredBackBufferHeight = 900,
                    IsFullScreen = false
                };
            _graphics.ApplyChanges();
            ConvertUnits.SetDisplayUnitToSimUnitRatio(1.0f);
            IsFixedTimeStep = true;
            //IsMouseVisible = true;

            Window.Title = string.Format("Warlords Revenge [Resolution {0}x{1}]", _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";

            //new-up components and add to Game.Components
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            var frameRateCounter = new FrameRateCounter(ScreenManager) { DrawOrder = 101 };
            Components.Add(frameRateCounter);
        }

        public ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var menuScreen = new MenuScreen("Menu Test");
            menuScreen.AddMenuItem("Section 1", EntryType.Separator, null);
            //menuScreen.AddMenuItem(simple1.GetTitle(), EntryType.Screen, simple1);
            menuScreen.AddMenuItem("Warlords Revenge", EntryType.Screen, new WarlordsRevengeGameScreen());
            menuScreen.AddMenuItem("Section 2", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 3", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 4", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 5", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 6", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 7", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 8", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 9", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 10", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 11", EntryType.Separator, null);
            menuScreen.AddMenuItem("Section 12", EntryType.Separator, null);
            //menuScreen.AddMenuItem("Section 13", EntryType.Separator, null);
            //menuScreen.AddMenuItem("Section 14", EntryType.Separator, null);
            //menuScreen.AddMenuItem("Section 15", EntryType.Separator, null);
            menuScreen.AddMenuItem("", EntryType.Separator, null);
            menuScreen.AddMenuItem("Exit", EntryType.ExitItem, null);

            ScreenManager.AddScreen(new BackgroundScreen());
            //ScreenManager.AddScreen(new WarlordsRevengeGameScreen());
            ScreenManager.AddScreen(menuScreen);
            ScreenManager.AddScreen(new LogoScreen(TimeSpan.FromSeconds(1.0)));
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Window.Title = string.Format("Warlords Revenge [Resolution {0}x{1}]", _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        }
    }
}