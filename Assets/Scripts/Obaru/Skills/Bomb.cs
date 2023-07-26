using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// ボムコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bomb : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("飛ぶ速さ")]
    [SerializeField]
    private float _moveSpeed = 1.5f;

    [Tooltip("スキルの生存時間")]
    [SerializeField]
    private float _lifeTime = 5.0f;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>現在のスキルレベル</summary>
    private int _currentSkillLevel = 1;
    /// <summary>Rigidbody2D格納用</summary>
    private Rigidbody2D _rb;
    /// <summary>コルーチン格納用</summary>
    private Coroutine _inactiveCoroutine = default;
    /// <summary>爆発生成コンポーネント格納用</summary>
    private BombExplosionGenerator _bombExplosionGenerator;
    /// <summary>親のTransform</summary>
    private Transform _parent;

    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bombExplosionGenerator = transform.parent.gameObject.GetComponent<BombExplosionGenerator>();
        _parent = transform.parent;
    }

    private void OnEnable()
    {
        _inactiveCoroutine = StartCoroutine(InactiveCoroutine());
    }

    private void OnDisable()
    {
        if (_inactiveCoroutine != null)
        {
            StopCoroutine(_inactiveCoroutine);
            _inactiveCoroutine = null;
        }
        _inactiveSubject.OnNext(Unit.Default);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy))
        {
            //爆発
            SetExplosion();
            //親を設定しなおして非アクティブ化
            transform.SetParent(_parent);
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

    /// <summary>
    /// velocityの設定
    /// </summary>
    /// <param name="dir"></param>
    public void SetVelocity(Vector3 dir)
    {
        _rb.velocity = dir * _moveSpeed;
    }

    /// <summary>
    /// 現在のスキルレベルを取得
    /// </summary>
    /// <param name="currentSkillLevel"></param>
    public void GetCurrentLevel(int currentSkillLevel)
    {
        _currentSkillLevel = currentSkillLevel;
    }

    /// <summary>
    /// 爆発をセット
    /// </summary>
    public void SetExplosion()
    {
        //使う爆発のオブジェクトを取得
        BombExplosion skillObj = _bombExplosionGenerator.ExplosionPool.Rent();
        
        //爆発がnullでなければ
        if (skillObj != null)
        {
            AudioManager.PlaySE(SEType.BombExplotion);

            //爆発オブジェクトのアクティブ化
            skillObj.gameObject.SetActive(true);
            //スキルレベルに応じてスケールを変化
            skillObj.SetScale(_currentSkillLevel);
            //爆発をボムの位置に移動
            skillObj.transform.position = transform.position;
            //親子関係解除
            skillObj.gameObject.transform.SetParent(null);
            //攻撃力を設定
            skillObj.SetAttackAmount(_currentAttackAmount);
        }
    }

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutine method
    /// <summary>
    /// 一定時間後に爆発して非アクティブ化
    /// </summary>
    /// <returns></returns>
    private IEnumerator InactiveCoroutine()
    {
        //一定時間後に爆発
        yield return new WaitForSeconds(_lifeTime);
        SetExplosion();
        //親を設定しなおして非アクティブ化
        transform.SetParent(_parent);
        gameObject.SetActive(false);
    }
    #endregion
}