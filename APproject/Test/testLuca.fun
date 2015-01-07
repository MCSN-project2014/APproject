fun add(x int, y int) int{
	return x+y;
}

fun minus(x int, y int) int{
	return x-y;
} 

fun test1(a int,b int) bool{
	return a==b;
}

fun main(){
	var result int = 0;
	var value int;
	var test bool;
	test = async{return test1(4+4-3+add(3,456),5)}; 
	var op int = 1;
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
