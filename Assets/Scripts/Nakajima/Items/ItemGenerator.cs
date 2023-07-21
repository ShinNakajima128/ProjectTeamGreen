using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// アイテムの生成、破棄をする機能を持つコンポーネント
/// </summary>
public class ItemGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("各アイテム")]
    [SerializeField]
    private Item[] _items = default;

    [Tooltip("生成される座標の絶対値")]
    [SerializeField]
    private Vector2 _generatePointAbsValue = default;
    #endregion

    #region private
    private Transform _playerTrans;
    private Dictionary<ItemType, ObjectPool<ItemBase>> _itemPoolDic = new Dictionary<ItemType, ObjectPool<ItemBase>>();
    private bool _isInGame = true;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Setup();
    }
    #endregion

    private void Start()
    {
        //ゲーム中かどうかの通知を発行するObserverに処理を登録
        StageManager.Instance.IsInGameObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => _isInGame = value);

        //ゲームリセット時の処理を登録
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnReset());
    }

    #region public method
    /// <summary>
    /// 生成する
    /// </summary>
    /// <param name="type">アイテムの種類</param>
    /// <param name="pos">生成する位置</param>
    public void Generate(ItemType type, Vector2 pos)
    {
        //アイテムが指定されていない場合は処理を行わない
        if (type == ItemType.None)
        {
            return;
        }

        //指定されたアイテムのオブジェクトデータを取得
        var item = _itemPoolDic.FirstOrDefault(x => x.Key == type).Value.Rent(10);

        if (item != null)
        {
            item.gameObject.SetActive(true);
            ////指定した座標に移動
            item.transform.localPosition = pos;
            Debug.Log($"{item.name}生成");
        }
    }

    /// <summary>
    /// ランダムな位置にアイテムを生成する
    /// </summary>
    public void RandoomGenerate(ItemType type)
    {
        StartCoroutine(RandomGenerateCoroutine(type));
    }
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;

        for (int i = 0; i < _items.Length; i++)
        {
            _itemPoolDic.Add(_items[i].ItemPrefab.ItemType, new ObjectPool<ItemBase>(_items[i].ItemPrefab, 
                                                                                     _items[i].Parent));
        }
    }

    private void OnReset()
    {
        _isInGame = true;
        _itemPoolDic.Select(x => x.Value)
                    .ToList()
                    .ForEach(x => x.Return());
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// ランダムな位置にアイテムを生成するコルーチン
    /// </summary>
    private IEnumerator RandomGenerateCoroutine(ItemType type)
    {
        var interval = new WaitForSeconds(_items.FirstOrDefault(x => x.ItemPrefab.ItemType == type).GenerateInterval);
        while (_isInGame)
        {
            float randomX, randomY;
            int randomRad = Random.Range(0, 360);

            randomX = _generatePointAbsValue.x * Mathf.Sin(Time.time * randomRad);
            randomY = _generatePointAbsValue.y * Mathf.Cos(Time.time * randomRad);

            Vector2 generatePos = new Vector2(_playerTrans.position.x + randomX, _playerTrans.position.y + randomY);

            //テストとして、現状は回復アイテムのみを生成する
            Generate(type, generatePos);
            Debug.Log("アイテム生成");
            yield return interval;
        }
    }
    #endregion
}

/// <summary>
/// 各アイテム情報
/// </summary>
[System.Serializable]
class Item
{
    /// <summary>アイテム名</summary>
    public string ItemName;
    /// <summary>生成するアイテムのPrefab</summary>
    public ItemBase ItemPrefab;
    /// <summary>アイテムの親オブジェクト</summary>
    public Transform Parent;
    /// <summary>つぎに生成されるまでの間隔</summary>
    [Range(0.1f, 10f)]
    public float GenerateInterval;
}
