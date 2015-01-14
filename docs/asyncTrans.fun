CODE FUN:
var tmp int = async{ return increment(5) };

CODE F#:
let mutable _task_tmp = Async.StartAsTask( async{ return 0})
let tmp = ref (0)
let _par_<unique_index> : int  = 5
_task_tmp <- Async.StartAsTask( async{ return increment _par_<unique_index>})