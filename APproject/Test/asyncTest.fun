fun test(a int, b int) int{
	return a + b- 50;
}

fun main(){
	var u url = 'http://127.0.0.1:5000';
	var c int; 
	c = dasync {u, return test(1, 10)};
	println(c);
}
