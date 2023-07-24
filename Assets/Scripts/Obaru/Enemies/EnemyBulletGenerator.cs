using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の弾生成
/// </summary>
public class EnemyBulletGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<EnemyBullet> BulletPool => _bulletPool;
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
    private EnemyBullet _bulletPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<EnemyBullet> _bulletPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _bulletPool = new ObjectPool<EnemyBullet>(_bulletPrefab, _parent);
    }
    #endregion

}