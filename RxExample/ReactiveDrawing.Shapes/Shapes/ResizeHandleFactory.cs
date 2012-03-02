using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing.Shapes
{
  /// <summary>
  /// リサイズハンドル生成メソッドクラス
  /// </summary>
  public static class ResizeHandleFactory
  {
    #region Shape Extensions
    /// <summary>
    /// 左上リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateTopLeftHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeNWSE, handleSize,
                           () => shape.Bounds.Location,
                           ResizeHandle.HandleAlignment.TopLeft);
      result.Draged += (_, e) =>
        shape.Bounds = new Rectangle(e.Location.X,
                                     e.Location.Y,
                                     shape.Bounds.Right - e.Location.X,
                                     shape.Bounds.Bottom - e.Location.Y);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 上辺中央リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateTopCenterHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeNS, handleSize,
                           () => new Point((shape.Bounds.Left + shape.Bounds.Right) / 2, shape.Bounds.Top),
                           ResizeHandle.HandleAlignment.TopCenter);
      result.Draged += (_, e) =>
        shape.Bounds = new Rectangle(shape.Bounds.Left,
                                     e.Location.Y,
                                     shape.Bounds.Width,
                                     shape.Bounds.Bottom - e.Location.Y);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 右上リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateTopRightHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeNESW, handleSize,
                           () => new Point(shape.Bounds.Right, shape.Bounds.Top),
                           ResizeHandle.HandleAlignment.TopRight);
      result.Draged += (_, e) =>
        shape.Bounds = new Rectangle(shape.Bounds.Left,
                                     e.Location.Y,
                                     e.Location.X - shape.Bounds.Left,
                                     shape.Bounds.Bottom - e.Location.Y);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 左辺中央リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateCenterLeftHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeWE, handleSize,
                           () => new Point(shape.Bounds.Left, (shape.Bounds.Top + shape.Bounds.Bottom) / 2),
                           ResizeHandle.HandleAlignment.CenterLeft);
      result.Draged += (_, e) =>
        shape.Bounds = new Rectangle(e.Location.X,
                                     shape.Bounds.Top,
                                     shape.Bounds.Right - e.Location.X,
                                     shape.Bounds.Height);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 右辺中央リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateCenterRightHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeWE, handleSize,
                           () => new Point(shape.Bounds.Right, (shape.Bounds.Top + shape.Bounds.Bottom) / 2),
                           ResizeHandle.HandleAlignment.CenterRight);
      result.Draged += (_, e) =>
        shape.Bounds = new Rectangle(shape.Bounds.Left,
                                     shape.Bounds.Top,
                                     e.Location.X - shape.Bounds.Left,
                                     shape.Bounds.Height);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 左下リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateBottomLeftHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeNESW, handleSize,
                           () => new Point(shape.Bounds.Left, shape.Bounds.Bottom),
                           ResizeHandle.HandleAlignment.BottomLeft);
      result.Draged += (_, e) => 
        shape.Bounds = new Rectangle(e.Location.X, 
                                     shape.Bounds.Top,
                                     shape.Bounds.Right - e.Location.X, 
                                     e.Location.Y - shape.Bounds.Top);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 下辺中央リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateBottomCenterHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeNS, handleSize,
                           () => new Point((shape.Bounds.Left + shape.Bounds.Right) / 2, shape.Bounds.Bottom),
                           ResizeHandle.HandleAlignment.BottomCenter);
      result.Draged += (_, e) => 
        shape.Bounds = new Rectangle(shape.Bounds.Left, 
                                     shape.Bounds.Top,
                                     shape.Bounds.Width, 
                                     e.Location.Y - shape.Bounds.Top);
      result.SetLocation();
      return result;
    }

    /// <summary>
    /// 右下リサイズハンドルの生成
    /// </summary>
    /// <param name="shape">ハンドルを保持するShapeオブジェクト</param>
    /// <param name="handleSize">ハンドルのサイズ</param>
    /// <returns>リサイズハンドルのインスタンス</returns>
    public static ResizeHandle CreateBottomRightHandle(this Shape shape, Size handleSize)
    {
      var result
        = new ResizeHandle(shape.Color, Cursors.SizeNWSE, handleSize,
                           () => new Point(shape.Bounds.Right, shape.Bounds.Bottom),
                           ResizeHandle.HandleAlignment.BottomRight);
      result.Draged += (_, e) => 
        shape.Bounds = new Rectangle(shape.Bounds.Left, 
                                     shape.Bounds.Top,
                                     e.Location.X - shape.Bounds.Left, 
                                     e.Location.Y - shape.Bounds.Top);
      result.SetLocation();
      return result;
    }
    #endregion
  }
}
