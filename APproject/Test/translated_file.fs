

let infinyt n : int =
    while true do
        n=n-1 |>ignore 
    n

let minus n =
    n-1

let  taskB = Async.StartAsTask( async{ return infinyt 2 }  )
let  taskC = Async.StartAsTask( async{ return minus 7  }  )

printf("prima B\n")
printf " is task c %d \n" taskC.Result
taskB.Wait()
printf("passato B\n")
taskC.Wait()

printf " is task b %d \n" taskB.Result
printf " is task c %d \n" taskC.Result