using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// オブジェクトを再利用する機能をもつクラス
/// </summary>
public class ObjectPool<T> where T: Object
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    /// <summary>オブジェクトのプール</summary>
    private Queue<T> _pool;
    /// <summary>プーリングしているオブジェクト</summary>
    private T _object;
    /// <summary>生成されたオブジェクトの親オブジェクト</summary>
    private Transform _parent;
    /// <summary>同時に使用する限度</summary>
    private uint _activationLimit = 0;
    #endregion

    #region Constant
    private const uint DEFAULT_LIMIT = 50;
    #endregion

    #region Event
    #endregion

    #region public method
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="obj">プーリングするオブジェクト</param>
    /// /// <param name="parent">親オブジェクト</param>
    public ObjectPool(T obj, Transform parent)
    {
        _pool = new Queue<T>();
        _object = obj;
        _parent = parent;
    }
   
    /// <summary>
    /// 使用する
    /// </summary>
    /// <returns>使用するオブジェクト</returns>
    public T Rent(uint limit = DEFAULT_LIMIT)
    {
        if (_pool.Count > 0)
        {
            Debug.Log($"Queueから取り出した");
            return _pool.Dequeue();
        }
        else
        {
            if (_parent.childCount >= limit)
            {
                Debug.Log("生成上限です");
                return null;
            }

            //無かった場合は新しく作り、それを渡す
            var obj = Object.Instantiate(_object, _parent);

            //インターフェースを取得するため、GameObjectに変換
            var o = obj as GameObject;

            //プーリング機能のインターフェースを取得できた場合
            if (o.TryGetComponent(out IPoolable p))
            {
                //非アクティブとなった時にQueueに戻る処理を登録
                p.InactiveObserver
                 .Subscribe(_ =>
                 {
                     _pool.Enqueue(obj);
                     Debug.Log("poolに帰還");
                 });

                Debug.Log($"新しく{obj.name}を作成");
            }
            return obj;
        }
    }

    /// <summary>
    /// プールの状態を確認する
    /// </summary>
    public void CheckPoolStatus()
    {
        Debug.Log($"オブジェクト：{_object} プーリングしている数：{_pool.Count()}");
    }
    #endregion

    #region private method
    #endregion
}
