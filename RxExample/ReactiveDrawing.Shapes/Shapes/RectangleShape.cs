using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 四角形 クラス
  /// </summary>
  public class RectangleShape : Shape
  {
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

      #region init ResizeHandles

      Size handleSize = new Size(7, 7);
      EventHandler dropHandler =
        (o, e) =>
        {
          this.m_isDragging = false;
          SetBounds(this.Bounds.Abs());
        };

      this.m_handles = new ResizeHandle[8];

      //Top-Left
      this.m_handles[0]
        = new ResizeHandle(this.Color, Cursors.SizeNWSE, handleSize,
                           () => this.Bounds.Location);
      this.m_handles[0].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(e.Location.X, e.Location.Y,
                         this.Bounds.Right - e.Location.X, this.Bounds.Bottom - e.Location.Y);
        };
      
      //Top-Center
      this.m_handles[1]
        = new ResizeHandle(this.Color, Cursors.SizeNS, handleSize,
                           () => new Point((this.Bounds.Left + this.Bounds.Right) / 2, this.Bounds.Top));
      this.m_handles[1].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, e.Location.Y,
                         this.Bounds.Width, this.Bounds.Bottom - e.Location.Y);
        };
      
      //Top-Right
      this.m_handles[2]
        = new ResizeHandle(this.Color, Cursors.SizeNESW, handleSize,
                           () => new Point(this.Bounds.Right, this.Bounds.Top));
      this.m_handles[2].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, e.Location.Y,
                         e.Location.X - this.Bounds.Left, this.Bounds.Bottom - e.Location.Y);
        };
      
      //Center-Left
      this.m_handles[3]
        = new ResizeHandle(this.Color, Cursors.SizeWE, handleSize,
                           () => new Point(this.Bounds.Left, (this.Bounds.Top + this.Bounds.Bottom) / 2));
      this.m_handles[3].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(e.Location.X, this.Bounds.Top,
                         this.Bounds.Right - e.Location.X, this.Bounds.Height);
        };
      
      //Center-Right
      this.m_handles[4]
        = new ResizeHandle(this.Color, Cursors.SizeWE, handleSize,
                           () => new Point(Bounds.Right, (this.Bounds.Top + this.Bounds.Bottom) / 2));
      this.m_handles[4].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, this.Bounds.Top,
                         e.Location.X - this.Bounds.Left, this.Bounds.Height);
        };
      
      //Bottom-Left
      this.m_handles[5]
        = new ResizeHandle(this.Color, Cursors.SizeNESW, handleSize,
                           () => new Point(Bounds.Left, this.Bounds.Bottom));
      this.m_handles[5].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(e.Location.X, this.Bounds.Top,
                         this.Bounds.Right - e.Location.X, e.Location.Y - this.Bounds.Top);
        };
      
      //Bottom-Center
      this.m_handles[6]
        = new ResizeHandle(this.Color, Cursors.SizeNS, handleSize,
                           () => new Point((this.Bounds.Left + this.Bounds.Right) / 2, this.Bounds.Bottom));
      this.m_handles[6].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, this.Bounds.Top,
                         this.Bounds.Width, e.Location.Y - this.Bounds.Top);
        };
      
      //Bottom-Right
      this.m_handles[7]
        = new ResizeHandle(this.Color, Cursors.SizeNWSE, handleSize,
                           () => new Point(this.Bounds.Right, this.Bounds.Bottom));
      this.m_handles[7].NotifyDrag +=
        (o, e) =>
        {
          this.m_isDragging = true;
          this.SetBounds(this.Bounds.Left, this.Bounds.Top,
                         e.Location.X - this.Bounds.Left, e.Location.Y - this.Bounds.Top);
        };

      foreach (ResizeHandle h in this.m_handles)
      {
        h.Group = this.Group;
        h.NotifyDrop += dropHandler;
        h.SetLocation();
      }

      #endregion
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    public RectangleShape(Rectangle bounds)
      : this(bounds, Color.Black)
    { }

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
        g.DrawRectangle(pen, this.Bounds.Abs());
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
      return Bounds.Contains(location) ? this : null;
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

  }


}
