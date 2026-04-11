using UnityEngine;

namespace SL.Services
{
    public interface IPrefabFactory
    {
        GameObject InstantiatePrefab(GameObject prefab);
        GameObject InstantiatePrefab(GameObject prefab, Transform parent);
        T InstantiatePrefabForComponent<T>(GameObject prefab);
        T InstantiatePrefabForComponent<T>(GameObject prefab, Transform parent);
        GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}