﻿using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using MVVMSidekick.Startups;
using ImageLib;
using ImageLib.Cache.Memory.CacheImpl;
using ImageLib.Cache.Storage;
using ImageLib.Cache.Storage.CacheImpl;
using ImageLib.Gif;
using Q42.WinRT.Data;
using Q42.WinRT.Storage;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace CnBetaUWA
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

		public static void InitNavigationConfigurationInThisAssembly()
		{
			StartupFunctions.RunAllConfig();
            InitImagLab();
            InitQ42();
            InitAppSetting();
        }

        private static void InitAppSetting()
        {
            if (!SettingsHelper.Contains(CnBetaHelper.SettingNightMode))
            {
                SettingsHelper.Set(CnBetaHelper.SettingNightMode, false);
            }

            if (!SettingsHelper.Contains(CnBetaHelper.SettingImageMode))
            {
                SettingsHelper.Set(CnBetaHelper.SettingImageMode, true);
            }

            if (!SettingsHelper.Contains(CnBetaHelper.SettingSelectedTotics))
            {
                var topics = new List<TopicType>
                {
                    new TopicType
                    {
                        Id = 9,
                        IsSelected = true,
                        Name = "Apple",
                        
                        LogoUrl = "http://static.cnbetacdn.com/topics/apple.png"
                    },
                    new TopicType
                    {
                        Id = 52,
                        IsSelected = true,
                        Name = "Google",
                        
                        LogoUrl = "http://static.cnbetacdn.com/topics/850a52010eb551a.png"
                    }
                };
               
                var jsontopics = SerializerHelper.ToJson(topics);
                SettingsHelper.Set(CnBetaHelper.SettingSelectedTotics, jsontopics);
            }

        }
        private  static void InitImagLab()
        {
         
            ImageConfig.Initialize(new ImageConfig.Builder()
            {
                CacheMode = ImageLib.Cache.CacheMode.MemoryAndStorageCache,
                IsLogEnabled = true,
                MemoryCacheImpl = new WeakMemoryCache<string, IRandomAccessStream>(),
                StorageCacheImpl = new LimitedStorageCache(ApplicationData.Current.LocalCacheFolder,
              "ImagLabCache", new SHA1CacheGenerator(), 1024 * 1024 * 1024)
            }.AddDecoder<GifDecoder>().Build());
        }

        private async static void InitQ42()
        {
            await WebDataCache.Init();
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            //Init MVVM-Sidekick Navigations:
            InitNavigationConfigurationInThisAssembly();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
               // rootFrame.Navigate(typeof(BlankPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
