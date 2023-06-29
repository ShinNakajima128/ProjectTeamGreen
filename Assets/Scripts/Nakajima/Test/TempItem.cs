using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用の仮アイテムのコンポーネント
/// </summary>
public class TempItem : ItemBase
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    private Coroutine _coroutine;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(TestCoroutine());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// アイテムを使用する
    /// </summary>
    /// <param name="player"></param>
    public override void Use(PlayerController player)
    {
        Debug.Log($"{ItemType}を使用した");
        gameObject.SetActive(false);

        //HPを回復する処理は、以下のように記述する
        //player.Heal(回復量);

        

        //他のステータスを上げる処理は、まだステータス側の機能ができていないため
        //まだ記述できませんが、近日中にステータスの機能を作成する予定です
    }

    /// <summary>
    /// 非アクティブ状態にする。ゲーム終了時など、不要となった時に実行される
    /// </summary>
    public override void Return()
    {
        Debug.Log($"{ItemType}を非アクティブにした");

        //座標をリセットして非表示にする
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.SetActive(false);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    private IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(15.0f);

        gameObject.SetActive(false);
    }
    #endregion
}
