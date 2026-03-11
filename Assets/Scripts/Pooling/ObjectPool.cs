using Blast.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Blast.Pooling
{
    public class ObjectPool<T> : IObjectPool where T : MonoBehaviour, IPoolable<T>
    {
        private Queue<T> _availableObjects;
        private List<T> _spawnedObjects;

        private T _prefab;
        private Transform _parent;
        private int _spawnCap;

        public T Spawn(IData spawnData)
        {
            T obj = _availableObjects.Count > 0
                ? _availableObjects.Dequeue()
                : CreateNewObject(addToQueue: false);

            _spawnedObjects.Add(obj);
            obj.gameObject.SetActive(true);
            obj.OnSpawn(spawnData);

            return obj;
        }

        public void Despawn(T obj)
        {
            if (!_spawnedObjects.Contains(obj)) return;

            obj.gameObject.SetActive(false);

            _spawnedObjects.Remove(obj);
            _availableObjects.Enqueue(obj);
        }

        private T CreateNewObject(bool addToQueue = true)
        {
            T newObject = GameObject.Instantiate(_prefab);
            newObject.transform.parent = _parent;

            newObject.ReturnToPool = Despawn;
            newObject.gameObject.SetActive(false);

            if (addToQueue) _availableObjects.Enqueue(newObject);

            return newObject;
        }

        public ObjectPool(T prefab, int spawnCap, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
            _spawnCap = spawnCap;

            _availableObjects = new Queue<T>();
            _spawnedObjects = new List<T>();

            for (int i = 0; i < _spawnCap; i++)
            {
                CreateNewObject();
            }
        }
    }
}