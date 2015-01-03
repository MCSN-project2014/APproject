
fun add( x int , y int ) int {
	var s int;
	s = x - y;
	return s + x ; 
}

fun add1( x int , y int ) int {
	var s int;
	s = x - y;
	return s + x ; 
}

fun outadd() fun( int ) int {
	var sum int = 10;
	return fun( x int) int {
		sum = sum + x;
		return sum;
	};
}


fun main (){

var t  bool = true ;
var s  int ;


if ( t == false ) {
	s = 0;
	}
else {s = 1;}



}