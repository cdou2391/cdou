using System;
using System.Collections.Generic;
using System.IO;

namespace Music_Player
{
    class GetFiless
    {
        public List<string> GetFiles(string path)
        {
            var files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path, "*.m4a", SearchOption.AllDirectories));
                files.AddRange(Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories));
                files.AddRange(Directory.GetFiles(path, "*.wma", SearchOption.AllDirectories));
            }
            catch (UnauthorizedAccessException) { }

            return files;

        }
    }
}
