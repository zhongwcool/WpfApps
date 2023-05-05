using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App16.Python.Control;

public class Serial2TaskExecutor : ITaskExecutor
{
    private readonly Queue<Task> _taskQueue = new();

    public void AddTask(Action action)
    {
        var newTask = new Task(action);
        lock (_taskQueue) // 队列需要进行同步操作
        {
            _taskQueue.Enqueue(newTask);
            if (_taskQueue.Count == 1) // 如果是第一个任务，则启动任务执行
            {
                ExecuteTasksSequentially();
            }
        }
    }

    private async Task ExecuteTasksSequentially()
    {
        while (true)
        {
            Task task;
            lock (_taskQueue) // 队列需要进行同步操作
            {
                if (_taskQueue.Count == 0) break;
                task = _taskQueue.Peek();
                task.Start();
            }

            await task;
            lock (_taskQueue) // 队列需要进行同步操作
            {
                _taskQueue.Dequeue();
            }
        }
    }
}