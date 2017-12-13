using System;
using System.Collections.Generic;

public interface IHeap<TData, TPriotity>
{
    Tuple<TData, TPriotity> ExtractPair();
    TData Pop();

    void Insert(Tuple<TData, TPriotity> kvPair);
    void Insert(TData data, TPriotity priority);

    bool IsEmpty { get; }
}