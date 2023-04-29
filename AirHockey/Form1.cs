/*
 * 
 * Koen Gerber
 * ICS3U
 * Mr T
 * April 29, 2023
 * 
 * Air Hockey Summative
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;

namespace AirHockey
{
    public partial class Form1 : Form
    {
        Rectangle puck = new Rectangle(280, 430, 40, 40);
        Rectangle border = new Rectangle(5, 5, 590, 890);
        Rectangle player1 = new Rectangle(275, 200, 50, 50);
        Rectangle player2 = new Rectangle(275, 650, 50, 50);

        SolidBrush whiteBrush = new SolidBrush (Color.White);
        SolidBrush blueBrush = new SolidBrush (Color.Blue);
        SolidBrush greenBrush = new SolidBrush(Color.LimeGreen);
        Pen redPen = new Pen(Color.Red, 10);
        Pen bluePen = new Pen(Color.Blue, 10);

        SoundPlayer augh = new SoundPlayer(Properties.Resources.aughSound);
        SoundPlayer boing = new SoundPlayer(Properties.Resources.boingSound);
        SoundPlayer hooray = new SoundPlayer(Properties.Resources.hooraySound);
        SoundPlayer score = new SoundPlayer(Properties.Resources.scoreSound);

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 10;
        int puckXSpeed = 0;
        int puckYSpeed = 0;

        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftDown = false;
        bool rightDown = false;


        int tickCounter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw screen
            e.Graphics.DrawArc(bluePen, 175, 325, 250, 250, 0, 360);
            e.Graphics.DrawLine(bluePen, 0, 450, 600, 450);
            e.Graphics.DrawRectangle (redPen, border);
            e.Graphics.DrawLine(bluePen, 200, 5, 400, 5);
            e.Graphics.DrawLine(bluePen, 200, 895, 400, 895);
            e.Graphics.FillPie(blueBrush, 200, -90, 200, 200, 0, 180);
            e.Graphics.FillPie(blueBrush, 200, 810, 200, 200, 180, 360);
            e.Graphics.FillRectangle(greenBrush, player1);
            e.Graphics.FillRectangle(greenBrush, player2);
            e.Graphics.FillEllipse(whiteBrush, puck);

            //update score
            player1ScoreLabel.Text = $"{player1Score}";
            player2ScoreLabel.Text = $"{player2Score}";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move puck
            puck.Y += puckYSpeed;
            puck.X += puckXSpeed;

            //move player 1 
            if (wDown == true && player1.Y > 10)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < 445 - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            if (aDown == true && player1.X > 10)
            {
                player1.X -= playerSpeed;
            }

            if (dDown == true && player1.X < this.Width - player1.Width - 10)
            {
                player1.X += playerSpeed;
            }

            //move player 2 
            if (upArrowDown == true && player2.Y > 455)
            {
                player2.Y -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height - 10)
            {
                player2.Y += playerSpeed;
            }

            if (leftDown == true && player2.X > 10)
            {
                player2.X -= playerSpeed;
            }

            if (rightDown == true && player2.X < this.Width - player2.Width - 10)
            {
                player2.X += playerSpeed;
            }

            //check if puck hit top or bottom wall and change direction if it does and check if it goes in goal
            if (puck.Y < 10)
            {
                puck.Y = 10;
                puckYSpeed = -puckYSpeed;

               if (puck.X >= 200 && puck.X + puck.Width <= 400)
                {
                    score.Play();
                    
                    player2Score++;

                    puck.X = 280;
                    puck.Y = 370;

                    ResetPositions();
                }
            }

            if (puck.Y > this.Height - puck.Height - 10)
            {
                puck.Y = this.Height - puck.Height - 10;
                puckYSpeed = -puckYSpeed;

                if (puck.X >= 200 && puck.X + puck.Width <= 400)
                {
                    score.Play();

                    player1Score++;

                    puck.X = 280;
                    puck.Y = 490;
                    
                    ResetPositions();
                }
            }

            //check if puck hit right wall or left wall and change direction
            if (puck.X + puck.Width > 590)
            {
                puck.X = 590 - puck.Width;
                puckXSpeed *= -1;
            }
            if (puck.X < 10)
            {
                puck.X = 10;
                puckXSpeed *= -1;
            }

            //check if puck hits either player. If it does change the direction 
            //and place the puck in front of the player hit 
            if (player1.IntersectsWith(puck))
            {
                augh.Play();

                int x = player1.X;
                int y = player1.Y;
                CheckCollisionDirection(x, y);
            }
            if (player2.IntersectsWith(puck))
            {
                boing.Play();
                
                int x = player2.X;
                int y = player2.Y;
                CheckCollisionDirection(x, y);
            }
            
            //slow puck down every ten ticks
            if (tickCounter % 10 == 0)
            {
                //slow down puck
                if (puckXSpeed > 0)
                {
                    puckXSpeed -= 1;
                }
                if (puckYSpeed > 0)
                {
                    puckYSpeed -= 1;
                }
            }
            tickCounter++;

            //check for winners
            if (player1Score == 3)
            {
                hooray.Play();
                
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player  1  Wins!!!";
            }
            else if (player2Score == 3)
            {

                hooray.Play();
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player  2  Wins!!!";
            }

            Refresh();

        }
        
        public void ResetPositions()
        {
            puckXSpeed = 0;
            puckYSpeed = 0;

            player1.Y = 200;
            player1.X = 275;
            player2.Y = 650;
            player2.X = 275;
        }

        public void CheckCollisionDirection(int playerX, int playerY)
        {

            //puck to left of paddle
            if (puck.X <= playerX && puck.Y <= playerY + player1.Height / 2 && puck.Y >= playerY - player1.Height / 2)
            {
                puck.X -= puck.Width;
                puckXSpeed = -10;
            }
            //puck to top of paddle
            if (puck.Y <= playerY && puck.X <= playerX + player1.Width / 2 && puck.X >= playerX - player1.Width / 2)
            {
                puck.Y -= puck.Height;
                puckYSpeed = -10;
            }
            //puck to right of paddle
            if (puck.X >= playerX && puck.Y <= playerY + player1.Height / 2 && puck.Y >= playerY - player1.Height / 2)
            {
                puck.X -= puck.Width;
                puckXSpeed = 10;
            }
            //puck to bottom of paddle
            if (puck.Y >= playerY && puck.X <= playerX + player1.Width / 2 && puck.X >= playerX - player1.Width / 2)
            {
                puck.Y += puck.Height;
                puckYSpeed = 10;
            }
        }
    }
}
