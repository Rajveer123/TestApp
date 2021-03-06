﻿using Prism;
using Prism.Ioc;
using XFTest.ViewModels;
using XFTest.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTest.Interface;
using XFTest.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XFTest
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterServices();
            containerRegistry.RegisterPages();
        }

    }
    public static class ContainerRegistryExtensions
    {
        /// <summary>
        /// Register implementing class with interface
        /// </summary>
        /// <param name="containerRegistry"></param>
        public static void RegisterServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarFitApiService, CatFitApiService>();
        }
        /// <summary>
        /// Register view with view models
        /// </summary>
        /// <param name="containerRegistry"></param>
        public static void RegisterPages(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CleaningList, CleaningListViewModel>();
        }
    }
}
