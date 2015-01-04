open System
open System.IO

let rec add x  y  = 
    
    x + y
let rec minus x  y  = 
    
    x - y

[<EntryPoint>]
let main argv = 
    let mutable result = 0
    let mutable value = 0
    let mutable op = Convert.ToInt32(Console.ReadLine())

    while op <> 9 do
        value <- Convert.ToInt32(Console.ReadLine())

       
    Console.WriteLine(result)
    Console.ReadLine()|>ignore
    0
