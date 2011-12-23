using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace ReactiveDrawing
{
  public class MouseGesture
  {
    public class MouseGestureEventArgs : EventArgs
    {
      public string Command { set; get; }
      public MouseGestureEventArgs(string command)
      {
        this.Command = command;
      }
    }
    public event EventHandler<MouseGestureEventArgs> MotionCaptured;
    public event EventHandler<MouseGestureEventArgs> CommandExecuted;

    private int m_maxCount;
    private Dictionary<string, Action> m_gestures;
    private CompositeDisposable disposables = new CompositeDisposable();

    public MouseGesture()
    {
      this.m_gestures = new Dictionary<string, Action>();
    }

    public void Add(string command, Action gesture)
    {
      this.m_gestures.Add(command, gesture);
      if (this.m_maxCount < command.Length)
        this.m_maxCount = command.Length;
    }

    public void Run(Control target, MouseButtons button, int interval)
    {
      string command = string.Empty;

      //ドラッグ中の処理
      this.disposables.Add(
        target.MouseDragAsObservable(button, null, null)
          .Select(    //<= マウスの移動方向を↑,↓,←,→の文字に置き換える
          e =>
          {
            var arrow = GetArrowString(e.Location.X - e.startLocation.X,
                                       e.Location.Y - e.startLocation.Y,
                                       interval);
            if (arrow != string.Empty)
              e.startLocation = e.Location;
            return arrow;
          })
          .Where(e => e != string.Empty)  //<= 何れかの方向が検知された場合のみ通す
          .Subscribe(
          e =>
          {
            if (command.Length < this.m_maxCount && !command.EndsWith(e))
            {
              command += e;
              if (MotionCaptured != null)
                MotionCaptured(this, new MouseGestureEventArgs(command));
            }
          })
      );

      //ドラッグ終了時の処理
      this.disposables.Add(
        target.MouseUpAsObservable()
        .Where(e => e.Button == button)
        .Subscribe(
          e =>
          {
            Action gesture;
            if (m_gestures.TryGetValue(command, out gesture))
            {
              gesture();
              if (CommandExecuted != null)
                CommandExecuted(this, new MouseGestureEventArgs(command));
            }
            command = string.Empty;
          })
      );
    }

    public void Stop()
    {
      this.disposables.Clear();
    }

    private string GetArrowString(int dx, int dy, int interval)
    {
      int px = Math.Abs(dx);
      int py = Math.Abs(dy);
      if (px > py)
      {
        if (px < interval)
          return string.Empty;
        return (dx > 0) ? "→" : "←";
      }
      else
      {
        if (py < interval)
          return string.Empty;
        return (dy > 0) ? "↓" : "↑";
      }
    }
  }
}
