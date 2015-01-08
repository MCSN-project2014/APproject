
/*

fun add( x int , y int ) int {
	var s int ;
	s = x + y ;
	s = x - y ;
    s = x * y ; 
	return s;
}
*/

fun bib ( n int ) int {
	var retVal int = -1;
	if  n == 0 || n==1 {
		retVal = 1;
	}else {
		 retVal=(bib(n-1) + bib(n-2));
	}
	return retVal;
	}

fun fib ( n int ) int {
	var a int ; 
	var b int ;

	a = async { return bib( n-1 ) };
	b = async { return bib( n-2 ) };
	return a + b;
}




/*

fun outsideAdder( x int , y int) fun ( int ) int {
	var sum int = 10;
	return fun(x int ) int {
		sum = sum + x;
		return sum ;
	};
}

fun add(x int, y int) int{
    return x+y;
}

fun minus(x int, y int) int{
    return x-y;
} 
*/


fun main (){

println(fib (4) );
/*
var result int = 0;
var value int;
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
*/

/*

var t  bool = true ;
var s  int ;

if ( t == false ) {
	s = 0;
	s = 1;
	s = 3;
	}
else {s = 1;}

var adder  fun = outsideAdder();
var anotherAdder fun ;
println ( adder (5 ));
anotherAdder = adder ;
println(anotherAdder(5));

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