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
    /// <summary>規定の描画オブジェクト</summary>
    public IShape DefaultItem { set; get; }

    #region Private Fields
    
    private List<IShape> m_shapes;
    private CompositeDisposable m_disposables;
    
    #endregion Private Fields

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="defaultItem">規定の描画オブジェクト</param>
    public DrawingManager(IShape defaultItem)
    {
      this.DefaultItem = defaultItem;
      this.m_shapes = new List<IShape>();
      this.m_disposables = new CompositeDisposable();
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
      
      this.m_disposables.Add(
        //MouseMoveイベント
        target.MouseMoveAsObservable()
          .Where(e => e.Button == MouseButtons.None)  //<-ボタンが押されていない場合のみ通す
          .Subscribe(
          e =>
          {
            //カーソルの下にあるm_shapes内のオブジェクトをactiveに設定する。
            //オブジェクトのない場所にカーソルがある場合はDefaultItemを設定する。
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
        })
      );

      this.m_disposables.Add(
        //MouseDragイベント
        target.MouseDragAsObservable(button,
          e =>
          {
            foreach (IShape item in m_shapes)
            {
              item.IsSelected = item.Group == active.Group;
            }
            target.Refresh();
          },
          e =>
          {
            var item = active.Drop() as IShape;
            if (item != null)
              m_shapes.Add(item);
            target.Refresh();
          })
          .Subscribe(
          e =>
          {
            active.Drag(e);
            target.Refresh();
          })
      );

    }
  }
}
