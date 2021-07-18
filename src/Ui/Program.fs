namespace Ui

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.Media
open Avalonia.Themes.Fluent
open Live.Avalonia

type App() =
    inherit Application()
    
    interface ILiveView with
        member _.CreateView(window: Window) =
            window.Title <- "Duets"
            window.Width <- 1280.0
            window.Height <- 720.0
            window.MinWidth <- 1280.0
            window.MinHeight <- 720.0
            window.TransparencyLevelHint <- WindowTransparencyLevel.AcrylicBlur
            window.Background <- Brushes.Transparent
            window.ExtendClientAreaToDecorationsHint <- true
#if DEBUG
            window.AttachDevTools()
#endif
            Shell.MainControl() :> obj

    override this.Initialize() =
        this.Styles.Add (FluentTheme(baseUri = null, Mode = FluentThemeMode.Dark))
        this.Styles.Load "avares://Ui/Styles.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime ->
            let window = new LiveViewHost(this, fun msg -> printfn $"{msg}");
            window.StartWatchingSourceFilesForHotReloading();
            window.Show();
            base.OnFrameworkInitializationCompleted()
        | _ -> ()

module Program =
    [<EntryPoint>]
    let main (args: string []) =
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)