using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 四角形 クラス
  /// </summary>
  public class RectangleShape : Shape
  {
    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    /// <param name="color">色</param>
    public RectangleShape(Rectangle bounds, Color color)
      : base(bounds, color)
    {
      this.IsDragging = false;
      this.IsSelected = true;
      this.Cursor = Cursors.SizeAll;

      //リサイズハンドルの設定
      Size handleSize = new Size(7, 7);
      this.Handles = new ResizeHandle[8]
          {
            this.CreateTopLeftHandle(handleSize),
            this.CreateTopCenterHandle(handleSize),
            this.CreateTopRightHandle(handleSize),
            this.CreateCenterLeftHandle(handleSize),
            this.CreateCenterRightHandle(handleSize),
            this.CreateBottomLeftHandle(handleSize),
            this.CreateBottomCenterHandle(handleSize),
            this.CreateBottomRightHandle(handleSize)
          };

    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    public RectangleShape(Rectangle bounds)
      : this(bounds, Color.Black)
    { }
    #endregion

    #region Public Methods

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public override void Draw(Graphics g)
    {

      using (Pen pen = new Pen(Color))
      {
        if (this.IsDragging)
          pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        this.DrawShape(g, pen);
      }

      if (this.IsDragging || !this.IsSelected)
        return;

      foreach (ResizeHandle handle in this.Handles)
      {
        handle.Draw(g);
      }
    }

    /// <summary>
    /// ドラッグ処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public override void Drag(MouseDragEventArgs e)
    {
      this.IsDragging = true;
      if (this.ActiveHandle != null)
      {
        this.ActiveHandle.Drag(e);
        return;
      }
      this.SetBounds(Bounds.Location + (Size)e.Location - (Size)e.LastLocation,
                     Bounds.Size);

    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="location">マウス位置</param>
    /// <returns>マウスポインタと重なっているIDraggableオブジェクトを返す</returns>
    public override IDraggable HitTest(Point location)
    {
      foreach (ResizeHandle handle in this.Handles)
      {
        ActiveHandle = handle.HitTest(location) as ResizeHandle;
        if (ActiveHandle != null)
        {
          this.Cursor = ActiveHandle.Cursor;
          return this;
        }
      }

      //カーソルが図形の内部にあるかを判定する
      using (GraphicsPath path = new GraphicsPath())
      using (Region region = new Region(AddShapeTo(path)))
      {
        //GraphicsPath.IsVisibleでも判定できるが、非常に遅いためRegionで判定する
        if (!region.IsVisible(location))
        {
          return null;
        }
        this.Cursor = Cursors.SizeAll;
        return this;
      }
    }

    /// <summary>
    /// ドロップ処理
    /// </summary>
    /// <returns>常にNullを返す</returns>
    public override IDraggable Drop()
    {
      if (this.IsDragging)
      {
        //ドラッグ中はマイナスのサイズ(Left,TopがRight,Bottomより大)を許し、
        //ドロップしたタイミングでプラスのサイズとなるように補正する。
        this.Bounds = this.Bounds.Abs();
        this.IsDragging = false;
      }
      this.OnDropped(new System.EventArgs());
      return null;
    }
    #endregion

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

    /// <summary>
    /// GraphicsPathに図形を追加する
    /// </summary>
    /// <param name="path">GraphicsPath</param>
    /// <returns>追加後のPathを返す</returns>
    protected virtual GraphicsPath AddShapeTo(GraphicsPath path)
    {
      path.AddRectangle(Bounds);
      return path;
    }
    #endregion
  }


}
