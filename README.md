# ProjectTeamGreen
チームグリーンのチーム制作プロジェクト

## 注意点
- ブランチは各作業者毎に作ること
- ビルド時に扱うSceneを編集する時は、同時に作業をしているメンバーがいないか作業前に確認する

## 開発環境

| エンジン | バージョン  |
| ---------- | ----------- |
| Unity      | [参照先](ProjectSettings/ProjectVersion.txt#L1) |

## コード規則

変数名は[キャメルケース](https://e-words.jp/w/%E3%82%AD%E3%83%A3%E3%83%A1%E3%83%AB%E3%82%B1%E3%83%BC%E3%82%B9.html) (先頭小文字)

メンバー変数の接頭辞には「＿」(アンダースコア)を付けること

関数名　クラス名　プロパティの名前は[パスカルケース](https://wa3.i-3-i.info/word13955.html) (先頭大文字)  

### ブランチ名

ブランチの名前は[スネークケース](https://e-words.jp/w/%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9.html#:~:text=%E3%82%B9%E3%83%8D%E3%83%BC%E3%82%AF%E3%82%B1%E3%83%BC%E3%82%B9%E3%81%A8%E3%81%AF%E3%80%81%E3%83%97%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%9F%E3%83%B3%E3%82%B0,%E3%81%AA%E8%A1%A8%E8%A8%98%E3%81%8C%E3%81%93%E3%82%8C%E3%81%AB%E5%BD%93%E3%81%9F%E3%82%8B%E3%80%82)

### コミット時の規則
コミットする際は、メッセージ記入前に「[コミットの種類]」を先頭に付け、どんな作業を行ったか一目で分かりやすくする
#### 【例】
- ファイル、アセット等の追加->[add]
- ファイル、Prefabの更新->[update]
- ファイルの修正->[fixed]
- ファイルの削除->[deleted]
    
### boolean メソッド命名規則

> https://qiita.com/GinGinDako/items/6e8b696c4734b8e92d2b
### region 規則

```shell
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class #SCRIPTNAME# : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
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

    private void Update()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
```
# ゲームの仕様書

## タイトル

## 素材アセットリスト

