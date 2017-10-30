module Program

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Giraffe.Middleware
open Giraffe.Razor.Middleware
open System.IO

let staticFileOptions = StaticFileOptions()

let configureApp (app : IApplicationBuilder) =
  app
    .UseStatusCodePagesWithReExecute("/{0}")
    .UseExceptionHandler("/500")
    .UseStaticFiles(staticFileOptions)
    .UseGiraffe Main.app
  |> ignore

let configureServices (services : IServiceCollection) =
  Path.Combine(Directory.GetCurrentDirectory(), "Views")
  |> services.AddRazorEngine
  |> ignore

[<EntryPoint>]
let main _ =
  let builder =
    WebHostBuilder()
      .UseKestrel()
      .UseWebRoot(Directory.GetCurrentDirectory())
      .Configure(Action<IApplicationBuilder> configureApp)
      .ConfigureLogging(fun loggerfactory -> loggerfactory.AddConsole() |> ignore)
      .ConfigureServices(Action<IServiceCollection> configureServices)
      .UseIISIntegration()
  builder.Build().Run()
  0