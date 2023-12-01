using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace MinesweeperModel
{
    // Reference Game: https://www.google.com/search?q=minesweeper&rlz=1C1EJFC_enUS867US867&oq=mine&aqs=chrome.0.69i59j46i67i433j46i67j46i67i433j0i20i263i433i512j46i433i512l2j69i60.1199j0j7&sourceid=chrome&ie=UTF-8
    // There is much more to the IModel than what we completed in class
    // the first casualty of war is the plan
    public interface IModel
    {
        //ops
        /// <summary>
        /// Opens Cell
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>List of Points that changed due to this operation. If Bomb, that point will be only returned value</returns>
        List<System.Drawing.Point> OpenCell(int col, int row);
        void SetupOnFirstMove(DifficultyLevel level, int col, int row);
        //void SetupAfterFirstMove(DifficultyLevel level, int col, int row);
        int FlagCell(int col, int row);
        int GetTime(); // In Seconds

        int EndGameProtocol(int x);

        Cell GetCell(int col, int row);
    }

    public class Model : IModel
    {
        //imp
        internal Cell[,] board;
        internal bool firstMove = true;
        private int colTotal, rowTotal;
        private DifficultyLevel level = DifficultyLevel.Easy; //todo need to fix later
        private List<Point> changedCells;
        internal int flagCount;
        private Random r;
        internal int moveCount;
        internal bool win;
        internal bool lose;


        public int ColTotal => colTotal;
        public int RowTotal => rowTotal;

        public bool Win => win;
        public bool Lose => lose;


        public int FlagCount => flagCount;
        public Model()
        {
        }
        

        public Model(DifficultyLevel level)
        {
            this.level = level;
            colTotal = level.GetSize().X;
            rowTotal = level.GetSize().Y;
            board = new Cell[level.GetSize().Y, level.GetSize().X];
            r = new Random();
            
            InitializeBoard();
        }

        public void SetDifficultyLevelAndNewGame(DifficultyLevel l)
        {
            this.level = l;
            colTotal = l.GetSize().X;
            rowTotal = l.GetSize().Y;
            board = null; // destroy any potential earlier made board
            board = new Cell[l.GetSize().Y, l.GetSize().X];
            InitializeBoard();
        }

        internal void InitializeBoard()
        {
            this.firstMove = true;
            this.moveCount = 0;
            changedCells = new List<Point>();
            win = false;
            lose = false;
            this.flagCount = level.GetNumBombs();
            for (int i = 0; i < rowTotal; i++)
            for (int j = 0; j < colTotal; j++)
            {
                board[i, j] = new Cell(){isOpened = false, IsBomb = false, IsFlagged = false, BombCount = 0};
            }
        }


        /// <summary>
        /// puts a flag on cell.
        /// Cell becomes "unclickable"
        /// decrement flagCount on top of game
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="flagOn"></param>
        /// Probably don't need to return a list; might fix later if there's time
        public int FlagCell(int col, int row)
        {
            
            // data validation
            BoundsCheckerExThrower(col, row);
            Cell cell = GetCell(col, row);
            if (cell.isOpened)
            {
                return flagCount;
            }

            cell.IsFlagged = !cell.IsFlagged;

            if (cell.IsFlagged)
            {
                flagCount--;
            }
            else
            {
                flagCount++;
            }

            
            return flagCount;
        }

        public Cell GetCell(int col, int row)
        {
            BoundsCheckerExThrower(col, row);
            return board[row, col];
        }

        public int GetTime()
        {

            throw new NotImplementedException();
        }

        public List<Point> OpenCell(int vert, int horiz)
        {
            changedCells.Clear();
            BoundsCheckerExThrower(vert, horiz);
            Cell cell = GetCell(vert, horiz);

            if (cell.IsFlagged)//first thing user can do is flag a tile
            {
                return changedCells;
            }

            if (firstMove)
            {
                Console.WriteLine("in firstMove logic block");
                SetupOnFirstMove(level, vert, horiz);
                LegalMoveProtocol(vert, horiz);
            }
            else
            {
                
                if (cell.isOpened || cell.IsFlagged)//user clicks used cell or flagged cell
                {
                    //return null;
                } 
                else if (cell.IsBomb && !cell.isOpened && !cell.IsFlagged)//user clicks on bomb
                {
                    LoserProtocol();
                } 
                else if (!cell.IsBomb && !cell.isOpened && !cell.IsFlagged)//legal move
                {
                    LegalMoveProtocol(vert, horiz);
                }
            }
            moveCount += changedCells.Count;

            win = DidPlayerWin(vert, horiz);//todo
            lose = DidPlayerLose(vert, horiz);

            if (win)
            {
                WinnerProtocol();
            }
            else if (lose)
            {
                LoserProtocol();
            }

            return changedCells;
        }


        private void LegalMoveProtocol(int col, int row)
        {
            Cell cell = GetCell(col, row);
            cell.isOpened = true;
            changedCells.Add(new Point(col, row));
            if (cell.BombCount == 0)
            {
                BlankCellProtocol(col, row);
            }
        }

        private void BlankCellProtocol(int col, int row)
        {
            int[] a = { -1, 0, 1 };
            int x, y;

            foreach (int r in a)
            {
                foreach (int c in a)
                {
                    x = row + r;
                    y = col + c;
                    
                    if (!IsOutOfBounds(y, x))//nested logic to avoid errors
                    {
                        Cell cell = GetCell(y, x);//todo double check row and col
                        //LegalMoveProtocol(y,x);
                        bool notAvailable = (cell.IsBomb  || cell.isOpened);//todo might be able to recurse if flagged
                        if (notAvailable)
                        {
                            //just don't touch this cell at all; if I return, method doesn't work
                        }
                        else if (!notAvailable && cell.BombCount == 0)//todo double check row and col
                        {
                            UnflagForBlankCellProt(y, x);
                            cell.isOpened = true;
                            changedCells.Add(new Point(y,x));
                            BlankCellProtocol(y,x);
                        }
                        else if (!notAvailable && cell.BombCount > 0)
                        {
                            UnflagForBlankCellProt(y,x);
                            cell.isOpened = true;
                            changedCells.Add(new Point(y, x));
                            //return;
                        }
                    }
                }
            }
        }

        private void UnflagForBlankCellProt(int col, int row)//
        {
            Cell cell = GetCell(col, row);
            if (cell.IsFlagged)
            {
                FlagCell(col, row);
            }
        }

        public void SetupOnFirstMove(DifficultyLevel level, int col, int row)//todo make a list of points of the bombs so I dont check every single cell
        {
            Console.WriteLine("in setupOnfirstMove");
            BoundsCheckerExThrower(col, row);
            Cell cell = GetCell(col, row);

            //cell.BombCount = 0; //the next 2 lines might be useless
            cell.IsBomb = false; 
            cell.isOpened = true;    //= new Cell() {IsBomb = false, BombCount = 0, isOpened = true};  //todo make sure this is right
            PutBombsOnBoardExceptOnFirstMove(level, col, row);
            SetBombCountAllCells();
            this.firstMove = false;
        }

        internal void PutBombsOnBoardExceptOnFirstMove(DifficultyLevel level, int col, int row)
        {
            List<Point> bombLocations = new List<Point>();
            int bombs = level.GetNumBombs(), colB, rowB;
            Random r = new Random();

            for (int i = 0; i < bombs; i++)
            {
                //colB = r.Next(colTotal);
                //rowB = r.Next(rowTotal);
                do {
                    colB = r.Next(colTotal);
                    rowB = r.Next(rowTotal);
                } while (IsOutOfBounds(colB, rowB));

                Cell BombCell = GetCell(colB, rowB);

                //Console.WriteLine(colB + "----" + rowB);
                if (BombCell.IsBomb || (colB == col && rowB == row) )//checks if already a bomb or it's what user clocked on. Need to make no bomb is surrounded by bombs
                {
                    i--;
                    continue;
                }
                BombCell.IsBomb = true;
                BombCell.isOpened = false;
                BombCell.IsFlagged = false;
                bombLocations.Add(new Point(colB, rowB));
            }
            //SetBomBCountNextToBombs(bombLocations);
        }

        internal void SetBomBCountNextToBombs(List<Point> bombLocations)
        {
            foreach (var p in bombLocations)
            {
                SetCellBombCount(p.X, p.Y);
            }
        }

        private void NumBombTotalForDebug()
        {
            for (int i = 0; i < rowTotal; i++)
            for (int j = 0; j < ColTotal; j++)
            {
                if (board[i, j].IsBomb)
                {
                    Console.WriteLine("Look a bomb 111111111111111111111111111");
                }
            }
        }

        internal int SetCellBombCount(int col, int row)
        {
            //NumBombTotalForDebug();
            int count = 0;
            int[] a = {-1, 0, 1};
            int x,y;
            foreach (int r in a)
            {
                foreach (int c in a)
                {
                    x = row + r;
                    y = col + c;

                    //Console.WriteLine(x + " --- " + y +" before if");
                    if (!IsOutOfBounds(y, x))
                    {
                        Cell cell = GetCell(y, x);
                        //Console.WriteLine(x + " --- " + y + " after if");

                        if (cell.IsBomb && !(x == row && y == col))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private bool IsOutOfBounds(int col, int row)
        {
            return (col >= colTotal || row >= rowTotal || row < 0 || col < 0);
        }

        internal void SetBombCountAllCells()
        {
            for (int r = 0; r < rowTotal; r++)
            for (int c = 0; c < colTotal; c++)
            {
                int bCount = SetCellBombCount(c, r);
                board[r, c].BombCount = bCount;
            }

            
        }

        // public void SetupAfterFirstMove(DifficultyLevel level, int col, int row)
        // {
        //     BoundsCheckerExThrower(col, row);
        //     int bombs = level.GetNumBombs(), colB, rowB;
        //     Random r = new Random();
        //
        //     for (int i = 0; i < bombs; i++)
        //     {
        //         colB = r.Next(colTotal);
        //         rowB = r.Next(rowTotal);
        //         if (board[colB, rowB].IsBomb || board[colB, rowB].BombCount == 8)//checks if already a bomb or surrounded by bombs
        //         {
        //             i--;
        //             continue;
        //         }
        //         board[colB, rowB].IsBomb = true;
        //     }
        // }

        private void BoundsCheckerExThrower(int col, int row)
        {
            string errorMsg = "";
            bool ossur = true;

            //Console.WriteLine(board.Length + " --- " ); // board seems to be null
            if (col < 0 || row < 0)
            {
                errorMsg = "vert = " + col + " horiz = " + row + "; out of bounds as they can't be negative";
            }
            else if (col > colTotal || row > rowTotal) // todo need to figure this out
            {
                errorMsg = "vert = " + col + " horiz = " + row + "; out of bounds. BoardCol = " 
                           + board.GetLength(0) + "BoardRow = " + board.GetLength(1);
            }
            else
            {
                ossur = false;
            }

            if (ossur)
            {
                throw new IndexOutOfRangeException(errorMsg);
            }
            
        }

        internal bool DidPlayerWin(int col, int row)
        {
            Console.WriteLine("moveCount = " + moveCount);
            Console.WriteLine("Other thingy = " + ((rowTotal * colTotal) - this.level.GetNumBombs() ) );
            return moveCount == (rowTotal * colTotal) - this.level.GetNumBombs();
        }

        internal bool DidPlayerLose(int col, int row)
        {
            return GetCell(col, row).IsBomb;
        }

        internal void WinnerProtocol()
        {

        }

        internal void LoserProtocol()
        {
            UncoverAllCells();
        }

        public int EndGameProtocol(int winOrLose)
        {
            return winOrLose;
        }


        private void UncoverAllCells()
        {
            for (int r = 0; r < rowTotal; r++)
            {
                for (int c = 0; c < colTotal; c++)
                {
                    Cell cell = GetCell(c, r);
                    cell.isOpened = true;
                    cell.IsFlagged = false;
                    changedCells.Add(new Point(c,r));
                }
            }
        }

        public override string ToString()
        {
            SetBombCountAllCells();
            StringBuilder sb = new StringBuilder();
            string str;

            for (int r = 0; r < RowTotal; r++)
            for (int c = 0; c < ColTotal; c++)
            {
                sb.Append("| "+ board[r, c].ToString());
                if (ColTotal - c == 1)
                {
                    sb.AppendFormat("|\n");
                    for (int i = 0; i < colTotal; i++)
                    {
                        sb.Append("----");
                    }
                    sb.AppendFormat("\n");
                }
            }

            return sb.ToString();
        }
    }


}
