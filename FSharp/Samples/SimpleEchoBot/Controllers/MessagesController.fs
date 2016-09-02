namespace SimpleEchoBot.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Net.Http
open System.Web.Http
open System.Threading
open System.Threading.Tasks
open System.Runtime.CompilerServices

open Microsoft.Bot.Connector
open Microsoft.Bot.Builder
open Microsoft.Bot.Builder.Dialogs

[<Serializable>]
type EchoDialog() = 
    interface IDialog<obj> with
        member x.StartAsync (context : IDialogContext) = 
            async { context.Wait x.MessageReceivedAsync }|> Async.StartAsTask :> Task

    member x.MessageReceivedAsync (context : IDialogContext) (argument : IAwaitable<IMessageActivity>) =
        
        let work = async { return argument.GetAwaiter().GetResult } |> Async.RunSynchronously
        let message = work ()

        // Post message back to user
        context.PostAsync (sprintf "You said: %s" message.Text) 
        |> Async.AwaitIAsyncResult 
        |> Async.Ignore
        |> Async.RunSynchronously

        async { context.Wait x.MessageReceivedAsync } |> Async.StartAsTask :> Task


/// Retrieves values.
[<BotAuthentication>]
type MessagesController() =
    inherit ApiController()

    /// Gets all values.
    member x.Post(activity : Activity) = 

        let getActivityType (value : Activity) = 
            match value with 
            | null -> None 
            | _ -> Some (value.GetActivityType())

        match getActivityType activity with 
        | Some ActivityTypes.ContactRelationUpdate -> ()
        | Some ActivityTypes.ConversationUpdate -> ()
        | Some ActivityTypes.DeleteUserData -> ()
        | Some ActivityTypes.Message -> 

            async {
                do! Conversation.SendAsync(activity, fun () -> new EchoDialog() :> IDialog<obj>) 
                    |> Async.AwaitIAsyncResult 
                    |> Async.Ignore
            } |> Async.RunSynchronously

        | Some ActivityTypes.Ping -> ()
        | Some ActivityTypes.Typing -> ()
        | _ -> () // all cases including None

        new HttpResponseMessage(System.Net.HttpStatusCode.Accepted)
