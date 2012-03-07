using System;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace ReactiveDrawing
{
  /// <summary>
  /// Controlクラス拡張 静的クラス
  /// </summary>
  public static class ControlExtensions
  {

    /// <summary>
    ///  MouseDragイベントのIObservableオブジェクト生成
    /// </summary>
    /// <param name="control">System.Windows.Forms.Control</param>
    /// <param name="mouseButton">マウスボタン</param>
    /// <returns>マウスドラッグイベントのIObservable</returns>
    public static IObservable<MouseDragEventArgs> MouseDragAsObservable(
                                                this Control control,
                                                MouseButtons mouseButton)
    {
      var down = control.MouseDownAsObservable()
                  .Where(e => e.Button == mouseButton);
      var move = control.MouseMoveAsObservable();
      var Up = control.MouseUpAsObservable()
                .Where(e => e.Button == mouseButton);

      return down
              .SelectMany(
              e0 =>
              {
                return
                  move.TakeUntil(Up)
                      .Select(e => new MouseDragEventArgs(e0.Location, e0.Location, e.Location))
                      .Scan((e1, e2) => new MouseDragEventArgs(e1.StartLocation, e1.Location, e2.Location));
              });
    }

    /// <summary>
    /// MouseDownイベントのIObservableオブジェクト生成
    /// </summary>
    /// <param name="control">System.Windows.Forms.Control</param>
    /// <returns>マウスダウンイベントのIObservable</returns>
    public static IObservable<MouseEventArgs> MouseDownAsObservable(this Control control)
    {
      return Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
          h => (o, e) => h(e),
          h => control.MouseDown += h,
          h => control.MouseDown -= h);
    }

    /// <summary>
    /// MouseMoveイベントのIObservableオブジェクト生成
    /// </summary>
    /// <param name="control">System.Windows.Forms.Control</param>
    /// <returns>マウスムーブイベントのIObservable</returns>
    public static IObservable<MouseEventArgs> MouseMoveAsObservable(this Control control)
    {
      return Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
          h => (o, e) => h(e),
          h => control.MouseMove += h,
          h => control.MouseMove -= h);
    }

    /// <summary>
    /// MouseUpイベントのIObservableオブジェクト生成
    /// </summary>
    /// <param name="control">System.Windows.Forms.Control</param>
    /// <returns>マウスアップイベントのIObservable</returns>
    public static IObservable<MouseEventArgs> MouseUpAsObservable(this Control control)
    {
      return Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
          h => (o, e) => h(e),
          h => control.MouseUp += h,
          h => control.MouseUp -= h);
    }
  }
}
