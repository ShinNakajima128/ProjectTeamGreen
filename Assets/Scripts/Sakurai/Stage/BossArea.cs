using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BossArea : MonoBehaviour
{
    #region property
    #endregion

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
    private float _radius = 6f;

    private int _fenceAmount = 60;

    private GameObject _fence = null;

    private List<GameObject> _fences = new List<GameObject>();
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
        TimeManager.Instance.BossEventObserver
                   .TakeUntilDestroy(this)
                   .Subscribe(_ => StartCoroutine(AreaSet()));

        EnemyManager.Instance.DefeatedBossObserver
                    .TakeUntilDestroy(this)
                    .Subscribe(_ => AreaDeacticve());

        GameObject player = GameObject.FindGameObjectWithTag(GameTag.Player);
        if (player != null)
        {
            _player = player.transform;
        }
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void　AreaDeacticve()
    {
        foreach (var fence in _fences)
        {
            fence.SetActive(false);
        }
    }
    #endregion

    #region Coroutine method
    IEnumerator AreaSet()
    {
        float waitTime = 5.0f;
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < _fenceAmount; i++)
        {
            float angle = (360 / _fenceAmount) * i;

            float radian = angle * Mathf.Deg2Rad;

            //位置を計算
            Vector2 fencePosition = (Vector2)_player.position + new Vector2(_radius * Mathf.Cos(radian), _radius * Mathf.Sin(radian));

            if (_fences.Count == _fenceAmount)
            {
                _fences[i].transform.position = fencePosition;
                _fences[i].SetActive(true);
            }
            else
            {
                _fence = Instantiate(_fencePrefab, fencePosition, Quaternion.identity);

                _fence.transform.SetParent(this.transform);
                _fences.Add(_fence);
            }
        }
    }
    #endregion
}