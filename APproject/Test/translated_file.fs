open System
open System.IO

let test x  = 

    let mutable x = x
    x <- x + 1
    x

[<EntryPoint>]
let main argv = 
    let mutable t = true
    let mutable s = 0
    if t = false then
        s <- 0
        s <- 1
        s <- 3

    else
        s <- 1
    Console.ReadLine()|>ignore
    0
open System
open System.IO

let rec fib n  = 
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
