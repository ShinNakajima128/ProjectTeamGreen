using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大ボスの弾のプール機能
/// </summary>
public class BigBossBulletGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<BigBossBullet> BigBossBulletPool => _bigBossBulletPool;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("初期生成量")]
    [SerializeField]
    private uint _reserveAmount = 10;

    [Tooltip("上限")]
    [SerializeField]
    private uint _limit = 100;

    [Tooltip("弾")]
    [SerializeField]
    private BigBossBullet _bigBossPrefab = default;

    [Tooltip("親オブジェクト")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<BigBossBullet> _bigBossBulletPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _bigBossBulletPool = new ObjectPool<BigBossBullet>(_bigBossPrefab, _parent);
    }
    #endregion

}