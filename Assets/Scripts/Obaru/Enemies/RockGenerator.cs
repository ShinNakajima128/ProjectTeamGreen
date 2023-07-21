using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rockの生成
/// </summary>
public class RockGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<Rock> RockPool => _rockPool;
    #endregion

    #region serialize
    [Tooltip("プレハブ")]
    [SerializeField]
    private Rock _rockPrefab = default;

    [Tooltip("親")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<Rock> _rockPool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _rockPool = new ObjectPool<Rock>(_rockPrefab, _parent);
    }
    #endregion
}