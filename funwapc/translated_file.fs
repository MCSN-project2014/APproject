let add x  y  = 
    x + y
let minus x  y  = 
    x - y
    let mutable result = 0

    let mutable value = 0

    let mutable op = Console.ReadLine()


    while op <> 9do
        value <- Console.ReadLine()

        if op = 1 then
            result <- let task0 = Async.StartAsTask( async{ returnadd  result  value })

        if op = 2 then
            result <- let task1 = Async.StartAsTask( async{ returnminus  result  value })

        op <- Console.ReadLine()


    
    printfn(result)

let add x  y  = 
    x + y
let minus x  y  = 
    x - y
    let mutable result = 0

    let mutable value = 0

    let mutable op = Console.ReadLine()


    while op <> 9do
        value <- Console.ReadLine()

        if op = 1 then
            result <- let task0 = Async.StartAsTask( async{ returnadd  result  value })

        if op = 2 then
            result <- let task1 = Async.StartAsTask( async{ returnminus  result  value })

        op <- Console.ReadLine()


    
    printfn(result)

