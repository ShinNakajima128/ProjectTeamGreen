using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 中ボスタレットが投射するオブジェクト
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Rock : MonoBehaviour,IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("投げられる時に加わる力")]
    [SerializeField]
    private float _power = 5.0f;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>弾の持続時間</summary>
    private float _lifeTime = 5.0f;
    /// <summary>ランダムレンジの最小</summary>
    private int _randomRangeMin = 0;
    /// <summary>ランダムレンジの最大</summary>
    private int _randomRangeMax = 3;
    /// <summary>投げられるときのx軸の大きさ</summary>
    private float _dirXAmount = 0.5f;
    /// <summary>Rigidbody2Dコンポーネント格納用</summary>
    private Rigidbody2D _rb;
    /// <summary>コルーチン格納用</summary>
    private Coroutine _currentCoroutine;
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(InactiveCoroutine());
        Thrown();
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        _inactiveSubject.OnNext(Unit.Default);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Player))
        {
            //プレイヤーにダメージを与える
            var target = collision.GetComponent<IDamagable>();
            target.Damage(_currentAttackAmount);
            target.Knockback(transform);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// 攻撃力の設定
    /// </summary>
    /// <param name="amount"></param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region private method
    /// <summary>
    /// 投げられる処理
    /// </summary>
    private void Thrown()
    {
        float dirX = 0;

        //ランダムに投射方向を決定
        int num = UnityEngine.Random.Range(_randomRangeMin, _randomRangeMax);
        switch (num)
        {
            case (int)RockThrownDirectionX.Left:
                dirX = -_dirXAmount;
                break;
            case (int)RockThrownDirectionX.Center:
                dirX = 0;
                break;
            case (int)RockThrownDirectionX.Right:
                dirX = _dirXAmount;
                break;
            default:
                break;
        }
        //力を加える
        _rb.AddForce(new Vector2(dirX, 1).normalized * _power, ForceMode2D.Impulse);
    }
    #endregion

    #region Coroutine method
    /// <summary>
    /// 一定時間後に非アクティブ
    /// </summary>
    /// <returns></returns>
    private IEnumerator InactiveCoroutine()
    {
        //生存時間分待つ
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }
    #endregion
}

/// <summary>
/// 投げられるX軸方向
/// </summary>
enum RockThrownDirectionX
{
    /// <summary>左</summary>
    Left,
    /// <summary>左</summary>
    Center,
    /// <summary>左</summary>
    Right
}