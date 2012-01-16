using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 図形 基本クラス
  /// </summary>
  public abstract class Shape : IShape
  {
    #region Events
    /// <summary>ドロップ実行後に発生するイベント</summary>
    public event EventHandler Dropped
    {
      add { this.m_Dropped += value; }
      remove { this.m_Dropped -= value; }
    }

    /// <summary>ドラッグ実行後に発生するイベント</summary>
    public event EventHandler<MouseDragEventArgs> Draged
    {
      add { this.m_Draged += value; }
      remove { this.m_Draged -= value; }
    }
    #endregion Events

    #region Properties
    /// <summary>マウスカーソル</summary>
    public Cursor Cursor { set; get; }

    /// <summary>色</summary>
    public Color Color { set; get; }

    /// <summary>外接矩形</summary>
    public Rectangle Bounds { get; protected set; }

    /// <summary>ドラッグされている間 Trueを返す</summary>
    public bool IsDragging { get { return m_isDragging; } }


    /// <summary>選択されている間 Trueを返す</summary>
    public bool IsSelected { set; get; }

    /// <summary>親オブジェクト</summary>
    public IShape Parent { get; set; }

    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    protected Shape()
      : this(new Rectangle(), Color.Black)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    protected Shape(Rectangle bounds)
      : this(bounds, Color.Black)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    /// <param name="color">色</param>
    protected Shape(Rectangle bounds, Color color)
    {
      this.Bounds = bounds.Abs();
      this.Color = color;
      this.Parent = this;
    }
    #endregion

    #region abstract Methods
    /// <summary>
    /// 図形を描画します
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public abstract void Draw(Graphics g);

    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public abstract void Drag(MouseDragEventArgs e);

    /// <summary>
    /// マウスポインタとの当たり判定
    /// </summary>
    /// <param name="location">ポインタ座標</param>
    /// <returns>マウスポインタと重なっているIDraggableオブジェクト</returns>
    public abstract IDraggable HitTest(Point location);

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// ドロップ時に生成されるIDraggableオブジェクト
    /// </returns>
    public abstract IDraggable Drop();
    #endregion

    #region protected Methods
    /// <summary>
    /// Dragedイベントを発生させる
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    protected void OnDraged(MouseDragEventArgs e)
    {
      if (this.m_Draged != null)
        this.m_Draged(this, e);
    }

    /// <summary>
    /// Droppedイベントを発生させる
    /// </summary>
    /// <param name="e">イベントデータ</param>
    protected void OnDropped(EventArgs e)
    {
      if (this.m_Dropped != null)
        this.m_Dropped(this, e);

    }
    /// <summary>
    /// 外接矩形の設定
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="width">幅</param>
    /// <param name="height">高さ</param>
    /// <remarks>
    /// 新しい矩形の座標に合わせてリサイズハンドルの位置を再設定します
    /// </remarks>
    protected void SetBounds(int x, int y, int width, int height)
    {
      Bounds = new Rectangle(x, y, width, height);
      foreach (ResizeHandle handle in this.m_handles)
      {
        handle.SetLocation();
      }
    }

    /// <summary>
    /// 外接矩形の設定
    /// </summary>
    /// <param name="location">左上座標</param>
    /// <param name="size">幅,高さ</param>
    /// <remarks>
    /// 新しい矩形の座標に合わせてリサイズハンドルの位置を再設定します
    /// </remarks>
    protected void SetBounds(Point location, Size size)
    {
      this.SetBounds(location.X, location.Y, size.Width, size.Height);
    }

    /// <summary>
    /// 外接矩形の設定
    /// </summary>
    /// <param name="rect">矩形</param>
    /// <remarks>
    /// 新しい矩形の座標に合わせてリサイズハンドルの位置を再設定します
    /// </remarks>
    protected void SetBounds(Rectangle rect)
    {
      this.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion

    #region protected Feilds
    /// <summary>リサイズハンドル</summary>
    protected ResizeHandle[] m_handles = new ResizeHandle[0];
    /// <summary>ドラッグされている間 Trueを返す</summary>
    protected bool m_isDragging;
    /// <summary>ドロップ処理後イベント</summary>
    protected EventHandler m_Dropped;
    /// <summary>ドラッグ処理後イベント</summary>
    protected EventHandler<MouseDragEventArgs> m_Draged;
    #endregion
  }
}
