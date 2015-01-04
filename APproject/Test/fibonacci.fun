fun fib(n int) int{
	var retVal int;
	if  n == 0 {
		retVal = 1;
	}
	if  n == 1 {
		retVal = 1;
	}
	else {
		retVal = (fib(n-1) + fib(n-2));
	}
	return retVal;
}

fun main(){
	var retVal int = fib(5);
	println(retVal);
	return retVal;
}