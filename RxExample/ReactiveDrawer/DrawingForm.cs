using System.Drawing;
using System.Windows.Forms;
using ReactiveDrawing;
using ReactiveDrawing.Shapes;

namespace DrawingFormApp
{
  public partial class DrawingForm : Form
  {
    public DrawingForm()
    {
      InitializeComponent();
      this.DoubleBuffered = true;
      
      var drawingController = new DrawingManager(new RectanglePen(Color.Blue));
      drawingController.Run(this, MouseButtons.Left);

      MouseGesture gestures = new MouseGesture();
      gestures.MotionCaptured += (o, e) => this.Text = e.Command;
      gestures.Add("→←→",
        () =>
        {
          drawingController.Clear();
          this.Text = "";
          this.Refresh();
        });
      gestures.Add("↑↓",
        () =>
        {
          drawingController.DefaultItem.Color = Color.Red;
          this.Text = "";
          this.Refresh();
        });
      gestures.Run(this, MouseButtons.Right, 30);

      this.Paint += (o, e) => drawingController.Draw(e.Graphics);
        
    }


  }
}
