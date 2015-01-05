open System
open System.IO

let outsideAdder = 
    let  sum = ref 10
    
    fun  x  -> 
        sum.Value <- sum.Value + x
        sum.Value 


[<EntryPoint>]
let main argv = 
    let mutable adder = outsideAdder 
    let mutable anotherAdder = Unchecked.defaultof<'a>
    
    Console.WriteLine(adder  (5) )
    let anotherAdder = adder
    
    Console.WriteLine(anotherAdder  (5) )
    Console.ReadLine()|>ignore
    0
