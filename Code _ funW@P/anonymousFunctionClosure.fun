/** A test for closures of
anonymous functions
**/ 

fun outsideadder() fun(int) int{
	var sum int = 10;
	return fun(x int) int{
		sum = sum + x;
		return sum;
	};
}

fun main(){

	var adder1 fun = outsideadder();
	println(adder1(5));
	//result should be 15
	
	var adder1bis fun = adder1;
	println(adder1bis(27));
	//result should be 42

	var adder2 fun = outsideadder();
	println(adder2(3));
	//result should be 13
}

/** F# code generated:
open System
open System.IO
open System.Threading.Tasks

let  outsideadder() =
    let sum = ref(10)
    fun  x  ->
        let x = ref(x)
        sum := !sum + !x
        !sum


[<EntryPoint>]
let main argv = 
    let adder1 = ref(outsideadder ())
    Console.WriteLine( !adder1 (5) )
    let adder1bis = ref(!adder1)
    Console.WriteLine( !adder1bis (27) )
    let adder2 = ref(outsideadder ())
    Console.WriteLine( !adder2 (3) )
    Console.ReadLine()|>ignore
    0
**/