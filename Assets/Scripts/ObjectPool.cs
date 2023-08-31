using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    [SerializeField] private List<GameObject> _poolObject;
    [SerializeField] private GameObject _objectToPool;
    [SerializeField] private int _amountToPool;

  
    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        _poolObject = new List<GameObject>();
        GameObject poolItemTmp;
        for (int i = 0; i < _amountToPool; i++)
        {
            poolItemTmp = Instantiate(_objectToPool);
            poolItemTmp.SetActive(false);
            _poolObject.Add(poolItemTmp);
        }
    }

    public GameObject GetPoolObject()
    {
        for(int i = 0; i < _amountToPool; i++)
        {
            if (!_poolObject[i].activeInHierarchy)
            {
                return _poolObject[i];
            }
        }
        return null;
    }
 
}