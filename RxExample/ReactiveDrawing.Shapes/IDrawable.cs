using System.Drawing;
using System.Windows.Forms;

namespace ReactiveDrawing
{
  /// <summary>
  /// System.Drawing.Graphicsによる描画を行うインタフェース
  /// </summary>
  public interface IDrawable
  {
    /// <summary>色</summary>
    Color Color { set; get; }

    /// <summary>
    /// 図形描画
    /// </summary>
    /// <param name="g">Graphicsオブジェクト</param>
    void Draw(Graphics g);
  }
}
