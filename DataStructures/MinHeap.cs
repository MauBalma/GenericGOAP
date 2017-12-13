using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<TData, TPriotity> : IHeap<TData, TPriotity> where TPriotity : IComparable
{
    private List<Tuple<TData, TPriotity>> data;

    public bool IsEmpty { get { return data.Count <= 0; } }

    public MinHeap()
    {
        data = new List<Tuple<TData, TPriotity>>();
    }

    public void Insert(Tuple<TData, TPriotity> kvPair)
    {
        data.Add(kvPair);

        var curIndex = data.Count - 1;
        if (curIndex == 0) return;

        var parentIndex = (int)(curIndex - 1) / 2;

        while (data[curIndex].Item2.CompareTo(data[parentIndex].Item2) < 0)
        {
            Swap(curIndex, parentIndex);

            curIndex = parentIndex;
            parentIndex = (int)(curIndex - 1) / 2;
        }
    }

    public void Insert(TData data, TPriotity priority)
    {
        Insert(new Tuple<TData, TPriotity>(data, priority));
    }

    public Tuple<TData, TPriotity> ExtractPair()
    {
        var min = data[0];

        data[0] = data[data.Count - 1];
        data.RemoveAt(data.Count - 1);

        if (data.Count == 0) return min;

        int curIndex = 0;
        int leftIndex = 1;
        int rightIndex = 2;
        int minorIndex;

        if (data.Count > rightIndex)
            minorIndex = data[leftIndex].Item2.CompareTo(data[rightIndex].Item2) < 0 ? leftIndex : rightIndex;
        else if (data.Count > leftIndex)
            minorIndex = leftIndex;
        else return min;

        while (data[minorIndex].Item2.CompareTo(data[curIndex].Item2) < 0)
        {
            Swap(minorIndex, curIndex);

            curIndex = minorIndex;
            leftIndex = curIndex * 2 + 1;
            rightIndex = curIndex * 2 + 2;

            if (data.Count > rightIndex)
                minorIndex = data[leftIndex].Item2.CompareTo(data[rightIndex].Item2) < 0 ? leftIndex : rightIndex;
            else if (data.Count > leftIndex)
                minorIndex = leftIndex;
            else return min;
        }

        return min;
    }

    public TData Pop()
    {
        return ExtractPair().Item1;
    }

    private void Swap(int i1, int i2)
    {
        var aux = data[i1];
        data[i1] = data[i2];
        data[i2] = aux;
    }

}
