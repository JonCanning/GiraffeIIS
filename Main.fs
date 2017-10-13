module Main

open Giraffe.HttpHandlers
open Giraffe.Razor.HttpHandlers
open Giraffe.HttpContextExtensions

let app : HttpHandler =
  choose [
    route "/500" >=> razorHtmlView "Error" null
    route "/" >=> razorHtmlView "Index" null
    route "/error" >=> warbler (fun _ -> failwith "BOOM")
  ]