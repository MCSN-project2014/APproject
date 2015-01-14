
/* 
Two dasync{} execution in order to find  the maximun  of four numbers.
*/

fun greater( a int , b int) int {
	if a > b {
		return a;
	}
	else {
		return b;
	}
}

fun greaterDasync ( a int, b int, c int, d int ) int {
	
	var u url = 'http://funwap.sfcoding.com/';
	var r1 int;
	var r2 int;
	var r3 int;
	r1 = dasync{ u , return greater( a , b ) };
	r2 = dasync{ u , return greater( c , d ) };
	r3 = greater( r1 , r2 );
	return r3;

}


fun main (){
	var a int = 4 ;
	var b int = 2 ;
	var c int = 5;
	var d int = 1 ;

	var res int = greaterDasync( a , b , c , d );
	println( res ) ;
	
}