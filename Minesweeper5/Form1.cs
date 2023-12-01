using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinesweeperModel;

// bomb: 
// Flag:
///*
namespace Minesweeper5
{
    public partial class Form1 : Form
    {
        private int RowCount, ColCount;
        Button[,] buttons;
        MinesweeperModel.Model model = new Model();
        private int flags;
        private DifficultyLevel dl;
        
        public Form1()
        {
            InitializeComponent();

            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            comboBox1.SelectedIndex = 0;
            //this.button1.Click +=

            //model = new MinesweeperModel.Model(ColCount, RowCount);

            // comboBox1.SelectedIndex = 0;
            // buttons = new Button[RowCount, ColCount];
            // bool color = false;
            // for (int r = 0; r < RowCount; r++)
            //     for (int c = 0; c < ColCount; c++)
            //     {
            //         buttons[r, c] = new System.Windows.Forms.Button();
            //         buttons[r, c].Dock = DockStyle.Fill;
            //         // TabIndex in order 0..RowCount*ColCount-1
            //         //buttons[r, c].TabIndex = RowCount * ColCount - 1; //todo figure what this tabIndex thing is
            //         this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
            //         buttons[r, c].Text = $"[{r}][{c}]";
            //         buttons[r, c].UseVisualStyleBackColor = true;
            //         buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
            //         color = !color;
            //         buttons[r, c].Tag = new Point(c,r );
            //         buttons[r, c].Click += LeftClickProtocol;
            //         
            //     }

            // 
            // tableLayoutPanel1
            // 
            // this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // this.tableLayoutPanel1.ColumnCount = ColCount;
            // for (int c = 0; c < ColCount; c++)
            // {
            //     this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount)); 
            // }
            //
            //
            // this.tableLayoutPanel1.RowCount = RowCount;
            // for (int r = 0; r < RowCount; r++)
            //     this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));
        }

        public void GridSetup(DifficultyLevel lev, int hardness = 0)
        {
            flags = lev.GetNumBombs();
            UpdateBombsOnFlag();
            label3.Visible = false;
            button1.Visible = false;

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            int RowCount = lev.GetSize().Y, ColCount = lev.GetSize().X;
            comboBox1.SelectedIndex = hardness;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();

            buttons = new Button[RowCount, ColCount];
            bool color = false;
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColCount; c++)
                {
                    buttons[r, c] = new System.Windows.Forms.Button();
                    buttons[r, c].Dock = DockStyle.Fill;
                    // TabIndex in order 0..RowCount*ColCount-1
                    //buttons[r, c].TabIndex = RowCount * ColCount - 1; //todo figure what this tabIndex thing is
                    this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
                    buttons[r, c].Text = "";
                    buttons[r, c].UseVisualStyleBackColor = true;
                    buttons[r, c].BackColor = color ? Color.LightGreen : Color.GreenYellow;
                    color = !color;
                    buttons[r, c].Tag = new Point(c,r );
                    //buttons[r, c].Click += LeftClickProtocol;
                    buttons[r, c].MouseDown += ClickProtocol;
                    //buttons[r, c].MouseClick




                }

            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount));
            }


            this.tableLayoutPanel1.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++)
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));


            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(true);
            this.ResumeLayout(true);
        }

        /*private void GridSetup2(DifficultyLevel dl)
        {

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            RowCount = dl.GetSize().Y;
            ColCount = dl.GetSize().X;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();


            // Resize Window not allowed
            buttons = new Button[RowCount, ColCount];
            bool color = false;
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColCount; c++)
                {
                    buttons[r, c] = new System.Windows.Forms.Button();
                    buttons[r, c].Dock = DockStyle.Fill;
                    // TabIndex in order 0..RowCount*ColCount-1
                    //buttons[r, c].TabIndex = RowCount * ColCount - 1; //todo figure what this tabIndex thing is
                    this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
                    buttons[r, c].Text = "";
                    buttons[r, c].UseVisualStyleBackColor = true;
                    buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
                    color = !color;
                    buttons[r, c].Tag = new Point(c, r);
                    buttons[r, c].Click += LeftClickProtocol;

                }

            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount));
            }


            this.tableLayoutPanel1.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++)
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(true);
            this.ResumeLayout(true);

            //Refresh();
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void difficultyLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            dl = (DifficultyLevel)((ComboBox)sender).SelectedIndex;
            BeginGameStuff(dl);
        }

        private void BeginGameStuff(DifficultyLevel dl)
        {
            model.SetDifficultyLevelAndNewGame(dl);
            UpdateBombsOnFlag();
            this.GridSetup(dl, (int)dl);
        }


        private void UpdateBombsOnFlag()
        {
            label2.Text = "" + flags;
        }

        private void ClickProtocol(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;//sender
            Point p = (Point)b.Tag;

            Console.WriteLine(e.Button);
            if (e.Button == MouseButtons.Right)
            {
                RightClickProtocol(b, p);
            }
            else if (e.Button == MouseButtons.Left)
            {
                LeftClickProtocol(b,p);
            }
        }

        private void RightClickProtocol(Button b, Point p)
        {
            
            flags = model.FlagCell(p.X, p.Y);

            buttons[p.Y, p.X].Text = model.GetCell(p.X, p.Y).ToString();
            UpdateBombsOnFlag();
            //update display of how many flags/bombs are left
            
        }

        private void LeftClickProtocol(Button b, Point p)
        {
            
            List<Point> l = model.OpenCell(p.X, p.Y); // how to determine row and column?
            if (l != null)
            {
                foreach (var w in l)
                {
                    buttons[w.Y, w.X].Text = model.GetCell(w.X, w.Y).ToString();
                    buttons[w.Y, w.X].BackColor = Color.White;
                    //Console.WriteLine(w);
                }
                //UpdateGui(l, Color.White);
            }

            flags = model.FlagCount;
            UpdateBombsOnFlag();
            IsGameOver();
        }

        private void IsGameOver()
        {
            button1.Visible = true;
            if (model.Win)
            {
                WinProtocol();
            }

            if (model.Lose)
            {
                LoseProtocol();
            }
        }
        private void WinProtocol()
        {
            label3.Visible = true;
            label3.Text = "You WIN!!!";
        }

        private void LoseProtocol()
        {
            label3.Visible = true;
            label3.Text = "You lose!";
        }

        private void RestartGame(object sender, EventArgs e)
        {
            //DifficultyLevel dl = (DifficultyLevel)((ComboBox)sender).SelectedIndex;
            BeginGameStuff(dl);
        }
    }
}
//*/
//
/*
namespace Minesweeper5
{
    public partial class Form1 : Form
    {
        int RowCount = 5, ColCount = 8;
        Button[,] buttons;
        MinesweeperModel.Model model = new MinesweeperModel.Model();
        public Form1()
        {
            InitializeComponent();

            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            comboBox1.SelectedIndex = 0;

            // Resize Window not allowed
            //buttons = new Button[RowCount, ColCount];
            //bool color = false;
            //for (int r = 0; r < RowCount; r++)
            //    for (int c = 0; c < ColCount; c++)
            //    {
            //        buttons[r, c] = new System.Windows.Forms.Button();
            //        buttons[r, c].Dock = DockStyle.Fill;
            //        // TabIndex in order 0..RowCount*ColCount-1
            //        this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
            //        buttons[r, c].Text = $"[{r}][{c}]";
            //        buttons[r, c].UseVisualStyleBackColor = true;

            //        buttons[r, c].Click += OnButtonClick;
            //        buttons[r, c].Tag = new Point(c,r);
            //      //  buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
            //        color = ! color;
            //    }

            //// 
            //// tableLayoutPanel1
            //// 
            //this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.tableLayoutPanel1.ColumnCount = ColCount;
            //for (int c = 0; c < ColCount; c++)
            //{
            //    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount)); 
            //}


            //this.tableLayoutPanel1.RowCount = RowCount;
            //for (int r = 0; r < RowCount; r++)
            //    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));



            // TODO change cursor to arrow


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void OnButtonClick(object sender, EventArgs e)
        {

            Button sourceButton = (Button)sender;
            Point p = (Point)sourceButton.Tag;

            // alt not needed - loop through all buttons and check == with sender
            //var changedCells = model.OpenCell(p.X, p.Y); // how to dtermine row and column?
            model.OpenCell(p.X, p.Y);
            // UpdateUI based on move
            // OPtion1 update entire UI from model
            // Option 2 (RECOMMENDED) update cells that changed

            // if Bomb, how does model indicate that?
            buttons[p.Y, p.X].Text = ":-)";

            // Create a  New Thread
            // call in new Thread SlowComputation();
            // New thread calls Form Update to display completed

            // return before new thread completes

            // Starts an async method which calls SlowComputation
            // return from this event handler
            // async will continue running in this method

            //await SlowComputationAsync();
        }

        private void difficultyLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            DifficultyLevel dl = (DifficultyLevel)((ComboBox)sender).SelectedIndex;
            model.SetDifficultyLevelAndNewGame(dl);
            this.GridSetup(dl);
        }

        private void GridSetup(DifficultyLevel dl)
        {

            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            RowCount = dl.GetSize().Y;
            ColCount = dl.GetSize().X;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();


            // Resize Window not allowed
            buttons = new Button[RowCount, ColCount];
            bool color = false;
            for (int r = 0; r < RowCount; r++)
            for (int c = 0; c < ColCount; c++)
            {
                buttons[r, c] = new System.Windows.Forms.Button();
                buttons[r, c].Dock = DockStyle.Fill;
                // TabIndex in order 0..RowCount*ColCount-1
                this.tableLayoutPanel1.Controls.Add(buttons[r, c], c, r);
                buttons[r, c].Text = "";
                buttons[r, c].UseVisualStyleBackColor = true;

                buttons[r, c].Click += OnButtonClick;
                buttons[r, c].Tag = new Point(c, r);
                //  buttons[r, c].BackColor = color ? Color.Green : Color.GreenYellow;
                color = !color;
            }

            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount));
            }


            this.tableLayoutPanel1.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++){ 
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));
            }
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(true);
            this.ResumeLayout(true);

            //Refresh();
        }
    }
}
*/