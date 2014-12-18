
/*Comment*/

fun sum(){
	var x int;           // declaration of variable x in other environment        
	x = 3;
	while (x < 10){
		x = x-1;
	}
}

fun main(){
	var x int;           //declaration
	var y int = 7;       //declaration and assignment
	while (true) {
		x = 0;
		var z int = y+x;   
		sum();
	}
}