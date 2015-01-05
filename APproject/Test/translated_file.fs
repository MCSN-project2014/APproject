open System
open System.IO

let outsideAdder = 
    let  sum = ref 10
    
    fun  x  -> 
        sum.Value <- sum.Value + x
        sum.Value 


[<EntryPoint>]
let main argv = 
    let mutable adder = outsideAdder 
    let mutable anotherAdder = Unchecked.defaultof<'a>
    
    Console.WriteLine(adder  (5) )
    let anotherAdder = adder
    
    Console.WriteLine(anotherAdder  (5) )
    Console.ReadLine()|>ignore
    0
open System
open System.IO

let fib n  = 
    let mutable retVal = -1
    if n = 0 || n = 1 then
        retVal <- 1

    else
        retVal <- fib  (n - 1)  + fib  (n - 2) 
    
    retVal

[<EntryPoint>]
let main argv = 
    let mutable retVal = fib  (5) 
    
    Console.WriteLine(retVal)
    Console.ReadLine()|>ignore
    0
