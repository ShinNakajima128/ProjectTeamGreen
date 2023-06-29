using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの生成、破棄をする機能を持つコンポーネント
/// </summary>
public class ItemGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("次に生成するまでの間隔")]
    [SerializeField]
    private float _generateInterval = 1.0f;

    [Tooltip("各アイテム")]
    [SerializeField]
    private Item[] _items = default;
    #endregion

    #region private
    private Transform _playerTrans;
    private List<ObjectPool> _itemPoolList;
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

    private void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// 生成する
    /// </summary>
    /// <param name="type">アイテムの種類</param>
    /// <param name="pos">生成する位置</param>
    public void Generate(ItemType type, Vector2 pos)
    {
        //指定されたアイテムのオブジェクトデータを取得
        var item = _items.FirstOrDefault(x => x.ItemObj.ItemType == type).ItemObj;

        //指定した座標に生成。現状は仮の処理で、プーリングしたオブジェクトを使用する処理に修正予定
        Instantiate(item, pos, Quaternion.identity);
    }

    /// <summary>
    /// ランダムな位置にアイテムを生成する
    /// </summary>
    public void RandoomGenerate()
    {
        StartCoroutine(RandomGenerateCoroutine());
    }
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;
        _itemPoolList = new List<ObjectPool>();

        for (int i = 0; i < _items.Length; i++)
        {
            _itemPoolList.Add(new ObjectPool(_items[i].ItemObj.gameObject,
                                                       _items[i].ItemObj.ReserveAmount,
                                                       _items[i].ItemObj.ActivationLimit,
                                                       _items[i].Parent));
        }
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// ランダムな位置にアイテムを生成するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomGenerateCoroutine()
    {
        while (true)
        {
            float randomX = Random.Range(_playerTrans.position.x + 2, _playerTrans.position.x + 3.5f);
            float randomY = Random.Range(_playerTrans.position.y + 2, _playerTrans.position.y + 3.5f);

            randomX = Random.Range(0, 2) == 0 ? randomX : randomX * -1;
            randomY = Random.Range(0, 2) == 0 ? randomY : randomY * -1;

            Vector2 generatePos = new Vector2(randomX, randomY);

            //テストとして、現状は回復アイテムのみを生成する
            Generate(ItemType.Heal, generatePos);
            yield return new WaitForSeconds(_generateInterval);
        }
    }
    #endregion
}

[System.Serializable]
struct Item
{
    public ItemBase ItemObj;
    public Transform Parent;
}
