using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UniRx;

/// <summary>
/// ダメージを表記するためのテキスト
/// </summary>
public class DamageText : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("生存時間")]
    [SerializeField]
    private float _lifeTime = 0.3f;

    [Tooltip("上方に移動する値")]
    [SerializeField]
    private float _riseAmount = 0.5f;

    [Tooltip("文字の色が変わる1段階目")]
    [SerializeField]
    private float _middleBorder = 3f;

    [Tooltip("文字の色が変わる２段階目")]
    [SerializeField]
    private float _highBorder = 5f;

    [Tooltip("１段階目の文字色")]
    [SerializeField]
    private Color _middleColor = default;

    [Tooltip("２段階目の文字色")]
    [SerializeField]
    private Color _highColor = default;
    #endregion

    #region private
    /// <summary>ダメージテキスト</summary>
    private TextMeshProUGUI _damageText;
    /// <summary>コルーチン格納用</summary>
    private Coroutine _currentCoroutine;
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(InactiveText());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        //テキストのフェード値を元に戻す
        _damageText.DOFade(1, 0);
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region public method
    /// <summary>
    /// ダメージと表示位置をセット
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    public void SetDamageText(Transform target, float amount)
    {
        //ダメージによって色を変える
        if(amount >= _highBorder)
        {
            _damageText.color = _highColor;
        }
        else if(amount >= _middleBorder)
        {
            _damageText.color = _middleColor;
        }
        else
        {
            _damageText.color = Color.white;
        }

        _damageText.text = amount.ToString("f1");
        transform.position = target.position;
        TextRise();
    }
    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region private method
    /// <summary>
    /// 表示されたテキストが上に移動
    /// </summary>
    /// <returns></returns>
    private void TextRise()
    {
        //上昇
        transform.DOMoveY(transform.position.y + _riseAmount, _lifeTime);
        //フェードアウト
        _damageText.DOFade(0, _lifeTime)
                   .SetEase(Ease.OutQuad);
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// 一定時間待ってオブジェクトをを非アクティブに
    /// </summary>
    /// <returns></returns>
    private IEnumerator InactiveText()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    #endregion
}