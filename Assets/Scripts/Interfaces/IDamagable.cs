/// <summary>
/// ダメージを受ける機能のインターフェース
/// </summary>
public interface IDamagable
{
    #region property
    /// <summary>現在のHP</summary>
    int CurrentHp { get; }

    /// <summary>無敵状態かどうか</summary>
    bool IsInvincible { get; }
    #endregion

    #region public method
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    void Damage(int amount);
    #endregion
}
