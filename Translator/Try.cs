using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Compiler
{
    public static class Try
    {
        public static T These<T>(params Func<T>[] functions)
        {
            foreach (var function in functions.Where(function => function != null))
            {
                try
                {
                    return function();
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (ThreadInterruptedException)
                {
                    throw;
                }
                catch
                {

                }
            }
            return default(T);
        }

        public static void Each(params Action[] actions)
        {
            foreach (var action in actions.Where(action => action != null))
            {
                try
                {
                    action();
                }
                catch (ThreadAbortException)
                {
                    throw;
                } 
                catch(ThreadInterruptedException)
                {
                    throw;
                }
                catch
                {
                    
                }
            }
        }
    }
}
