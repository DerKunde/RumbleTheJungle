using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> heap = new List<(T item, int priority)>();

    public int Count
    {
        get { return heap.Count; }
    }

    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        int i = heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[parent].priority <= priority)
            {
                break;
            }
            heap[i] = heap[parent];
            i = parent;
        }
        heap[i] = (item, priority);
    }

    public T Dequeue()
    {
        int last = heap.Count - 1;
        T result = heap[0].item;
        int priority = heap[last].priority;
        heap.RemoveAt(last);
        last--;
        int i = 0;
        while (true)
        {
            int left = i * 2 + 1;
            if (left > last)
            {
                break;
            }
            int right = left + 1;
            int child = left;
            if (right <= last && heap[right].priority < heap[left].priority)
            {
                child = right;
            }
            if (heap[child].priority >= priority)
            {
                break;
            }
            heap[i] = heap[child];
            i = child;
        }
        heap[i] = (default(T), priority);
        return result;
    }

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }
}