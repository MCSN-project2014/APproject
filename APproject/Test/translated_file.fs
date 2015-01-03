<<<<<<< HEAD
=======
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


printfn(result)
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


printfn(result)
>>>>>>> b56a1cb3834d77dc97cc98be6e4ffb69b8d4b80a
