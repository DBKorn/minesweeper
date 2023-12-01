using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MinesweeperModel.Test
{
    [TestClass]
    public class UnitTest1
    {
        //private Model easyModel = new Model(DifficultyLevel.Easy.GetSize().X, DifficultyLevel.Easy.GetSize().Y);
        private Model easyModel = new Model(DifficultyLevel.Easy);
        [TestMethod]
        public void TestSetCellBombCount()
        {
            easyModel.board[0,0] = new Cell() {IsBomb = true};
            easyModel.board[0,1] = new Cell() {IsBomb = true};
            easyModel.board[0,2] = new Cell() {IsBomb = true};
            easyModel.board[1,2] = new Cell() {IsBomb = true};
            Console.WriteLine(easyModel);
            int bombsInB1 = easyModel.SetCellBombCount(1, 1);
            int bombsInB0 = easyModel.SetCellBombCount(0, 1);

            Assert.IsTrue(2 == bombsInB0);
            Assert.IsTrue(4 == bombsInB1);
            
        }

        [TestMethod]
        public void SeeModelToString()
        {
            easyModel.board[0, 0] = new Cell() { IsBomb = true };
            easyModel.board[0, 1] = new Cell() { IsBomb = true };
            easyModel.board[0, 2] = new Cell() { IsBomb = true };
            easyModel.board[1, 2] = new Cell() { IsBomb = true };

            Console.WriteLine(easyModel);
        }

        [TestMethod]
        public void EasyLevelIDimensions()
        {
            Model m = new Model(DifficultyLevel.Easy);
            int rows = DifficultyLevel.Easy.GetSize().Y;
            int cols = DifficultyLevel.Easy.GetSize().X;
            Assert.AreEqual(8, rows);
            Assert.AreEqual(10, cols);
        }
        


        [TestMethod]
        public void TestFlagOnCantOpen()//todo
        {
            easyModel.board[0, 0] = new Cell() { IsFlagged = true };

            List<Point> l = easyModel.OpenCell(0, 0);
            Assert.IsTrue(l.Count == 0);
        }

        [TestMethod]
        public void TestFlagOffToOn()//todo
        {
            int col = 1, row = 1;
            easyModel.board[col, row] = new Cell() { IsFlagged = false};
            int numFlags = easyModel.FlagCell(col, row);

            List<Point> opened = easyModel.OpenCell(col, row);


            Assert.AreEqual(0, opened.Count);
            Assert.AreEqual(DifficultyLevel.Easy.GetNumBombs() -1, numFlags);
            //Assert.AreEqual( 1, easyModel.flagCount);
        }

        [TestMethod]
        public void TestFlagOnToOff()//todo
        {
            int col = 1, row = 1;
            
            int InitialFlags = easyModel.flagCount = DifficultyLevel.Easy.GetNumBombs() - 1;

            SetUpBoardWithBombsAlongFirstRowAndCol();
            easyModel.board[col, row].IsFlagged = true;

            int numFlags = easyModel.FlagCell(col, row);

            List<Point> opened = easyModel.OpenCell(col, row);
            Assert.IsTrue(opened.Count >= 1);
            Assert.AreEqual(DifficultyLevel.Easy.GetNumBombs(), InitialFlags+1);
        }

        [TestMethod]
        public void TestNumBombsInEasyGame()
        {
            
            //easyModel.firstMove = true;

            easyModel.OpenCell(7, 7);

            int bombs = 0;
            for (int i = 0; i < easyModel.RowTotal; i++)
            {
                for (int j = 0; j < easyModel.ColTotal; j++)
                {
                    if (easyModel.GetCell(j,i).IsBomb)
                    {
                        bombs++;
                    }
                }
            }

            Assert.AreEqual(bombs, DifficultyLevel.Easy.GetNumBombs());
        }

        [TestMethod]
        public void TestBlankCellProtocol()
        {
            SetUpBoardWithBombsAlongFirstRowAndCol();
            easyModel.SetBombCountAllCells();
            easyModel.firstMove = false;
            easyModel.OpenCell(7, 7);
            //Console.WriteLine(easyModel);

            for (int i = 1; i < easyModel.RowTotal; i++)
            {
                for (int j = 1; j < easyModel.ColTotal; j++)
                {
                    //easyModel.board[i, j].isOpened;
                    Assert.IsTrue(easyModel.board[i, j].isOpened);
                }
            }

            Console.WriteLine(easyModel);

        }

        [TestMethod]
        public void TestGameWin()
        {
            SetUpBoardWithBombsAlongFirstRowAndColOnlyTillEasy();
            easyModel.SetBombCountAllCells();
            easyModel.firstMove = false;
            easyModel.OpenCell(7, 7);
            //Console.WriteLine(easyModel);

            Console.WriteLine(easyModel.moveCount + " " + ((easyModel.RowTotal * easyModel.ColTotal) - DifficultyLevel.Easy.GetNumBombs() ) );
            Assert.IsTrue(easyModel.Win);

            Console.WriteLine(easyModel);

        }
        [TestMethod]
        public void TestGameLose()
        {
            SetUpBoardWithBombsAlongFirstRowAndColOnlyTillEasy();
            easyModel.SetBombCountAllCells();
            easyModel.firstMove = false;
            easyModel.OpenCell(0, 0);
            Console.WriteLine(easyModel);
            Assert.IsTrue(easyModel.Lose);

            

        }
        private void SetUpBoardWithBombsAlongFirstRowAndCol()
        {
            easyModel.firstMove = false;
            for (int i = 0; i < easyModel.RowTotal; i++)
            {
                for (int j = 0; j < easyModel.ColTotal; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        easyModel.board[i, j] = new Cell() {isOpened = false, IsBomb = true, IsFlagged = false};
                    }
                    else
                    {
                        easyModel.board[i, j] = new Cell() {isOpened = false, IsBomb = false, IsFlagged = false};
                    }
                }
            }
        }

        private void SetUpBoardWithBombsAlongFirstRowAndColOnlyTillEasy()
        {
            easyModel.firstMove = false;
            int numBombs = DifficultyLevel.Easy.GetNumBombs();
            int count = 0;
            for (int i = 0; i < easyModel.RowTotal; i++)
            {
                for (int j = 0; j < easyModel.ColTotal; j++)
                {
                    if (i == 0 || j == 0 && count < numBombs)
                    {
                        easyModel.board[i, j] = new Cell() { isOpened = false, IsBomb = true, IsFlagged = false };
                        count++;
                    }
                    else
                    {
                        easyModel.board[i, j] = new Cell() { isOpened = false, IsBomb = false, IsFlagged = false };
                    }
                }
            }
        }
    }
}

