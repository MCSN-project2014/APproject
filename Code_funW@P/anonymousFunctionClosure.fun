/** 
A test for closures of
anonymous functions.
**/ 

fun add5() fun() int{
	var counter int = 0;
	return fun() int{
		counter = counter + 5;
		return counter;
	};
}

fun main(){
	var tick1 fun = add5();
	var tick2 fun = add5();
	
	println(tick1());
	println(tick2());
	println(tick2());
	
	var tick3 fun = tick1;
	var tmp int = tick3();
	println(tick3(););
	
}

