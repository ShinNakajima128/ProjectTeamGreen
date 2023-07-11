using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ダメージを表記するためのテキスト
/// </summary>
public class DamageText : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _lifeTime = 0.3f;
    #endregion

    #region private
    private TextMeshProUGUI _damageText;
    private Coroutine _displayCoroutine;
    private Coroutine _riseCoroutine;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _displayCoroutine = StartCoroutine(DisplayText());
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

    #region private method
    #endregion

    /// <summary>
    /// テキストの表示時間
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayText()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private IEnumerator TextRise()
    {
        while (true)
        {
            transform.Translate(Vector2.up * 0.01f);
            yield return null;
        }
    }
}