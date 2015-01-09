
/**
This code in funw@p test the Dasync method.

The server executes the checkPositive method :
- if the number inserted is greater tha zero, return true
- otherwise return false.

**/

fun checkPositive( n int  ) bool {
	if n > 0 {
	return true ;
	}
	else{
	return false;}
}

fun checkDasync( ) bool {
	var b bool;
	var u url = 'http://127.0.0.1:5000';
	var n int ;
	println( "Insert an integer, Server check if is greater than zero. Insert O for Exit.");
	n = readln();
	while n != 0 {	
			b = dasync{ u , return checkPositive( n ) };
			println( b);
			n  =  readln();
	}
	return b;
}



fun main (){
	checkDasync ( );
}
