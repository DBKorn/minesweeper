using System.Drawing;

namespace MinesweeperModel
{
    public enum DifficultyLevel
    {
        Easy, // 10 x 8
        Medium,// 
        Hard
    }

    public static class Extensions
    {
        public static Point GetSize(this DifficultyLevel dl )
        {
            switch (dl)
            {
                case DifficultyLevel.Easy:
                default:
                    return new Point { X = 10, Y = 8 };
                case DifficultyLevel.Medium:
                    return new Point { X = 18, Y = 14 };
                case DifficultyLevel.Hard:
                    return new Point { X = 24, Y = 20 };
            }
        }



        public static int GetNumBombs(this DifficultyLevel dl)
        {
            switch (dl)
            {
                case DifficultyLevel.Easy:
                default:
                    return 10;
                case DifficultyLevel.Medium:
                    return 40;
                case DifficultyLevel.Hard:
                    return 99;
            }
        }
    }
}