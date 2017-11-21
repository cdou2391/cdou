using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TagLib;
using System.Drawing;
using System.Media;
using System.Web.Services;

namespace Music_Player
{
    class AudioData
    {
        string lyricsPath = "http://api.chartlyrics.com/apiv1.asmx/";
        WebService lyricsSearch = new WebService();
        

        public string songTitle(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            FileInfo fileInfo = new FileInfo(songFile);
            string songT = song.Tag.Title;
            if (songT == null)
            {
                songT = fileInfo.Name;
            }
            return songT;
        }
        public string songArtist(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            string songA = song.Tag.FirstPerformer;
            if (songA == null)
            {
                songA = "Unknown Artist";
            }
            return songA;
        }
        public string songAlbum(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            var songAl = song.Tag.Album;
            if (songAl == null)
            {
                songAl = "Unknown Album";
            }
            return songAl;
        }
        public Bitmap songAlbumArt(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            IPicture  songArt = song.Tag.Pictures.FirstOrDefault();
            MemoryStream stream = new MemoryStream();
            Bitmap bm;
            Bitmap defaultImg = new Bitmap(@"A:\Documents\Visual Studio 2015\Projects\Music Player\Music Player\bin\Debug\images.jpg");
            if (songArt == null)
            {
                bm = defaultImg;
            }
            else
            {
                byte[] pData = songArt.Data.Data;
                stream.Write(pData, 0, Convert.ToInt32(pData.Length));
                bm = new Bitmap(stream, false);
            }
            return bm;
        }
        public string songGenre(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            String songG = song.Tag.FirstGenre;
            if (songG == null)
            {
                songG = "";
            }
            return songG;
        }
        public TimeSpan songLength(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            TimeSpan songL = song.Properties.Duration;
            if (songL == null)
            {
                songL = TimeSpan.Parse("") ;
            }
            return songL;
        }
        public string songYear(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            String songY = song.Tag.Year.ToString();
            if (songY == null)
            {
                songY = "";
            }
            return songY;
        }
        public string songLyrics(string songFile)
        {
            TagLib.File song = TagLib.File.Create(songFile);
            string songLrc = song.Tag.Lyrics;
            if (songLrc == null)
            {
                songLrc = "No Lyrics available for this song";
            }
            return songLrc;
        }
        public string dateModified(string songFile)
        {
            var songMod = System.IO.File.GetLastWriteTime(songFile);
            return songMod.ToString("yyyy/MM/dd");
        }
    }
}
