using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Potatoの生成
/// </summary>
public class PotatoGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<Potato> PotatoPool => _potatoPool;
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
    private Potato _potatoPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<Potato> _potatoPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _potatoPool = new ObjectPool<Potato>(_potatoPrefab, _parent);
    }
    #endregion
}