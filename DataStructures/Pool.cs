using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{

    private Func<T> factory;

    public event Action<T> onInit;
    public event Action<T> onDispose;

    private List<T> available;
    private HashSet<T> unavailable;

    //Opcionales
    public Func<int, int> resizeAddStrategy = x => x;

    public Pool(Func<T> Factory, Action<T> OnInit, Action<T> OnDispose, int initialCount)
    {
        //cachea
        factory = Factory;
        onInit += OnInit;
        onDispose += OnDispose;

        //inicializa
        available = new List<T>();
        unavailable = new HashSet<T>();

        //pupula babosa popula!
        Populate(initialCount);
    }


    public T Get()
    {
        //Si no hay disponibles resizea
        if (available.Count < 1) Populate(resizeAddStrategy(unavailable.Count));

        //Saca de disponibles
        var lastIndex = available.Count - 1;
        var obj = available[lastIndex];
        available.RemoveAt(lastIndex);

        //Mete en usados
        unavailable.Add(obj);

        //Incializa
        if (onInit != null) onInit(obj);

        return obj;
    }

    //Es realemente necesario guardar los que estan siendo usados?
    public void Release(T obj)
    {
        if (unavailable.Contains(obj))
        {
            //Saca de usados
            unavailable.Remove(obj);

            //Mete en disponibles
            available.Add(obj);

            //Finaliza
            if (onDispose != null) onDispose(obj);
        }
        else if(available.Contains(obj))
        {
            Debug.Log("Object alredy released. Check your logic.");
        }
        else Debug.Log("Object foreign to pool. Check your logic.");
    }

    private void Populate(int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            var obj = factory();
            onDispose(obj);
            available.Add(obj);
        }
    }

}
