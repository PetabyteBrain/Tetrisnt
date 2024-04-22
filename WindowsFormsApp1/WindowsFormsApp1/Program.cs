﻿using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create an instance of your main form and run it
            Form1 mainForm = new Form1();
            Application.Run(mainForm);
        }
    }
    public partial class Form1 : Form
    {
        private const int BoardWidth = 10;
        private const int BoardHeight = 22;
        private const int CellSize = 20;
        private int[,] tempGameBoard = new int[BoardWidth, BoardHeight];
        private int[,] permanentGameBoard = new int[BoardWidth, BoardHeight];
        int Score = 0;
        int Highscore = 0;
        int Level = 0;
        int completedlines = 0;
        Label scoreboard = new Label();
        Label highscore = new Label();
        Label linecount = new Label();

        // BLOCKS
        // TBLOCK ORIENTATIONS
        private int[,] Tblock = new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Tblock90 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 1, 0 }
        };
        private int[,] Tblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
        };
        private int[,] Tblock270 = new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        };
        // LBLOCK ORIENTATIONS
        private int[,] Lblock = new int[,]
        {
            { 0, 0, 1 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Lblock90 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 1 }
        };
        private int[,] Lblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 1, 0, 0 }
        };
        private int[,] Lblock270 = new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };
        // JBLOCK ORIENTATIONS
        private int[,] Jblock = new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Jblock90 = new int[,]
        {
            { 0, 1, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };
        private int[,] Jblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 1 }
        };
        private int[,] Jblock270 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        };
        // OBLOCK ORIENTATIONS
        private int[,] Oblock = new int[,]
        {
            { 1, 1 },
            { 1, 1 }
        };
        // IBLOCK ORIENTATIONS
        private int[,] Iblock = new int[,]
        {
            { 0, 0, 0, 0 },
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        };
        private int[,] Iblock90 = new int[,]
        {
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 }
        };
        private int[,] Iblock180 = new int[,]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 }
        };
        private int[,] Iblock270 = new int[,]
        {
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 }
        };

        public Form1()
        {
            //start
            start();

        }
        private void start()
        {
            gameplay();
        }
        private void gameplay()
        {
            // Initiate & Update
            InitializeComponent();
            InitializeGameBoard();
            InitializeScore();
            Initializehighscore();
            Updatehighscore();
            InitializeLinecount();
            UpdateLinecount();
            UpdateScore();

            // beginning of game
            blockpicker();
            this.KeyDown += Form1_KeyDown;
            UpdateGameBoard();
            drawblock(startx, starty);
            UpdateGameBoard();

        }

        private void InitializeGameBoard()
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Width = CellSize;
                    pictureBox.Height = CellSize;
                    pictureBox.Left =80 + x * CellSize;
                    pictureBox.Top = y * CellSize;
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    System.Drawing.Color color = GetCombinedColor(x, y);
                    pictureBox.BackColor = color;
                    this.Controls.Add(pictureBox);
                }
            }
        }
        private void InitializeScore()
        {

            scoreboard.Location = new Point(270, 120);
            scoreboard.AutoSize = true;
            scoreboard.Font = new Font("Calibri", 18);
            scoreboard.ForeColor = Color.Black;
            scoreboard.Padding = new Padding(6);
            this.Controls.Add(scoreboard);
            scoreboard.Refresh();

        }
        private void UpdateScore()
        {
            string finalscore = Score.ToString();
            scoreboard.Text = String.Format($"Score {finalscore}");
            if(Score > Highscore)
            {
                Updatehighscore();
            }
        }
        private void Initializehighscore()
        {
            highscore.Location = new Point(270, 50);
            highscore.AutoSize = true;
            highscore.Font = new Font("Calibri", 18);
            highscore.ForeColor = Color.Black;
            highscore.Padding = new Padding(6);
            this.Controls.Add(highscore);
            highscore.Refresh();
        }
        private void Updatehighscore()
        {
            Highscore = Score;
            string finalhighscore = Highscore.ToString();
            highscore.Text = String.Format($"Highscore: {finalhighscore}");
        }
        private void InitializeLinecount()
        {
            //completedlines
            linecount.Location = new Point(270, 0);
            linecount.AutoSize = true;
            linecount.Font = new Font("Calibri", 18);
            linecount.ForeColor = Color.Black;
            linecount.Padding = new Padding(6);
            this.Controls.Add(linecount);
            linecount.Refresh();
        }
        private void UpdateLinecount()
        {
            string Linecounter = completedlines.ToString();
            linecount.Text = String.Format($"Lines: {Linecounter}");
        }


        private System.Drawing.Color GetCombinedColor(int x, int y)
        {
            if (tempGameBoard[x, y] == 1 || permanentGameBoard[x, y] == 1)
                return System.Drawing.Color.Black;
            else
                return System.Drawing.Color.White;
        }


        private void UpdateGameBoard()
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    PictureBox pictureBox = (PictureBox)this.Controls[y * BoardWidth + x];
                    System.Drawing.Color color = GetCombinedColor(x, y);
                    pictureBox.BackColor = color;
                }
            }
        }





        // Example usage
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        int startx = 4;
        int starty = 0;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int blockSizeX = 0;
            int blockSizeY = 0;

            if (chosenblock == Tblock)
            {
                blockSizeX = Tblock.GetLength(1);
                blockSizeY = Tblock.GetLength(0);
            }
            else if (chosenblock == Lblock)
            {
                blockSizeX = Lblock.GetLength(1);
                blockSizeY = Lblock.GetLength(0);
            }
            else if (chosenblock == Jblock)
            {
                blockSizeX = Jblock.GetLength(1);
                blockSizeY = Jblock.GetLength(0);
            }
            else if (chosenblock == Iblock)
            {
                blockSizeX = Iblock.GetLength(1);
                blockSizeY = Iblock.GetLength(0);
            }
            else if (chosenblock == Oblock)
            {
                blockSizeX = Oblock.GetLength(1);
                blockSizeY = Oblock.GetLength(0);
            }

            // Calculate the maximum x and y values for the current block
            int maxX = BoardWidth - blockSizeX;
            int maxY = BoardHeight - blockSizeY;

            // Move the block according to the key pressed
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (startx < maxX)
                    {
                        clearblock();
                        startx += 1;
                        drawblock(startx, starty);
                        placeblockcheck();
                        UpdateGameBoard();
                    }
                    break;
                case Keys.Left:
                    if (startx > 0)
                    {
                        clearblock();
                        startx -= 1;
                        drawblock(startx, starty);
                        placeblockcheck();
                        UpdateGameBoard();
                    }
                    break;
                case Keys.Down:
                    if (starty < BoardHeight)
                    {
                        placeblockcheck();
                        clearblock();
                        starty += 1;
                        drawblock(startx, starty);
                        placeblockcheck();
                        UpdateGameBoard();
                    }
                    break;
                case Keys.Y:
                    if (starty < maxY)
                    {
                        clearblock();
                        rotation += 1;
                        Rotate();
                        clearblock();
                        drawblock(startx, starty);
                        UpdateGameBoard();
                        clearblock();
                    }
                    break;
                case Keys.X:
                    if (starty < maxY)
                    {
                        clearblock();
                        rotation -= 1;
                        Rotate();
                        clearblock();
                        drawblock(startx, starty);
                        UpdateGameBoard();
                        clearblock();
                    }
                    break;
            }

        }
        int rotation = 5;
        private void Rotate()
        {
            if(rotation > 5)
            {
                rotation = 2;
            }
            else if(rotation < 2)
            {
                rotation = 5;
            }
            switch (rotation)
            {
                case 2:
                    // 90
                    Console.WriteLine("90");
                    break;
                case 3:
                    // 180
                    Console.WriteLine("180");
                    break;
                case 4:
                    // 270
                    Console.WriteLine("270");
                    break;
                default:
                    // 0
                    Console.WriteLine("0");
                    break;
            }
        }

        object chosenblock = null;
        private void blockpicker()
        {
            Random rnd = new Random();
            int blocktype = rnd.Next(1, 6);

            switch (blocktype)
            {
                case 1:
                    chosenblock = Tblock;
                    break;
                case 2:
                    chosenblock = Lblock;
                    break;
                case 3:
                    chosenblock = Jblock;
                    break;
                case 4:
                    chosenblock = Iblock;
                    break;
                case 5:
                    chosenblock = Oblock;
                    break;

            }
        }
        private void placeblockcheck()
        {
            if (starty >= BoardHeight - 1)
            {
                placeblock();
                UpdateScore();
                return;

            }

            bool isBlocked = false;

            // Check if any part of the block would hit a permanent block or the bottom of the game board
            if (chosenblock == Oblock)
            {
                switch (rotation)
                {
                    default:
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 2; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        //0
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 2; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 2; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        //0
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 2; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int y = starty + 1; y <= starty + 3; y++)
                        {
                            for (int x = startx; x <= startx ; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int y = starty + 1; y <= starty ; y++)
                        {
                            for (int x = startx; x <= startx + 3; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int y = starty + 1; y <= starty + 3; y++)
                        {
                            for (int x = startx; x <= startx ; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        //0
                        for (int y = starty + 1; y <= starty ; y++)
                        {
                            for (int x = startx; x <= startx + 3; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            else if(chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 2; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        //0
                        for (int y = starty + 1; y <= starty + 1; y++)
                        {
                            for (int x = startx; x <= startx + 2; x++)
                            {
                                // Check if the current cell is outside the bounds of the game board
                                //if (x < 0 || x >= BoardWidth || y >= BoardHeight)
                                //{
                                //    
                                //    break;
                                //}

                                // Check if the current cell would hit a permanent block
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            if (isBlocked)
            {
                placeblock();
                
            }
            UpdateScore();


        }


        private void drawblock(int startX, int startY)
        {
            if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Tblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock90.GetLength(1); x++)
                            {
                                if (Tblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Tblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock180.GetLength(1); x++)
                            {
                                if (Tblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Tblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock270.GetLength(1); x++)
                            {
                                if (Tblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 5
                        for (int y = 0; y < Tblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock.GetLength(1); x++)
                            {
                                if (Tblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Lblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock90.GetLength(1); x++)
                            {
                                if (Lblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Lblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock180.GetLength(1); x++)
                            {
                                if (Lblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Lblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock270.GetLength(1); x++)
                            {
                                if (Lblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Lblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock.GetLength(1); x++)
                            {
                                if (Lblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Jblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock90.GetLength(1); x++)
                            {
                                if (Jblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Jblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock180.GetLength(1); x++)
                            {
                                if (Jblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Jblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock270.GetLength(1); x++)
                            {
                                if (Jblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Jblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock.GetLength(1); x++)
                            {
                                if (Jblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Iblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock90.GetLength(1); x++)
                            {
                                if (Iblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Iblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock180.GetLength(1); x++)
                            {
                                if (Iblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Iblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock270.GetLength(1); x++)
                            {
                                if (Iblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Iblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock.GetLength(1); x++)
                            {
                                if (Iblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Oblock)
            {
                for (int y = 0; y < Oblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Oblock.GetLength(1); x++)
                    {
                        if (Oblock[y, x] == 1)
                        {
                            int fieldX = startX + x;
                            int fieldY = startY + y;
                            if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                            {
                                tempGameBoard[fieldX, fieldY] = 1;
                            }
                        }
                    }
                }
            }
        }

        private void clearblock()
        {
            if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Tblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock90.GetLength(1); x++)
                            {
                                if (Tblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Tblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock180.GetLength(1); x++)
                            {
                                if (Tblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Tblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock270.GetLength(1); x++)
                            {
                                if (Tblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Tblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock.GetLength(1); x++)
                            {
                                if (Tblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Lblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock90.GetLength(1); x++)
                            {
                                if (Lblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Lblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock180.GetLength(1); x++)
                            {
                                if (Lblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Lblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock270.GetLength(1); x++)
                            {
                                if (Lblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Lblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock.GetLength(1); x++)
                            {
                                if (Lblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Jblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock90.GetLength(1); x++)
                            {
                                if (Jblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Jblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock180.GetLength(1); x++)
                            {
                                if (Jblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Jblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock270.GetLength(1); x++)
                            {
                                if (Jblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Jblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock.GetLength(1); x++)
                            {
                                if (Jblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Iblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock90.GetLength(1); x++)
                            {
                                if (Iblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Iblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock180.GetLength(1); x++)
                            {
                                if (Iblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Iblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock270.GetLength(1); x++)
                            {
                                if (Iblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Iblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock.GetLength(1); x++)
                            {
                                if (Iblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Oblock)
            {
                for (int y = 0; y < Oblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Oblock.GetLength(1); x++)
                    {
                        if (Oblock[y, x] == 1)
                        {
                            int fieldX = startx + x;
                            int fieldY = starty + y;
                            if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                            {
                                tempGameBoard[fieldX, fieldY] = 0;
                            }
                        }
                    }
                }
            }
        }

        private void placeblock()
        {
            if (starty >= 2 )
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    for (int x = 0; x < BoardWidth; x++)
                    {
                        if (tempGameBoard[x, y] == 1)
                        {
                            permanentGameBoard[x, y] = 1;
                        }
                    }
                }
                Score += 40;

                Array.Clear(tempGameBoard, 0, tempGameBoard.Length);

                CheckCompletedRows();


                rotation = 5;
                blockpicker();
                startx = 4;
                starty = 0;
                drawblock(startx, starty);
                UpdateGameBoard();
            }
        }

        private void CheckCompletedRows()
        {
            for (int y = BoardHeight - 1; y >= 0; y--)
            {
                bool rowCompleted = true;
                for (int x = 0; x < BoardWidth; x++)
                {
                    if (permanentGameBoard[x, y] == 0)
                    {
                        rowCompleted = false;
                        break;
                    }
                }
                if (rowCompleted)
                {
                    RemoveCompletedRow(y);
                    y++;
                }
            }
        }
        private void RemoveCompletedRow(int rowIndex)
        {
            for (int y = rowIndex; y > 0; y--)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    permanentGameBoard[x, y] = permanentGameBoard[x, y - 1];
                }
            }
            Score += 40 * (Level + 1);
            completedlines += 1;
            UpdateLinecount();
        }
    }
}