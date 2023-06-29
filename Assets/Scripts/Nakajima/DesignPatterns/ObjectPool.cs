using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを再利用する機能をもつクラス
/// </summary>
public class ObjectPool
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    /// <summary>オブジェクトのプール</summary>
    private List<GameObject> _pool;
    /// <summary>プーリングしているオブジェクト</summary>
    private GameObject _object;
    /// <summary>生成されたオブジェクトの親オブジェクト</summary>
    private Transform _parent;
    /// <summary>同時に使用する限度</summary>
    private uint _activationLimit = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region public method
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="obj">プーリングするオブジェクト</param>
    /// <param name="reserveAmount">最初に用意する数</param>
    /// <param name="limit">一度にアクティブになる限度</param>
    /// <param name="parent">親オブジェクト</param>
    public ObjectPool(GameObject obj, uint reserveAmount, uint limit, Transform parent)
    {
        _pool = new List<GameObject>();
        _object = obj;
        _parent = parent;
        _activationLimit = limit;

        for (int i = 0; i < reserveAmount; i++)
        {
            var o = Object.Instantiate(_object, parent);
            o.SetActive(false);
            _pool.Add(o);
        }
    }

    /// <summary>
    /// 使用する
    /// </summary>
    /// <returns>使用するオブジェクト</returns>
    public GameObject Rent()
    {
        if (_activationLimit <= _pool.Count(x => x.activeSelf))
        {
            Debug.Log($"{_object}はアクティブ限度数に達しています");
            return null;
        }
        //待機状態のオブジェクトを探し、あればそれを渡す
        foreach (var obj in _pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        //無かった場合は新しく作り、それを渡す
        var o = Object.Instantiate(_object as GameObject, _parent);
        _pool.Add(o);
        return o;
    }

    /// <summary>
    /// 使用しているオブジェクトを戻す
    /// </summary>
    public void Return()
    {
        foreach (var obj in _pool)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localEulerAngles = Vector3.zero;
            }
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
