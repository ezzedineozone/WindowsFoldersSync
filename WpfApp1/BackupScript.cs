using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    internal class BackupScript
    {
        public event EventHandler<string> BackupInitiatedEvent;
        public List<SaveSlot> slots;
        public BackupScript()
        {
            slots = new List<SaveSlot>();
        }
        public void AddSlot(SaveSlot saveSlot)
        {
            int x = slots.Count + 1;
            string order = x.ToString();
            saveSlot.Order = order;
            slots.Add(saveSlot);
        }
        public void BeginBackup(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            var formattedTime = dateTime.ToString().Split(' ')[0];
            BackupInitiatedEvent?.Invoke(this, formattedTime);

            foreach (var slot in slots)
            {
                var source = slot.Source;
                var destination = slot.Destination;
                if(Directory.Exists(source) && Directory.Exists(destination))
                {
                    Directory.Delete(destination, true);
                    var directories = GetDirectories(source);
                    var files = GetFiles(directories);
                    foreach (var dir in directories)
                    {
                        Directory.CreateDirectory($@"{destination}\{dir.Substring(source.Length)}");
                    }
                    foreach (var file in files)
                    {
                        File.Copy(file, @$"{destination}\{file.Substring(source.Length)}");
                    }
                }
                else
                {
                    MessageBox.Show("One of the directories you have entered does not exist");
                }
            }
        }
        public List<string> GetDirectories(string source)
        {
            var directories = new List<string>();
            var root = Directory.GetDirectories(source);
            if(root.Length != 0)
            {
                foreach(var directory in root)
                {
                    directories.Add(directory);
                    foreach(var dir in GetDirectories(directory))
                    {
                        directories.Add(dir);
                    }
                }
            }
            return directories;
        }
        public List<string> GetFiles(List<string> directories)
        {
            var files = new List<string>();
            foreach (var dir in directories)
            {
                foreach(var file in Directory.GetFiles(dir))
                {
                    files.Add(file);
                }
            }
            return files;
        }
    }
}
