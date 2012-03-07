using System;
using System.Collections.Generic;
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
    private int maxCount;
    /// <summary>マウスジェスチャ登録用コレクション</summary>
    private Dictionary<string, Action> gestures;
    /// <summary>イベント解除用</summary>
    private IDisposable disposable;
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MouseGesture()
    {
      this.gestures = new Dictionary<string, Action>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// ジェスチャの追加
    /// </summary>
    /// <param name="gesture">ジェスチャパターン</param>
    /// <param name="command">コマンド</param>
    public void Add(string gesture, Action command)
    {
      this.gestures.Add(gesture, command);
      if (this.maxCount < gesture.Length)
        this.maxCount = gesture.Length;
    }

    /// <summary>
    /// マウスジェスチャ検出開始
    /// </summary>
    /// <param name="target">マウスジェスチャを動作させるコントロール</param>
    /// <param name="button">マウスボタン</param>
    /// <param name="interval">移動方向の検出に必要な距離(画素数)</param>
    public void Start(Control target, MouseButtons button, int interval)
    {
      var move = target.MouseMoveAsObservable();
      var up = target.MouseUpAsObservable().Where(e => e.Button == button);

      this.disposable =
        target.MouseDownAsObservable().Where(e => e.Button == button)
          .SelectMany(arg0 =>
          {
            return move.TakeUntil(up)
              .Select(arg1 =>
              {
                //マウスの移動方向を↑,↓,←,→の文字に置き換える
                var arrow = GetArrowChar(arg1.Location.X - arg0.Location.X,
                                         arg1.Location.Y - arg0.Location.Y,
                                         interval);
                if (arrow != char.MinValue) //方向を検知(interval以上移動)した場合
                  arg0 = arg1;  //起点を更新
                return arrow;
              })
             .Where(arrow => arrow != char.MinValue)  //方向を検知した場合のみ通す
             .DistinctUntilChanged()                  //同じ方向を連続して通さない
             .Take(this.maxCount)                     //最大数(最長コマンドの長さ)までを取得
             .Aggregate(string.Empty,                 //矢印を連結してstringにする
                        (gesture, arrow) =>
                        {
                          OnDirectionCaptured(gesture + arrow);
                          return gesture + arrow;
                        })
             .Zip(up, (gesture, _) => gesture); //マウスUpが来るまで待機
          })
          .Subscribe(gesture =>
          {
            //ドラッグで取得したジェスチャパターンが存在したら、対応するコマンドを実行
            Action command;
            if (gestures.TryGetValue(gesture, out command))
            {
              command();
              OnCommandExcuted(gesture);
            }
          });
    }

    /// <summary>
    /// マウスジェスチャ検出停止
    /// </summary>
    public void Stop()
    {
      this.disposable.Dispose();
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
    private char GetArrowChar(int dx, int dy, int interval)
    {
      int px = Math.Abs(dx);
      int py = Math.Abs(dy);
      if (px > py)
      {
        if (px < interval)
          return char.MinValue;
        return (dx > 0) ? '→' : '←';
      }
      else
      {
        if (py < interval)
          return char.MinValue;
        return (dy > 0) ? '↓' : '↑';
      }
    }

    /// <summary>
    /// 方向検知後イベント 発生
    /// </summary>
    /// <param name="gesture">検知済み移動方向パターン</param>
    private void OnDirectionCaptured(string gesture)
    {
      if (this.DirectionCaptured != null)
        this.DirectionCaptured(this, new MouseGestureEventArgs(gesture));
    }

    /// <summary>
    /// コマンド実行後イベント 発生
    /// </summary>
    /// <param name="gesture">ジェスチャパターン</param>
    private void OnCommandExcuted(string gesture)
    {
      if (this.CommandExecuted != null)
        this.CommandExecuted(this, new MouseGestureEventArgs(gesture));
    }
    #endregion

    #region Nested Classes
    /// <summary>
    /// マウスジェスチャイベントデータ
    /// </summary>
    public class MouseGestureEventArgs : EventArgs
    {
      /// <summary>ジェスチャパターン文字列</summary>
      public string Gesture { set; get; }
      /// <summary>
      /// コンストラクタ
      /// </summary>
      /// <param name="gesture">ジェスチャパターン</param>
      public MouseGestureEventArgs(string gesture)
      {
        this.Gesture = gesture;
      }
    }
    #endregion
  }
}
