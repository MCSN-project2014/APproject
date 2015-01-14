
/* 
Test the dasync{} method.
The server checks if a number is positive then return true,
otherwise false.
For exit the user must type "0".
*/


fun positive ( a int ) bool {
	if a > 0 {
		return true;
	}
	else {
		return false;
	}
}


fun main (){
	var n int = -1;
	var u url ;
	println("Insert the url of the server");
	u = readln();
	while n != 0 {
		println("Insert a number,the server checks if is positive. For exit type 0");
		n = readln();
		var u url = 'http://funwap.sfcoding.com/';
		var r bool ;
		r = dasync{ u , return positive( n ) };
		println( r );
	
	}



}