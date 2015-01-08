/** 
A test for async 
computing the Fibonacci sequence 
for all numbers up until 5.
**/ 

fun bib(n int) int{
	var retVal int = -1;
	if  n == 0 || n==1{
		retVal = 1;
	}else {
		 retVal=(bib(n-1) + bib(n-2));
	}
	return retVal;
}


fun main(){
	var tmp1 int;
	var tmp2 int;
	var tmp3 int;
	var tmp4 int;
	var tmp5 int;
	
	tmp1 = async{ return bib(5) };
	tmp2 = async{ return bib(4) };
	tmp3 = async{ return bib(3) };
	tmp4 = async{ return bib(2) };
	tmp5 = async{ return bib(1) };
	
	println(tmp1);
	println(tmp2);
	println(tmp3);
	println(tmp4);
	println(tmp5);
	
}

/** F# code generated:
open System
open System.IO
open System.Threading.Tasks

let rec bib n  =
    let n = ref(n)
    let retVal = ref(-1)
    if !n = 0 || !n = 1 then
        retVal := 1
    else
        retVal := bib (!n - 1) + bib (!n - 2)
    !retVal

[<EntryPoint>]
let main argv = 
    let tmp1 = ref (0)
    let mutable _task_tmp1 = Async.StartAsTask( async{ return 0})
    let tmp2 = ref (0)
    let mutable _task_tmp2 = Async.StartAsTask( async{ return 0})
    let tmp3 = ref (0)
    let mutable _task_tmp3 = Async.StartAsTask( async{ return 0})
    let tmp4 = ref (0)
    let mutable _task_tmp4 = Async.StartAsTask( async{ return 0})
    let tmp5 = ref (0)
    let mutable _task_tmp5 = Async.StartAsTask( async{ return 0})
    let _par_50 = 5
    _task_tmp1 <- Async.StartAsTask( async{ return bib _par_50 })
    let _par_41 = 4
    _task_tmp2 <- Async.StartAsTask( async{ return bib _par_41 })
    let _par_32 = 3
    _task_tmp3 <- Async.StartAsTask( async{ return bib _par_32 })
    let _par_23 = 2
    _task_tmp4 <- Async.StartAsTask( async{ return bib _par_23 })
    let _par_14 = 1
    _task_tmp5 <- Async.StartAsTask( async{ return bib _par_14 })
    Console.WriteLine( _task_tmp1.Result )
    Console.WriteLine( _task_tmp2.Result )
    Console.WriteLine( _task_tmp3.Result )
    Console.WriteLine( _task_tmp4.Result )
    Console.WriteLine( _task_tmp5.Result )
    Console.ReadLine()|>ignore
    0

**/