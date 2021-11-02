using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks
{
    internal static class TaskExtensions
    {
        public static bool IsCompletedSuccessfully(this Task task)
        {
            return task.IsCompleted && !(task.IsFaulted || task.IsCanceled);
        }
    }
}
