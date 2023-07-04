using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆弾の作成
/// </summary>
public class BombGenerator : MonoBehaviour
{
    #region property
    public ObjectPool BombPool => _bombPool;
    #endregion

    #region serialize
    [Tooltip("初期生成量")]
    [SerializeField]
    private uint _reserveAmount = 10;

    [Tooltip("上限")]
    [SerializeField]
    private uint _limit = 100;

    [Tooltip("プレハブ")]
    [SerializeField]
    private Bomb _bombPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool _bombPool;
    #endregion


    #region unity methods
    private void Awake()
    {
        _bombPool = new ObjectPool(_bombPrefab.gameObject, _reserveAmount, _limit, _parent);
    }
    #endregion
}