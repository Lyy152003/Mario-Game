using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GameMario
{
    public partial class Startgame : Form
    {

        // THIẾT LẬP CÁC NÚT DI CHUYỂN NHÂN VẬT TRÊN BÀN PHÍM
        private void Startgame_KeyDown(object sender, KeyEventArgs e) // thực thi khi nhấn bàn phím
        {
            if (e.KeyCode == Keys.Left) 
                goLeft = true;
            if (e.KeyCode == Keys.Right) 
                goRight = true;
            if (e.KeyCode == Keys.Space && Jumping == false) // chăn việc nhảy gâó hai ba lần
                Jumping = true;
        }

        private void Startgame_KeyUp(object sender, KeyEventArgs e) // thực thi khi nhấn thả phím
        {
            if (e.KeyCode == Keys.Left)
                goLeft = false;
            if (e.KeyCode == Keys.Right)
                goRight = false;
            if (Jumping == true)
                Jumping = false;
        }

        // CÁC LỆNH CỦA PLAYER (DI CHUYỂN, CÓ COIN, LOSE, WIN ....)
        //---player
        bool goLeft, goRight, Jumping, hasKey;
        int jumpSpeed = 8;
        int force = 5; // kiểm soát độ nhảy tối đa player nhảy
        int score = 0; int highscore;
        int playerSpeed = 10;
        int check = 0; // 0 - losse, 1 - win

        //---boom
        int boomSpeed = 5; // kiem soat tốc độ boom
        Random rnd = new Random(); // thiết lập thông số ngẫu nhiên 
        
        private void MainTimerEvent(object sender, EventArgs e)
        {

            txtScore.Text = "Score: " + score;
            player.Top += jumpSpeed;

            Boom1.Top += boomSpeed;
            Boom2.Top += boomSpeed;
            Boom3.Top += boomSpeed;
            Boom4.Top += boomSpeed;
            Boom5.Top += boomSpeed;
            Boom6.Top += boomSpeed;

            player.Refresh(); // giảm độ rung giật

            // liên kết với player
            if (goLeft == true && player.Left > 25)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true && player.Left + (player.Width + 60) < this.ClientSize.Width) // Xét coi độ rộng bên trái nhỏ hơn đọ rộng của ô khung hiện tại
            {
                player.Left += playerSpeed;
            }
            if (Jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 8;
            }
            if(Jumping == true && force < 0)
            {
                Jumping = false;
            }

            // liên kết với boom
            if (Boom1.Top + Boom1.Height > this.ClientSize.Height)
            {
                Boom1.Top = -450;
                Boom1.Left = rnd.Next(20, 700);

            }
            if (Boom2.Top + Boom2.Height > this.ClientSize.Height)
            {
                Boom2.Top = -650;
                Boom2.Left = rnd.Next(20, 700);

            }
            if (Boom3.Top + Boom3.Height > this.ClientSize.Height)
            {
                Boom3.Top = -750;
                Boom3.Left = rnd.Next(20, 700);

            }
            if (Boom4.Top + Boom4.Height > this.ClientSize.Height)
            {
                Boom4.Top = -450;
                Boom4.Left = rnd.Next(20, 700);

            }
            if (Boom5.Top + Boom5.Height > this.ClientSize.Height)
            {
                Boom5.Top = -650;
                Boom5.Left = rnd.Next(20, 700);

            }
            if (Boom6.Top + Boom6.Height > this.ClientSize.Height)
            {
                Boom6.Top = -750;
                Boom6.Left = rnd.Next(20, 700);

            }

            if (score == 10)
                boomSpeed = 10;
            if (score == 20)
                boomSpeed = 20;
            

            player.Refresh();
            // xét các sự kiện sẽ xảy ra của player (các đối tượng nào có nhiều thì sẽ dùng vòng lặp foreach)
            foreach (Control x in this.Controls)
            {
                if(x is PictureBox && (string) x.Tag == "platform")
                {
                    if(player.Bounds.IntersectsWith(x.Bounds))
                    {
                        force = 8;
                        player.Top = x.Top - player.Height;
                        jumpSpeed = 0;
                    }
                    x.BringToFront();
                }
                if (x is PictureBox && (string) x.Tag == "Coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score += 1;
                        MusicHaveCoin();
                    }
                }
                if (x is PictureBox && (string)x.Tag == "Monster")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        check = 0;
                        GameOver();
                    }
                }
                if (x is PictureBox && (string)x.Tag == "boom")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        check = 0;
                        GameOver();
                    }
                }

            }
            
            if (player.Bounds.IntersectsWith(Key.Bounds))
            {
                Key.Visible = false;
                hasKey = true;
            }
           
            if (player.Bounds.IntersectsWith(Door.Bounds) && hasKey == true)
            {
                check = 1;
                MarioWin.Visible = true;
                ptBPlayAgain.BringToFront();
                MarioWin.BringToFront();
                
                GameOver();

            }

            if (player.Top + player.Height > this.ClientSize.Height)
            {
                check = 0;
                GameOver();
                
            }

        }


        // LIST FUNTION GIAO DIỆN KHI CHƠI (NHẠC, RESTART, CLOSE,...)
        
        //Music
        private void MusicHaveCoin()
        {
            SoundPlayer s = new SoundPlayer(@"D:\GameMario\GameMario\Music\Nhac-chuong-mario-an-tien-www_tiengdong_com.wav");
            s.Play();
        }
        private void MusicLose()
        {
            SoundPlayer s = new SoundPlayer(@"D:\GameMario\GameMario\Music\Nhac-chuong-game-over-www_tiengdong_com.wav");
            s.Play();
        }
        private void MusicWin()
        {
            SoundPlayer s = new SoundPlayer(@"D:\GameMario\GameMario\Music\Am-thanh-chien-thang-www_tiengdong_com.wav");
            s.Play();
        }
       
       

        //chơi lại - thoát 
        private void Startgame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MarioWin_Click(object sender, EventArgs e)
        {

        }

        private void ptBPlayAgain_Click(object sender, EventArgs e)
        {
            force = 5;
            Jumping = false;
            jumpSpeed = 0;
            playerSpeed = 10;
            player.Location = new Point(80, 355);
            boomSpeed = 5;
            Boom1.Location = new Point(263,27);
            Boom2.Location = new Point(863, 27);
            Boom3.Location = new Point(1035, 27);
            Boom4.Location = new Point(27, 119);
            Boom5.Location = new Point(519, 99);
            Boom6.Location = new Point(919, 187);
            score = 0;
            txtScore.Text = "Score: " + score;
            Key.Visible = true;
            MarioWin.Visible = false;
            foreach (Control x in this.Controls)
            {

                if (x is PictureBox && (string)x.Tag == "Coin" || (string)x.Tag == "key")
                {
                    x.Visible = true;
                }
            }
            GameTimer.Start();
        }
       
        private void GameOver()
        {
            if(check == 0)
            {
                MusicLose();
                GameTimer.Stop();
            }
            else
            {
                MusicWin();
                GameTimer.Stop();
            }
            if (score > highscore)
            {
                highscore = score;
                txtHighScore.Text = "High Score: " + highscore;
            }
            
        }

        public Startgame()
        {
            InitializeComponent();
            //CHỈNH PHÔNG ẢNH
            ptB1.Controls.Add(player);
            player.BackColor = Color.Transparent;            
            //Key
            ptB1.Controls.Add(Key);
            Key.BackColor = Color.Transparent;
            //Door
            ptB1.Controls.Add(Door);
            Door.BackColor = Color.Transparent;
            //home
            ptB1.Controls.Add(Home);
            Home.BackColor = Color.Transparent;
            //RSG
            ptB1.Controls.Add(ptBPlayAgain);
            ptBPlayAgain.BackColor = Color.Transparent;
           
            /* boom
            ptB1.Controls.Add(Boom1);
            Boom1.BackColor = Color.Transparent;
            ptB1.Controls.Add(Boom2);
            Boom2.BackColor = Color.Transparent;
            ptB1.Controls.Add(Boom3);
            Boom3.BackColor = Color.Transparent;*/

        }


    }
}
