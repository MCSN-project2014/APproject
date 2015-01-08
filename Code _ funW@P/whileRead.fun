/** A test for while loops 
and readln() function. A 
very nerd test indeed.
**/ 

fun main(){
	var input int;
	var attempt bool; // at least two
	
	input = readln();
	
	while ( input != 42 ){ 
		input = readln();
		attempt = true;
	}
	
	println("the answer to life the universe and everything");
	println("https://www.youtube.com/watch?v=aboZctrHfK8");
		
}

/** F# code generated:
open System
open System.IO
open System.Threading.Tasks


[<EntryPoint>]
let main argv = 
    let input = ref (0)
    input := Convert.ToInt32(Console.ReadLine())

    while !input <> 42 do
        input := Convert.ToInt32(Console.ReadLine())

    Console.WriteLine( "the answer to life the universe and everything" )
    Console.WriteLine( "https://www.youtube.com/watch?v=aboZctrHfK8" )
    Console.ReadLine()|>ignore
    0
**/