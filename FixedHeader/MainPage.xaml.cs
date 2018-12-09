using Xamarin.Forms;

namespace FixedHeader
{
    public partial class MainPage : ContentPage
    {
        double _titleTextTop;

        public MainPage()
        {
            InitializeComponent();
            TheScroll.PropertyChanged += OnScrollViewPropertyChanged;
            BearImage.SizeChanged += OnBearImageSizeChanged;
            TitleText.SizeChanged += OnTitleTextSizeChanged;
        }

        private void OnTitleTextSizeChanged(object sender, System.EventArgs e)
        {
            TitleText.SizeChanged -= OnTitleTextSizeChanged;

            //As soon as the news header has been repositioned, we can grab the actual screen top position
            _titleTextTop = TitleText.Y;

            //Remark: GetScreenCoordinates will get the actual position on screen instead of the actual position inside the parent
            //_titleTextTop = GetScreenCoordinates(TitleText).Y;
        }

        private void OnBearImageSizeChanged(object sender, System.EventArgs e)
        {
            //When the bear image has been loaded, reposition the news header to the bottom of this image
            TitleText.Margin = new Thickness(0, BearImage.Height - 40, 0, 0);
        }

        private void OnScrollViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(ScrollView.ScrollYProperty.PropertyName))
            {
                var scrolled = ((ScrollView)sender).ScrollY;
                System.Diagnostics.Debug.WriteLine($"Y position: {scrolled.ToString()}");

                if (scrolled < _titleTextTop)
                    TitleText.TranslationY = (0 - scrolled);
                else
                    TitleText.TranslationY = (0 - _titleTextTop);
            }
        }

        public (double X, double Y) GetScreenCoordinates(VisualElement view)
        {
            // A view's default X- and Y-coordinates are LOCAL with respect to the boundaries of its parent,
            // and NOT with respect to the screen. This method calculates the SCREEN coordinates of a view.
            // The coordinates returned refer to the top left corner of the view.

            // Initialize with the view's "local" coordinates with respect to its parent
            double screenCoordinateX = view.X;
            double screenCoordinateY = view.Y;

            // Get the view's parent (if it has one...)
            if (view.Parent.GetType() != typeof(App))
            {
                VisualElement parent = (VisualElement)view.Parent;


                // Loop through all parents
                while (parent != null)
                {
                    // Add in the coordinates of the parent with respect to ITS parent
                    screenCoordinateX += parent.X;
                    screenCoordinateY += parent.Y;

                    // If the parent of this parent isn't the app itself, get the parent's parent.
                    if (parent.Parent.GetType() == typeof(App))
                        parent = null;
                    else
                        parent = (VisualElement)parent.Parent;
                }
            }

            // Return the final coordinates...which are the global SCREEN coordinates of the view
            return (screenCoordinateX, screenCoordinateY);
        }
    }
}