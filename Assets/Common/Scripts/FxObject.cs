using UnityEngine;
using System.Collections;

public sealed class FxObject : PoolObject
{
    private ParticleSystem _particleSystem;

    protected override void OnPoolInitialize()
    {
        base.OnPoolInitialize();

        _particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void OnPoolActivation()
    {
        base.OnPoolActivation();

        StartCoroutine(DisposeFx());
    }

    private IEnumerator DisposeFx()
    {
        yield return new WaitForSeconds(_particleSystem.duration);
        PoolManager.Despawn(_particleSystem.gameObject);
    }
}
