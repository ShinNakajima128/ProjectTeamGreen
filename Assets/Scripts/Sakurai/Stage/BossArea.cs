using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ボス出現時の戦闘エリア
/// </summary>
public class BossArea : MonoBehaviour
{
    #region serialize
    [Header("変数")]
    [Tooltip("ボス戦の柵")]
    [SerializeField]
    private GameObject _fencePrefab = default;

    [Tooltip("プレイヤーの位置")]
    [SerializeField]
    private Transform _player = default;
    #endregion

    #region private
    /// <summary>プレイヤーからの半径</summary>
    private float _radius = 6f;

    /// <summary>フェンスの数</summary>
    private int _fenceAmount = 60;

    /// <summary>出現させたあとのフェンス</summary>
    private GameObject _fence = null;

    /// <summary>フェンス格納するリスト</summary>
    private List<GameObject> _fences = new List<GameObject>();
    #endregion

    #region unity methods
    private void Start()
    {
        //ボス出現イベント発生時
        TimeManager.Instance.BossEventObserver
                   .TakeUntilDestroy(this)
                   .Subscribe(_ => StartCoroutine(AreaSet()));

        //ボス死亡イベント発生時
        EnemyManager.Instance.DefeatedBossObserver
                    .TakeUntilDestroy(this)
                    .Subscribe(_ => AreaDeacticve());

        GameObject player = GameObject.FindGameObjectWithTag(GameTag.Player);
        if (player != null)
        {
            _player = player.transform;
        }
    }
    #endregion

    #region private method
    /// <summary>
    /// ボスエリアを非アクティブにする。
    /// </summary>
    private void　AreaDeacticve()
    {
        foreach (var fence in _fences)
        {
            fence.SetActive(false);
        }
    }
    #endregion

    #region Coroutine method
    /// <summary>
    /// ボスエリアをセット
    /// </summary>
    IEnumerator AreaSet()
    {
        float waitTime = 5.0f;
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < _fenceAmount; i++)
        {
            //フェンスの個数から角度を計算
            float angle = (360 / _fenceAmount) * i;

            //角度をラジアンに変更
            float radian = angle * Mathf.Deg2Rad;

            //ラジアンから円形にポジションを設定
            Vector2 fencePosition = (Vector2)_player.position + new Vector2(_radius * Mathf.Cos(radian), _radius * Mathf.Sin(radian));

            //2度目以降はインスタンス生成せずにアクティブにするのみ
            if (_fences.Count == _fenceAmount)
            {
                _fences[i].transform.position = fencePosition;
                _fences[i].SetActive(true);
            }
            else
            {
                //１度目はインスタンス生成
                _fence = Instantiate(_fencePrefab, fencePosition, Quaternion.identity);

                _fence.transform.SetParent(this.transform);
                _fences.Add(_fence);
            }
        }
    }
    #endregion
}