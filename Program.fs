module Program

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Giraffe.Middleware
open Giraffe.Razor.Middleware
open System.IO

let staticFileOptions = StaticFileOptions()

let configureApp (app : IApplicationBuilder) =
  app.UseStatusCodePagesWithReExecute "/{0}" |> ignore
  app.UseExceptionHandler "/500" |> ignore
  app.UseGiraffe Main.app
  app.UseStaticFiles staticFileOptions |> ignore

let configureServices (services : IServiceCollection) =
  Path.Combine(Directory.GetCurrentDirectory(), "Views")
  |> services.AddRazorEngine
  |> ignore

[<EntryPoint>]
let main args =
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