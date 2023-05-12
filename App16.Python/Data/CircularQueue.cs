using System;
using System.Collections.Generic;

namespace App16.Python.Data;

public class CircularQueue<T>
{
    private readonly T[] _items;
    private int _head;
    private int _tail;

    public CircularQueue(int capacity)
    {
        _items = new T[capacity];
        _head = -1;
        _tail = -1;
    }

    public void Enqueue(T item)
    {
        // if (IsFull())
        //     throw new OverflowException("Queue overflow.");

        if (IsEmpty())
            _head = 0;

        _tail = (_tail + 1) % _items.Length;
        _items[_tail] = item;
    }

    public T Dequeue()
    {
        // if (IsEmpty())
        //     throw new InvalidOperationException("Queue is empty.");

        T item = _items[_head];

        if (_head == _tail)
        {
            _head = -1;
            _tail = -1;
        }
        else
        {
            _head = (_head + 1) % _items.Length;
        }

        return item;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Queue is empty.");

        return _items[_head];
    }

    public bool IsEmpty()
    {
        return _head == -1;
    }

    public bool IsFull()
    {
        return ((_tail + 1) % _items.Length) == _head;
    }

    public int Count()
    {
        if (IsEmpty())
            return 0;

        return (_tail >= _head ? (_tail - _head) : (_items.Length - _head + _tail + 1));
    }

    public List<T> GetList()
    {
        var list = new List<T>();
        if (IsEmpty())
            return list;

        var index = _head;
        while (index != _tail)
        {
            list.Add(_items[index]);
            index = (index + 1) % _items.Length;
        }

        list.Add(_items[_tail]);
        return list;
    }
}