#region Using Statements

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.impl;

#endregion

public class PoolObject : EventView
{
    [Inject(ContextKeys.CONTEXT)]
    public IContext Context { get; set; }

    #region Fields

    /// <summary>
    /// The prefab name for this object. GameObject name can be changed
    /// at runtime, and the prefab name is stored to facilitate pooling.
    /// </summary>
    public string PrefabName;

    /// <summary>
    /// Called when the object is spawned.
    /// </summary>
    public EventHandler Spawned;

    /// <summary>
    /// Called when the object is despawned.
    /// </summary>
    public EventHandler Despawned;

    /// <summary>
    /// Dynamic objects do not cache their hierarchy.
    /// </summary>
    public bool IsDynamic = false;

    /// <summary>
    /// Cache a list of attached GameObjects for non-dynamic objects.
    /// </summary>
    private List<GameObject> _gameObjectCache;

    /// <summary>
    /// Cache a list of attached Renderers for non-dynamic objects.
    /// </summary>
    private List<Renderer> _rendererCache;

    public bool IsSpawned { get; private set; }

    /// <summary>
    /// Is the object counting down to an auto-despawn?
    /// </summary>
    private bool _hasDespawnTimer = false;

    public float TimeLastSpawned { get; private set; }

    public float TimeLastDespawned { get; private set; }

    /// <summary>
    /// The time that the despawn timer was initialized.
    /// </summary>
    private float DespawnTimerInitialized = -1f;

    /// <summary>
    /// The total time until the despawn timer is completed.
    /// </summary>
    private float DespawnDelay = -1f;

    public PoolObject()
    {
        TimeLastDespawned = -1f;
        TimeLastSpawned = -1f;
        IsSpawned = false;
    }

    /// <summary>
    /// Get the age of the object since spawn.
    /// </summary>
    protected float Age
    {
        get
        {
            if (!IsSpawned)
                return -1f;
            return Time.time - TimeLastSpawned;
        }
    }

    /// <summary>
    /// Get the age of the object as a scalar relating to the despawn timer. If
    /// a timed despawn has not been triggered, the object's age will always be 1.
    /// </summary>
    private float AgeAsScalar
    {
        get
        {
            if (!IsSpawned) return -1f;
            if (DespawnDelay <= 0f) return 1f;
            return (Time.time - DespawnTimerInitialized) / DespawnDelay;
        }
    }

    #endregion

    #region Methods

    protected virtual void OnPoolInitialize()
    {

    }

    /// <summary>
    /// Local initialize.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _gameObjectCache = new List<GameObject>();
        _rendererCache = new List<Renderer>();
        RefreshCache();
        SetActive(false);

        gameObject.AddComponent<EventView>().enabled = false; //hotfix! For some reason any StrangeIoC inject returns null without it!

        OnPoolInitialize();
    }

    /// <summary>
    /// Calculate the GameObject and Renderer hierarchy.
    /// </summary>
    private void RefreshCache()
    {
        _gameObjectCache.Clear();
        _rendererCache.Clear();
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
            _gameObjectCache.Add(t.gameObject);
        foreach (GameObject go in _gameObjectCache)
            foreach (Renderer r in go.GetComponents<Renderer>())
                _rendererCache.Add(r);
    }

    /// <summary>
    /// Set active recursively (cached).
    /// </summary>
    private void SetActive(bool active)
    {
        foreach (GameObject go in _gameObjectCache)
            go.active = active;

        foreach (Renderer r in _rendererCache)
            r.enabled = active;
    }

    protected virtual void OnPoolActivation()
    {

    }

    /// <summary>
    /// Enable the GameObject and all children.
    /// </summary>
    public void OnSpawn()
    {
        if (IsSpawned)
            return;

        IsSpawned = true;
        TimeLastSpawned = Time.time;

        if (IsDynamic)
            RefreshCache();

        SetActive(true);

        if (Spawned != null) Spawned(this, null);


        foreach (var view in GetComponentsInChildren<View>())
        {
            Context.RemoveView(view);
            Context.AddView(view);
        }

        OnPoolActivation();
    }

    protected virtual void OnPoolDeactivation()
    {
    }

    /// <summary>
    /// Disable the GameObject and all children.
    /// </summary>
    public void OnDespawn()
    {
        if (!IsSpawned) return;

        foreach (var mediator in GetComponentsInChildren<Mediator>())
        {
            mediator.OnRemove();
            Destroy(mediator);
        }


        OnPoolDeactivation();

        IsSpawned = false;
        _hasDespawnTimer = false;
        TimeLastDespawned = Time.time;
        DespawnTimerInitialized = -1f;
        DespawnDelay = -1f;
        StopAllCoroutines();

        if (IsDynamic) RefreshCache();

        SetActive(false);

        if (Despawned != null) Despawned(this, null);
    }

    /// <summary>
    /// Despawn this gameObject.
    /// </summary>
    public void Despawn()
    {
        PoolManager.Despawn(gameObject);
    }

    /// <summary>
    /// Despawn in the specified number of seconds.
    /// </summary>
    public void DespawnAfterSeconds(float delay)
    {
        if (!IsSpawned)
            return;

        DespawnTimerInitialized = Time.time;
        DespawnDelay = delay - Time.deltaTime;

        if (!_hasDespawnTimer)
            StartCoroutine(CRDespawnAfterSeconds(delay));
    }

    /// <summary>
    /// Despawn in the specified number of seconds.
    /// </summary>
    private IEnumerator CRDespawnAfterSeconds(float delay)
    {
        _hasDespawnTimer = true;
        while (Time.time < DespawnTimerInitialized + DespawnDelay)
            yield return null;
        Despawn();
    }

    #endregion
}
