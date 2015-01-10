fun add() int{
   return 5;
}

fun main(){
var x int = add();
x = x + add() * (add());
println(x);
}