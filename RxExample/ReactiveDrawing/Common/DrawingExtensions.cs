using System.Drawing;

namespace ReactiveDrawing
{
  /// <summary>
  /// System.Drawing名前空間クラスの拡張メソッドを提供するクラス
  /// </summary>
  public static class DrawingExtensions
  {
    /// <summary>
    /// Rectangleの負のサイズを正の値に変換する
    /// </summary>
    /// <param name="rect">矩形</param>
    /// <returns>変換後の矩形</returns>
    public static Rectangle Abs(this Rectangle rect)
    {
      int x = rect.Location.X;
      int y = rect.Location.Y;
      int width = rect.Size.Width;
      int height = rect.Size.Height;

      if (rect.Size.Width < 0)
      {
        x += rect.Size.Width;
        width *= -1;
      }
      if (rect.Size.Height < 0)
      {
        y += rect.Size.Height;
        height *= -1;
      }
      return new Rectangle(x, y, width, height);
    }
    
  }
}
