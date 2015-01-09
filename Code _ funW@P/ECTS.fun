/** A test for nested if.
Loads of them!
**/ 

fun main(){
	println("ECTS Grade Converter - Italy");
	var input int;
	while input != 33 {
		println("Insert your grade between 1 and 32(e lode) - EXIT 33");
		input = readln();
		if input >= 18 && input != 33{
			println("You passed the exam with:");
			if input == 32 && input != 33 {
				println("A+");
			}
			else{
				if input >= 27 && input != 33 {
					println("A");
				}
				else {
					if input >= 24 && input != 33 {
						println("C");	
					}
					else{
						if input >= 20 && input != 33 {
							println("D");
						}
						else {
							if input != 33{
								println("E");
							}
						}
						}
				}
			}
		} else {
			
			if input != 33{
				println("You failed the exam");
				println("F");
			}
		}
	}
	
	
}

/** F# code generated:
open System
open System.IO
open System.Threading.Tasks


[<EntryPoint>]
let main argv = 
    Console.WriteLine( "ECTS Grade Converter - Italy" )
    let input = ref (0)
    while !input <> 33 do
        Console.WriteLine( "Insert your grade between 1 and 32(e lode) - EXIT 33" )
        input := Convert.ToInt32(Console.ReadLine())
        if !input >= 18 && !input <> 33 then
            Console.WriteLine( "You passed the exam with:" )
            if !input = 32 && !input <> 33 then
                Console.WriteLine( "A+" )
            else
                if !input >= 27 && !input <> 33 then
                    Console.WriteLine( "A" )
                else
                    if !input >= 24 && !input <> 33 then
                        Console.WriteLine( "C" )
                    else
                        if !input >= 20 && !input <> 33 then
                            Console.WriteLine( "D" )
                        else
                            if !input <> 33 then
                                Console.WriteLine( "E" )
        else
            if !input <> 33 then
                Console.WriteLine( "You failed the exam" )
                Console.WriteLine( "F" )
    Console.ReadLine()|>ignore
    0

**/