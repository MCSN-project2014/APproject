
open System
open System.IO

let add x  y  = 

    let mutable x = x
    let mutable y = y
    let mutable s = 0
    s <- x + y
    s <- x - y
    s <- x * y
    s
let fib n  = 

    let mutable n = n
    let mutable a = 0
    let mutable b = 0
    a <- let task0 = Async.StartAsTask( async{ return fib  (n - 1) })
    b <- let task1 = Async.StartAsTask( async{ return fib  (n - 1) })
    a + b

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
