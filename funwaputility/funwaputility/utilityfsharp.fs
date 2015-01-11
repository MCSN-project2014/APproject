

// define a library for .fun (funW@p) code translated into f#.

namespace funwaputility

open System
open System.Net.Http
open System.Text

module PostMethods = 

    let getPostAsyncInt (url:string, data) = 
        async {
            let httpClient = new System.Net.Http.HttpClient()
            let contentPost:StringContent = new StringContent( data , Encoding.UTF8, "applicatio/json")
            let! response=  httpClient.PostAsync(url, contentPost) |> Async.AwaitTask
            response.EnsureSuccessStatusCode () |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return Convert.ToInt32(content)
            }

    let getPostAsyncBool (url:string, data) = 
        async {
            let httpClient = new System.Net.Http.HttpClient()
            let contentPost:StringContent = new StringContent( data , Encoding.UTF8, "applicatio/json")
            let! response=  httpClient.PostAsync(url, contentPost) |> Async.AwaitTask
            response.EnsureSuccessStatusCode () |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return Convert.ToBoolean(content)
            }

module Readline = 


    let _readln() =
        let mutable tmp = true
        let input = ref(0)
        while tmp do
            try
                input := Convert.ToInt32(Console.ReadLine())
                tmp <- false;
            with
            | :? System.FormatException as ex ->
                Console.WriteLine("funW@P->F# - Only integer input is allowed. Try again.")
        !input