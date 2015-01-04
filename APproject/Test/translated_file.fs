open System
open System.IO

let rec fib n  = 
    let mutable retVal = -1
    if n = 0 || n = 1 then
        retVal <- 1

    else
        retVal <- fib  (n - 1)  + fib  (n - 2) 
    
    retVal
let mutable retVal = fib  (5) 

Console.WriteLine(retVal)
