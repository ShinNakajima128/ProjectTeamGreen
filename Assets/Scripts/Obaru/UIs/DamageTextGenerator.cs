using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextGenerator : MonoBehaviour
{
    #region property
    public ObjectPool DamageTextPool => _damageTextPool;
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
    private DamageText _damageTextPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool _damageTextPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _damageTextPool = new ObjectPool(_damageTextPrefab.gameObject, _reserveAmount, _limit, _parent);
    }
    #endregion
}