![alt text](funwap_Logo/fun_logo-min.jpg)

**funW@ap** is an imperative-functional programming language, with function as first class citizen and transparent management of asynchronous operation executed local machine or in a different machine ([funw@p-server](https://github.com/MCSN-project2014/funwap-server)).

##How to use funw@p
Funw@p is distributed with the interpreter `funwapi.exe` and the compiler `funwapc.exe`, you can run it in linux or windows command line.
The two executable have the following option:
 ```
 funwapi [OPTIONS] <input_file.fun> [-o <output_file>]
  OPTIONS:
         -v|--verbose   ->    print extra information
         -h|--help      ->    show help
```
```
funwapc [OPTIONS] <input_file.fun>
  OPTIONS:
         -v|--verbose   ->    print extra information
         -h|--help      ->    show help
```
to use the primitive command `dasync{ url , return funCall() }` you have to install and run a [funw@p-server](https://github.com/MCSN-project2014/funwap-server) on your pc or a remote machine.

##Get funW@ap
You can compile the two executible from source, downloading this repository or downloading the binary file from here [here](https://github.com/MCSN-project2014/APproject/releases)

##Example
A simple hello world
```
fun main(){
 println("Hello World");
}
```

The declared function `fun test() fun(int)int` return a function `fun(int)int` that take as argument a int and return a int. You can also read from keyboard with the command `readln()`.
```
fun test() fun(int)int{
 var tmp int = 0;
 return fun(x int) int{
   return tmp + x; 
   }
}

fun main(){
 var f fun = test();
 var read int = readln();
 println(f(read));
 println(f(2));
}
```

##Prerequisites
- .NET 4.5 or Mono 3+
