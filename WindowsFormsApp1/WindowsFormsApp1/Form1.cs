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

        int Tickdelay = 0;

        private void Form_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            oldtimer = newtimer;
            if (starty <= BoardHeight - 2)
            {
                if (newtimer > oldtimer || newtimer == oldtimer)
                {
                    RotationLock(startx, starty);
                    if (Tickdelay == 0)
                    {
                        Tickdelay += 1;
                        //Tick 1
                        UpdateLevelcount();
                        CheckCollision();
                        RotationLock(startx, starty);
                        placeblockcheck();
                        clearblock();

                        drawblock(startx, starty);
                        UpdateGameBoard();

                        if (starty == BoardHeight - 2)
                        {
                            placeblock();
                        }
                        RotationLock(startx, starty);
                    }
                    else if (Tickdelay == 1)
                    {
                        Tickdelay -= 1;
                        //Tick 2
                        Tickdelay = 0;

                        UpdateLevelcount();
                        clearblock();

                        if (starty <= BoardHeight - 1)
                        {
                            starty += 1;
                        }
                        drawblock(startx, starty);
                        UpdateGameBoard();
                        UpdateLevel();

                    }
                    else
                    {
                        Tickdelay = 0;
                    }
                    newtimer = +1;
                    oldtimer = newtimer;
                }
                else
                {

                    RotationLock(startx, starty);
                    newtimer = +1;



                }
            }

        }

        /*
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            oldtimer = newtimer;
            if (starty <= BoardHeight - 2)
            {
                if (newtimer > oldtimer || newtimer == oldtimer)
                {
                    RotationLock(startx, starty);
                    if (Tickdelay == 0)
                    {
                        Tickdelay += 1;
                        //Tick 1
                        UpdateLevelcount();
                        CheckCollision();
                        RotationLock(startx, starty);
                        placeblockcheck();
                        clearblock();

                        drawblock(startx, starty);
                        UpdateGameBoard();

                        if (starty == BoardHeight - 2)
                        {
                            placeblock();
                        }
                        RotationLock(startx, starty);
                    }
                    else if (Tickdelay == 1)
                    {
                        Tickdelay -= 1;
                        //Tick 2
                        Tickdelay = 0;

                        UpdateLevelcount();
                        clearblock();

                        if (starty <= BoardHeight - 1)
                        {
                            starty += 1;
                        }
                        drawblock(startx, starty);
                        UpdateGameBoard();
                        UpdateLevel();

                    }
                    else
                    {
                        Tickdelay = 0;
                    }
                    newtimer = +1;
                    oldtimer = newtimer;
                }
                else
                {

                    RotationLock(startx, starty);
                    newtimer = +1;
                    

                    
                }
            }
            
        } 
        */

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }


}
