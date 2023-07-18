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
    #endregion

    #region private
    private Vector2 _itemPosition;


    private Vector2 _velocity;

    private float _period;

    Rigidbody2D _rb2d;

    private float m_arrivalTime = 0.6f;
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

    private void OnEnable()
    {
        _itemPosition = transform.position;
        _period = m_arrivalTime;
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

    public IEnumerator ItemGet(PlayerController player, Transform playerPos)
    {
        _itemPosition = transform.position;

        Vector2 initialDirection = _itemPosition - (Vector2)playerPos.position;
        initialDirection.Normalize();
        float initialSpeed = 3.0f; //初期移動速度
        _velocity = initialDirection * initialSpeed;

        float useDistance = 0.2f;

        while (Vector2.Distance(_itemPosition,playerPos.position) > useDistance)
        {
            var acceleration = _rb2d.velocity;
            var diff = (Vector2)playerPos.position - _itemPosition;
            acceleration += (diff - _velocity * _period) * 2.0f / (_period * _period);
            _period -= Time.deltaTime;

            _velocity += acceleration * Time.deltaTime;
            _itemPosition += _velocity * Time.deltaTime;
            transform.position = _itemPosition;

            yield return null;
        }
        Use(player);
        this.gameObject.SetActive(false);
    }

    public void ReturnPool()
    {
        throw new NotImplementedException();
    }
}