using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using System.IO;

namespace Music_Player
{
    
    class EditSongTags
    {
        AudioData songData = new AudioData();
        public void songTitle(string song,string newTitle)
        {
            TagLib.File f = TagLib.File.Create(song);
            f.Tag.Title = newTitle;
            f.Save();
        }
        public void songArtist(string song, string[] newArtist)
        {
            TagLib.File f = TagLib.File.Create(song);
            f.Tag.Performers= newArtist;
            f.Save();
        }
        public void songAlbum(string song, string newAlbum)
        {
            TagLib.File f = TagLib.File.Create(song);
            f.Tag.Album = newAlbum;
            f.Save();
        }
        public void songGenre(string song, string[] newGenre)
        {
            TagLib.File f = TagLib.File.Create(song);
            f.Tag.Genres = newGenre;
            f.Save();
        }
        public void songYear(string song, UInt32 newYear)
        {
            TagLib.File f = TagLib.File.Create(song);
            f.Tag.Year = newYear;
            f.Save();
        }
    }
}
