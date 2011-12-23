using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Drawing;

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
    /// <param name="captured">ドラッグ開始時の処理を記述するデリゲート</param>
    /// <param name="released">ドラッグ完了時の処理を記述するデリゲート</param>
    /// <returns>マウスドラッグイベントのIObservable</returns>
    public static IObservable<MouseDragEventArgs> MouseDragAsObservable(
                                                this Control control,
                                                MouseButtons mouseButton,
                                                Action<MouseEventArgs> captured,
                                                Action<MouseEventArgs> released)
    {
      var down = control.MouseDownAsObservable()
                  .Where(e => e.Button == mouseButton)
                  .Do(e => { if (captured != null)captured(e); });
      var move = control.MouseMoveAsObservable();
      var Up = control.MouseUpAsObservable()
                .Where(e => e.Button == mouseButton)
                .Do(e => { if (released != null)released(e); });
            
      return down
              .SelectMany(
              e0 =>
              {
                return
                  move.TakeUntil(Up)
                      .Select(e => new MouseDragEventArgs(e, e0.Location, e0.Location))
                      .Scan((e1, e2) => new MouseDragEventArgs(e2, e1.startLocation, e1.Location));
              });
    }

    /// <summary>
    ///  MouseDragイベントのIObservableオブジェクト生成(Move_Skip_Take)
    /// </summary>
    /// <param name="control">System.Windows.Forms.Control</param>
    /// <param name="mouseButton">マウスボタン</param>
    /// <param name="captured">ドラッグ開始時の処理を記述するデリゲート</param>
    /// <param name="released">ドラッグ完了時の処理を記述するデリゲート</param>
    /// <returns>マウスドラッグのIObservableオブジェクト</returns>
    public static IObservable<MouseEventArgs> MouseDragAsObservable_MST(
                                                this Control control,
                                                MouseButtons mouseButton,
                                                Action<MouseEventArgs> captured,
                                                Action<MouseEventArgs> released)
    {
      return
          control.MouseMoveAsObservable()
              .SkipUntil(control.MouseDownAsObservable()
              .Where(e => e.Button == mouseButton)
              .Do(captured))
              .TakeUntil(control.MouseUpAsObservable()
              .Where(e => e.Button == mouseButton)
              .Do(released))
              .Repeat();
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
