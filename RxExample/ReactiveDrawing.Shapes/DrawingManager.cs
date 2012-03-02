using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using ReactiveDrawing.Shapes;
using System.Linq;

namespace ReactiveDrawing
{
  /// <summary>
  /// 図形描画オブジェクト管理クラス
  /// </summary>
  public class DrawingManager
  {
    #region Properties
    /// <summary>規定のドラッグオブジェクト</summary>
    public IDraggable DefaultItem { set; get; }
    /// <summary>管理対象オブジェクトのコレクション</summary>
    private List<IDraggable> Items { get; set; }
    /// <summary>選択中のオブジェクトを管理するオブジェクト</summary>
    private CompositeDraggable SelectedItems { set; get; }
    /// <summary>選択可能なオブジェクトのコレクション</summary>
    private IEnumerable<ISelectable> Selectables { get; set; }
    /// <summary>マウスイベントのデタッチ用</summary>
    private CompositeDisposable Disposables { get; set; }
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
    public DrawingManager(IDraggable defaultItem)
    {
      this.DefaultItem = defaultItem;
      this.Items = new List<IDraggable>();
      this.Disposables = new CompositeDisposable();
      this.Selectables = Enumerable.Empty<ISelectable>();
      this.SelectedItems = new CompositeDraggable(Enumerable.Empty<IDraggable>());
      DrawingManager.Selector.Dropped += (o, e) =>
      {
        var selectBounds = (o as Shape).Bounds.Abs();
        foreach (ISelectable shape in this.Items.OfType<ISelectable>())
          shape.SelectBy(selectBounds);

        SelectedItems = new CompositeDraggable(
          this.Selectables.Where(item => item.IsSelected).OfType<IDraggable>());
      };
    }

    /// <summary>
    /// 図形描画オブジェクトのクリア
    /// </summary>
    public void Clear()
    {
      this.Items.Clear();
      this.Selectables = Enumerable.Empty<ISelectable>();
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public void Draw(Graphics g)
    {

      foreach (IDrawable item in Items.OfType<IDrawable>())
        item.Draw(g);

      var DrawItem = DefaultItem as IDrawable;
      if (DrawItem != null)
        DrawItem.Draw(g);

    }

    /// <summary>
    /// マウスイベントの停止
    /// </summary>
    public void Stop()
    {
      this.Disposables.Clear();
    }

    /// <summary>
    /// マウスイベントの開始
    /// </summary>
    /// <param name="target">イベント発生元コントロール</param>
    /// <param name="button">ドラッグイベントに対応するマウスボタン</param>
    public void Start(Control target, MouseButtons button)
    {

      IDraggable active = this.DefaultItem;

      //ボタンが押されていない間のマウスムーブイベント
      this.Disposables.Add(
        target.MouseMoveAsObservable()
          .Where(e => e.Button == MouseButtons.None)
          .Subscribe(
          e =>
          {
            //選択中のオブジェクトを先行してチェック
            active = SelectedItems.HitTest(e.Location);
            if (active != null)
            {
              target.Cursor = active.Cursor;
              return;
            }

            //カーソルの下にあるm_shapes内のオブジェクトをactiveに設定する。
            //オブジェクトのない場所にカーソルがある場合はDefaultItemがActiveとなる。
            active = DefaultItem;

            foreach (IDraggable item in Items.Reverse<IDraggable>())
            {
              var result = item.HitTest(e.Location);
              if (result != null)
              {
                active = result;
                break;
              }
            };
            target.Cursor = active.Cursor;
          }));

      //マウスダウンイベント
      this.Disposables.Add(
        target.MouseDownAsObservable()
          .Subscribe(
          e =>
          {
            var act = active as ISelectable;
            if (act != null && !act.IsSelected)
            {
              foreach (ISelectable item in this.Selectables)
              {
                item.IsSelected = item == act;
              }
              this.SelectedItems = new CompositeDraggable(Enumerable.Empty<IDraggable>());
              target.Refresh();
            };
          }));

      //ドラッグイベント
      this.Disposables.Add(
        target.MouseDragAsObservable(button)
          .Subscribe(
          e =>
          {
            active.Drag(e);
            target.Refresh();
          }));

      //マウスアップイベント
      this.Disposables.Add(
         target.MouseUpAsObservable()
           .Subscribe(
           e =>
           {
             var item = active.Drop();
             if (item != null)
               this.AddItem(item);
             target.Refresh();
           }));
    }
    #endregion

    #region Private Methods
    private void AddItem(IDraggable item)
    {
      Items.Add(item);
      Selectables = Items.OfType<ISelectable>();
    }
    #endregion

  }
}
