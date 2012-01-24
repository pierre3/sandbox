using System;
using System.Drawing;

namespace ReactiveDrawing
{
  /// <summary>
  /// マウスドラッグイベント引数
  /// </summary>
  public class MouseDragEventArgs : EventArgs
  {
    /// <summary>マウスボタン押下位置</summary>
    public Point StartLocation { set; get; }

    /// <summary>直前のマウス位置</summary>
    public Point LastLocation { set; get; }

    /// <summary>現在のマウス位置</summary>
    public Point Location { set; get; }
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="startLocation">マウスボタンを押下した位置</param>
    /// <param name="lastLocation">直前のマウス位置</param>
    /// <param name="Location">現在のマウス位置</param>
    public MouseDragEventArgs(Point startLocation, Point lastLocation, Point Location)
    {
      this.StartLocation = startLocation;
      this.LastLocation = lastLocation;
      this.Location = Location;
    }
  }

}
