open System
open System.IO

let mutable a = 0
while a <> 10 do
    
    Console.WriteLine(a)
    a <- a + 1
open System
open System.IO

let rec fib n  = 
    let mutable retVal = 0
    if n = 0 then
        retVal <- 1
    if n = 1 then
        retVal <- 1

    else
        retVal <- fib  n - 1  + fib  n - 2 
    
    retVal
let mutable retVal = fib  5 

Console.WriteLine(retVal)
