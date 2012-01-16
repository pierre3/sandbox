using System;
using System.Drawing;
using System.Windows.Forms;
using ReactiveDrawing;
using ReactiveDrawing.Shapes;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Collections.Generic;

namespace DrawingFormApp
{
  public partial class DrawingForm : Form
  {
    /// <summary>
    /// メインフォーム
    /// </summary>
    public DrawingForm()
    {
      InitializeComponent();
      this.DoubleBuffered = true;

      var drawingManager = new DrawingManager(new RectanglePen(Color.Blue));
      drawingManager.Run(this, MouseButtons.Left);

      var gestures = new MouseGesture();
      gestures.DirectionCaptured += (o, e) => this.Text = e.CommandKey;
      gestures.Add("→←→",
        () =>
        {
          drawingManager.Clear();
          this.Text = "クリア";
          this.Refresh();
        });
      gestures.Add("↑↓",
        () =>
        {
          drawingManager.DefaultItem = new EllipsePen(Color.Red);
          this.Text = "楕円";
          this.Refresh();
        });
      gestures.Add("↑→↓←",
        () =>
        {
          drawingManager.DefaultItem = new RectanglePen(Color.Blue);
          this.Text = "四角形";
          this.Refresh();
        });
      gestures.Add("↓→↑",
        () =>
        {
          drawingManager.DefaultItem = DrawingManager.Selector;
          this.Text = "選択";
          this.Refresh();
        });
      gestures.Run(this, MouseButtons.Right, 30);


      this.Paint += (o, e) => drawingManager.Draw(e.Graphics);

    }


  }
}
