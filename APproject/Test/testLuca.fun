fun add(x int, y int) int{
	return x+y;
}

fun minus(x int, y int) int{
	return x-y;
} 

fun main(){
	var op int = readln{};
	var x int = readln{};
	var y int = readln{};
	var result int;
	if op == 1 {
		result = async{return add(x,y)};
	}else {
		if op==2 {
			result = async{return minus(x,y)};
		}
	}
	println{result};

}