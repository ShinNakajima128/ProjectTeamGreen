using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBackGround : MonoBehaviour
{ 
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("タイルのサイズ")]
    [SerializeField]
    private float _tileSize = 16f;

    [Tooltip("プレイヤーの位置取得")]
    [SerializeField]
    private GameObject _player;

    [Tooltip("背景用のプレハブ")]
    [SerializeField]
    private GameObject _tilePrefab;
    #endregion

    #region private

    //タイルの配列
    private GameObject[,] tiles = new GameObject[3, 3];
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    void Start()
    {
        // タイルの初期配置
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // タイルを生成
                GameObject tile = Instantiate(_tilePrefab);

                // タイルを配列に保存
                tiles[i, j] = tile;

                // タイルの位置を設定
                tile.transform.position = new Vector3((j - 1) * _tileSize, (1 - i) * _tileSize, 0);
            }
        }
    }


    void Update()
    {
        // プレイヤーの座標を取得
        Vector3 playerPos = _player.transform.position;

        // タイルの端に近づいたら
        if (playerPos.x > _tileSize / 2)
        {
            ShiftTilesLeft();
        }
        else if (playerPos.x < -_tileSize / 2)
        {
            ShiftTilesRight();
        }

        if (playerPos.y > _tileSize / 2)
        {
            ShiftTilesDown();
        }
        else if (playerPos.y < -_tileSize / 2)
        {
            ShiftTilesUp();
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    // タイルを左にシフト
    void ShiftTilesLeft()
    {
        // 左にシフトするコードを書く
        GameObject[] temp = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            temp[i] = tiles[i, 2];
        }

        // 中央と左の列を右にシフト
        for (int i = 0; i < 3; i++)
        {
            tiles[i, 2] = tiles[i, 1];
            tiles[i, 1] = tiles[i, 0];
        }

        // 一番左の列を更新
        for (int i = 0; i < 3; i++)
        {
            tiles[i, 0] = temp[i];
            tiles[i, 0].transform.position += new Vector3(-3f * _tileSize, 0, 0);
        }
    }

    // タイルを右にシフト
    void ShiftTilesRight()
    {
        // 右にシフトするコードを書く
        // 一番左の列を保存
        GameObject[] temp = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            temp[i] = tiles[i, 0];
        }

        // 中央と右の列を左にシフト
        for (int i = 0; i < 3; i++)
        {
            tiles[i, 0] = tiles[i, 1];
            tiles[i, 1] = tiles[i, 2];
        }

        // 一番右の列を更新
        for (int i = 0; i < 3; i++)
        {
            tiles[i, 2] = temp[i];
            tiles[i, 2].transform.position += new Vector3(3f * _tileSize, 0, 0);
        }
    }

    // タイルを下にシフト
    void ShiftTilesDown()
    {
        // 下にシフトするコードを書く
        GameObject[] temp = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            temp[i] = tiles[0, i];
        }

        // 中央と下の行を上にシフト
        for (int i = 0; i < 3; i++)
        {
            tiles[0, i] = tiles[1, i];
            tiles[1, i] = tiles[2, i];
        }

        // 一番下の行を更新
        for (int i = 0; i < 3; i++)
        {
            tiles[2, i] = temp[i];
            tiles[2, i].transform.position += new Vector3(0, -3f * _tileSize, 0);
        }
    }

    // タイルを上にシフト
    void ShiftTilesUp()
    {
        // 上にシフトするコードを書く
        GameObject[] temp = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            temp[i] = tiles[2, i];
        }

        // 中央と上の行を下にシフト
        for (int i = 0; i < 3; i++)
        {
            tiles[2, i] = tiles[1, i];
            tiles[1, i] = tiles[0, i];
        }

        // 一番上の行を更新
        for (int i = 0; i < 3; i++)
        {
            tiles[0, i] = temp[i];
            tiles[0, i].transform.position += new Vector3(0, 3f * _tileSize, 0);
        }
    }
    #endregion
}