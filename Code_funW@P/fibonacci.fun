/** A test in honor of
a well famous "pisano" and
great mathematician 
http://en.wikipedia.org/wiki/Fibonacci
**/ 

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
	//computes Fibonacci of 6
	var x int = 6;
	var retVal int = fib(x);
	println(retVal);

}


