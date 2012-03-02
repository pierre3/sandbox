using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 複合ドラッグ可能オブジェクト クラス
  /// </summary>
  class CompositeDraggable : IDraggable
  {
    #region Properties
    /// <summary>ドラッグ可能オブジェクトのコレクション</summary>
    private IEnumerable<IDraggable> Items { get; set; }
    /// <summary>操作対象のオブジェクト</summary>
    private IDraggable Active { get; set; }
    #endregion

    #region public Methods
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="items">ドラッグ可能オブジェクトのコレクション</param>
    public CompositeDraggable(IEnumerable<IDraggable> items)
    {
      if (items == null)
        throw new ArgumentNullException("shapes");
      this.Items = items;
    }

    #region ReactiveDrawing.IDraggable の実装
    /// <summary>カーソル</summary>
    public System.Windows.Forms.Cursor Cursor
    {
      get
      { return (Active != null) ? Active.Cursor : System.Windows.Forms.Cursors.Default; }
      set
      { this.Cursor = value; }
    }

    /// <summary>ドラッグ中</summary>
    public bool IsDragging
    {
      get
      {
        return this.Items.All(shape => shape.IsDragging);
      }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="location">カーソル位置</param>
    /// <returns>ヒットしたオブジェクト</returns>
    public IDraggable HitTest(System.Drawing.Point location)
    {
      Active = null;
      foreach (Shape shape in this.Items.OfType<Shape>())
        shape.ActiveateHandle(ResizeHandle.HandleAlignment.None);

      foreach (IDraggable draggable in this.Items.Reverse<IDraggable>())
      {
        Active = draggable.HitTest(location);
        if (Active != null)
          return this;
      }
      return null;
    }

    /// <summary>
    /// ドラッグ
    /// </summary>
    /// <param name="e">マウスドラッグイベント引数</param>
    public void Drag(MouseDragEventArgs e)
    {
      var act = Active as Shape;
      ResizeHandle activeHandle = null;
      if (act != null)
        activeHandle = act.ActiveHandle;
      if (activeHandle == null)
      {
        foreach (IDraggable draggable in this.Items)
          draggable.Drag(e);
        return;
      }

      var actAnchor = activeHandle.GetAnchor();
      foreach (IDraggable draggable in this.Items)
      {
        var shape = draggable as Shape;
        if (shape == null)
        {
          continue;
        }
        shape.ActiveateHandle(activeHandle.Alignment);
        var anchor = shape.ActiveHandle.GetAnchor();
        var delta = (Size)(anchor - (Size)actAnchor);
        var args = new MouseDragEventArgs(e.StartLocation + delta,
                                          e.LastLocation + delta,
                                          e.Location + delta);
        shape.Drag(args);
      }
    }

    /// <summary>
    /// ドロップ
    /// </summary>
    /// <returns>ドロップ時に生成されるIDraggableオブジェクト</returns>
    public IDraggable Drop()
    {
      foreach (IDraggable draggable in this.Items)
        draggable.Drop();
      return null;
    }
    #endregion
    #endregion
  }
}
