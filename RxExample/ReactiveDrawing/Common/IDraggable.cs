using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing
{
  /// <summary>
  /// ドラッグ可能なオブジェクトのインタフェース
  /// </summary>
  public interface IDraggable
  {
    #region Properties
    
    /// <summary>マウスカーソル</summary>
    Cursor Cursor { set; get; }
    
    /// <summary>ドラッグされている間 Trueを返す</summary>
    bool IsDragging { get; }
    
    #endregion Properties

    #region Methods
    
    /// <summary>
    /// マウスポインタとの当たり判定
    /// </summary>
    /// <param name="location">ポインタ座標</param>
    /// <returns>マウスポインタと重なっているIDraggableオブジェクト</returns>
    IDraggable HitTest(Point location);
    
    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    void Drag(MouseDragEventArgs e);

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// ドロップ時に生成されるIDraggableオブジェクト
    /// </returns>
    IDraggable Drop();
    
    #endregion
  }
}
