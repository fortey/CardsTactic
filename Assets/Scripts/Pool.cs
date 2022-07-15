using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    [SerializeField] private List<T> _pool = new List<T>();

    public T Get()
    {
        if (_pool.Count == 0)
        {
            return Instantiate<T>(_prefab, this.transform);
        }
        else
        {
            var ob = _pool[0];
            _pool.RemoveAt(0);
            ob.gameObject.SetActive(true);
            return ob;
        }
    }

    public void Push(T ob)
    {
        ob.gameObject.SetActive(false);
        _pool.Add(ob);
    }
}
