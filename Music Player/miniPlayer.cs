using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.Runtime.InteropServices;


namespace Music_Player
{
    public partial class miniPlayer : Form
    {
        public miniPlayer()
        {
            InitializeComponent();
        }
        AudioData dataS = new AudioData();
        GetFiless getFiles = new GetFiless();
        Form1 frm1 = new Form1();
        internal bool shuffle = Form1.shuffleOn;
        internal static int volume = Form1.volBeforeMute;
        WaveOut waveOutDevice = Form1.waveOutDevice;
        private void btnPrev_Click(object sender, EventArgs e)
        {

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            frm1.btnPlay.PerformClick();
        }
        
        private void miniPlayer_Load(object sender, EventArgs e)
        {
            //frm1.Visible = false;
            lblPlayingSong.Text = dataS.songTitle(Form1.songSel);
            compactModeToolStripMenuItem.Visible = false;
            if (shuffle == true)
            {
                pictureBox2.Visible = false;
                pictureBox3.Visible = true;
            }
            else
            {
                pictureBox2.Visible = true;
                pictureBox3.Visible = false;
            }
        }
        
        private void fullModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm1.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            waveOutDevice.Volume = (float)volume / 100;
            tBVolume.Value = volume;
            lblVol.Text = Convert.ToString("Vol: " + tBVolume.Value + "%");
            pictureBox4.Hide();
            pictureBox5.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            volume = tBVolume.Value;
            waveOutDevice.Volume = 0;
            tBVolume.Value = 0;
            lblVol.Text = Convert.ToString("Vol: " + tBVolume.Value + "%");
            pictureBox4.Show();
            pictureBox5.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            shuffle = true;
            pictureBox2.Visible = false;
            pictureBox3.Visible = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            shuffle = false;
            pictureBox2.Visible = true;
            pictureBox3.Visible = false;
        }
        public static class NativeMethods
        {
            //Winm WindowsSound
            [DllImport("winmm.dll")]
            internal static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
            [DllImport("winmm.dll")]
            internal static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        }
        private void tBVolume_Scroll(object sender, EventArgs e)
        {
            uint CurrVol;
            NativeMethods.waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            int NewVolume = ((ushort.MaxValue / 100) * tBVolume.Value);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            NativeMethods.waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            lblVol.Text = Convert.ToString("Vol: " + tBVolume.Value + "%");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            frm1.btnPrev.PerformClick();
        }
    }
}
