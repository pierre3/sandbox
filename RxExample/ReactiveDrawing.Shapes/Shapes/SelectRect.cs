using System;
using System.Drawing;
namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 四角形 クラス
  /// </summary>
  public class SelectRect : RectanglePen
  {
    #region Constructors

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SelectRect()
      : base()
    { }

    /// <summary>
    /// コンストラクタ 
    /// </summary>
    /// <param name="color">色</param>
    public SelectRect(Color color)
      : base(color)
    {}

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public override void Drag(MouseDragEventArgs e)
    {
      this.IsDragging = true;
      Bounds = new Rectangle(e.StartLocation, (Size)e.Location - (Size)e.StartLocation);
    }

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// RectangleShapeオブジェクト
    /// </returns>
    public override IDraggable Drop()
    {
      if (!this.IsDragging)
        return null;

      this.IsDragging = false;
      this.OnDropped(new EventArgs());
      return null;
    }

    #endregion

  }
}