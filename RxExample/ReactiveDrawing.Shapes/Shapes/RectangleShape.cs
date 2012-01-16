using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
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
      this.IsSelected = true;
      this.Cursor = Cursors.SizeAll;
      
      //リサイズハンドルの設定
      Size handleSize = new Size(7, 7);
      this.m_handles = new ResizeHandle[8]
          {
            CreateLeftTopHandle(handleSize),
            CreateTopCenterHandle(handleSize),
            CreateTopRightHandle(handleSize),
            CreateCenterLeftHandle(handleSize),
            CreateCenterRightHandle(handleSize),
            CreateBottomLeftHandle(handleSize),
            CreateBottomCenterHandle(handleSize),
            CreateBottomRightHandle(handleSize)
          };

      foreach (ResizeHandle h in this.m_handles)
      {
        h.Parent = this;
        h.Dropped += (o, e) => this.m_isDragging = false;
        h.SetLocation();
      }
      
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
        if (this.m_isDragging)
          pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        this.DrawShape(g, pen);
      }

      if (this.m_isDragging || !this.IsSelected)
        return;

      foreach (ResizeHandle handle in this.m_handles)
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
      this.m_isDragging = true;
      this.SetBounds(
                  Bounds.Location + (Size)e.Location - (Size)e.LastLocation,
                  Bounds.Size);
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="location">マウス位置</param>
    /// <returns>マウスポインタと重なっているIDraggableオブジェクトを返す</returns>
    public override IDraggable HitTest(Point location)
    {
      foreach (ResizeHandle handle in this.m_handles)
      {
        var hitItem = handle.HitTest(location);
        if (hitItem != null)
          return hitItem;
      }

      //カーソルが図形の内部にあるかを判定する
      using (GraphicsPath path = new GraphicsPath())
      using (Region region = new Region(AddShapeTo(path)))
      {
        //GraphicsPath.IsVisibleでも判定できるが、非常に遅いためRegionで判定する
        return region.IsVisible(location) ? this : null;
      }
    }

    /// <summary>
    /// ドロップ処理
    /// </summary>
    /// <returns>常にNullを返す</returns>
    public override IDraggable Drop()
    {
      if (this.m_isDragging)
      {
        //ドラッグ中はマイナスのサイズ(Left,TopがRight,Bottomより大)を許し、
        //ドロップしたタイミングでプラスのサイズとなるように補正する。
        this.SetBounds(this.Bounds.Abs());
        this.m_isDragging = false;
      }
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

    #region Private Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateLeftTopHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeNWSE, handleSize,
                           () => this.Bounds.Location);
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(e.Location.X, e.Location.Y,
                         this.Bounds.Right - e.Location.X, this.Bounds.Bottom - e.Location.Y);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateTopCenterHandle(Size handleSize)
    {

      var result
        = new ResizeHandle(this.Color, Cursors.SizeNS, handleSize,
                           () => new Point((this.Bounds.Left + this.Bounds.Right) / 2, this.Bounds.Top));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, e.Location.Y,
                         this.Bounds.Width, this.Bounds.Bottom - e.Location.Y);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateTopRightHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeNESW, handleSize,
                           () => new Point(this.Bounds.Right, this.Bounds.Top));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, e.Location.Y,
                         e.Location.X - this.Bounds.Left, this.Bounds.Bottom - e.Location.Y);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateCenterLeftHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeWE, handleSize,
                           () => new Point(this.Bounds.Left, (this.Bounds.Top + this.Bounds.Bottom) / 2));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(e.Location.X, this.Bounds.Top,
                         this.Bounds.Right - e.Location.X, this.Bounds.Height);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateCenterRightHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeWE, handleSize,
                           () => new Point(Bounds.Right, (this.Bounds.Top + this.Bounds.Bottom) / 2));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, this.Bounds.Top,
                         e.Location.X - this.Bounds.Left, this.Bounds.Height);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateBottomLeftHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeNESW, handleSize,
                           () => new Point(Bounds.Left, this.Bounds.Bottom));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(e.Location.X, this.Bounds.Top,
                         this.Bounds.Right - e.Location.X, e.Location.Y - this.Bounds.Top);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateBottomCenterHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeNS, handleSize,
                           () => new Point((this.Bounds.Left + this.Bounds.Right) / 2, this.Bounds.Bottom));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, this.Bounds.Top,
                         this.Bounds.Width, e.Location.Y - this.Bounds.Top);
        };
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handleSize"></param>
    /// <returns></returns>
    private ResizeHandle CreateBottomRightHandle(Size handleSize)
    {
      var result
        = new ResizeHandle(this.Color, Cursors.SizeNWSE, handleSize,
                           () => new Point(this.Bounds.Right, this.Bounds.Bottom));
      result.Draged +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, this.Bounds.Top,
                         e.Location.X - this.Bounds.Left, e.Location.Y - this.Bounds.Top);
        };
      return result;
    }
    #endregion
  }


}
