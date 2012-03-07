using System.Drawing;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// Rectangle描画用ペン クラス
  /// </summary>
  public class EllipsePen : RectanglePen
  {
    #region Constructors

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EllipsePen()
      : base()
    { }

    /// <summary>
    /// コンストラクタ 
    /// </summary>
    /// <param name="color">色</param>
    public EllipsePen(Color color)
      : base(color)
    { }

    #endregion Constructors

    #region Public Methods

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
      return new EllipseShape(this.Bounds, this.Color);
    }

    #endregion Public Methods

    #region Protected Methods
    /// <summary>
    /// 図形の描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    /// <param name="pen">ペン</param>
    protected override void DrawShape(Graphics g, Pen pen)
    {
      g.DrawEllipse(pen, this.Bounds.Abs());
    }
    #endregion
  }
}
