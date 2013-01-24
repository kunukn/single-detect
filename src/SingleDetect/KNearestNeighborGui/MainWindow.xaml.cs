using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SingleDetectGuiCore.Code;
using SingleDetectGuiCore.Code.Gui;
using SingleDetectGuiCore.Code.Logging;
using SingleDetectLibrary.Code;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.Logging;

namespace KNearestNeighborGui
{
    /// <summary>   
    /// Author: Kunuk Nykjaer    
    /// MIT license        
    /// </summary>
    public partial class MainWindow
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly ILog2 _log = new NoLog(); //new Log4Net();

        readonly DispatcherTimer _dispatcherTimer;
        readonly RenderTargetBitmap _renderTargetBitmap =
            new RenderTargetBitmap(W, H, 96, 96, PixelFormats.Pbgra32);
        readonly DrawingVisual _drawingVisual = new DrawingVisual();

        private readonly string _elapsedAlgoInit;
        private string _elapsedAlgoUpdateKnn;
        public readonly Random Rand = new Random();
        readonly ISingleDetectAlgorithm _algorithm;
        private readonly Animation _animation;

        // Always finish current frame update before next frame is started
        private bool _isRunningFrameUpdate;

        #region ** config **

        public const int K = 100; // K nearest neighbors
        public const int DotsCount = 1000; // dots      
        public static readonly int DotsMovingCount = 100; // moving dots per frame
        const int Dotsize = 1; // draw dot size    

        // View port
        private static readonly Rectangle Rect = new Rectangle
                                                     {
                                                         XMin = 0, YMin = 0, 
                                                         XMax = 600, YMax = 500,

        // For Single detect: Dots where nearest neighbor has distance larger than this are single dots
        // For KNN: Defining the grid length
                                                         MaxDistance = 50,
                                                     };

        private const bool IsDrawEnabled = true;     
      
        #endregion  ** config **

        static readonly int W = (int)Rect.Width;
        static readonly int H = (int)Rect.Height;

        #region ** img **
        public ImageSource Img
        {
            get { return (ImageSource)GetValue(ImgProperty); }
            set { SetValue(ImgProperty, value); }
        }
        public static readonly DependencyProperty ImgProperty =
            DependencyProperty.Register("Img", typeof(ImageSource), typeof(MainWindow));
        #endregion img

        #region ** sliders **

        // Frame rate
        public int SliderTop
        {
            get { return (int)GetValue(SliderTopProperty); }
            set { SetValue(SliderTopProperty, value); }
        }
        public static readonly DependencyProperty SliderTopProperty =
            DependencyProperty.Register("SliderTop", typeof(int), typeof(MainWindow),
            new UIPropertyMetadata(200)); // default frame rate (1 frame per x msec)

        // Movement speed
        public int SliderBottom
        {
            get { return (int)GetValue(SliderBottomProperty); }
            set { SetValue(SliderBottomProperty, value); }
        }
        public static readonly DependencyProperty SliderBottomProperty =
            DependencyProperty.Register("SliderBottom", typeof(int), typeof(MainWindow),
            new UIPropertyMetadata(10)); // default value

        // Show Grid
        public int SliderLeft
        {
            get { return (int)GetValue(SliderLeftProperty); }
            set { SetValue(SliderLeftProperty, value); }
        }
        public static readonly DependencyProperty SliderLeftProperty =
            DependencyProperty.Register("SliderLeft", typeof(int), typeof(MainWindow),
            new UIPropertyMetadata(0)); // default value

        #endregion sliders

        static void Init()
        {
            DrawUtil.IsDrawEnabled = IsDrawEnabled;
            DrawUtil.Init(Dotsize);
        }

        void InitDraw()
        {
            // Draw all points
            using (DrawingContext dc = _drawingVisual.RenderOpen())
            {
                // Clear background                
                DrawUtil.ClearBackground(dc, (int)Rect.XMin, (int)Rect.YMin, W, H);

                // Draw all points                
                DrawUtil.DrawDots(dc, _algorithm.Points);

                dc.Close();
            }
            _renderTargetBitmap.Render(_drawingVisual);
        }

        public MainWindow()
        {
            try
            {
                Init();

                Img = _renderTargetBitmap;  // Set up the image source
                this.DataContext = this;    // Enable the controls to bind to the properties

                InitializeComponent(); // wpf

                var points = new List<P>();                
                points.Add(new P{X = W/2,Y = H/2,});
                var rand = new Random();
                for (var i = 0; i < DotsCount; i++)
                {
                    points.Add(new P
                    {
                        X = rand.Next((int)Rect.XMin, (int)Rect.XMax),
                        Y = rand.Next((int)Rect.YMin, (int)Rect.YMax),
                    });
                }


                _stopwatch.Start();
                _algorithm = new SingleDetectAlgorithm(points, Rect, StrategyType.Grid, _log);
                _animation = new Animation(_algorithm, Rect);

                _stopwatch.Stop();
                _elapsedAlgoInit = _stopwatch.Elapsed.ToString();
                _stopwatch.Reset();

                // Draw all points once
                InitDraw();

                // Render on frame rate
                _dispatcherTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(SliderTop),
                                         DispatcherPriority.Normal,
                                         FrameUpdate,
                                         this.Dispatcher) { IsEnabled = true };
            }
            catch (Exception ex)
            {
                _log.Error(MethodBase.GetCurrentMethod(), ex);
                throw;
            }
        }

        //
        void FrameUpdate(object sender, EventArgs e)
        {
            if (_isRunningFrameUpdate) return;

            _isRunningFrameUpdate = true;

            // Movement range
            var max = SliderBottom;
            var min = -max;

            _stopwatch.Start();

            // Draw on the drawing context
            using (DrawingContext dc = _drawingVisual.RenderOpen())
            {
                _animation.SelectMovingDots(DotsMovingCount);
                
                // Clear prev frame knn
                DrawUtil.RedrawDots(dc, new[] { _algorithm.Knn.Origin }, ShapeType.Selected);
                DrawUtil.RedrawDots(dc, _algorithm.Knn.GetNNs(), ShapeType.NearestNeighbor);

                // Clear prev frame moving dots                
                DrawUtil.ClearDots(dc, _animation.Moving);

                // Update moving pos                
                _animation.UpdateMovingPosition(min, max);

                // Draw updated pos                                
                DrawUtil.DrawDots(dc, _animation.Moving);

                // Update KNN
                var origin = _algorithm.Points[0]; // first item as origin
                _elapsedAlgoUpdateKnn = string.Format("msec {0}", _algorithm.UpdateKnn(origin, K));

                // Draw updated KNN                            
                DrawUtil.DrawDots(dc, new[] { origin }, ShapeType.Selected);
                DrawUtil.DrawDots(dc, _algorithm.Knn.GetNNs(), ShapeType.NearestNeighbor);

                var showGrid = SliderLeft == 1; // toggle                
                DrawUtil.DrawGrid(dc, showGrid, W, H, Rect.XGrid, Rect.YGrid);

                dc.Close();
            }

            _stopwatch.Stop();

            var sb = new StringBuilder();
            sb.AppendFormat("\n\nK Nearest Neighbors: \n\n");
            sb.AppendFormat("{0}\n", _algorithm.Strategy.Name);
            sb.AppendFormat("Origin: {0}\n", _algorithm.Knn.Origin);
            //sb.AppendFormat(_algorithm.Knn.NNs.Aggregate("", (a, b) => a + b + "\n"));
            sb.AppendFormat("K: {0}\n", _algorithm.Knn.K);
            sb.AppendFormat("NNs: {0}\n", _algorithm.Knn.NNs.Count);
            sb.AppendFormat("MaxDistance: {0}\n", _algorithm.Rect.MaxDistance);
            sb.AppendFormat("Square: {0}\n\n", _algorithm.Rect.Square);
            sb.AppendFormat("SliderTop\nmsec per frame: {0}\n\n", SliderTop);
            sb.AppendFormat("SliderLeft\nshow grid: {0}\n\n", SliderLeft);
            sb.AppendFormat("SliderBottom\nmovement speed: {0}\n\n", SliderBottom);
            sb.AppendFormat("dots: {0}\n", _algorithm.Points.Count);
            sb.AppendFormat("moving dots: {0}\n", _animation.Moving.Count);
            sb.AppendFormat("grid: {0};{1}\n", _algorithm.Rect.XGrid, _algorithm.Rect.YGrid);
            sb.AppendFormat("Draw enabled: {0}\n", DrawUtil.IsDrawEnabled);
            sb.AppendFormat("\nElapsed Algo Init: \n{0}\n", _elapsedAlgoInit);
            sb.AppendFormat("\nElapsed Algo update Knn: \n{0}\n", _elapsedAlgoUpdateKnn);
            sb.AppendFormat("\n\nTime per frame: \n{0}\n", _stopwatch.Elapsed.ToString());

            _textBlock.Text = sb.ToString();

            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(SliderTop); // update
            _renderTargetBitmap.Render(_drawingVisual); // Render the drawing to the bitmap

            _stopwatch.Reset();            
            _isRunningFrameUpdate = false;
        }
    }
}
