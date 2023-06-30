using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを追跡する敵の機能を持つコンポ―ネント
/// </summary>
public class ChaseEnemy : EnemyBase
{

    #region property
    #endregion

    #region serialize
    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 2.0f;
    #endregion

    #region private
    private bool _isFliped = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion

    #region coroutine method
    /// <summary>
    /// 各敵の行動のコルーチン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnActionCoroutine()
    {
        while (_isActionable)
        {
            //プレイヤーとの距離を計算
            float distance = Vector2.Distance(transform.localPosition, _playerTrans.localPosition);
            
            //近づける限界の距離よりも遠い場合
            if (distance >= ApproachDistance)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, _playerTrans.position, _moveSpeed * Time.deltaTime);
            }

            //プレイヤーより左の位置にいたら画像を反転
            if (!_isFliped && transform.localPosition.x < _playerTrans.position.x)
            {
                _enemyRenderer.flipX = true;
                _isFliped = true;
            }
            //プレイヤーより右の位置にいたら画像を元に戻す
            else if (_isFliped && transform.localPosition.x >= _playerTrans.position.x)
            {
                _enemyRenderer.flipX = false;
                _isFliped = false;
            }
            yield return null;
        }
    }
    #endregion
}
