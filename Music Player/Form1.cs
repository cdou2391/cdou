using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using WMPLib;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using NAudio.Wave;
using System.Runtime.InteropServices;
using Music_Player.ChatLyrics;

namespace Music_Player
{
    public partial class Form1 : Form
    {
   
        public Form1()
        {
            InitializeComponent();
            this.Text = Application.ProductName + " " + Application.ProductVersion;
        }
        
        
        //Getting the classes needed
        AudioData dataS = new AudioData();
        GetFiless getFiles = new GetFiless();
        internal static List<string> files;
        internal static string songSel;
        bool shuffleOn = false;
        AudioFileReader reader;
        internal string songPath;

       
       
        WaveOut waveOutDevice = new WaveOut();
        Stopwatch watch1= new Stopwatch();

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Enabled=true;
            richTextBox1.Text = "Lyrics";

            pictureBox2.Visible = true;
            pictureBox3.Visible = false;
            //timer.Start();
            timer3.Interval = 1000;
            this.treeView1.Nodes.Add(TraverseDirectory(@"A:\Music\"));
            //timer.Stop()

            
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
            //loading the music files and displaying them in the listview
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                listView1.Items.Clear();
                files = getFiles.GetFiles(folderBrowserDialog1.SelectedPath);
                int x = 0;
                foreach (var fil in files)
                {
                    FileInfo fileInfo = new FileInfo(fil);
                    try
                    {
                        string[] songMetaData = { dataS.songTitle(fil), dataS.songArtist(fil) ,dataS.songAlbum(fil),
                            dataS.songLength(fil).ToString(@"mm\;ss"),dataS.songGenre(fil),dataS.songYear(fil),
                            dataS.dateModified(fil)};

                        listView1.Items.Add(fil).SubItems.AddRange(songMetaData);
                        x++;
                    }
                    catch (Exception)
                    {
                        listView1.Items.Add(fil).SubItems.Add(fileInfo.Name);
                        x++;
                    }
                    if (x % 1500 == 0)
                    {
                        MessageBox.Show(x + " songs have loaded so far!");
                    }
                    else if (x== files.Count())
                    {
                        MessageBox.Show("Done! All " + x + " songs have been loaded");
                    }
                }
            }
        }
        public void songPlay(string songPath1)
        {
            try
            {
                waveOutDevice.Dispose();
                
                reader = new AudioFileReader(songPath1);
                waveOutDevice.Init(reader);
                waveOutDevice.Play();
                timer3.Start();
                string[] songMetaData = { dataS.songTitle(songPath1), dataS.songArtist(songPath1), dataS.songLength(songPath1).ToString(@"mm\:ss") };
                FileInfo fileInfo = new FileInfo(songPath1);
                label1.Text = dataS.songTitle(songPath1) + " - " + dataS.songArtist(songPath1);

                richTextBox1.Enabled = true;
                richTextBox1.Text = dataS.songLyrics(songPath1);

                pictureBox1.Visible = true;
                pictureBox1.Image = dataS.songAlbumArt(songPath1);
                //label3.Text = dataS.songLength(songPath1).ToString(@"mm\:ss");

                waveOutDevice.Volume = (float)0.4;
                int vol = (int)(waveOutDevice.Volume * 100);
                trackBar1.Value = vol;
                lblVol.Text = Convert.ToString("Vol: " + trackBar1.Value + "%");

                btnPlay.Visible = false;
                btnPause.Visible = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Select a song to play first \r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                songPath = listView1.SelectedItems[0].Text;
                songPlay(songPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Select a song to play first \r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }

        private void toolStripPlay_Click(object sender, EventArgs e)
        {
            try
            {
                songPath = listView1.SelectedItems[0].Text;
                songPlay(songPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select a song to play first \r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void deleteFromFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Permanently delete the files selected
            string fileList = listView1.SelectedItems[0].Text;
            var userChoice = MessageBox.Show("Are you want to delete that file?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (userChoice == DialogResult.Yes)
            {
                System.IO.File.Delete(fileList);
                listView1.Items.Clear();
                List<string> files = getFiles.GetFiles(folderBrowserDialog1.SelectedPath);
                foreach (var fil in files)
                {
                    FileInfo fileInfo = new FileInfo(fil);
                    if (fil.Contains("copy"))
                    {
                        listView1.Items.Add(fil).SubItems.Add(fileInfo.Name);
                    }
                }
            }
        }

        private void deleteFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //delete the file selected from the list
            string fileList = listView1.SelectedItems[0].Text;
            var userChoice = MessageBox.Show("Are you want to delete that file?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (userChoice == DialogResult.Yes)
            {
                listView1.SelectedItems[0].Remove();
            }
        }
        private ColumnHeader SortingColumn = null;
        private void ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Get the new sorting column.
            ColumnHeader new_sorting_column = listView1.Columns[e.Column];

            // Figure out the new sorting order.
            System.Windows.Forms.SortOrder sort_order;
            if (SortingColumn == null)
            {
                // New column. Sort ascending.
                sort_order = SortOrder.Ascending;
            }
            else
            {
                // See if this is the same column.
                if (new_sorting_column == SortingColumn)
                {
                    // Same column. Switch the sort order.
                    if (SortingColumn.Text.StartsWith("> "))
                    {
                        sort_order = SortOrder.Descending;
                    }
                    else
                    {
                        sort_order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending.
                    sort_order = SortOrder.Ascending;
                }

                // Remove the old sort indicator.
                SortingColumn.Text = SortingColumn.Text.Substring(2);
            }

            // Display the new sort order.
            SortingColumn = new_sorting_column;
            if (sort_order == SortOrder.Ascending)
            {
                SortingColumn.Text = "> " + SortingColumn.Text;
            }
            else
            {
                SortingColumn.Text = "< " + SortingColumn.Text;
            }

            // Create a comparer.
            listView1.ListViewItemSorter =
                new ListViewItemComparer(e.Column, sort_order);

            // Sort.
            listView1.Sort();

        }
        class ListViewItemComparer: IComparer
        {
            private int ColumnNumber;
            private SortOrder SortOrder;

            public ListViewItemComparer(int column_number,SortOrder sort_order)
            {
                ColumnNumber = column_number;
                SortOrder = sort_order;
            }

            // Compare two ListViewItems.
            public int Compare(object object_x, object object_y)
            {
                // Get the objects as ListViewItems.
                ListViewItem item_x = object_x as ListViewItem;
                ListViewItem item_y = object_y as ListViewItem;

                // Get the corresponding sub-item values.
                string string_x;
                if (item_x.SubItems.Count <= ColumnNumber)
                {
                    string_x = "";
                }
                else
                {
                    string_x = item_x.SubItems[ColumnNumber].Text;
                }

                string string_y;
                if (item_y.SubItems.Count <= ColumnNumber)
                {
                    string_y = "";
                }
                else
                {
                    string_y = item_y.SubItems[ColumnNumber].Text;
                }

                // Compare them.
                int result;
                double double_x, double_y;
                if (double.TryParse(string_x, out double_x) &&
                    double.TryParse(string_y, out double_y))
                {
                    // Treat as a number.
                    result = double_x.CompareTo(double_y);
                }
                else
                {
                    DateTime date_x, date_y;
                    if (DateTime.TryParse(string_x, out date_x) &&
                        DateTime.TryParse(string_y, out date_y))
                    {
                        // Treat as a date.
                        result = date_x.CompareTo(date_y);
                    }
                    else
                    {
                        // Treat as a string.
                        result = string_x.CompareTo(string_y);
                    }
                }

                // Return the correct result depending on whether
                // we're sorting ascending or descending.
                if (SortOrder == SortOrder.Ascending)
                {
                    return result;
                }
                else
                {
                    return -result;
                }
            }
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            //pause the palying song
            btnPlay.Visible = true;
            btnPause.Visible =false;
            waveOutDevice.Pause();
            btnPlay.Focus();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            // play the selected song using the button play
            try
            {
                if (waveOutDevice.PlaybackState==PlaybackState.Paused)
                {
                     waveOutDevice.Resume();

                    btnPlay.Visible = false;
                    btnPause.Visible = true;
                    btnPause.Focus();
                }
                else
                {
                    songPath = listView1.SelectedItems[0].Text;
                    songPlay(songPath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Select a song to play first \r\n" + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }
        

        private void bntStop_Click(object sender, EventArgs e)
        {
            btnPlay.Visible = true;
            btnPause.Visible = false;
            waveOutDevice.Dispose();
            timer3.Stop();

            //double songLengthInSeconds = reader.TotalTime.TotalSeconds;
            //MessageBox.Show(songLengthInSeconds.ToString());
        }
        Random rnd = new Random();
       
        private void btnNext_Click(object sender, EventArgs e)
        {
            //play the next song on the list
            
            try
            {
                int rndIndex = rnd.Next(0, listView1.Items.Count - 1);
                btnPlay.Visible = false;
                btnPause.Visible = true;
                var nowSong = listView1.FindItemWithText(dataS.songTitle(songPath)); 
                int songIndex = listView1.Items.IndexOf(nowSong);
                listView1.Items[songIndex].Selected = false;

                if (songIndex == listView1.Items.Count-1)

                {
                    songIndex = 0;
                }
                else if (shuffleOn == true)
                {
                    songIndex = rndIndex;
                }
                else
                {
                    songIndex = songIndex + 1;
                }

                listView1.Items[songIndex].Selected = true;
                listView1.Select();
            
                string songPath1 = listView1.Items[songIndex].Text;
                songPlay(songPath1);
                songPath = songPath1;
            }
            catch (Exception)
            {
                btnPlay.Visible = true;
                btnPause.Visible = false;
            }

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            //play the previuos song on the list
            try
            {
                btnPlay.Visible = false;
                btnPause.Visible = true;
                var nowSong = listView1.FindItemWithText(dataS.songTitle(songPath));
                int songIndex = listView1.Items.IndexOf(nowSong);
                listView1.Items[songIndex].Selected = false;

                if (songIndex == 0)
                {
                    songIndex = listView1.Items.Count - 1;
                }
                else
                {
                    songIndex = songIndex - 1;
                }

                listView1.Items[songIndex].Selected = true;

                listView1.Select();
            
                string songPath1 = listView1.Items[songIndex].Text;
                songPlay(songPath1);
                songPath = songPath1;
            }
            catch (Exception)
            {
                btnPlay.Visible =true;
                btnPause.Visible = false;

            }
        }
        private bool spacePressed = false;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            //KeyEventArgs ke = e;
            if (e != null && e.KeyCode != Keys.Space)
            {
                spacePressed = false;
            }

            if (e.KeyCode == Keys.Space)
            {
                spacePressed = true;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (spacePressed == true)
            {
                e.Handled = true;
                //MediaPlayer2.Ctlcontrols.pause();
            }
        }

        private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
            //Exit the application
            Application.Exit();
        }
        
        ToolTip ttp1 = new ToolTip();

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            shuffleOn = true;
            pictureBox2.Visible = false;
            pictureBox3.Visible = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            shuffleOn = false;
            pictureBox2.Visible = true;
            pictureBox3.Visible = false;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            ttp1.Show("Turn shuffle off",pictureBox3);
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            ttp1.Show("Turn shuffle on", pictureBox2);
        }


        private TreeNode TraverseDirectory(string path)
        {
            TreeNode result = new TreeNode(path);
            try {
                foreach (var subdirectory in Directory.GetDirectories(path))
                {
                    result.Nodes.Add(TraverseDirectory(subdirectory));
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }

            return result;
        }
        
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string songPath = treeView1.SelectedNode.Text;
                listView1.Items.Clear();
                files = getFiles.GetFiles(songPath);
                int x = 0;
                foreach (var fil in files)
                {
                    FileInfo fileInfo = new FileInfo(fil);
                    try
                    {
                        string[] songMetaData = { dataS.songTitle(fil), dataS.songArtist(fil) ,dataS.songAlbum(fil),
                            dataS.songLength(fil).ToString(@"mm\;ss"),dataS.songGenre(fil),dataS.songYear(fil),
                            dataS.dateModified(fil)};

                        listView1.Items.Add(fil).SubItems.AddRange(songMetaData);
                        x++;
                    }
                    catch (Exception)
                    {
                        listView1.Items.Add(fil).SubItems.Add(fileInfo.Name);
                        x++;
                    }
                    if (x % 1500 == 0)
                    {
                        MessageBox.Show(x + " songs have loaded so far!");
                    }
                    else if (x == files.Count())
                    {
                        MessageBox.Show("Done! All " + x + " songs have been loaded");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nPlease select a valid folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void editPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit editFrm = new edit();
            
            songSel = listView1.SelectedItems[0].Text;
            editFrm.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void showNowPlayingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.Items)
            {
                if(txtSearch.Text.Length>0 && item.SubItems[1].Text.ToLower().StartsWith(txtSearch.Text.ToLower()))
                {
                    item.Selected = true;

                }
                else
                {
                    //item.Selected = false;
                    //item.BackColor = Color.Red;
                    //item.ForeColor = Color.Black;
                }
                if (listView1.SelectedItems.Count == 1)
                {
                    //listView1.Focus();
                }
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
        }
        // Manage the volume
        public static class NativeMethods
        {
            //Winm WindowsSound
            [DllImport("winmm.dll")]
            internal static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
            [DllImport("winmm.dll")]
            internal static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            uint CurrVol;
            NativeMethods.waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            int NewVolume = ((ushort.MaxValue / 100) * trackBar1.Value);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            NativeMethods.waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            lblVol.Text = Convert.ToString("Vol: " + trackBar1.Value + "%");
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (waveOutDevice != null)
            {
                try
                {
                    reader.CurrentTime = TimeSpan.FromSeconds(reader.TotalTime.TotalSeconds * trackBar2.Value / 100.0);
                }
                catch { }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            { label3.Text = ((int)reader.CurrentTime.Minutes).ToString("D02") + ":"
                        + ((int)reader.CurrentTime.Seconds).ToString("D02") + "/"
                        + reader.TotalTime.ToString(@"mm\:ss");

                int songLengthInSeconds = (int)reader.TotalTime.TotalSeconds;
                int songCurrentInSeconds = ((int)reader.CurrentTime.Minutes * 60) + (int)reader.CurrentTime.Seconds;
                trackBar2.Minimum = 0;
                trackBar2.Maximum = songLengthInSeconds;
                trackBar2.Value = songCurrentInSeconds;
            }
            catch(Exception)
            {
                btnNext.PerformClick();
            }
        }
        
    }
}
