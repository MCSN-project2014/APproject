

fun minus ( n int) int {
	return n-1;
}

fun fib (n int) int {
	var b int;
	var g int = 4;
	var u url = 'http://127.0.0.1:5000';
	b = dasync{ u , minus(g-2) };
	return b;
}


fun main (){
	println( fib (4) );
}
