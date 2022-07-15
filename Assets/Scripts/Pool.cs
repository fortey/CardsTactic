using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private List<GameObject> _pool = new List<GameObject>();

    public GameObject Get()
    {
        if (_pool.Count == 0)
        {
            var go = Instantiate(_prefab, this.transform);
            go.SetActive(true);
            return go;
        }
        else
        {
            var go = _pool[0];
            _pool.RemoveAt(0);
            go.SetActive(true);
            return go;
        }
    }

    public void Push(GameObject go)
    {
        go.SetActive(false);
        _pool.Add(go);
    }
}
