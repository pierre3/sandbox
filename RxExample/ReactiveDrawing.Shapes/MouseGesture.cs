using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace ReactiveDrawing
{
  /// <summary>
  /// マウスジェスチャクラス
  /// </summary>
  public class MouseGesture
  {
    #region Events
    /// <summary>移動方向検出後 イベント</summary>
    public event EventHandler<MouseGestureEventArgs> DirectionCaptured;
    /// <summary>コマンド実行後 イベント</summary>
    public event EventHandler<MouseGestureEventArgs> CommandExecuted;
    #endregion

    #region Private Fields
    /// <summary>コマンド最大文字数</summary>
    private int m_maxCount;
    /// <summary>マウスジェスチャ登録用コレクション</summary>
    private Dictionary<string, Action> m_gestures;
    /// <summary>イベント解除用</summary>
    private CompositeDisposable disposables = new CompositeDisposable();
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MouseGesture()
    {
      this.m_gestures = new Dictionary<string, Action>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// ジェスチャコマンドの追加
    /// </summary>
    /// <param name="commandKey">コマンド</param>
    /// <param name="command">ジェスチャ</param>
    public void Add(string commandKey, Action command)
    {
      this.m_gestures.Add(commandKey, command);
      if (this.m_maxCount < commandKey.Length)
        this.m_maxCount = commandKey.Length;
    }

    /// <summary>
    /// マウスジェスチャ検出開始
    /// </summary>
    /// <param name="target">マウスジェスチャを動作させるコントロール</param>
    /// <param name="button">マウスボタン</param>
    /// <param name="interval">移動方向の検出に必要な距離(画素数)</param>
    public void Run(Control target, MouseButtons button, int interval)
    {
      var commandKey = string.Empty;

      //ドラッグ中の処理
      this.disposables.Add(
        target.MouseDragAsObservable(button, null, null)
          .Select(    //<= マウスの移動方向を↑,↓,←,→の文字に置き換える
          e =>
          {
            var arrow = GetArrowString(e.Location.X - e.StartLocation.X,
                                       e.Location.Y - e.StartLocation.Y,
                                       interval);
            if (arrow != string.Empty)
              e.StartLocation = e.Location;
            return arrow;
          })
          .Where(e => e != string.Empty)  //<= 何れかの方向が検知された場合のみ通す
          .Subscribe(
          e =>
          {
            if (commandKey.Length < this.m_maxCount && !commandKey.EndsWith(e))
            {
              commandKey += e;
              if (DirectionCaptured != null)
                DirectionCaptured(this, new MouseGestureEventArgs(commandKey));
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
            if (m_gestures.TryGetValue(commandKey, out gesture))
            {
              gesture();
              if (CommandExecuted != null)
                CommandExecuted(this, new MouseGestureEventArgs(commandKey));
            }
            commandKey = string.Empty;
          })
      );
    }

    /// <summary>
    /// マウスジェスチャ検出停止
    /// </summary>
    public void Stop()
    {
      this.disposables.Clear();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 矢印の取得
    /// </summary>
    /// <param name="dx">移動量x</param>
    /// <param name="dy">移動量y</param>
    /// <param name="interval">矢印の取得に必要な移動量</param>
    /// <returns>移動方向を表す文字(↑,↓,←,→)</returns>
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
    #endregion

    #region Nested Classes
    /// <summary>
    /// マウスジェスチャイベントデータ
    /// </summary>
    public class MouseGestureEventArgs : EventArgs
    {
      /// <summary>コマンド</summary>
      public string CommandKey { set; get; }
      /// <summary>
      /// コンストラクタ
      /// </summary>
      /// <param name="commandKey">コマンド</param>
      public MouseGestureEventArgs(string commandKey)
      {
        this.CommandKey = commandKey;
      }
    }
    #endregion
  }
}
