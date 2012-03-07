using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// 図形 基本クラス
  /// </summary>
  public abstract class Shape : IDraggable, IDrawable, ISelectable
  {
    #region Events
    /// <summary>ドロップ実行後に発生するイベント</summary>
    public event EventHandler Dropped;

    /// <summary>ドラッグ実行後に発生するイベント</summary>
    public event EventHandler<MouseDragEventArgs> Draged;

    /// <summary>選択状態変更イベント</summary>
    public event EventHandler SelectChanged;
    #endregion

    #region Properties
    /// <summary>マウスカーソル</summary>
    public Cursor Cursor { set; get; }

    /// <summary>色</summary>
    public Color Color { set; get; }

    /// <summary>外接矩形</summary>
    public Rectangle Bounds
    {
      get { return _bounds; }
      set
      { this.SetBounds(value.X, value.Y, value.Width, value.Height); }
    }

    /// <summary>ドラッグされている間 Trueを返す</summary>
    public bool IsDragging { get; protected set; }

    /// <summary>選択されている間 Trueを返す</summary>
    public bool IsSelected
    {
      get { return this._isSelected; }
      set
      {
        if (this._isSelected == value)
          return;
        this._isSelected = value;
        OnSelectChanged();
      }
    }

    /// <summary>操作中のオブジェクト</summary>
    public ResizeHandle ActiveHandle { get; protected set; }

    /// <summary>リサイズハンドル</summary>
    protected ResizeHandle[] Handles { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// コンストラクタ
    /// </summary>
    protected Shape()
      : this(new Rectangle(), Color.Black)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    protected Shape(Rectangle bounds)
      : this(bounds, Color.Black)
    { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bounds">外接矩形</param>
    /// <param name="color">色</param>
    protected Shape(Rectangle bounds, Color color)
    {
      this.Handles = new ResizeHandle[0];
      this.Bounds = bounds.Abs();
      this.Color = color;
      
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 選択枠による選択
    /// </summary>
    /// <remarks>
    /// Boundsが選択枠に含まれる場合にIsSelectedをTrueに設定する。
    /// 含まれない場合はFalseに設定する
    /// </remarks>
    /// <param name="selectRect">選択枠</param>
    public void SelectBy(Rectangle selectRect)
    {
      this.IsSelected = selectRect.Contains(this.Bounds.Abs());
    }

    /// <summary>
    /// 指定ハンドルをアクティブにする
    /// </summary>
    /// <param name="alignment">アクティブにするハンドルを指定</param>
    public void ActiveateHandle(ResizeHandle.HandleAlignment alignment)
    {
      this.ActiveHandle = this.Handles.Where(h => h.Alignment == alignment)
                                      .FirstOrDefault();
    }

    #region Abstract Methods
    /// <summary>
    /// 図形を描画します
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    public abstract void Draw(Graphics g);

    /// <summary>
    /// ドラッグ中に実行される処理
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    public abstract void Drag(MouseDragEventArgs e);

    /// <summary>
    /// マウスポインタとの当たり判定
    /// </summary>
    /// <param name="location">ポインタ座標</param>
    /// <returns>マウスポインタと重なっているIDraggableオブジェクト</returns>
    public abstract IDraggable HitTest(Point location);

    /// <summary>
    /// ドラッグ終了時に実行される処理
    /// </summary>
    /// <returns>
    /// ドロップ時に生成されるIDraggableオブジェクト
    /// </returns>
    public abstract IDraggable Drop();
    #endregion
    #endregion

    #region Protected Methods
    /// <summary>
    /// 選択状態変更イベント 発生
    /// </summary>
    protected void OnSelectChanged()
    {
      if (this.SelectChanged != null)
        this.SelectChanged(this, new EventArgs());
    }

    /// <summary>
    /// Dragedイベントを発生させる
    /// </summary>
    /// <param name="e">マウスドラッグイベントデータ</param>
    protected void OnDraged(MouseDragEventArgs e)
    {
      if (this.Draged != null)
        this.Draged(this, e);
    }

    /// <summary>
    /// Droppedイベントを発生させる
    /// </summary>
    /// <param name="e">イベントデータ</param>
    protected void OnDropped(EventArgs e)
    {
      if (this.Dropped != null)
        this.Dropped(this, e);
    }

    /// <summary>
    /// 外接矩形の設定
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="width">幅</param>
    /// <param name="height">高さ</param>
    /// <remarks>
    /// 新しい矩形の座標に合わせてリサイズハンドルの位置を再設定します
    /// </remarks>
    protected void SetBounds(int x, int y, int width, int height)
    {
      _bounds = new Rectangle(x, y, width, height);
      foreach (ResizeHandle handle in this.Handles)
      {
        handle.SetLocation();
      }
    }

    /// <summary>
    /// 外接矩形の設定
    /// </summary>
    /// <param name="location">左上座標</param>
    /// <param name="size">幅,高さ</param>
    /// <remarks>
    /// 新しい矩形の座標に合わせてリサイズハンドルの位置を再設定します
    /// </remarks>
    protected void SetBounds(Point location, Size size)
    {
      this.SetBounds(location.X, location.Y, size.Width, size.Height);
    }


    #endregion

    #region Private Feilds
    /// <summary>選択状態</summary>
    private bool _isSelected;
    private Rectangle _bounds;
    #endregion
  }
}
