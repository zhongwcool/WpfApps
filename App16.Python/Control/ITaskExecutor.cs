using System;

namespace App16.Python.Control;

public interface ITaskExecutor
{
    public void AddTask(Action action);
}