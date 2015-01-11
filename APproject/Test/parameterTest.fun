fun add(a int, b int) fun() int{
	return fun() int {
		var i int = 0;
		while i<20{
			println("boooooo");
			a=a+b;
			i = i+1;
		}
		return a;
	};
}

fun add2() fun() int{
	return add(3,5);
}

fun main(){
	var f fun = add(3,5);
	println(f());
}