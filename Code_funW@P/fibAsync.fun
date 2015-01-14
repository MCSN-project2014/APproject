/** 
A test for async 
computing the Fibonacci sequence 
for all numbers up until 5.
**/ 

fun bib(n int) int{
	var retVal int = -1;
	if  n == 0 || n==1{
		retVal = 1;
	}else {
		 retVal=(bib(n-1) + bib(n-2));
	}
	return retVal;
}


fun main(){
	var tmp1 int;
	var tmp2 int;
	var tmp3 int;
	var tmp4 int;
	var tmp5 int;
	
	tmp1 = async{ return bib(5) };
	tmp2 = async{ return bib(4) };
	tmp3 = async{ return bib(3) };
	tmp4 = async{ return bib(2) };
	tmp5 = async{ return bib(1) };
	
	println(tmp1);
	println(tmp2);
	println(tmp3);
	println(tmp4);
	println(tmp5);
	
}
