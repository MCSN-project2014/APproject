fun add() int{
   return 5;
}

fun main(){
var x int = add() + add();
x = x + add();
println(x);
}