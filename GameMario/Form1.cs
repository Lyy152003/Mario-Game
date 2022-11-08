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

namespace GameMario
{

    public partial class fLogin : Form
    {
        SoundPlayer s = new SoundPlayer(@"D:\GameMario\GameMario\Music\y2meta.com-Game-of-Thrones-8-bit-_128-kbps_.wav");
        public fLogin()
        {
            s.Play();
            InitializeComponent();
        }
       

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thật sự muốn thoát trò chơi ?", "Thông báo!" ,MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            s.Stop();
            this.Hide();
            Startgame f = new Startgame();
            f.ShowDialog();
            this.Show();
        }
       
    }
}
