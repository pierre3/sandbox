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
    #region Events
    
    /// <summary>ドロップ通知イベント</summary>
    public event EventHandler NotifyDrop;

    /// <summary>ドラッグ通知イベント</summary>
    public event EventHandler<MouseDragEventArgs> NotifyDrag;

    #endregion Events

    #region Private Feilds 

    /// <summary>自分を表示すべき位置を取得するデリゲート</summary>
    private Func<Point> m_getAnchor;

    #endregion Private Feilds

    #region Constructors

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="color">色</param>
    /// <param name="cursor">マウスカーソル</param>
    /// <param name="size">サイズ</param>
    /// <param name="getAnchor">表示位置取得デリゲート</param>
    public ResizeHandle(Color color, Cursor cursor, Size size, Func<Point> getAnchor)
    {
      this.Bounds = new Rectangle(new Point(), size);
      this.m_getAnchor = getAnchor;
      this.Cursor = cursor;
      this.Color = color;
    }
    
    #endregion Constructors

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
      if (NotifyDrag != null)
        NotifyDrag(this, e);
    }

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// ドロップ時に生成されるIDraggableオブジェクト
    /// </returns>
    public override IDraggable Drop()
    {
      if (NotifyDrop != null)
        NotifyDrop(this, new EventArgs());
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
      Point anchor = m_getAnchor();
      this.Bounds = new Rectangle(anchor.X - this.Bounds.Width / 2,
                                  anchor.Y - this.Bounds.Height / 2,
                                  this.Bounds.Width,
                                  this.Bounds.Height);
    }

    #endregion Public Methods

  }
}
