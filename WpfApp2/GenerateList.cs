using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class GenerateList
    {
        private Random rand = new Random();

        public int[] GenerateSolvableGame()
        {
            int[] board;

            do
            {
                board = Enumerable.Range(0, 16).ToArray();
                Shuffle(board);
            }
            while (!IsSolvable(board));

            return board;
        }

        private void Shuffle(int[] arr)
        {
            for(int i = arr.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        private bool IsSolvable(int[] board)
        {
            int inversions = 0;

            for(int i = 0; i < 16; i++)
            {
                if (board[i] == 0) continue;

                for (int j = i + 1; j < 16; j++)
                {
                    if (board[j] == 0) continue;
                    if (board[i] > board[j]) inversions++;
                }
            }
            int blankIndex = Array.IndexOf(board, 0);
            int blankRow = blankIndex / 4;
            int blankRowFromBottom = 4 - blankRow;

            return (inversions + blankRowFromBottom) % 2 == 0;
        }
    }
}
