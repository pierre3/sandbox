
namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// System.Drawing.Graphicsによる描画機能を提供する
  /// ドラッグ可能なオブジェクトのインタフェース
  /// </summary>
  public interface IShape : IDraggable, IDrawable
  {
    /// <summary>選択状態</summary>
    bool IsSelected { set; get; }
    
    /// <summary>グループID</summary>
    object Group { get; set; }
  }
}
