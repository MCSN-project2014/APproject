fun test() int{
  return 1;
}

fun main(){
  var a int = dasync {'http://funwap.sfcoding.com', return test()};
  a = a + 1;
  println(a);
}
