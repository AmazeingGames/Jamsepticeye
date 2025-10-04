using System;
using System.Collections.Generic;

public class ObservableList<T> : List<T>
{
    public event Action<T> ItemAdded;
    public event Action<T> ItemRemoved;

    public new void Add(T item)
    {
        base.Add(item);
        ItemAdded?.Invoke(item);
    }

    public new bool Remove(T item)
    {
        bool removed = base.Remove(item);
        if (removed)
            ItemRemoved?.Invoke(item);
        return removed;
    }

    public new void Clear()
    {
        foreach (var item in this)
            ItemRemoved?.Invoke(item);
        base.Clear();
    }
}