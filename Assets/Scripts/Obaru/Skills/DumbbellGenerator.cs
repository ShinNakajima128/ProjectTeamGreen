using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ダンベルの生成
/// </summary>
public class DumbbellGenerator : MonoBehaviour
{
    #region property
    public ObjectPool DumbbellPool => _dumbbellPool;
    #endregion

    #region serialize
    [SerializeField]
    private uint _reserveAmount = 10;

    [SerializeField]
    private uint _limit = 100;

    [SerializeField]
    private Dumbbell _dumbbellPrefab = default;

    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool _dumbbellPool;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _dumbbellPool = new ObjectPool(_dumbbellPrefab.gameObject, _reserveAmount, _limit, _parent);
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}