using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        decimal oldtimer = 0;
        decimal newtimer = 1;

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            oldtimer = newtimer;
            if (starty <= BoardHeight - 2)
            {
                if (newtimer > oldtimer | newtimer == oldtimer)
                {
                    CheckCollision();
                    placeblockcheck();
                    clearblock();
                    UpdateGameBoard();
                    drawnextblock();

                    newtimer = +1;
                    oldtimer = newtimer;
                    if (starty <= BoardHeight - 1)
                    {
                        starty += 1;
                    }

                    drawblock(startx, starty);
                    UpdateGameBoard();

                    if (starty == BoardHeight - 2)
                    {
                        placeblock();
                    }
                    placeblockcheck();
                }
                else
                {
                    clearblock();
                    UpdateGameBoard();

                    newtimer = +1;
                    if (starty <= BoardHeight - 1)
                    {
                        starty += 1;
                    }

                    drawblock(startx, starty);
                    UpdateGameBoard();
                    placeblockcheck();
                    UpdateLevel();
                }
            }
            
        }

    }


}
