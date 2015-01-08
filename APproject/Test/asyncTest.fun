fun test() bool{
	return true;
}

fun main(){
	var b,c bool;
	c = async {return test()};
	b = async {return test()};
	println(b);
	println(c);
	b = false;
	println(b);
}
