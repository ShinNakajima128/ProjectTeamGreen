using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 各アイテムのベースクラス
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class ItemBase : MonoBehaviour,IPoolable
{
    #region property
    public ItemType ItemType => _itemData.ItemType;
    public uint ReserveAmount => _itemData.ReserveAmount;
    public uint ActivationLimit => _itemData.ActivationLimit;

    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("アイテムデータ")]
    [SerializeField]
    private ItemData _itemData = default;

    [Tooltip("プレイヤーのポジション")]
    [SerializeField]
    private Transform _playerPositon;
    #endregion

    #region private
    private Vector2 _itemPosition;

    private bool _isUpdated = false;

    private Vector2 _velocty;

    private float _period;

    Rigidbody2D _rb2d;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    private void OnDisable()
    {
        _inactiveSubject.OnNext(Unit.Default);   
    }
    #endregion

    #region public method
    #endregion

    #region protected method
    /// <summary>
    /// 使用する
    /// </summary>
    public abstract void Use(PlayerController player);
    /// <summary>
    /// ステージ上から無くなる
    /// </summary>
    public abstract void Return();
    #endregion

    #region private method
    #endregion

    IEnumerator ItemGet(PlayerController player)
    {
        if (_isUpdated)
        {
            _itemPosition = transform.position;
            _isUpdated = false;
        }

        var acceleration = _rb2d.velocity;
        var diff = (Vector2)_playerPositon.position - _itemPosition;
        acceleration += (diff - _velocty * _period) * 2.0f / (_period * _period);
        _period -= Time.deltaTime;

        _velocty += acceleration * Time.deltaTime;
        _itemPosition += _velocty * Time.deltaTime;
        transform.position = _itemPosition;

        Vector2 currentDistance = transform.position;

        yield return null;

        //if (currentDistance <= 0)
        //{
        //    Use(player);
        //}

    }

    public void ReturnPool()
    {
        throw new NotImplementedException();
    }
}