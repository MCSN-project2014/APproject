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
