using System.Text.Json;
using TextRPG.Manager;

namespace TextRPG.Object
{
   
    internal class Player
    {

        public void Init()
        {
            Inventory = new List<Item>();
            Sinven = new List<string>();        
        }
        
        public void PushItem(Item item)
        {
            Inventory.Add(item);
        }//인벤토리에 추가
        public void SellItem(int index)
        {
            if (Inventory[index] == null)
                return;

            money += (int)(Inventory[index].value * 0.5f);
            Inventory[index] = null;
            Inventory.RemoveAt(index);
        }//이벤토리에서 판매
        public void SwapItem(int index)
        {
            Item temp = Inventory[index];
            if (Inventory[index] == null)
                return;

            if (Inventory[index].type == ITEMTYPE.WEAPON)
            {
                Inventory[index] = weapon;
                weapon = temp;
            }
            else
            {
                Inventory[index] = armor;
                armor = temp;
            }

            if (Inventory[index] == null)
                Inventory.RemoveAt(index);
        }//인벤토리에서 교체
        
        public void Save()
        {
            if(weapon != null)
                Sweapon = weapon.name;
            if(armor != null)
                Sarmor = armor.name;
            if(Inventory.Count != 0)
                foreach (Item item in Inventory)
                    Sinven.Add(item.name);

            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(Path.Combine(FileManager.Instance().path, "Player.json"), jsonString);
        }
        public void Load()
        {
            string jsonstring = File.ReadAllText(Path.Combine(FileManager.Instance().path, "Player.json"));
            Player player = JsonSerializer.Deserialize<Player>(jsonstring);

            this.name       = player.name;
            this.job        = player.job;
            this.health     = player.health;
            this.maxhealth  = player.maxhealth;
            this.shield     = player.shield;
            this.damage     = player.damage;
            this.money      = player.money;
            this.level      = player.level;
            this.accuracy   = player.accuracy;

            weapon = FileManager.Instance().GetItem(player.Sweapon);
            armor = FileManager.Instance().GetItem(player.Sarmor);
            foreach (string str in player.Sinven)
                Inventory.Add(FileManager.Instance().GetItem(str));
        } 

        public List<Item> Inventory;
        public Item weapon = null;
        public Item armor  = null;

        public string       name      { get; set; }
        public string       Sweapon   { get; set; }
        public string       Sarmor    { get; set; }
        public List<string> Sinven    { get; set; }

        public JOBTYPE job      { get; set; }
        public int health       { get; set; }
        public int maxhealth    { get; set; }
        public int shield       { get; set; }
        public int damage       { get; set; }
        public int money        { get; set; }
        public int level        { get; set; }
        public float accuracy   { get; set; }   

    }

    
}
