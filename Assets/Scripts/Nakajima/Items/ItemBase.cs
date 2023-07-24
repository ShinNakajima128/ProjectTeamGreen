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

    [Tooltip("非表示になるプレイヤーとの距離")]
    [SerializeField]
    protected float _hideDistance = 10f;
    #endregion

    #region private
    private Vector2 _itemPosition;


    private Vector2 _velocity;

    private float _period;

    Rigidbody2D _rb2d;

    private float m_arrivalTime = 0.6f;

    private Transform _playerTrans;
    #endregion

    #region protected
    protected bool _isGetting = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    protected virtual void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;
    }

    protected virtual void Start()
    {
        //プレイヤーから一定距離離れたらプールに戻る処理を登録
        Observable.Interval(TimeSpan.FromSeconds(3f))
                  .TakeUntilDestroy(this)
                  .Where(_ => gameObject.activeSelf)
                  .Subscribe(_ =>
                  {
                      if (Vector2.Distance(_playerTrans.position, transform.position) >= _hideDistance)
                      {
                          Return();
                      }
                  });
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
        if (_isGetting)
        {
            yield break;
        }
        _isGetting = true;
        _itemPosition = transform.position;

        Vector2 initialDirection = _itemPosition - (Vector2)playerPos.position;
        initialDirection.Normalize();
        float initialSpeed = 3.0f; //初期移動速度
        _velocity = initialDirection * initialSpeed;

        //float useDistance = 0.2f;

        while (true)
        {
            var acceleration = _rb2d.velocity;
            var diff = (Vector2)playerPos.position - _itemPosition;
            acceleration += (diff - _velocity * _period) * 2.0f / (_period * _period);
            _period -= Time.deltaTime;

            if (_period <= 0)
            {
                break;
            }

            _velocity += acceleration * Time.deltaTime;
            _itemPosition += _velocity * Time.deltaTime;
            transform.position = _itemPosition;

            yield return null;
        }
        Use(player);
        _isGetting = false;
        this.gameObject.SetActive(false);
    }

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
}