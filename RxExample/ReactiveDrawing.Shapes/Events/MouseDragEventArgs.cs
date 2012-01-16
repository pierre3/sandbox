using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing
{
  /// <summary>
  /// マウスドラッグイベントデータ
  /// </summary>
  public class MouseDragEventArgs : MouseEventArgs
  {
    /// <summary>マウスボタン押下位置</summary>
    public Point StartLocation { set; get; }
    
    /// <summary>直前のマウス位置</summary>
    public Point LastLocation { set; get; }
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="moveArgs">ドラッグ中のマウスイベントデータ</param>
    /// <param name="startLocation">マウスボタンを押下した位置</param>
    /// <param name="lastLocation">直前のマウス位置</param>
    public MouseDragEventArgs(MouseEventArgs moveArgs, Point startLocation, Point lastLocation)
      : base(moveArgs.Button, moveArgs.Clicks, moveArgs.X, moveArgs.Y, moveArgs.Delta)
    {
      this.StartLocation = startLocation;
      this.LastLocation = lastLocation;
    }
  }

}
