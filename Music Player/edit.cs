using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Music_Player
{
    public partial class edit : Form
    {
        public edit()
        {
            InitializeComponent();
        }
        AudioData dataS = new AudioData();
        GetFiless getFiles = new GetFiless();
        string sel = Form1.songSel;
        Form1 frm1 = new Form1();
        FileInfo fI = new FileInfo(Form1.songSel);
        
        public void loading_Load(object sender, EventArgs e)
        {
            if (sel!=null)
            {
                AudioData songTags = new AudioData();
                txtTitle.Text = songTags.songTitle(sel);
                txtArtist.Text = songTags.songArtist(sel);
                txtAlbum.Text = songTags.songAlbum(sel);
                txtGenre.Text = songTags.songGenre(sel);
                txtYear.Text = songTags.songYear(sel);
                picAlbum.Visible = true;
                picAlbum.Image = dataS.songAlbumArt(sel);

                label1.Text = "File: " + fI.Name ;
            }


        }
        private void btnExit_Click(object sender, EventArgs e)
        {

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditSongTags editTags = new EditSongTags();
            editTags.songTitle(sel, txtTitle.Text);
            string[] artistS = { txtArtist.Text };
            editTags.songArtist(sel, artistS);
            editTags.songAlbum(sel, txtAlbum.Text);
            string[] genreS = { txtGenre.Text };
            editTags.songGenre(sel, genreS);
            editTags.songYear(sel, UInt32.Parse(txtYear.Text));
            MessageBox.Show("New information Saved!");
            this.Hide();
        }
    }
}
