using Blast.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blast.Pooling
{
    public class ObjectPool<T> : IObjectPool where T : MonoBehaviour, IPoolable<T>
    {
        private Queue<T> _availableObjects;
        private List<T> _spawnedObjects;

        private T _prefab;
        private Transform _activeParent;
        private Transform _inactiveParent;
        private int _initialCap;
        private int _objectCount;

        Action<T> _returnAction;

        public T Spawn(ISpawnData spawnData)
        {
            T obj = _availableObjects.Count > 0
                ? _availableObjects.Dequeue()
                : CreateNewObject(addToQueue: false);

            obj.transform.parent = _activeParent;

            _spawnedObjects.Add(obj);
            
            obj.gameObject.SetActive(true);
            obj.OnSpawn(spawnData);

            return obj;
        }

        public void Despawn(T obj)
        {
            if (!_spawnedObjects.Contains(obj)) return;

            obj.gameObject.SetActive(false);
            obj.transform.parent = _inactiveParent;

            _spawnedObjects.Remove(obj);
            _availableObjects.Enqueue(obj);
        }

        private T CreateNewObject(bool addToQueue = true)
        {
            T newObject = GameObject.Instantiate(_prefab);
            newObject.transform.parent = _inactiveParent;
            newObject.gameObject.SetActive(false);

            newObject.ReturnToPool += _returnAction;
            newObject.ReturnToPool += Despawn;

            _objectCount++;
            newObject.name += $" ({_objectCount})";

            if (addToQueue) _availableObjects.Enqueue(newObject);

            return newObject;
        }

        public ObjectPool(T prefab, int spawnCap, Action<T> returnAction, Transform activeParent, Transform inactiveParent)
        {
            _prefab = prefab;
            _initialCap = spawnCap;
            _returnAction = returnAction;

            _activeParent = activeParent;
            _inactiveParent = inactiveParent is not null ? inactiveParent : activeParent;

            _availableObjects = new Queue<T>();
            _spawnedObjects = new List<T>();
            _objectCount = 0;

            for (int i = 0; i < _initialCap; i++)
            {
                CreateNewObject();
            }


        }
    }
}