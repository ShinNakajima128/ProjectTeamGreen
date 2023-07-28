using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発の生成
/// </summary>
public class BombExplosionGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<BombExplosion> ExplosionPool => _explosionPool;
    #endregion

    #region serialize
    [Tooltip("プレハブ")]
    [SerializeField]
    private BombExplosion _explosionPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<BombExplosion> _explosionPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _explosionPool = new ObjectPool<BombExplosion>(_explosionPrefab, _parent);
    }
    #endregion
}