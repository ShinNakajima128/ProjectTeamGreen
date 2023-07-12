using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// ダメージを表記するためのテキスト
/// </summary>
public class DamageText : MonoBehaviour
{
    #region serialize
    [Tooltip("生存時間")]
    [SerializeField]
    private float _lifeTime = 0.3f;

    [Tooltip("上昇値")]
    [SerializeField]
    private float _riseAmount = 0.5f;
    #endregion

    #region private
    /// <summary>ダメージテキスト</summary>
    private TextMeshProUGUI _damageText;
    /// <summary>コルーチン格納用</summary>
    private Coroutine _currentCoroutine;
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
        _damageText.text = amount.ToString();
        transform.position = target.position;
        TextRise();
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