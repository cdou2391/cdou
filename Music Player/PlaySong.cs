using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WMPLib;
using System.Windows.Forms;

namespace Music_Player
{
    class PlaySong
    {
        AudioData dataS = new AudioData();
        public void songPlay(string songPath)
        {
            Form1 frm1 = new Form1();
            try
            {
                frm1.MediaPlayer1.URL = songPath;

                string[] songMetaData = { dataS.songTitle(songPath), dataS.songArtist(songPath), dataS.songLength(songPath).ToString(@"mm\:ss") };
                FileInfo fileInfo = new FileInfo(songPath);
                frm1.label1.Text = dataS.songTitle(songPath) + " - " + dataS.songArtist(songPath);

                frm1.richTextBox1.Enabled = true;
                frm1.richTextBox1.Text = dataS.songLyrics(songPath);

                frm1.pictureBox1.Visible = true;
                frm1.pictureBox1.Image = dataS.songAlbumArt(songPath);


                double currentPos = frm1.MediaPlayer1.Ctlcontrols.currentPosition;
                frm1.label3.Text = dataS.songLength(songPath).ToString(@"mm\:ss");
                frm1.btnPlay.Visible = false;
                frm1.btnPause.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please choose a song to play first \r\n" + ex.Message);
            }
        }
    }
}
