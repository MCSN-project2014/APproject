/** A test for boolean 
operators and async. It
computes the result according
to a Reduce pattern.
**/ 

fun max(n int, m int) int{
	var result int;
	if n >= m { result = n;}
	else { result = m;}
	return result;
}

fun main(){
	
	var x int = 3;
	var y int = 4;
	var z int = 103;
	var w int = -100;
	var u int = 62;
	
	
	x = async{ return max(x, y)};
	y = async{ return max(z, w)};
	z = u;
	
	w = async{ return max(x,y)};
	x = async{ return max(w,u)};
	
	println(x);
	
}
