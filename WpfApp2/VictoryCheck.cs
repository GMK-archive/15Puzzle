using System.Windows.Controls;

namespace WpfApp2
{
    public class VictoryCheck
    {
        public bool IsSolved(Button?[,] tiles)
        {
            int expected = 1;

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (y == 3 && x == 3)
                        return tiles[y, x] == null;

                    if (tiles[y, x] == null)
                        return false;

                    int value = int.Parse(tiles[y, x].Content.ToString());
                    if (value != expected)
                        return false;

                    expected++;
                }
            }

            return true;
        }
    }
}
