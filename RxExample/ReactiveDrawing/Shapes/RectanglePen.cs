﻿using System.Drawing;

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
      if (!this.IsDragging)
        return;

      using (Pen pen = new Pen(Color))
      {
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        this.DrawShape(g, pen);
      }
    }

    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public override void Drag(MouseDragEventArgs e)
    {
      this.IsDragging = true;
      Bounds = new Rectangle(
                  e.StartLocation,
                  (Size)e.Location - (Size)e.StartLocation);
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
      return new RectangleShape(this.Bounds, this.Color);
    }

    #endregion Public Methods

    #region Protected Methods
    /// <summary>
    /// 図形の描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    /// <param name="pen">ペン</param>
    protected virtual void DrawShape(Graphics g, Pen pen)
    {
      g.DrawRectangle(pen, this.Bounds.Abs());
    }
    #endregion
  }
}
