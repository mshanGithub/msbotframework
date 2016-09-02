namespace SimpleEchoBot

open System
open System.Net.Http
open System.Web
open System.Web.Http
open System.Web.Routing

open Newtonsoft.Json

type HttpRoute = {
    controller : string
    id : RouteParameter }

type Global() =
    inherit System.Web.HttpApplication() 

    static member RegisterWebApi(config: HttpConfiguration) =
        
        // Configure serialization
        config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling <- NullValueHandling.Ignore
        config.Formatters.JsonFormatter.SerializerSettings.ContractResolver <- Serialization.CamelCasePropertyNamesContractResolver()  
        config.Formatters.JsonFormatter.SerializerSettings.Formatting <- Formatting.Indented

        JsonConvert.DefaultSettings <- fun () -> 
            new JsonSerializerSettings(
                ContractResolver = new Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore)


        // Configure routing
        config.MapHttpAttributeRoutes()
        config.Routes.MapHttpRoute(
            "DefaultApi", // Route name
            "api/{controller}/{id}", // URL with parameters
            { controller = "{controller}"; id = RouteParameter.Optional } // Parameter defaults
        ) |> ignore
    

        // Additional Web API settings

    member x.Application_Start() =
        GlobalConfiguration.Configure(Action<_> Global.RegisterWebApi)
