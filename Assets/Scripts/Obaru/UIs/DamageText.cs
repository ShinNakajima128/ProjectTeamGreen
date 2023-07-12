using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ダメージを表記するためのテキスト
/// </summary>
public class DamageText : MonoBehaviour
{
    #region serialize
    [Tooltip("生存時間")]
    [SerializeField]
    private float _lifeTime = 0.3f;

    [Tooltip("上昇スピード")]
    [SerializeField]
    private float _riseSpeed = 0.01f;
    #endregion

    #region private
    private TextMeshProUGUI _damageText;
    private Coroutine _displayCoroutine;
    private Coroutine _riseCoroutine;
    #endregion

    #region unity methods
    private void Awake()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _displayCoroutine = StartCoroutine(InactiveText());
    }

    private void OnDisable()
    {
        if (_displayCoroutine != null)
        {
            StopCoroutine(_displayCoroutine);
            _displayCoroutine = null;
        }

        if(_riseCoroutine != null)
        {
            StopCoroutine(_riseCoroutine);
            _riseCoroutine = null;
        }
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
        //var targetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        //_rect.position = targetScreenPos;
        transform.position = target.position;
        _riseCoroutine = StartCoroutine(TextRise());
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

    /// <summary>
    /// 表示されたテキストが上に移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator TextRise()
    {
        while (true)
        {
            transform.Translate(Vector2.up * _riseSpeed);
            yield return null;
        }
    }
    #endregion
}