fun test() int{
  return 1;
}

fun main(){
  var a int = async {return test()};
  a = a + 1;
  println(a);
}
