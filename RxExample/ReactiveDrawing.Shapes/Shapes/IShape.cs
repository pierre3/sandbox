using System.Drawing;
namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// System.Drawing.Graphicsによる描画機能を提供する
  /// ドラッグ可能なオブジェクトのインタフェース
  /// </summary>
  public interface IShape : IDraggable, IDrawable
  {
    /// <summary>外接矩形</summary>
    Rectangle Bounds { get; }

    /// <summary>選択状態</summary>
    bool IsSelected { set; get; }

    /// <summary>親オブジェクト</summary>
    IShape Parent { get; set; }
  }
}
