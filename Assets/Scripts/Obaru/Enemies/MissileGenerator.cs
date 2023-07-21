using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Missileの生成
/// </summary>
public class MissileGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<Missile> MissilePool => _missilePool;
    #endregion

    #region serialize
    [Tooltip("プレハブ")]
    [SerializeField]
    private Missile _missilePrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<Missile> _missilePool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _missilePool = new ObjectPool<Missile>(_missilePrefab, _parent);
    }
    #endregion
}