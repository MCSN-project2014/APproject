
fun add(x int, y int) int {
	return 5;
}


fun main(){
	var x int = 7;
	var y int = 5+3+5+7;
	var result int;
	result = async{return add(x,y)};

}