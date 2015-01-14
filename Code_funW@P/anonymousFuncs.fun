/** 
A test for closures of
anonymous functions
**/ 

fun tick() fun() int{
	var counter int = 0;
	return fun() int{
		counter = counter + 1;
		return counter;
	};
}

fun main(){
	var tick1 fun = tick();
	var tmp int = 0;
	while tmp != 10{
		println("Insert a number");
		var x int = readln();
		if tmp == 5{
			println("halfway through");
		}
		tmp = tick1();
	}
	println("It's over");
}

