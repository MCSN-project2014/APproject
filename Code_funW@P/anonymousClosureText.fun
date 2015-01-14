/** 
A test for closures of
anonymous functions.
**/ 

fun outadd() fun(int) int {
	var sum int = 10;
	return fun( x int ) int{
		sum = sum + x;
		return sum;
	};
}

fun main(){
	var adder fun = outadd() ;
	var another fun  = adder ;
	
	println( adder(5)); 		// 15
	println( another(60)); 	// 75

	var yetanother fun = outadd();
	println(yetanother(60)) ; 	//70
	println( adder(60)); 		// 135
	
	
}

