using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APproject.FSCodeGenerator
{
  	public enum TaskLabels {Task , NoTask };

    class FSCodeGenEnvironment
    {

        //List<Dictionary< Obj, TaskLabels>> memory;
        //  Dictionary<string, Task< int>> tasksMemory;
        //Stack<Dictionary<string, Task<int>>> environmentTasks; //the function name (string) associated with the task<int>
        Stack<Dictionary<string, string>> environmentTasks;

        public FSCodeGenEnvironment()
        {
            environmentTasks = new Stack<Dictionary<string, string>>();

        }


        public void  addScope(){

             environmentTasks.Push( new Dictionary<string,string>);
        }

        public void  removeScope(){
            environmentTasks.Pop();
        }

        public Dictionary<string, string> getScope()
        {
            return environmentTasks.Peek();
        }


        public void updateTask( string s , string task ){
           Dictionary<string, string> tempDictionary = environmentTasks.Peek();
               if (tempDictionary.ContainsKey(s))
               {
                   tempDictionary[s]= task;
               }
        }

        



    }
}
