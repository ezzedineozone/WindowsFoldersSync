using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class BackupScript
    {
        public readonly List<SaveSlot> slots;
        public BackupScript()
        {
            slots = new List<SaveSlot>();
        }
        public void AddSlot(string source, string destination)
        {
            int x = slots.Count + 1;
            string order = x.ToString();
            var saveSlot = new SaveSlot(source, destination, order); ;
            slots.Add(saveSlot);
        }
    }
}
