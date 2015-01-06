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
