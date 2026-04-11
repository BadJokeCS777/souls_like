using System;
using SL.Services;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace SL.Game.Factories
{
    public class PrefabFactory : IPrefabFactory
    {
        private readonly DiContainer _container;

        public PrefabFactory(DiContainer container)
        {
            _container = container;
        }

        public GameObject InstantiatePrefab(GameObject prefab)
        {
            try
            {
                return Inject(prefab);
            }
            catch (Exception e)
            {
                throw new Exception($"InstantiatePrefab error: {prefab} : \n{e}");
            }
        }

        public GameObject InstantiatePrefab(GameObject prefab, Transform parent) => Inject(prefab, parent);

        public T InstantiatePrefabForComponent<T>(GameObject prefab)
        {
            return InstantiatePrefabForComponent<T>(prefab, null);
        }

        public T InstantiatePrefabForComponent<T>(GameObject prefab, Transform parent)
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return _container.InstantiatePrefab(prefab, position, rotation, parent);
        }

        private GameObject Inject(GameObject prefab)
        {
            var instance = Object.Instantiate(prefab);
            _container.InjectGameObject(instance);
            return instance;
        }

        private GameObject Inject(GameObject prefab, Transform parent)
        {
            var instance = Object.Instantiate(prefab, parent);
            _container.InjectGameObject(instance);
            return instance;
        }
    }
}