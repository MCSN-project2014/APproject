fun test1(a int,b int) bool{
	return a==b;
}

fun main(){
	var test bool;
	var u url;
	test = dasync{u, test1(4,4)}; 

}
