/*
fun add( x int , y int ) int {
	var s int;
	return s + x ; 
}


fun fib ( n int ) int {
	var a int ; 
	var b int ;

	a = async { return fib( n-1 ) };
	b = async { return fib( n-1 ) };
	return a + b;
}
*/
fun outsideAdder() fun ( int ) int {
	var sum int = 10;
	return fun(x int ) int {
		sum = sum + x;
		return sum ;
	};
}

fun main (){


var adder  fun = outsideAdder();
var anotherAdder fun ;
println ( adder (5 ));
anotherAdder = adder ;
println(anotherAdder(5));



/*
var t  bool = true ;
var s  int ;


if ( t == false ) {
	s = 0;
	}
else {s = 1;}

for var i int =0 ; i < 32 ; i= i+1 {
	var t bool ; 
	var s int ; 
	if ( t == false ) {
		if ( t == false ) {
		if ( t == false ) {
			if ( t == false ) {
			s = 0;
			}
		else {s = 1;}
			}
		else {s = 1;}
		}
	else {if ( t == false ) {
			s = 0;
			}
		else {s = 1;}
		}
		}
	else {s = 1;}
 
}


var a int =0;
while ( a != 10 ){

	println( a );
	a = a+1 ;
}
*/
}