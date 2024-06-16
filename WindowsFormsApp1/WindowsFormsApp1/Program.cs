using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }
    }

    public partial class Form1 : Form
    {
        private Panel gameBoardPanel;
        private Panel nextBlockPanel;
        private const int BoardWidth = 14;
        private const int BoardHeight = 23;
        private const int CellSize = 20;
        private const int nextblocklimit = 4;
        private int[,] tempGameBoard = new int[BoardWidth, BoardHeight];
        private int[,] permanentGameBoard = new int[BoardWidth, BoardHeight];
        private int[,] nextblockscreen = new int[nextblocklimit, nextblocklimit];

        private int[,] permanentGameBoardOld = new int[BoardWidth, BoardHeight];
        private int[,] tempGameBoardOld = new int[BoardWidth, BoardHeight];
        private int[,] nextblockscreenOld = new int[nextblocklimit, nextblocklimit];

        bool AllowMoveLeft = true;
        bool AllowMoveRight = true;

        bool AllowRotationLeft = true;
        bool AllowRotationRight = true;


        int Score = 0;
        int Highscore = 0;

        int FinalScore = 0;
        int FinalHighScore = 0;

        int Level = 0;
        int onelevelup = 0;
        int completedlines = 0;

        object chosenblock = null;
        int nextblock = 0;
        int blocktype = 0;

        Label scoreboard = new Label();
        Label highscore = new Label();
        Label linecount = new Label();
        Label levelcount = new Label();
        bool gameover = false;

        Label finalhighscorep = new Label();
        Label finalscorep = new Label();

        Button startbutton = new Button();
        Button quitbutton = new Button();
        Button restartbutton = new Button();

        //Level Buttons
        Button level0button = new Button();
        Button level1button = new Button();
        Button level2button = new Button();
        Button level3button = new Button();
        Button level4button = new Button();
        Button level5button = new Button();
        Button level6button = new Button();
        Button level7button = new Button();
        Button level8button = new Button();
        Button level9button = new Button();

        PictureBox startscreenimage = new PictureBox();

        // BLOCKS
        // TBLOCK ORIENTATIONS
        private int[,] Tblock = new int[,]
        {
            { 0, 3, 0 },
            { 3, 3, 3 },
            { 0, 0, 0 }
        };
        private int[,] Tblock90 = new int[,]
        {
            { 0, 3, 0 },
            { 0, 3, 3 },
            { 0, 3, 0 }
        };
        private int[,] Tblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 3, 3, 3 },
            { 0, 3, 0 }
        };
        private int[,] Tblock270 = new int[,]
        {
            { 0, 3, 0 },
            { 3, 3, 0 },
            { 0, 3, 0 }
        };
        // LBLOCK ORIENTATIONS
        private int[,] Lblock = new int[,]
        {
            { 0, 0, 7 },
            { 7, 7, 7 },
            { 0, 0, 0 }
        };
        private int[,] Lblock90 = new int[,]
        {
            { 0, 7, 0 },
            { 0, 7, 0 },
            { 0, 7, 7 }
        };
        private int[,] Lblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 7, 7, 7 },
            { 7, 0, 0 }
        };
        private int[,] Lblock270 = new int[,]
        {
            { 7, 7, 0 },
            { 0, 7, 0 },
            { 0, 7, 0 }
        };
        // JBLOCK ORIENTATIONS
        private int[,] Jblock = new int[,]
        {
            { 9, 0, 0 },
            { 9, 9, 9 },
            { 0, 0, 0 }
        };
        private int[,] Jblock90 = new int[,]
        {
            { 0, 9, 9 },
            { 0, 9, 0 },
            { 0, 9, 0 }
        };
        private int[,] Jblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 9, 9, 9 },
            { 0, 0, 9 }
        };
        private int[,] Jblock270 = new int[,]
        {
            { 0, 9, 0 },
            { 0, 9, 0 },
            { 9, 9, 0 }
        };
        // OBLOCK ORIENTATIONS
        private int[,] Oblock = new int[,]
        {
            { 2, 2 },
            { 2, 2 }
        };
        // IBLOCK ORIENTATIONS
        private int[,] Iblock = new int[,]
        {
            { 0, 0, 0, 0 },
            { 4, 4, 4, 4 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        };
        private int[,] Iblock90 = new int[,]
        {
            { 0, 0, 4, 0 },
            { 0, 0, 4, 0 },
            { 0, 0, 4, 0 },
            { 0, 0, 4, 0 }
        };
        private int[,] Iblock180 = new int[,]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 4, 4, 4, 4 },
            { 0, 0, 0, 0 }
        };
        private int[,] Iblock270 = new int[,]
        {
            { 0, 4, 0, 0 },
            { 0, 4, 0, 0 },
            { 0, 4, 0, 0 },
            { 0, 4, 0, 0 }
        };
        // SBLOCK ORIENTATIONS
        private int[,] Sblock = new int[,]
        {
            { 0, 5, 5 },
            { 5, 5, 0 },
            { 0, 0, 0 }
        };
        private int[,] Sblock90 = new int[,]
        {
            { 0, 5, 0 },
            { 0, 5, 5 },
            { 0, 0, 5 }
        };
        private int[,] Sblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 0, 5, 5 },
            { 5, 5, 0 }
        };
        private int[,] Sblock270 = new int[,]
        {
            { 5, 0, 0 },
            { 5, 5, 0 },
            { 0, 5, 0 }
        };
        // ZBLOCK ORIENTATIONS
        private int[,] Zblock = new int[,]
        {
            { 6, 6, 0 },
            { 0, 6, 6 },
            { 0, 0, 0 }
        };
        private int[,] Zblock90 = new int[,]
        {
            { 0, 0, 6 },
            { 0, 6, 6 },
            { 0, 6, 0 }
        };
        private int[,] Zblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 6, 6, 0 },
            { 0, 6, 6 }
        };
        private int[,] Zblock270 = new int[,]
        {
            { 0, 6, 0 },
            { 6, 6, 0 },
            { 6, 0, 0 }
        };

        public Form1()
        {
            //start
            start();
        }
        private void start()
        {
            InitializeComponent();
            timer1.Stop();
            InitializeStartScreen();
            InitializePanels();

            this.DoubleBuffered = true;

            this.KeyDown += Form1_KeyDown;
        }
        private void InitializePanels()
        {
            // Initialize game board panel
            gameBoardPanel = new DoubleBufferedPanel
            {
                Width = BoardWidth * CellSize,
                Height = BoardHeight * CellSize,
                Left = 40,
                Top = 70
            };
            gameBoardPanel.Paint += GameBoardPanel_Paint;
            this.Controls.Add(gameBoardPanel);

            // Initialize next block panel
            nextBlockPanel = new DoubleBufferedPanel
            {
                Width = nextblocklimit * CellSize,
                Height = nextblocklimit * CellSize,
                Left = 400,
                Top = 300
            };
            nextBlockPanel.Paint += NextBlockPanel_Paint;
            this.Controls.Add(nextBlockPanel);
        }


        private void levelselect()
        {
            int levelbuttonwidth = 100;
            int levelbuttonheight = 64;

            int distancex = 100;
            int distancey = 175;
            //Level0 Select
            this.Controls.Add(level0button);
            level0button.Location = new Point(10 + 0 * distancex, distancey);
            level0button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level0button.Click += level0button_Click;
            level0button.MouseEnter += OnMouseEnterButtonLevel0;
            level0button.MouseLeave += OnMouseLeaveButtonLevel0;
            level0button.Image = Resource1._0button0;
            level0button.BringToFront();

            level0button.FlatStyle = FlatStyle.Flat;
            level0button.FlatAppearance.BorderSize = 0;
            level0button.TabStop = false;
            //Level1 Select
            this.Controls.Add(level1button);
            level1button.Location = new Point(10 + 1 * distancex, distancey);
            level1button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level1button.Click += level1button_Click;
            level1button.MouseEnter += OnMouseEnterButtonLevel1;
            level1button.MouseLeave += OnMouseLeaveButtonLevel1;
            level1button.Image = Resource1._1button0;
            level1button.BringToFront();

            level1button.FlatStyle = FlatStyle.Flat;
            level1button.FlatAppearance.BorderSize = 0;
            level1button.TabStop = false;
            //Level2 Select
            this.Controls.Add(level2button);
            level2button.Location = new Point(10 + 2 * distancex, distancey);
            level2button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level2button.Click += level2button_Click;
            level2button.MouseEnter += OnMouseEnterButtonLevel2;
            level2button.MouseLeave += OnMouseLeaveButtonLevel2;
            level2button.Image = Resource1._2button0;
            level2button.BringToFront();

            level2button.FlatStyle = FlatStyle.Flat;
            level2button.FlatAppearance.BorderSize = 0;
            level2button.TabStop = false;
            //Level3 Select
            this.Controls.Add(level3button);
            level3button.Location = new Point(10 + 3 * distancex, distancey);
            level3button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level3button.Click += level3button_Click;
            level3button.MouseEnter += OnMouseEnterButtonLevel3;
            level3button.MouseLeave += OnMouseLeaveButtonLevel3;
            level3button.Image = Resource1._3button0;
            level3button.BringToFront();

            level3button.FlatStyle = FlatStyle.Flat;
            level3button.FlatAppearance.BorderSize = 0;
            level3button.TabStop = false;
            //Level4 Select
            this.Controls.Add(level4button);
            level4button.Location = new Point(10 + 4 * distancex, distancey);
            level4button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level4button.Click += level4button_Click;
            level4button.MouseEnter += OnMouseEnterButtonLevel4;
            level4button.MouseLeave += OnMouseLeaveButtonLevel4;
            level4button.Image = Resource1._4button0;
            level4button.BringToFront();

            level4button.FlatStyle = FlatStyle.Flat;
            level4button.FlatAppearance.BorderSize = 0;
            level4button.TabStop = false;
            //Level5 Select
            this.Controls.Add(level5button);
            level5button.Location = new Point(10 + 0 * distancex, 100 + distancey);
            level5button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level5button.Click += level5button_Click;
            level5button.MouseEnter += OnMouseEnterButtonLevel5;
            level5button.MouseLeave += OnMouseLeaveButtonLevel5;
            level5button.Image = Resource1._5button0;
            level5button.BringToFront();

            level5button.FlatStyle = FlatStyle.Flat;
            level5button.FlatAppearance.BorderSize = 0;
            level5button.TabStop = false;
            //Level6 Select
            this.Controls.Add(level6button);
            level6button.Location = new Point(10 + 1 * distancex, 100 + distancey);
            level6button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level6button.Click += level6button_Click;
            level6button.MouseEnter += OnMouseEnterButtonLevel6;
            level6button.MouseLeave += OnMouseLeaveButtonLevel6;
            level6button.Image = Resource1._6button0;
            level6button.BringToFront();

            level6button.FlatStyle = FlatStyle.Flat;
            level6button.FlatAppearance.BorderSize = 0;
            level6button.TabStop = false;
            //Level7 Select
            this.Controls.Add(level7button);
            level7button.Location = new Point(10 + 2 * distancex, 100 + distancey);
            level7button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level7button.Click += level7button_Click;
            level7button.MouseEnter += OnMouseEnterButtonLevel7;
            level7button.MouseLeave += OnMouseLeaveButtonLevel7;
            level7button.Image = Resource1._7button0;
            level7button.BringToFront();

            level7button.FlatStyle = FlatStyle.Flat;
            level7button.FlatAppearance.BorderSize = 0;
            level7button.TabStop = false;
            //Level8 Select
            this.Controls.Add(level8button);
            level8button.Location = new Point(10 + 3 * distancex, 100 + distancey);
            level8button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level8button.Click += level8button_Click;
            level8button.MouseEnter += OnMouseEnterButtonLevel8;
            level8button.MouseLeave += OnMouseLeaveButtonLevel8;
            level8button.Image = Resource1._8button0;
            level8button.BringToFront();

            level8button.FlatStyle = FlatStyle.Flat;
            level8button.FlatAppearance.BorderSize = 0;
            level8button.TabStop = false;
            //Level9 Select
            this.Controls.Add(level9button);
            level9button.Location = new Point(10 + 4 * distancex, 100 + distancey);
            level9button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level9button.Click += level9button_Click;
            level9button.MouseEnter += OnMouseEnterButtonLevel9;
            level9button.MouseLeave += OnMouseLeaveButtonLevel9;
            level9button.Image = Resource1._9button0;
            level9button.BringToFront();

            level9button.FlatStyle = FlatStyle.Flat;
            level9button.FlatAppearance.BorderSize = 0;
            level9button.TabStop = false;
        }
        private void OnMouseEnterButtonLevel0(object sender, EventArgs e)
        {
            level0button.Image = Resource1._0button1;
        }
        private void OnMouseLeaveButtonLevel0(object sender, EventArgs e)
        {
            level0button.Image = Resource1._0button0;
        }
        private void OnMouseEnterButtonLevel1(object sender, EventArgs e)
        {
            level1button.Image = Resource1._1button1;
        }
        private void OnMouseLeaveButtonLevel1(object sender, EventArgs e)
        {
            level1button.Image = Resource1._1button0;
        }
        private void OnMouseEnterButtonLevel2(object sender, EventArgs e)
        {
            level2button.Image = Resource1._2button1;
        }
        private void OnMouseLeaveButtonLevel2(object sender, EventArgs e)
        {
            level2button.Image = Resource1._2button0;
        }
        private void OnMouseEnterButtonLevel3(object sender, EventArgs e)
        {
            level3button.Image = Resource1._3button1;
        }
        private void OnMouseLeaveButtonLevel3(object sender, EventArgs e)
        {
            level3button.Image = Resource1._3button0;
        }
        private void OnMouseEnterButtonLevel4(object sender, EventArgs e)
        {
            level4button.Image = Resource1._4button1;
        }
        private void OnMouseLeaveButtonLevel4(object sender, EventArgs e)
        {
            level4button.Image = Resource1._4button0;
        }
        private void OnMouseEnterButtonLevel5(object sender, EventArgs e)
        {
            level5button.Image = Resource1._5button1;
        }
        private void OnMouseLeaveButtonLevel5(object sender, EventArgs e)
        {
            level5button.Image = Resource1._5button0;
        }
        private void OnMouseEnterButtonLevel6(object sender, EventArgs e)
        {
            level6button.Image = Resource1._6button1;
        }
        private void OnMouseLeaveButtonLevel6(object sender, EventArgs e)
        {
            level6button.Image = Resource1._6button0;
        }
        private void OnMouseEnterButtonLevel7(object sender, EventArgs e)
        {
            level7button.Image = Resource1._7button1;
        }
        private void OnMouseLeaveButtonLevel7(object sender, EventArgs e)
        {
            level7button.Image = Resource1._7button0;
        }
        private void OnMouseEnterButtonLevel8(object sender, EventArgs e)
        {
            level8button.Image = Resource1._8button1;
        }
        private void OnMouseLeaveButtonLevel8(object sender, EventArgs e)
        {
            level8button.Image = Resource1._8button0;
        }
        private void OnMouseEnterButtonLevel9(object sender, EventArgs e)
        {
            level9button.Image = Resource1._9button1;
        }
        private void OnMouseLeaveButtonLevel9(object sender, EventArgs e)
        {
            level9button.Image = Resource1._9button0;
        }
        private void gameplay()
        {
            gameover = false;
            // Ensure panels are initialized first
            InitializeGameBoard();
            initializenextblock();
            ShowGameBoard();

            // Trigger initial updates to display the initial state
            UpdateGameBoard();
            updatenextblock();

            InitializeScore();
            Initializehighscore();
            Updatehighscore();
            InitializeLinecount();
            UpdateLinecount();
            InitializeLevel();
            UpdateLevel();
            UpdateScore();

            timer1.Start();

            blockpicker();

            drawblock(startx, starty);
            UpdateGameBoard();
        }

        private void InitializeStartScreen()
        {
            //START BUTTON
            this.Controls.Add(startbutton);
            startbutton.Location = new Point(100, 280);
            startbutton.Size = new Size(150, 75);

            startbutton.Image = Resource1.StartButton0;
            startbutton.FlatStyle = FlatStyle.Flat;
            startbutton.FlatAppearance.BorderSize = 0;
            startbutton.TabStop = false;

            startbutton.Click += startbutton_Click;
            startbutton.MouseEnter += OnMouseEnterButton0;
            startbutton.MouseLeave += OnMouseLeaveButton0;

            //QUIT BUTTON
            QuitButton();

            quitbutton.Click += quitbutton_Click;
            quitbutton.MouseEnter += OnMouseEnterButton1;
            quitbutton.MouseLeave += OnMouseLeaveButton1;

            //Background Picture
            BackroundImage();
            startscreenimage.Image = Resource1.TitleScreen;
            startbutton.BringToFront();
            quitbutton.BringToFront();
        }
        private void BackroundImage()
        {
            this.Controls.Add(startscreenimage);
            startscreenimage.Location = new Point(0, 0);
            startscreenimage.Size = new Size(520, 540);
            Controls.Add(startscreenimage);
            startscreenimage.SizeMode = PictureBoxSizeMode.StretchImage;
            startscreenimage.BringToFront();
        }
        private void OnMouseEnterButton0(object sender, EventArgs e)
        {
            startbutton.Image = Resource1.StartButton1;
        }
        private void OnMouseLeaveButton0(object sender, EventArgs e)
        {
            startbutton.Image = Resource1.StartButton0;
        }
        void startbutton_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(startbutton);
            this.Controls.Remove(quitbutton);
            this.Controls.Remove(restartbutton);

            BackroundImage();
            startscreenimage.Image = Resource1.levelselectscreen;
            levelselect();
        }
        private void OnMouseEnterButton1(object sender, EventArgs e)
        {
            quitbutton.Image = Resource1.QuitButton1;
        }
        private void OnMouseLeaveButton1(object sender, EventArgs e)
        {
            quitbutton.Image = Resource1.QuitButton0;
        }
        void quitbutton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }
        void level0button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 0;
            gameplay();
        }
        void level1button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 1;
            gameplay();
        }
        void level2button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 2;
            gameplay();
        }
        void level3button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 3;
            gameplay();
        }
        void level4button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 4;
            gameplay();
        }
        void level5button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 5;
            gameplay();
        }
        void level6button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 6;
            gameplay();
        }
        void level7button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 7;
            gameplay();
        }
        void level8button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 8;
            gameplay();
        }
        void level9button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 9;
            gameplay();
        }
        private void startscriptremove()
        {
            this.Controls.Remove(startbutton);
            this.Controls.Remove(quitbutton);
            this.Controls.Remove(restartbutton);
            this.Controls.Remove(startscreenimage);

            this.Controls.Remove(level0button);
            this.Controls.Remove(level1button);
            this.Controls.Remove(level2button);
            this.Controls.Remove(level3button);
            this.Controls.Remove(level4button);
            this.Controls.Remove(level5button);
            this.Controls.Remove(level6button);
            this.Controls.Remove(level7button);
            this.Controls.Remove(level8button);
            this.Controls.Remove(level9button);
        }


        private void ShowGameBoard()
        {
            gameBoardPanel.Invalidate();
            nextBlockPanel.Invalidate();

            // Reinitialize game components if necessary
            //InitializeComponent();
        }
        private void HideGameBoard()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    permanentGameBoardOld[x, y] = 0;
                    tempGameBoardOld[x, y] = 0;
                    permanentGameBoard[x, y] = 0;
                    tempGameBoard[x, y] = 0;
                }
            }
            for (int x = 0; x < nextblocklimit; x++)
            {
                for (int y = 0; y < nextblocklimit; y++)
                {
                    nextblockscreen[x, y] = 0;
                    nextblockscreenOld[x, y] = 0;
                }
            }
            gameBoardPanel.Invalidate();
            nextBlockPanel.Invalidate();
            //this.Invalidate(); // Ensure the form is redrawn
        }
        private void RemoveGameBoard(object sender, PaintEventArgs e)
        {
            /*Graphics g = e.Graphics;
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    using (Image img = Resource1.Blank0)
                    {
                        g.DrawImage(img, x * CellSize, y * CellSize, CellSize, CellSize);
                    }
                }
            }*/
            gameBoardPanel.Invalidate();
            nextBlockPanel.Invalidate();
            this.Invalidate();
        }
        private void ShowGameOverScreen()
        {
            //Gameover Score
            Finalscorev();

            //Gameover Highscore
            Finalhighscorev();
            // Generate Quit Button
            QuitButton();

            RestartButton();

        }
        private void RestartButton()
        {
            this.Controls.Add(restartbutton);
            restartbutton.Location = new Point(100, 280);
            restartbutton.Size = new Size(150, 75);

            restartbutton.Click += restartbutton_Click;
            restartbutton.MouseEnter += OnMouseEnterRestartButton;
            restartbutton.MouseLeave += OnMouseLeaveRestartButton;
            restartbutton.Image = Resource1.RestartButton0;
            restartbutton.FlatStyle = FlatStyle.Flat;
            restartbutton.FlatAppearance.BorderSize = 0;
            restartbutton.TabStop = false;
            restartbutton.BringToFront();
        }
        private void OnMouseEnterRestartButton(object sender, EventArgs e)
        {
            restartbutton.Image = Resource1.RestartButton1;
        }
        private void OnMouseLeaveRestartButton(object sender, EventArgs e)
        {
            restartbutton.Image = Resource1.RestartButton0;
        }
        private void QuitButton()
        {
            this.Controls.Add(quitbutton);
            quitbutton.Location = new Point(275, 280);
            quitbutton.Size = new Size(150, 75);

            quitbutton.Image = Resource1.QuitButton0;
            quitbutton.FlatStyle = FlatStyle.Flat;
            quitbutton.FlatAppearance.BorderSize = 0;
            quitbutton.TabStop = false;
            quitbutton.BringToFront();
        }
        void restartbutton_Click(object sender, EventArgs e)
        {
            Score = 0;
            Highscore = 0;

            this.Controls.Remove(quitbutton);
            this.Controls.Remove(restartbutton);
            this.Controls.Remove(finalhighscorep);
            this.Controls.Remove(finalscorep);

            Level = 0;
            completedlines = 0;

            levelselect();
            BackroundImage();
            startscreenimage.Image = Resource1.levelselectscreen; //BUGS
            startscreenimage.SendToBack();
            gameBoardPanel.SendToBack();
            nextBlockPanel.SendToBack();
        }

        private void Finalhighscorev()
        {
            //Gameover Highscore
            finalhighscorep.Visible = true;
            finalhighscorep.Location = new Point(150, 120);
            finalhighscorep.AutoSize = true;
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Github\Tetrisnt\WindowsFormsApp1\WindowsFormsApp1\fonts\Press_Start_2P\PressStart2P-Regular.ttf");
            finalhighscorep.Font = new Font(pfc.Families[0], 20, FontStyle.Regular);
            finalhighscorep.ForeColor = Color.Black;
            finalhighscorep.Padding = new Padding(6);
            this.Controls.Add(finalhighscorep);
            string Endhighscore = FinalHighScore.ToString();
            finalhighscorep.Text = String.Format($"Highscore: {Endhighscore}");
            finalhighscorep.Refresh();
            finalhighscorep.BringToFront();
        }
        private void Finalscorev()
        {
            //Gameover Score
            finalscorep.Visible = true;
            finalscorep.Location = new Point(150, 60);
            finalscorep.AutoSize = true;
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Github\Tetrisnt\WindowsFormsApp1\WindowsFormsApp1\fonts\Press_Start_2P\PressStart2P-Regular.ttf");
            finalscorep.Font = new Font(pfc.Families[0], 20, FontStyle.Regular);
            finalscorep.ForeColor = Color.Black;
            finalscorep.Padding = new Padding(6);
            this.Controls.Add(finalscorep);
            string Endscore = FinalScore.ToString();
            finalscorep.Text = String.Format($"Score: {Endscore}");
            finalscorep.Refresh();
            finalscorep.BringToFront();

        }
        private void InitializeGameOver()
        {
            
            //gameBoardPanel.Paint += RemoveGameBoard;
            timer1.Stop();
            this.Controls.Remove(scoreboard);
            this.Controls.Remove(highscore);
            this.Controls.Remove(linecount);
            this.Controls.Remove(levelcount);
            if(Highscore >= FinalHighScore)
            {
                FinalHighScore = Highscore;
            }
            
            FinalScore = Score;
            HideGameBoard();
            ShowGameOverScreen();

        }
        private void InitializeGameBoard()
        {
            for (int x = 2; x < BoardWidth - 2; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    tempGameBoard[x, y] = 0;  // Initialize with empty cells
                    permanentGameBoard[x, y] = 0;
                }
            }
            for(int x = 0; x < 2; x++)
            {
                for(int y = 0; y < BoardHeight; y++)
                {
                    permanentGameBoard[x, y] = 11;
                }
            }
            for (int x = BoardWidth - 2; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    permanentGameBoard[x, y] = 11;
                }
            }
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = BoardHeight - 1; y < BoardHeight; y++)
                {
                    permanentGameBoard[x, y] = 11;
                }
            }
            UpdateGameBoard();
        }
        private void InitializeScore()
        {

            scoreboard.Location = new Point(320, 120);
            scoreboard.AutoSize = true;
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Github\Tetrisnt\WindowsFormsApp1\WindowsFormsApp1\fonts\Press_Start_2P\PressStart2P-Regular.ttf");
            scoreboard.Font = new Font(pfc.Families[0], 13, FontStyle.Regular);
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
            highscore.Location = new Point(320, 50);
            highscore.AutoSize = true;
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Github\Tetrisnt\WindowsFormsApp1\WindowsFormsApp1\fonts\Press_Start_2P\PressStart2P-Regular.ttf");
            highscore.Font = new Font(pfc.Families[0], 13, FontStyle.Regular);
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
        private void InitializeLevel()
        {

            levelcount.Location = new Point(320, 180);
            levelcount.AutoSize = true;
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Github\Tetrisnt\WindowsFormsApp1\WindowsFormsApp1\fonts\Press_Start_2P\PressStart2P-Regular.ttf");
            levelcount.Font = new Font(pfc.Families[0], 13, FontStyle.Regular);
            levelcount.ForeColor = Color.Black;
            levelcount.Padding = new Padding(6);
            this.Controls.Add(levelcount);
            levelcount.Refresh();
            levelcount.Visible = true;

        }
        private void UpdateLevel()
        {
            string Levelachieved = Level.ToString();
            levelcount.Text = String.Format($"Level {Levelachieved}");
        }
        private void InitializeLinecount()
        {
            //completedlines
            linecount.Location = new Point(75, 20);
            linecount.AutoSize = true;
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Github\Tetrisnt\WindowsFormsApp1\WindowsFormsApp1\fonts\Press_Start_2P\PressStart2P-Regular.ttf");
            linecount.Font = new Font(pfc.Families[0], 15, FontStyle.Regular);
            linecount.ForeColor = Color.Black;
            linecount.Padding = new Padding(6);
            this.Controls.Add(linecount);
            linecount.Refresh();
        }
        private void UpdateLevelcount()
        {
            if(onelevelup >= 10)
            {
                Level += 1;
                onelevelup = 0;
            }
            
            UpdateTimerInterval();
            UpdateLevel();
        }
        private void UpdateTimerInterval()
        {
            if(timer1.Interval > 0)
            {
                timer1.Interval = 1000 - (Level * 100);
            }
        }
        private void UpdateLinecount()
        {
            string Linecounter = completedlines.ToString();
            linecount.Text = String.Format($"Lines - {Linecounter}");
        }
        private System.Drawing.Image GetCombinedColor(int x, int y)
        {
            if (tempGameBoard[x, y] == 1 || permanentGameBoard[x, y] == 1)
                return Resource1.TetrominoBlack;
            if (tempGameBoard[x, y] == 2 || permanentGameBoard[x, y] == 2)
                return Resource1.TetrominoRed;
            if (tempGameBoard[x, y] == 3 || permanentGameBoard[x, y] == 3)
                return Resource1.TetrominoOrange;
            if (tempGameBoard[x, y] == 4 || permanentGameBoard[x, y] == 4)
                return Resource1.TetrominoYellow;
            if (tempGameBoard[x, y] == 5 || permanentGameBoard[x, y] == 5)
                return Resource1.TetrominoPurple;
            if (tempGameBoard[x, y] == 6 || permanentGameBoard[x, y] == 6)
                return Resource1.TetrominoPink;
            if (tempGameBoard[x, y] == 7 || permanentGameBoard[x, y] == 7)
                return Resource1.TetrominoBlue;
            if (tempGameBoard[x, y] == 8 || permanentGameBoard[x, y] == 8)
                return Resource1.TetrominoLightBlue;
            if (tempGameBoard[x, y] == 9 || permanentGameBoard[x, y] == 9)
                return Resource1.TetrominoGreen;
            if (tempGameBoard[x, y] == 10 || permanentGameBoard[x, y] == 10)
                return Resource1.Blank1;
            if (tempGameBoard[x, y] == 11 || permanentGameBoard[x, y] == 11)
                return Resource1.Blank2;
            else
                return Resource1.Blank0;
        }
        private System.Drawing.Image GetCombinedColorNext(int x, int y)
        {
            if (nextblockscreen[x, y] == 1)
                return Resource1.TetrominoBlack;
            if (nextblockscreen[x, y] == 2)
                return Resource1.TetrominoRed;
            if (nextblockscreen[x, y] == 3)
                return Resource1.TetrominoOrange;
            if (nextblockscreen[x, y] == 4)
                return Resource1.TetrominoYellow;
            if (nextblockscreen[x, y] == 5)
                return Resource1.TetrominoPurple;
            if (nextblockscreen[x, y] == 6)
                return Resource1.TetrominoPink;
            if (nextblockscreen[x, y] == 7)
                return Resource1.TetrominoBlue;
            if (nextblockscreen[x, y] == 8)
                return Resource1.TetrominoLightBlue;
            if (nextblockscreen[x, y] == 9)
                return Resource1.TetrominoGreen;
            if (nextblockscreen[x, y] == 10)
                return Resource1.Blank1;
            if (nextblockscreen[x, y] == 11)
                return Resource1.Blank2;
            else
                return Resource1.Blank0;
        }
        private void UpdateGameBoard()
        {
            // Clear previous positions
            if (!gameover)
            {
                for (int x = 2; x < BoardWidth - 2; x++)
                {
                    for (int y = 0; y < BoardHeight; y++)
                    {
                        if (permanentGameBoardOld[x, y] != permanentGameBoard[x, y] || tempGameBoardOld[x, y] != tempGameBoard[x, y])
                        {
                            Rectangle rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                            gameBoardPanel.Invalidate(rect);
                        }
                        permanentGameBoardOld[x, y] = permanentGameBoard[x, y];
                        tempGameBoardOld[x, y] = tempGameBoard[x, y];
                    }
                }
            }
            Debug.WriteLine("Updated the Game Board");
        }


        // Example usage
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        int startx = 5;
        int starty = 0;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int blockSizeX = 0;
            int blockSizeY = 0;
            // Calculate the maximum x and y values for the current block
            int maxX = BoardWidth - blockSizeX;
            int maxY = BoardHeight - blockSizeY;
            int minX = BoardWidth - blockSizeX;
            if (gameover == false)
            {
                if (chosenblock == Tblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Tblock90.GetLength(1);
                            blockSizeY = Tblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Tblock180.GetLength(1);
                            blockSizeY = Tblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Tblock270.GetLength(1);
                            blockSizeY = Tblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Tblock.GetLength(1);
                            blockSizeY = Tblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Lblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Lblock90.GetLength(1);
                            blockSizeY = Lblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Lblock180.GetLength(1);
                            blockSizeY = Lblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Lblock270.GetLength(1);
                            blockSizeY = Lblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Lblock.GetLength(1);
                            blockSizeY = Lblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Jblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Jblock90.GetLength(1);
                            blockSizeY = Jblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Jblock180.GetLength(1);
                            blockSizeY = Jblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Jblock270.GetLength(1);
                            blockSizeY = Jblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Jblock.GetLength(1);
                            blockSizeY = Jblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Iblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Iblock90.GetLength(1);
                            blockSizeY = Iblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 0 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Iblock180.GetLength(1);
                            blockSizeY = Iblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 6 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Iblock270.GetLength(1);
                            blockSizeY = Iblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Iblock.GetLength(1);
                            blockSizeY = Iblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 6 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Oblock)
                {
                    blockSizeX = Oblock.GetLength(1);
                    blockSizeY = Oblock.GetLength(0);

                    switch (e.KeyCode)
                    {
                        case Keys.Right:
                            if (startx < maxX - 4 && AllowMoveRight == true)
                            {
                                clearblock();
                                startx += 1;
                                drawblock(startx, starty);
                                placeblockcheck();
                                UpdateGameBoard();
                            }
                            break;
                        case Keys.Left:
                            if (startx > 2 && AllowMoveLeft == true)
                            {
                                clearblock();
                                startx -= 1;
                                drawblock(startx, starty);
                                placeblockcheck();
                                UpdateGameBoard();
                            }
                            break;
                        case Keys.Down:
                            if (starty < BoardHeight - 3)
                            {
                                placeblockcheck();
                                clearblock();
                                starty += 1;
                                drawblock(startx, starty);
                                placeblockcheck();
                                UpdateGameBoard();
                            }
                            break;
                    }
                }
                if (chosenblock == Sblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Sblock90.GetLength(1);
                            blockSizeY = Sblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Sblock180.GetLength(1);
                            blockSizeY = Sblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Sblock270.GetLength(1);
                            blockSizeY = Sblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Sblock.GetLength(1);
                            blockSizeY = Sblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                if (chosenblock == Zblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Zblock90.GetLength(1);
                            blockSizeY = Zblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Zblock180.GetLength(1);
                            blockSizeY = Zblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Zblock270.GetLength(1);
                            blockSizeY = Zblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Zblock.GetLength(1);
                            blockSizeY = Zblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                switch (e.KeyCode)
                {
                    case Keys.Y:
                        if (starty < maxY && AllowRotationLeft == true)
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
                        if (starty < maxY && AllowRotationRight == true)
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
            CheckCollision();
            RotationLock(startx, starty);
        }
        private void CheckCollision()
        {
            if(chosenblock == Oblock)
            {
                for (int y = starty; y <= starty + 1; y++)
                {
                    for (int xleft = startx - 1; xleft <= startx - 1; xleft++)
                    {
                        for (int xright = startx + 2; xright <= startx + 2; xright++)
                        {
                            if (permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            if (permanentGameBoard[xright, y] == 0)
                            {
                                AllowMoveRight = true;
                            }
                            if (permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            if (permanentGameBoard[xleft, y] == 0)
                            {
                                AllowMoveLeft = true;
                            }
                        }
                    }
                }
            }
            else if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            for (int y = starty; y <= starty + 3; y++)
                            {
                                for (int xleft = startx + 1; xleft <= startx + 1; xleft++)
                                {
                                    for (int xright = startx + 3; xright <= startx + 3; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 3:
                        {//180
                            for (int y = starty + 2; y <= starty + 2; y++)
                            {
                                for (int xleft = startx - 1; xleft <= startx - 1; xleft++)
                                {
                                    for (int xright = startx + 4; xright <= startx + 4; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 4:
                        {//270
                            for (int y = starty; y <= starty + 3; y++)
                            {
                                for (int xleft = startx; xleft <= startx; xleft++)
                                {
                                    for (int xright = startx + 2; xright <= startx + 2; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {// 0
                            for (int y = starty + 1; y <= starty + 1; y++)
                            {
                                for (int xleft = startx - 1; xleft <= startx - 1; xleft++)
                                {
                                    for (int xright = startx + 4; xright <= startx + 4; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty + 2] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }
                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }
                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }
                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }
                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 2, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
        }
        int Rotationlockcountright = 0;
        int Rotationlockcountleft = 0;
        private void RotationLock(int x, int y)
        {
            Rotationlockcountright = 0;
            Rotationlockcountleft = 0;
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for(int i = 0; i < 4; i++)
                        {
                            if (permanentGameBoard[x + i, y + 2] != 0)//180
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0)//0
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int i = 0; i < 4; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0)//270
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 2, y + i] != 0)//90
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int i = 0; i < 4; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0)//0
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 2] != 0)//180
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    default:
                        //0
                        for (int i = 0; i < 4; i++)
                        {
                            if (permanentGameBoard[x + 2, y + i] != 0)//90
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0)//270
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;

                }
            }
            else if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 1, y + 2] != 0)//180
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 1, y] != 0)//0
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y + 1] != 0)//270
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y + 1] != 0)//90
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 1, y] != 0)//0
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 1, y + 2] != 0)//180
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    default:
                        //0
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y + 1] != 0)//90
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y + 1] != 0)//270
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;

                }
            }
            else if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x, y + 2] != 0)//180
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 2, y] != 0)//0
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y] != 0)//270
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y + 2] != 0)//90
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 2, y] != 0)//0
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x, y + 2] != 0)//180
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    default:
                        //0
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y + 2] != 0)//90
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y] != 0)//270
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;

                }
            }
            else if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 2, y + 2] != 0)//180
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x, y] != 0)//0
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y + 2] != 0)//270
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y] != 0)//90
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x, y] != 0)//0
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 2, y + 2] != 0)//180
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    default:
                        //0
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y] != 0)//90
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y + 2] != 0)//270
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;

                }
            }
            else if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int i = 0; i < 2; i++)
                        {
                            if (permanentGameBoard[x + 1 + i, y + 1] != 0 || permanentGameBoard[x + i, y + 2] != 0)//180
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1 + i, y] != 0 || permanentGameBoard[x + i, y + 1] != 0)//0
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x, y + i] != 0 || permanentGameBoard[x + 1, y + 1 + i] != 0)//270
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y + 1 + i] != 0)//90
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1 + i, y] != 0 || permanentGameBoard[x + i, y + 1] != 0)//0
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1 + i, y + 1] != 0 || permanentGameBoard[x + i, y + 2] != 0)//180
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    default:
                        //0
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x + 2, y + 1 + i] != 0)//90
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x, y + i] != 0 || permanentGameBoard[x + 1, y + 1 + i] != 0)//270
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                }
            }
            else if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        for (int i = 0; i < 2; i++)
                        {
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 1 + i, y + 2] != 0)//180
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y] != 0 || permanentGameBoard[x + 1 + i, y + 1] != 0)//0
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 3:
                        //180
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y + 1 + i] != 0)//270
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 2, y + i] != 0 || permanentGameBoard[x + 1, y + 1 + i] != 0)//90
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    case 4:
                        //270
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + i, y] != 0 || permanentGameBoard[x + 1 + i, y + 1] != 0)//0
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + i, y + 1] != 0 || permanentGameBoard[x + 1 + i, y + 2] != 0)//180
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                    default:
                        //0
                        for (int i = 0; i < 3; i++)
                        {
                            if (permanentGameBoard[x + 2, y + i] != 0 || permanentGameBoard[x + 1, y + 1 + i] != 0)//90
                            {
                                Rotationlockcountright += 1;
                            }
                            if (permanentGameBoard[x + 1, y + i] != 0 || permanentGameBoard[x, y + 1 + i] != 0)//270
                            {
                                Rotationlockcountleft += 1;
                            }
                        }
                        break;
                }
            }
            else
            {

            }

            if (Rotationlockcountleft > 0)
            {
                AllowRotationLeft = false;
            }
            else
            {
                AllowRotationLeft = true;
            }
            if (Rotationlockcountright > 0)
            {
                AllowRotationRight = false;
            }
            else
            {
                AllowRotationRight = true;
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

        private void blockpicker()
        {
            Random rnd = new Random();
            if (blocktype == 0 && nextblock == 0)
            {
                nextblock = rnd.Next(1, 8);
                blocktype = rnd.Next(1, 8);
                Console.WriteLine("set first Value to Blockpicker");
            }
            else
            {
                blocktype = nextblock;
                nextblock = rnd.Next(1, 8);
                Console.WriteLine("Convert and set new Value to Blockpicker");
            }

            // Block type Lock:
            //int  blocktype = 7;

            switch (blocktype)
            {
                case 1:
                    chosenblock = Tblock;
                    Console.WriteLine("Chosenblock = Tblock");
                    break;
                case 2:
                    chosenblock = Lblock;
                    Console.WriteLine("Chosenblock = Lblock");
                    break;
                case 3:
                    chosenblock = Jblock;
                    Console.WriteLine("Chosenblock = Jblock");
                    break;
                case 4:
                    chosenblock = Iblock;
                    Console.WriteLine("Chosenblock = Iblock");
                    break;
                case 5:
                    chosenblock = Oblock;
                    Console.WriteLine("Chosenblock = Oblock");
                    break;
                case 6:
                    chosenblock = Sblock;
                    Console.WriteLine("Chosenblock = Sblock");
                    break;
                case 7:
                    chosenblock = Zblock;
                    Console.WriteLine("Chosenblock = Zblock");
                    break;
            }
            Debug.WriteLine($"The BLocks are {blocktype} and {nextblock}");

            clearnextblock();
            drawnextblock();
            updatenextblock();
        }
        private void initializenextblock()
        {
            for (int x = 0; x < nextblocklimit; x++)
            {
                for (int y = 0; y < nextblocklimit; y++)
                {
                    nextblockscreen[x, y] = 0;  // Initialize with empty cells
                }
            }
            Debug.WriteLine("Initialized Next Block");
        }
        private void GameBoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (!gameover)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    for (int y = 0; y < BoardHeight; y++)
                    {
                        Image image = GetCombinedColor(x, y);
                        g.DrawImage(image, x * CellSize, y * CellSize, CellSize, CellSize);
                    }
                }
            }
        }
        private void NextBlockPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (!gameover)
            {
                for (int x = 0; x < nextblocklimit; x++)
                {
                    for (int y = 0; y < nextblocklimit; y++)
                    {
                        Image image = GetCombinedColorNext(x, y);
                        g.DrawImage(image, x * CellSize, y * CellSize, CellSize, CellSize);
                    }
                }
            }
        }


        private void updatenextblock()
        {
            for (int x = 0; x < nextblocklimit; x++)
            {
                for (int y = 0; y < nextblocklimit; y++)
                {
                    if (nextblockscreenOld[x, y] != nextblockscreen[x, y])
                    {
                        Rectangle rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);
                        nextBlockPanel.Invalidate(rect);
                    }

                    nextblockscreenOld[x, y] = nextblockscreen[x, y];
                }
            }
            Debug.WriteLine("Updated the Next Block");
        }
        private void clearnextblock()
        {
            for (int y = 0; y < nextblocklimit; y++)
            {
                for (int x = 0; x < nextblocklimit; x++)
                {
                    nextblockscreen[x, y] = 0; // Clear all cells
                }
            }
            Debug.WriteLine($"Cleared Next BLock");
        }
        private void drawnextblock()
        {
            Debug.WriteLine("Nextblock: " + nextblock);
            if(nextblock == 1)
            {
                for (int y = 0; y < Tblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Tblock.GetLength(1); x++)
                    {
                        if (Tblock[x, y] != 0)
                        {
                            
                            int screenX = 1 + x;
                            int screenY = 0 + y;

                            nextblockscreen[screenY, screenX] = Tblock[x, y];
                        }
                    }
                }
            }
            else if(nextblock == 2)
            {
                for (int y = 0; y < Lblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Lblock.GetLength(1); x++)
                    {
                        if (Lblock[y, x] != 0)
                        {
                            int screenX = 0 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = Lblock[y, x];
                            }
                        }
                    }
                }
            }
            else if (nextblock == 3)
            {
                for (int y = 0; y < Jblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Jblock.GetLength(1); x++)
                    {
                        if (Jblock[y, x] != 0)
                        {
                            int screenX = 0 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = Jblock[y, x];
                            }
                        }
                    }
                }
            }
            else if (nextblock == 4)
            {
                for (int y = 0; y < Iblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Iblock.GetLength(1); x++)
                    {
                        if (Iblock[y, x] != 0)
                        {
                            int screenX = 0 + x;
                            int screenY = 0 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = Iblock[y, x];
                            }
                        }
                    }
                }
            }
            else if (nextblock == 5)
            {
                for (int y = 0; y < Oblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Oblock.GetLength(1); x++)
                    {
                        if (Oblock[y, x] != 0)
                        {
                            int screenX = 1 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = Oblock[y, x]; 
                            }
                        }
                    }
                }
            }
            else if(nextblock == 6)
            {
                for (int y = 0; y < Sblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Sblock.GetLength(1); x++)
                    {
                        if (Sblock[y, x] != 0)
                        {
                            int screenX = 0 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = Sblock[y, x];
                            }
                        }
                    }
                }
            }
            else if(nextblock == 7)
            {
                for (int y = 0; y < Zblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Zblock.GetLength(1); x++)
                    {
                        if (Zblock[y, x] != 0)
                        {
                            int screenX = 0 + x; 
                            int screenY = 1 + y; 

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = Zblock[y, x];
                            }
                        }
                    }
                }
            }

            
        }
        private void gameovercheck()
        {

            for (int x = 2; x < BoardWidth - 2; x++)
            {
                if (permanentGameBoard[x, 3] != 0)
                {
                    gameover = true;
                }
            }
            if(gameover)
            {
                InitializeGameOver();
            }
        }
        private void placeblockcheck()
        {
            gameovercheck();

            if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if(starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
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
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
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
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
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
                        if (starty >= BoardHeight - 5)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 5)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if(chosenblock == Oblock)
            {
                if (starty >= BoardHeight - 3)
                {
                    placeblock();
                    UpdateScore();
                    return;
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }


            bool isBlocked = false;

            // Check if any part of the block would hit a permanent block or the bottom of the game board
            if (chosenblock == Oblock)
            {
                switch (rotation)
                {
                    default: // -- works
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
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- Works
                        for (int y = starty; y <= starty + 4; y++)
                        {
                            for (int x = startx + 2; x <= startx + 2; x++)
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
                        //180 -- works
                        for (int y = starty + 1; y <= starty + 3; y++)
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
                        //270 -- works
                        for (int y = starty; y <= starty + 4; y++)
                        {
                            for (int x = startx + 1; x <= startx + 1; x++)
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
                        //0 -- works
                        for (int y = starty + 1; y <= starty + 2; y++)
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
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx , starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
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
                                if (Tblock90[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Tblock90[y, x];
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
                                if (Tblock180[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Tblock180[y, x];
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
                                if (Tblock270[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Tblock270[y, x];
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
                                if (Tblock[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Tblock[y, x];
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
                                if (Lblock90[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Lblock90[y, x];
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
                                if (Lblock180[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Lblock180[y, x];
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
                                if (Lblock270[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Lblock270[y, x];
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
                                if (Lblock[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Lblock[y, x];
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
                                if (Jblock90[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Jblock90[y, x];
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
                                if (Jblock180[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Jblock180[y, x];
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
                                if (Jblock270[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Jblock270[y, x];
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
                                if (Jblock[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Jblock[y, x];
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
                                if (Iblock90[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Iblock90[y, x];
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
                                if (Iblock180[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Iblock180[y, x];
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
                                if (Iblock270[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Iblock270[y, x];
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
                                if (Iblock[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Iblock[y, x];
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
                        if (Oblock[y, x] != 0)
                        {
                            int fieldX = startX + x;
                            int fieldY = startY + y;
                            if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                            {
                                tempGameBoard[fieldX, fieldY] = Oblock[y, x];
                            }
                        }
                    }
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Sblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock90.GetLength(1); x++)
                            {
                                if (Sblock90[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Sblock90[y, x];
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Sblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock180.GetLength(1); x++)
                            {
                                if (Sblock180[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Sblock180[y, x];
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Sblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock270.GetLength(1); x++)
                            {
                                if (Sblock270[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Sblock270[y, x];
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 0
                        for (int y = 0; y < Sblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock.GetLength(1); x++)
                            {
                                if (Sblock[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Sblock[y, x];
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Zblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock90.GetLength(1); x++)
                            {
                                if (Zblock90[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Zblock90[y, x];
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Zblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock180.GetLength(1); x++)
                            {
                                if (Zblock180[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Zblock180[y, x];
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Zblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock270.GetLength(1); x++)
                            {
                                if (Zblock270[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Zblock270[y, x];
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 5
                        for (int y = 0; y < Zblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock.GetLength(1); x++)
                            {
                                if (Zblock[y, x] != 0)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = Zblock[y, x];
                                    }
                                }
                            }
                        }
                        break;
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
                                if (Tblock90[y, x] != 0)
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
                                if (Tblock180[y, x] != 0)
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
                                if (Tblock270[y, x] != 0)
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
                                if (Tblock[y, x] != 0)
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
                                if (Lblock90[y, x] != 0)
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
                                if (Lblock180[y, x] != 0)
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
                                if (Lblock270[y, x] != 0)
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
                                if (Lblock[y, x] != 0)
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
                                if (Jblock90[y, x] != 0)
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
                                if (Jblock180[y, x] != 0)
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
                                if (Jblock270[y, x] != 0)
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
                                if (Jblock[y, x] != 0)
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
                                if (Iblock90[y, x] != 0)
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
                                if (Iblock180[y, x] != 0)
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
                                if (Iblock270[y, x] != 0)
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
                                if (Iblock[y, x] != 0)
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
                        if (Oblock[y, x] != 0)
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
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Sblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock90.GetLength(1); x++)
                            {
                                if (Sblock90[y, x] != 0)
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
                        // 180
                        for (int y = 0; y < Sblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock180.GetLength(1); x++)
                            {
                                if (Sblock180[y, x] != 0)
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
                        // 270
                        for (int y = 0; y < Sblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock270.GetLength(1); x++)
                            {
                                if (Sblock270[y, x] != 0)
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
                        // 0
                        for (int y = 0; y < Sblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock.GetLength(1); x++)
                            {
                                if (Sblock[y, x] != 0)
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
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Zblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock90.GetLength(1); x++)
                            {
                                if (Zblock90[y, x] != 0)
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
                        // 180
                        for (int y = 0; y < Zblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock180.GetLength(1); x++)
                            {
                                if (Zblock180[y, x] != 0)
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
                        // 270
                        for (int y = 0; y < Zblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock270.GetLength(1); x++)
                            {
                                if (Zblock270[y, x] != 0)
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
                        // 5
                        for (int y = 0; y < Zblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock.GetLength(1); x++)
                            {
                                if (Zblock[y, x] != 0)
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
            UpdateGameBoard();
        }

        private void placeblock()
        {
            if (starty >= 2 )
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    for (int x = 0; x < BoardWidth; x++)
                    {
                        if (tempGameBoard[x, y] != 0)
                        {
                            permanentGameBoard[x, y] = tempGameBoard[x, y];
                        }
                    }
                }
                Array.Clear(tempGameBoard, 0, tempGameBoard.Length);

                CheckCompletedRows();

                AllowMoveLeft = true;
                AllowMoveRight = true;
                AllowRotationLeft = true;
                AllowRotationRight = true;
                Rotationlockcountright = 0;
                Rotationlockcountleft = 0;
                rotation = 5;
                startx = 5;
                starty = 0;
                blockpicker();
                drawblock(startx, starty);
                UpdateGameBoard();
            }
        }

        private void CheckCompletedRows()
        {
            for (int y = BoardHeight - 2; y >= 0; y--)
            {
                bool rowCompleted = true;
                for (int x = 2; x < BoardWidth - 2; x++)
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
                    y++; // Check the same row again as rows have been shifted down
                }
            }
            UpdateLevel();
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
            // Clear the top row
            for (int x = 0; x < BoardWidth; x++)
            {
                permanentGameBoard[x, 0] = 0;
            }

            Score += 40 + ((Level + 1) * 40);
            completedlines += 1;
            UpdateLinecount();
            UpdateLevel();
            onelevelup += 1;
            UpdateLevel();
        }
    }
}
