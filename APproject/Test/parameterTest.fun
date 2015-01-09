fun tick() fun() int{
    var counter int = 0;
    return fun() int{
        counter = counter + 1;
        return counter;
    };
}

fun main(){
    var tick1 fun = tick();
    var tmp int = (tick1());
    while tmp != 10 {
        var x int = readln();
        if tick1() == 5 {
            println("halfway through");
        }
        tmp = tick1();
    }
    println("It's over");
}