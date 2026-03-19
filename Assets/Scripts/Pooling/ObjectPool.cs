using Blast.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blast.Pooling
{
    /// <summary>
    /// Class that reprensents a pool of objects.
    /// </summary>
    /// <typeparam name="T">The Type of object that the pool is made of.</typeparam>
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

            obj.OnReturnToPool += Despawn;
            obj.OnReturnToPool += _returnAction;

            obj.gameObject.SetActive(true);
            obj.OnSpawn(spawnData);

            return obj;
        }

        public void Despawn(T obj)
        {
            if (!_spawnedObjects.Contains(obj)) return;

            obj.gameObject.SetActive(false);
            obj.transform.parent = _inactiveParent;

            obj.OnReturnToPool = null;

            _spawnedObjects.Remove(obj);
            _availableObjects.Enqueue(obj);
        }

        private T CreateNewObject(bool addToQueue = true)
        {
            T newObject = GameObject.Instantiate(_prefab);
            newObject.transform.parent = _inactiveParent;
            newObject.gameObject.SetActive(false);

            _objectCount++;
            newObject.name += $" ({_objectCount})";

            if (addToQueue) _availableObjects.Enqueue(newObject);

            return newObject;
        }

        private Transform CreateParent(string prefix, T prefab, Transform parent)
        {
            var newParent = new GameObject($"{prefix} - {prefab.name}s").transform;
            newParent.position = Vector3.zero;
            newParent.parent = parent;

            return newParent;
        }

        public ObjectPool(T prefab, int spawnCap,Transform parent, Action<T> returnAction = null)
        {
            _prefab = prefab;
            _initialCap = spawnCap;
            _returnAction = returnAction;

            _activeParent = CreateParent("Active", prefab, parent);
            _inactiveParent = CreateParent("Inactive", prefab, parent);

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