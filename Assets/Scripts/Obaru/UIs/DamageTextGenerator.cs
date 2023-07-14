using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージテキストオブジェクトの生成
/// </summary>
public class DamageTextGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<DamageText> DamageTextPool => _damageTextPool;
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
    private ObjectPool<DamageText> _damageTextPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _damageTextPool = new ObjectPool<DamageText>(_damageTextPrefab, _parent);
    }
    #endregion
}