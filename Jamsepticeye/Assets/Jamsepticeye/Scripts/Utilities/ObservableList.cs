using System;
using System.Collections.Generic;

public class ObservableList<T> : List<T>
{
    public event EventHandler<T> AddedItemEventHandler;
    public event EventHandler<T> RemovedItemEventHandler;

    public new void Add(T item)
    {
        base.Add(item);
        AddedItemEventHandler?.Invoke(this, item);
    }

    public new bool Remove(T item)
    {
        bool removed = base.Remove(item);
        if (removed)
            RemovedItemEventHandler?.Invoke(this, item);
        return removed;
    }

    public new void Clear()
    {
        foreach (var item in this)
            RemovedItemEventHandler?.Invoke(this, item);
        base.Clear();
    }
}