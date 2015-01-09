

fun minus ( n int , m int ) int {
	return n-m;
}

fun fib (n int) int {
	var b int;
	var g int = 4;
	var f int = 3 ;
	var u url = 'http://127.0.0.1:5000';
	b = dasync{ u , return minus(g , f ) };
	g = dasync{ u , return minus ( g , f )};
	return b+g;
}


fun main (){
	println( fib (4) );
}
