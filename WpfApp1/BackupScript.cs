using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class BackupScript
    {
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
    }
}
