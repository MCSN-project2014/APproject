fun test() bool{
	return true;
}

fun main(){
	var b bool;
	b = async {return test()};
	println(b);
	println(b);
	b = false;
	println(b);
}
