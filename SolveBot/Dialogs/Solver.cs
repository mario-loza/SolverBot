using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace SolveBot.Dialogs
{
    public class Solver
    {
        public dynamic Execute(string expression)
        {
            dynamic dynamicResult;
            ScriptEngine engine = Python.CreateEngine();
            try
            {
                dynamicResult = engine.Execute(expression);
            }
            catch
            {
                dynamicResult = "non solvable";
            }

            return dynamicResult;
        }
    }
}




