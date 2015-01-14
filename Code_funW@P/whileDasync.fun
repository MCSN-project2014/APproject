/** A test for the while loops.
 Also tests Dasync.**/

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
	var  i int= 4;
	var u url = 'http://funwap.sfcoding.com';
	while i > 0 {
		var plus, minus, mul, div int;
		if  i == 1 {
			println("x");
			plus = dasync{ u,return increment(x) };
			minus = dasync{u, return decrement(x) };
			mul = dasync { u,return byTwo(x) };
			div = dasync{ u, return divTwo(x) };
		}
		if  i == 2 {
			println("y");
			plus = dasync{ u, return increment(y) };
			minus =dasync{ u,return decrement(y) };
			mul = dasync{ u, return byTwo(y) };
			div = dasync{ u, return divTwo(y) };
		}
		if  i == 3 {
			println("z");
			plus = dasync{ u, return increment(z) };
			minus = dasync{ u, return decrement(z) };
			mul = dasync{ u, return byTwo(z) };
			div = dasync{ u, return divTwo(z)};
		}
		if  i == 4 {
			println("w");
			plus = dasync{ u, return increment(w) };
			minus = dasync{ u, return decrement(w) };
			mul = dasync{ u, return byTwo(w) };
			div = dasync{ u,return divTwo(w) };
		}
		println(plus);
		println(minus);
		println(mul);
		println(div);
		i = i - 1;
	}
}