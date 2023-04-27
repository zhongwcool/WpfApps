using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace App16.Python.Control;

public class SerialTaskExecutor
{
    private readonly ConcurrentQueue<Action> _taskQueue = new();

    public void AddTask(Action newTask)
    {
        _taskQueue.Enqueue(newTask);
        if (_taskQueue.Count == 1) // 如果是第一个任务，则启动任务执行
        {
            if (!_inProcess) ExecuteTasksSequentially();
        }
    }

    private bool _inProcess = false;

    private async Task ExecuteTasksSequentially()
    {
        _inProcess = true;
        while (_taskQueue.TryDequeue(out var task))
        {
            await Task.Run(task);
        }

        _inProcess = false;
    }
}