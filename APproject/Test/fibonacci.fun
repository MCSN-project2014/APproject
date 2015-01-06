fun fib(n int) int{
	var retVal int = -1;
	if  n == 0 || n==1{
		retVal = 1;
	}else {
		 retVal=(fib(n-1) + fib(n-2));
	}
	return retVal;
}

fun main(){
	var x int= 6;
	var retVal int = fib(x);
	println(retVal);
}