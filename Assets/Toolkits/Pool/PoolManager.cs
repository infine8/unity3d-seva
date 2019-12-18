#region Using Statements
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

public class PoolManager : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// Static instance.
    /// </summary>
    private static PoolManager s_instance;
    public static PoolManager Instance
    {
        get { return s_instance; }
        private set { s_instance = value; }
    }

    /// <summary>
    /// Public list maintained for Unity editing. Converted to a Dictionary for
    /// fast lookups at run-time.
    /// </summary>
    public List<PrefabPool> PrefabPoolCollection;

    /// <summary>
    /// Collection of pools.
    /// </summary>
    private static Dictionary<string, PrefabPool> Pools = new Dictionary<string, PrefabPool>();

    #endregion

    #region Methods

    /// <summary>
    /// Local initialize.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        InitializePrefabPools();
    }

    /// <summary>
    /// On level loaded, clear the existing pools.
    /// </summary>
    private void OnLevelWasLoaded()
    {
        Pools.Clear();
    }

    /// <summary>
    /// Initializes all PrefabPools in the collection, and adds them to the Dictionary.
    /// </summary>
    private void InitializePrefabPools()
    {
        if (PrefabPoolCollection == null)
            return;

        foreach (PrefabPool pp in PrefabPoolCollection)
        {
            if (pp == null || pp.Prefab == null)
                continue;

            pp.Awake();
            Pools.Add(pp.Prefab.PrefabName, pp);
        }
    }

    /// <summary>
    /// Returns true if a pool of the specified name already exists.
    /// </summary>
    public static bool PoolExists(string name)
    {
        return Pools.ContainsKey(name);
    }

    /// <summary>
    /// Returns true if a pool of the specified type already exists.
    /// </summary>
    public static bool PoolExists(GameObject go)
    {
        PoolObject po = go.GetComponent<PoolObject>();
        return (po != null && Pools.ContainsKey(po.PrefabName));
    }

    /// <summary>
    /// Spawn a GameObject from the specified pool, if the pool's hard limit
    /// has not been met. If the pool does not exist, the returned GameObject
    /// will be a null reference.
    /// </summary>
    public static GameObject Spawn(string name, Transform parent = null)
    {
        PrefabPool pp;
        var obj = (Pools.TryGetValue(name, out pp)) ? pp.Spawn() : null;

        if (parent != null && obj != null)
        {
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;

        }
        return obj;
    }

    /// <summary>
    /// Spawn a GameObject of the specified type, if the pool's hard limit has not been met.
    /// If the pool does not exist, the GameObject.Instantiate method is used to return a
    /// copy of the specified object.
    /// </summary>
    public static GameObject Spawn(GameObject go, Transform parent = null)
    {
        PoolObject po = go.GetComponent<PoolObject>();

        var obj = (po != null && PoolExists(po.PrefabName)) ? Pools[po.PrefabName].Spawn() : Instantiate(go) as GameObject;
        if (parent != null)
        {
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;

        }
        return obj;
    }

    /// <summary>
    /// Despawn the specified GameObject, returning it to its pool.
    /// If the GameObject has no pool, it is destroyed instead.
    /// </summary>
    public static void Despawn(GameObject go)
    {
        if (go == null)
            return;

        PoolObject po = go.GetComponent<PoolObject>();
        if (po == null || !PoolExists(po.PrefabName))
            Destroy(go);
        else
            Pools[po.PrefabName].Despawn(go);
    }

    /// <summary>
    /// Update the PoolManager.
    /// </summary>
    private void Update()
    {
        foreach (PrefabPool pp in PrefabPoolCollection)
            pp.Poll();
    }

    private void OnDisable()
    {
        Pools.Clear();
        PrefabPoolCollection.Clear();
    }

    #endregion
}