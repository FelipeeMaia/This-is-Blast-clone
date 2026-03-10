using System.Collections.Generic;
using UnityEngine;

namespace Blast.Pooling
{
    public class ObjectPool<TObject, TInfo> where TObject : MonoBehaviour, IPoolable<TObject, TInfo>
    {
        private Queue<TObject> ObjectsToSapwn;
        private List<TObject> ObjectsSapwned;
        private int _spawnCap;

        public TObject Spawn(TInfo spawnInfo)
        {
            TObject obj = null;

            if (ObjectsToSapwn.Count > 0)
            {
                obj = ObjectsToSapwn.Dequeue();
                ObjectsSapwned.Add(obj);

                obj.OnSpawn(spawnInfo);
                obj.gameObject.SetActive(true);
            }

            return obj;
        }

        public void Despawn(TObject obj)
        {
            ObjectsSapwned.Remove(obj);
            ObjectsToSapwn.Enqueue(obj);

            obj.gameObject.SetActive(false);
        }

        public ObjectPool(TObject objectPrefab, int spawnCap, Transform objectsParent)
        {
            _spawnCap = spawnCap;

            ObjectsToSapwn = new Queue<TObject>();
            ObjectsSapwned = new List<TObject>();

            for (int i = 0; i < _spawnCap; i++)
            {
                TObject newObject = GameObject.Instantiate(objectPrefab);
                newObject.transform.parent = objectsParent;
                ObjectsToSapwn.Enqueue(newObject);

                newObject.ReturnToPool += Despawn;
                newObject.gameObject.SetActive(false);
            }
        }
    }
}