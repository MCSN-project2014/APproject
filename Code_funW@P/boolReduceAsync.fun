/** A test for boolean 
operators and async. It
computes the result according
to a Reduce pattern.
**/ 

fun max(n int, m int) int{
	var result int;
	if n >= m { result = n;}
	else { result = m;}
	return result;
}

fun main(){
	
	var x int = 3;
	var y int = 4;
	var z int = 103;
	var w int = -100;
	var u int = 62;
	
	
	x = async{ return max(x, y)};
	y = async{ return max(z, w)};
	z = u;
	
	w = async{ return max(x,y)};
	
	x = async{ return max(w,u)};
	
	println(x);
	
}

/** F# code generated:
open System
open System.IO
open System.Threading.Tasks

let  max n  m  =
    let n = ref(n)
    let m = ref(m)
    let result = ref (0)
    if !n >= !m then
        result := !n
    else
        result := !m
    !result

[<EntryPoint>]
let main argv = 
    let mutable _task_x = Async.StartAsTask( async{ return 0})
    let x = ref(3)
    let mutable _task_y = Async.StartAsTask( async{ return 0})
    let y = ref(4)
    let z = ref(103)
    let mutable _task_w = Async.StartAsTask( async{ return 0})
    let w = ref(-100)
    let u = ref(62)
    let _par_x0 = _task_x.Result
    let _par_y1 = _task_y.Result
    _task_x <- Async.StartAsTask( async{ return max _par_x0 _par_y1 })
    let _par_z2 = !z
    let _par_w3 = _task_w.Result
    _task_y <- Async.StartAsTask( async{ return max _par_z2 _par_w3 })
    z := !u
    let _par_x4 = _task_x.Result
    let _par_y5 = _task_y.Result
    _task_w <- Async.StartAsTask( async{ return max _par_x4 _par_y5 })
    let _par_w6 = _task_w.Result
    let _par_u7 = !u
    _task_x <- Async.StartAsTask( async{ return max _par_w6 _par_u7 })
    Console.WriteLine( _task_x.Result )
    Console.ReadLine()|>ignore
    0
**/