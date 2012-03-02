using System;
using System.Drawing;

namespace ReactiveDrawing
{
  /// <summary>
  /// 選択可能なオブジェクトのインタフェース
  /// </summary>
  public interface ISelectable
  {
    /// <summary>選択状態が変更されると発生するイベント</summary>
    event EventHandler SelectChanged;

    /// <summary>選択状態</summary>
    bool IsSelected { set; get; }

    /// <summary>
    /// 選択枠による選択
    /// </summary>
    /// <param name="selectRect">選択枠</param>
    void SelectBy(Rectangle selectRect);

  }
}
