open System
open System.IO
let rec fib n  = 
    let mutable a = 0
    let mutable b = 0
    a <- let task0 = Async.StartAsTask( async{ return fib  n - 1 })
    b <- let task1 = Async.StartAsTask( async{ return fib  n - 1 })
    
    a + b


    open System
open System.IO
let add x  y  = 
   
   x + y

let minus x  y  = 
   
   x - y

let mutable result = 0
let mutable value = 0
let mutable op = Convert.ToInt32(Console.ReadLine())

while op <> 9 do
   value <- Convert.ToInt32(Console.ReadLine())
   if op = 1 then
       result <- let task0 = Async.StartAsTask( async{ return add  result  value })
   if op = 2 then
       result <- let task1 = Async.StartAsTask( async{ return minus  result  value })
   op <- Convert.ToInt32(Console.ReadLine())


Console.WriteLine(result)