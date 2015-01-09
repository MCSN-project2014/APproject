/** A test for the "for" translated 
as a while in F. Also tests async.**/

fun decrement (x int) int {
	return x - 1;
}

fun increment (x int) int{
	return x + 1;
}

fun byTwo (x int) int{
	return 2 * x;
}

fun divTwo (x int) int{
	return x / 2;
}

fun main(){
	var x, y, z, w int;
	x = 3;
	y = 5;
	z = 8;
	w = 9;
	for  var i int = 4; i > 0; i = i - 1 {
		var plus, minus, mul, div int;
		if  i == 1 {
			println("x");
			plus = async{ return increment(x) };
			minus = async{ return decrement(x) };
			mul = async { return byTwo(x) };
			div = async{ return divTwo(x) };
		}
		if  i == 2 {
			println("y");
			plus = async{ return increment(y) };
			minus = async{ return decrement(y) };
			mul = async { return byTwo(y) };
			div = async{ return divTwo(y) };
		}
		if  i == 3 {
			println("z");
			plus = async{ return increment(z) };
			minus = async{ return decrement(z) };
			mul = async { return byTwo(z) };
			div = async{ return divTwo(z)};
		}
		if  i == 4 {
			println("w");
			plus = async{ return increment(w) };
			minus = async{ return decrement(w) };
			mul = async { return byTwo(w) };
			div = async{ return divTwo(w) };
		}
		println(plus);
		println(minus);
		println(mul);
		println(div);
	}
}
