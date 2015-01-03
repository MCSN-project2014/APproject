fun add(x int, y int) int{
	return x+y;
}

fun minus(x int, y int) int{
	return x-y;
} 

fun main(){
	var result int = 0;
	var value int;
	var op int = readln();
	while op != 9 {
		value = readln();
		if op == 1 {
			result = async{return add(result,value)};
		}
		if op==2 {
			result = async{return minus(result,value)};
		}
		op = readln();
	}
	println(result);
}