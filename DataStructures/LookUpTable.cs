using System;
using System.Collections.Generic;

public class LookUpTable<TKey, TValue>
{

    private Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
    private Func<TKey, TValue> map;

    public LookUpTable(Func<TKey, TValue> map)
    {
        this.map = map;
    }

    public TValue Get(TKey key)
    {
        return dic.ContainsKey(key) ? dic[key] : dic[key] = map(key);
    }

	public void Release(TKey key)
	{
		dic.Remove(key);
	}


}
