using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.PlayeInventory
{
    public class PlayerInventoryInfo
    {
        public string steamid;

        public int entityid;

        public string playername;

        public IList<ItemData> bag;

        public IList<ItemData> belt;

        public Equipment equipment;
    }

    public class Equipment
    {
        public ItemData head;

        public ItemData eyes;

        public ItemData face;

        public ItemData armor;

        public ItemData jacket;

        public ItemData shirt;

        public ItemData legarmor;

        public ItemData pants;

        public ItemData boots;

        public ItemData gloves;
    }
}
