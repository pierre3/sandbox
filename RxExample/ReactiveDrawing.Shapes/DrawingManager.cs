using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using ReactiveDrawing.Shapes;

namespace ReactiveDrawing
{
  /// <summary>
  /// 図形描画オブジェクト管理クラス
  /// </summary>
  public class DrawingManager
  {
    #region Properties
    /// <summary>規定の描画オブジェクト</summary>
    public IShape DefaultItem { set; get; }
    #endregion

    #region Constants
    /// <summary>
    /// 選択オブジェクト
    /// </summary>
    public static readonly SelectRect Selector = new SelectRect(Color.Black);
    #endregion

    #region Public Methods
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="defaultItem">規定の描画オブジェクト</param>
    public DrawingManager(IShape defaultItem)
    {
      this.DefaultItem = defaultItem;
      this.m_shapes = new List<IShape>();
      this.m_disposables = new CompositeDisposable();
      DrawingManager.Selector.Dropped += (o, e) =>
      {
        var selectBounds = (o as IShape).Bounds.Abs();
        this.m_shapes.ForEach(
          shape => shape.IsSelected = selectBounds.Contains(shape.Bounds));
      };
    }

    /// <summary>
    /// 図形描画オブジェクトのクリア
    /// </summary>
    public void Clear()
    {
      this.m_shapes.Clear();
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public void Draw(Graphics g)
    {
      m_shapes.ForEach(item => item.Draw(g));
      DefaultItem.Draw(g);
    }

    /// <summary>
    /// マウスイベントの停止
    /// </summary>
    public void Stop()
    {
      this.m_disposables.Clear();
    }

    /// <summary>
    /// マウスイベントの開始
    /// </summary>
    /// <param name="target">イベント発生元コントロール</param>
    /// <param name="button">ドラッグイベントに対応するマウスボタン</param>
    public void Run(Control target, MouseButtons button)
    {

      IShape active = this.DefaultItem;

      //ボタンが押されていない間のマウスムーブイベント
      this.m_disposables.Add(
        target.MouseMoveAsObservable()
          .Where(e => e.Button == MouseButtons.None)
          .Subscribe(
          e =>
          {
            //カーソルの下にあるm_shapes内のオブジェクトをactiveに設定する。
            //オブジェクトのない場所にカーソルがある場合はDefaultItemがActiveとなる。
            active = this.DefaultItem;
            foreach (IShape item in m_shapes)
            {
              var result = item.HitTest(e.Location) as IShape;
              if (result != null)
              {
                active = result;
                break;
              }
            };
            target.Cursor = active.Cursor;
          }));

      //マウスダウンイベント
      this.m_disposables.Add(
        target.MouseDownAsObservable()
          .Subscribe(
          e =>
          {
            foreach (IShape item in m_shapes)
            {
              item.IsSelected = item.Parent == active.Parent;
            }
            target.Refresh();
          }));

      //ドラッグイベント
      this.m_disposables.Add(
        target.MouseDragAsObservable(button)
          .Subscribe(
          e =>
          {
            active.Drag(e);
            target.Refresh();
          }));

      //マウスアップイベント
      this.m_disposables.Add(
         target.MouseUpAsObservable()
           .Subscribe(
           e =>
           {
             var item = active.Drop() as IShape;
             if (item != null)
               m_shapes.Add(item);
             target.Refresh();
           }));
    }
    #endregion

    #region Private Fields
    private List<IShape> m_shapes;
    private CompositeDisposable m_disposables;
    #endregion

  }
}
