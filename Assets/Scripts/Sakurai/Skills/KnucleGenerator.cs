using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ナックルのプール機能
/// </summary>
public class KnucleGenerator : MonoBehaviour
{
    #region property
    public ObjectPool KnucklePool => _knucklePool;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("初期の段階で持たせるプール数")]
    [SerializeField]
    private uint _reserveAmount = 10;

    [Tooltip("プールの最大値")]
    [SerializeField]
    private uint _limit = 100;

    [Tooltip("ナックルプレハブ")]
    [SerializeField]
    private Knuckle _knucklePrefab = default;

    [Tooltip("")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool _knucklePool;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _knucklePool = new ObjectPool(_knucklePrefab.gameObject, _reserveAmount, _limit, _parent);
    }

    #endregion

}