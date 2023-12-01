namespace MinesweeperModel
{
    public class Cell
    {
        
        public bool IsBomb { get; set; }
        public int BombCount { get; set; }
        public bool IsFlagged { get; set; }

        public bool isOpened { get; set; }

        public override string ToString()//will make better one as i go
        {
            string str = "";
            if (IsBomb && !IsFlagged && isOpened)
            {
                str = "💣";
            }
            else if (IsFlagged)
            {
                str = "🚩";
            }
            else if (isOpened)
            {
                //return  "X";
                if (BombCount == 0)
                {
                    str = "";
                }
                else if(IsBomb)
                {
                    str = "💣";
                }
                else
                {
                    str =  BombCount + "";
                }
            }

            return str;
        }
    }
}