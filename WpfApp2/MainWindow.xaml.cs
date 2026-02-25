using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public Button[,] tiles = new Button[4, 4];
        public int emptyRow = 3;
        public int emptyCol = 3;

        private bool isShuffling = false;
        private VictoryCheck checker = new VictoryCheck();
        private GenerateList generator = new GenerateList();

        public MainWindow()
        {
            InitializeComponent();
            CreatePuzzle();
        }

        private void CreatePuzzle()
        {
            PuzzleGrid.Children.Clear();

            int[] board = generator.GenerateSolvableGame();
            int index = 0;
            if (board.Length != 16) throw new Exception("Generator returned invalid board length!");

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    int value = board[index++];

                    if (value == 0)
                    {
                        tiles[y, x] = null;
                        emptyRow = y;
                        emptyCol = x;
                        continue;
                    }

                    Button b = new Button();
                    b.Content = value.ToString();
                    b.Background = Brushes.AntiqueWhite;
                    b.FontFamily = new FontFamily("Bahnschrift SemiLight Condensed");
                    b.FontSize = 24;
                    b.Click += Tile_Click;

                    Grid.SetRow(b, y);
                    Grid.SetColumn(b, x);
                    PuzzleGrid.Children.Add(b);

                    tiles[y, x] = b;
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
                return;

            Grid.SetRow(clicked, emptyRow);
            Grid.SetColumn(clicked, emptyCol);

            tiles[emptyRow, emptyCol] = clicked;
            tiles[y, x] = null;

            emptyRow = y;
            emptyCol = x;

            if (!isShuffling && checker.IsSolved(tiles))
            {
                MessageBox.Show("Wygrałeś!");
            }
        }

        private void MoveTileInternal(Button tile)
        {
            int y = Grid.GetRow(tile);
            int x = Grid.GetColumn(tile);

            Grid.SetRow(tile, emptyRow);
            Grid.SetColumn(tile, emptyCol);

            tiles[emptyRow, emptyCol] = tile;
            tiles[y, x] = null;

            emptyRow = y;
            emptyCol = x;
        }

        private List<Button> GetMovableTiles()
        {
            List<Button> list = new List<Button>();

            foreach (Button b in PuzzleGrid.Children)
            {
                int y = Grid.GetRow(b);
                int x = Grid.GetColumn(b);

                bool isAdjacent =
                    (y == emptyRow && Math.Abs(x - emptyCol) == 1) ||
                    (x == emptyCol && Math.Abs(y - emptyRow) == 1);

                if (isAdjacent)
                    list.Add(b);
            }

            return list;
        }

        private List<int> GetBoardState()
        {
            List<int> state = new List<int>();

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (tiles[y, x] == null)
                    {
                        state.Add(0);
                    }
                    else
                    {
                        state.Add(int.Parse(tiles[y, x].Content.ToString()));
                    }
                }
            }

            return state;
        }

        private int CountInversions(List<int> state)
        {
            int inv = 0;

            for (int i = 0; i < state.Count; i++)
            {
                for (int j = i + 1; j < state.Count; j++)
                {
                    if (state[i] != 0 && state[j] != 0 && state[i] > state[j])
                        inv++;
                }
            }

            return inv;
        }

        private bool IsSolvable()
        {
            var state = GetBoardState();
            int inversions = CountInversions(state);
            int emptyRowFromBottom = 4 - emptyRow;

            return (inversions + emptyRowFromBottom) % 2 == 0;
        }

        private async Task ShuffleOnceAsync()
        {
            isShuffling = true;
            Random r = new Random();

            for (int i = 0; i < 200; i++)
            {
                var neighbors = GetMovableTiles();
                if (neighbors.Count == 0)
                    continue;

                Button tile = neighbors[r.Next(neighbors.Count)];
                MoveTileInternal(tile);

                await Task.Delay(1);
            }

            isShuffling = false;
        }

        private async Task ShuffleAsync()
        {
            await ShuffleOnceAsync();
            bool CanBeSolved = IsSolvable();
            if (!CanBeSolved)
            {
                await ShuffleOnceAsync();
            }
            else
            {
                return;
            }
        }

        private async void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            await ShuffleAsync();
        }
    }
}
