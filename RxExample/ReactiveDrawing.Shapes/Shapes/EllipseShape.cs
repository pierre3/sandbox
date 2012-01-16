using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 楕円 クラス
  /// </summary>
  public class EllipseShape : RectangleShape
  {
    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    /// <param name="color">色</param>
    public EllipseShape(Rectangle bounds, Color color)
      : base(bounds, color)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    public EllipseShape(Rectangle bounds)
      : base(bounds)
    { }
    #endregion

    #region Methods
    /// <summary>
    /// 図形の種類に特化した描画処理
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    /// <param name="pen">ペン</param>
    protected override void DrawShape(Graphics g, Pen pen)
    {
      g.DrawEllipse(pen, this.Bounds.Abs());
    }

    /// <summary>
    /// GraphicsPathに図形を追加する
    /// </summary>
    /// <param name="path">GraphicsPath</param>
    /// <returns>追加後のGraphicsPathを返す</returns>
    protected override GraphicsPath AddShapeTo(GraphicsPath path)
    {
      path.AddEllipse(Bounds);
      return path;
    }
    #endregion

  }
}
