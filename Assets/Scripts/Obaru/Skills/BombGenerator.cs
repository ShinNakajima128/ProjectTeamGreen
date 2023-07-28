using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆弾の作成
/// </summary>
public class BombGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<Bomb> BombPool => _bombPool;
    #endregion

    #region serialize
    [Tooltip("プレハブ")]
    [SerializeField]
    private Bomb _bombPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<Bomb> _bombPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _bombPool = new ObjectPool<Bomb>(_bombPrefab, _parent);
    }
    #endregion
}