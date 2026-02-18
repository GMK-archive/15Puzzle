using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[,] tiles = new Button[4, 4];
        private int emptyRow = 3; 
        private int emptyCol = 3;
        public MainWindow()
        {
            InitializeComponent();
            CreatePuzzle();
        }

        private void CreatePuzzle()
        {
            int number = 1;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if(y == 3 && x == 3)
                    {
                        break;
                    }
                    Button b = new Button();
                    //b.Height = 80;
                    //b.Width = 80;
                    b.Content = number.ToString();
                    b.Background = Brushes.AntiqueWhite;
                    b.FontFamily = new FontFamily("Bahnschrift SemiLight Condensed");
                    //b.FontFamily = new FontFamily("Freestyle Script");
                    b.FontSize = 24;
                    b.Click += Tile_Click;
                    Grid.SetRow(b, y);
                    Grid.SetColumn(b, x);
                    PuzzleGrid.Children.Add(b);
                    tiles[y, x] = b;
                    number++;
                }
            }
        }
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            int y = Grid.GetRow(clicked);
            int x = Grid.GetColumn(clicked);
            bool isAdjacent = 
                (y == emptyRow && Math.Abs(x - emptyCol) == 1) ||
                (x == emptyCol && Math.Abs(y - emptyRow) == 1);
            if (!isAdjacent)
            {
                return;
            }
            Grid.SetRow(clicked, emptyRow);
            Grid.SetColumn(clicked, emptyCol);
            emptyRow = y;
            emptyCol = x;
        }
        private void Shufle()
        {
            Random r = new Random();

            for (int i = 0; i < 200; i++)
            {
                var neighbors = GetMovableTiles();
                if (neighbors.Count == 0)
                {
                    continue;
                }
                Button tile = neighbors[r.Next(neighbors.Count)];
                Tile_Click(tile, null);
            }
        }
        private List<Button> GetMovableTiles()
        {
            List<Button> list = new List<Button>();
            foreach (Button b in PuzzleGrid.Children)
            {
                int y = Grid.GetRow(b);
                int x = Grid.GetColumn(b);
                bool isAdjesent =
                    (y == emptyRow && Math.Abs(x - emptyCol) == 1) ||
                    (x == emptyCol && Math.Abs(y - emptyRow) == 1);
                if (isAdjesent)
                {
                    list.Add(b);
                }
            }
            return list;
            
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            Shufle();
        }
    }
}