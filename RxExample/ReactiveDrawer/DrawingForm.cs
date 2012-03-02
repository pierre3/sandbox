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
      drawingManager.Start(this, MouseButtons.Left);

      var gesture = new MouseGesture();
      gesture.DirectionCaptured += (o, e) => this.Text = e.Gesture;
      gesture.Add("→←→",
        () =>
        {
          drawingManager.Clear();
          this.Text = "クリア";
          this.Refresh();
        });
      gesture.Add("↑↓",
        () =>
        {
          drawingManager.DefaultItem = new EllipsePen(Color.Red);
          this.Text = "楕円";
          this.Refresh();
        });
      gesture.Add("↑→↓←",
        () =>
        {
          drawingManager.DefaultItem = new RectanglePen(Color.Blue);
          this.Text = "四角形";
          this.Refresh();
        });
      gesture.Add("↓→↑",
        () =>
        {
          drawingManager.DefaultItem = DrawingManager.Selector;
          this.Text = "選択";
          this.Refresh();
        });
      gesture.Start(this, MouseButtons.Right, 30);


      this.Paint += (o, e) => drawingManager.Draw(e.Graphics);

    }


  }
}
