fun add(a fun, b fun) fun(bool)bool{
	var c bool = b();
	return fun (a1 bool) bool{
		return a() != a1;
	};
}

fun main(){
	var y fun = add(fun () bool {
						return true;
					},
					fun () bool{
						println("ciao");
						return true;
					}); 
	println(y(false));
}