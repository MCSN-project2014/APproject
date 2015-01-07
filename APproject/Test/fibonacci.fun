fun fib(n int) int{
	var retVal int = -1;
	if  n == 0 || n==1{
		retVal = 1;
	}else {
		 retVal=(fib(n-1) + fib(n-2));
	}
	return retVal;
}


fun outsideadder() fun(int) int{
	var sum int = 10;
	return fun(x int) int{
		sum = sum + x;
		return sum;
	};
}

fun main(){
	var x int= 6;
	var retVal int = fib(x);
	println(retVal);
	var f1 fun = outsideadder();
	println(f1(5));
	var f2 fun = f1;
	println(f2(5));

	var fAncora fun = outsideadder();
	println(fAncora(5));

}