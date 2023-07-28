using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンベルの生成
/// </summary>
public class DumbbellGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<Dumbbell> DumbbellPool => _dumbbellPool;
    #endregion

    #region serialize
    [Tooltip("プレハブ")]
    [SerializeField]
    private Dumbbell _dumbbellPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<Dumbbell> _dumbbellPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _dumbbellPool = new ObjectPool<Dumbbell>(_dumbbellPrefab, _parent);
    }
    #endregion
}