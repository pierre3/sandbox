using System.Drawing;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// Rectangle描画用ペン クラス
  /// </summary>
  public class RectanglePen : Shape
  {
    #region Constructors

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RectanglePen()
      : base()
    { }

    /// <summary>
    /// コンストラクタ 
    /// </summary>
    /// <param name="color">色</param>
    public RectanglePen(Color color)
      : base(new Rectangle(), color)
    { }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// マウスカーソルとの当たり判定
    /// </summary>
    /// <param name="location">マウス位置</param>
    /// <returns>常にNullを返します。</returns>
    public override IDraggable HitTest(Point location)
    {
      return null;
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public override void Draw(Graphics g)
    {
      if (!this.m_isDragging)
        return;

      using (Pen pen = new Pen(Color))
      {
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        g.DrawRectangle(pen, this.Bounds.Abs());
      }
    }
    
    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public override void Drag(MouseDragEventArgs e)
    {
      this.m_isDragging = true;
      Bounds = new Rectangle(
                  e.startLocation,
                  (Size)e.Location - (Size)e.startLocation);
    }

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// RectangleShapeオブジェクト
    /// </returns>
    public override IDraggable Drop()
    {
      if (!this.m_isDragging)
        return null;

      this.m_isDragging = false;
      return new RectangleShape(this.Bounds, this.Color);
    }

    #endregion Public Methods
  }
}
