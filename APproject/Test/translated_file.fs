open System
open System.IO

let mutable a = 0
while a <> 10 do
    
    Console.WriteLine(a)
    a <- a + 1
open System
open System.IO

let rec fib n  = 
    let mutable retVal = 
    if n = 0 then
        retVal <- 0
    if n = 1 then
        retVal <- 1

    else
        retVal <- fib  n - 1  + fib  n - 2 
    
    retVal
let mutable f = fib  5 
