fun fib(n int) int{
	var retVal int = -1;
	if  n == 0 {
		retVal = 0;
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
	return fib(5);
}