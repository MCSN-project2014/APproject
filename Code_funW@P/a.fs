open System
open System.IO
open System.Threading.Tasks
open funwaputility.PostMethods
open funwaputility.Readline

let  positive a  =
    let a = ref(a)
    if !a > 0 then
        true
    else
        false


[<EntryPoint>]
let main argv = 
    let n = ref(-1)
    while !n <> 0 do
        Console.WriteLine( "Insert a number,the server checks if is positive. For exit type 0" )
        n := _readln()
        let u = ref("http://funwap.sfcoding.com/")
        let r = ref (true)
        let mutable _task_r = Async.StartAsTask( async{ return true})
        let _par_n0 : int  = !n
        let tempJsonData = "[{\"a\" : "+ Convert.ToString (_par_n0)+"}]&{\"recursive\":false,\"children\":[{\"recursive\":false,\"children\":[{\"recursive\":false,\"children\":[{\"type\":1,\"recursive\":false,\"value\":{\"name\":\"a\",\"type\":1,\"kind\":0,\"isUsedFromAfun\":false,\"isUsedInAsync\":false,\"isUsedInDasync\":false,\"recursive\":false,\"asyncControl\":false,\"returnIsSet\":false},\"label\":0,\"line\":0,\"column\":0},{\"type\":0,\"recursive\":false,\"value\":0,\"label\":0,\"line\":0,\"column\":0}],\"label\":20,\"line\":0,\"column\":0,\"type\":0},{\"recursive\":false,\"children\":[{\"recursive\":false,\"children\":[{\"type\":0,\"recursive\":false,\"value\":true,\"label\":0,\"line\":0,\"column\":0}],\"label\":11,\"line\":0,\"column\":0,\"type\":0}],\"label\":7,\"line\":0,\"column\":0,\"type\":0},{\"recursive\":false,\"children\":[{\"recursive\":false,\"children\":[{\"type\":0,\"recursive\":false,\"value\":false,\"label\":0,\"line\":0,\"column\":0}],\"label\":11,\"line\":0,\"column\":0,\"type\":0}],\"label\":7,\"line\":0,\"column\":0,\"type\":0}],\"label\":5,\"line\":0,\"column\":0,\"type\":0}],\"label\":7,\"line\":0,\"column\":0,\"type\":0}"
        _task_r <- Async.StartAsTask( getPostAsyncBool( !u,tempJsonData ))
        Console.WriteLine( _task_r.Result )
    Console.ReadLine()|>ignore
    0
