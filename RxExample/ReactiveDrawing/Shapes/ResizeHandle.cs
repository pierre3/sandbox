using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// リサイズハンドル
  /// </summary>
  public class ResizeHandle : Shape
  {
    #region Properties
    /// <summary>自分を表示すべき位置を取得するデリゲート</summary>
    public Func<Point> GetAnchor { set; get; }

    /// <summary>ハンドルの配置</summary>
    public HandleAlignment Alignment { get; private set; }
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="color">色</param>
    /// <param name="cursor">マウスカーソル</param>
    /// <param name="size">サイズ</param>
    /// <param name="getAnchor">表示位置取得デリゲート</param>
    /// <param name="alignment">ハンドルの配置</param>
    public ResizeHandle(Color color, Cursor cursor, Size size,
      Func<Point> getAnchor, HandleAlignment alignment)
    {
      this.Bounds = new Rectangle(new Point(), size);
      this.GetAnchor = getAnchor;
      this.Cursor = cursor;
      this.Color = color;
      this.Alignment = alignment;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// マウスポインタとの当たり判定
    /// </summary>
    /// <param name="location">ポインタ座標</param>
    /// <returns>マウスポインタと重なっているIDraggableオブジェクト</returns>
    public override IDraggable HitTest(Point location)
    {
      return this.Bounds.Contains(location) ? this : null;
    }

    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public override void Drag(MouseDragEventArgs e)
    {
      this.OnDraged(e);
    }

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// ドロップ時に生成されるIDraggableオブジェクト
    /// </returns>
    public override IDraggable Drop()
    {
      this.OnDropped(new EventArgs());
      return null;
    }

    /// <summary>
    /// 図形描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public override void Draw(Graphics g)
    {
      using (Pen pen = new Pen(this.Color))
      {
        g.FillEllipse(Brushes.White, this.Bounds);
        g.DrawEllipse(pen, this.Bounds);
      }
    }

    /// <summary>
    /// 表示位置の設定
    /// </summary>
    public void SetLocation()
    {
      Point anchor = GetAnchor();
      this.Bounds = new Rectangle(anchor.X - this.Bounds.Width / 2,
                                  anchor.Y - this.Bounds.Height / 2,
                                  this.Bounds.Width,
                                  this.Bounds.Height);
    }
    #endregion

    #region Nested Classes
    /// <summary>ハンドルの配置 列挙型</summary>
    public enum HandleAlignment
    {
      /// <summary>未設定</summary>
      None,
      /// <summary>左上</summary>
      TopLeft,
      /// <summary>上辺中央</summary>
      TopCenter,
      /// <summary>右上</summary>
      TopRight,
      /// <summary>左辺中央</summary>
      CenterLeft,
      /// <summary>右辺中央</summary>
      CenterRight,
      /// <summary>左下</summary>
      BottomLeft,
      /// <summary>下辺中央</summary>
      BottomCenter,
      /// <summary>右下</summary>
      BottomRight
    }
    #endregion
  }
}
