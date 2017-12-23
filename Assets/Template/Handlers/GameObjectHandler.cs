using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectHandler  
{
    private bool _isDoneLoading;
    private GameObject _prefab;
    private GameObject[] _pool;
    private List<GameObject> _loosePool;
    private int _poolSize;
    private int _maxLooseSize;
    private bool _expandable;
    public GameObject[] Pool { get { return _pool; } }
    public List<GameObject> LoosePool { get { return _loosePool; } }
    public GameObjectHandler(GameObject prefab,int poolSize, bool expandable = true, string parent = "")
    {
        _prefab = prefab;
        _poolSize = poolSize;
        _maxLooseSize = _poolSize/4;
        var defaultPos = new Vector3();
        Transform parentTrans = null;
        if (!String.IsNullOrEmpty(parent))
        {
            parentTrans = GameObject.Find(parent).transform;
        }
        _pool = new GameObject[_poolSize];
        _loosePool = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            var gm = (GameObject)GameObject.Instantiate(_prefab, defaultPos, Quaternion.identity);
            gm.SetActive(false);
            _pool[i] = gm;
            if (parentTrans != null)
            {
                gm.transform.parent = parentTrans;
            }
        }
        _expandable = expandable;
        _isDoneLoading = true;
    }
     public GameObject RetrieveInstance(Vector3 position, Quaternion rotation,string parent)
    {
         Transform parentTrans = null;
        if (!String.IsNullOrEmpty(parent))
        {
            parentTrans = GameObject.Find(parent).transform;
        }
        for (int i = 0; i < _poolSize; i++)
        {
            var gm = _pool[i];
            if (gm != null && !gm.activeSelf)
            {
                gm.transform.position = position;
                gm.transform.rotation = rotation;
                gm.transform.parent = parentTrans;
                gm.SetActive(true);
                return gm;
            }
        }
        if (_expandable)
        {
            var newGm = (GameObject) GameObject.Instantiate(_prefab, position, rotation,parentTrans);
            _loosePool.Add(newGm);
            if (_loosePool.Count >= _maxLooseSize)
            {
                var temp = _pool.ToList();
                temp.AddRange(_loosePool);
                _pool = temp.ToArray();
                _loosePool.Clear();
            }
            return newGm;
        }
       return null;
    }
    public void DeactivateAll()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            _pool[i].SetActive(false);
        }
    }
    public bool IsDoneLoading()
    {
        return _isDoneLoading;
    }
}