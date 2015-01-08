

fun add ( x int, y int ) int  {

	return x + y;
}



fun main (){

	var x int= 2;
	var y int = 4;
	var a int = 0;
	var b int = 0 ;
	a = async {return add( x , y ) };
	b = async {return add( x+1, y+1)};
	println( a + b);

}