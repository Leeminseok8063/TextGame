using System.Text.Json;
using TextRPG.Object;

public class WeaponInfo
{
    public string Name  { get; set; }
    public int Damage   { get; set; }
    public int Value    { get; set; }
}
public class ArmorInfo
{
    public string Name  { get; set; }
    public int Defence  { get; set; }
    public int Value    { get; set; }
}
public class MapInfo
{
    public string Name          { get; set; }
    public string Description   { get; set; }
    public int LevelLimit       { get; set; }
}
public class MonsterInfo
{
    public string Name      { get; set; }
    public int Damage       { get; set; }
    public int AttackCount  { get; set; }
    public int Health       { get; set; }
}

namespace TextRPG.Manager
{
    class FileManager
    {
        public static FileManager Instance()
        {
            if(instance == null)
                instance = new FileManager();
            return instance;
        }
        public void Init()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (File.Exists(path + "\\WeaponsInfo.json"))
            {
                string jsonstring = File.ReadAllText(Path.Combine(path, "WeaponsInfo.json"));
                Weapons = JsonSerializer.Deserialize<List<WeaponInfo>>(jsonstring) ?? new List<WeaponInfo>();
            }
            if (File.Exists(path + "\\ArmorsInfo.json"))
            {
                string jsonstring = File.ReadAllText(Path.Combine(path, "ArmorsInfo.json"));
                Armors = JsonSerializer.Deserialize<List<ArmorInfo>>(jsonstring) ?? new List<ArmorInfo>();
            }
            if (File.Exists(path + "\\MapsInfo.json"))
            {
                string jsonstring = File.ReadAllText(Path.Combine(path, "MapsInfo.json"));
                Maps = JsonSerializer.Deserialize<List<MapInfo>>(jsonstring) ?? new List<MapInfo>();
            }
            if (File.Exists(path + "\\MonstersInfo.json"))
            {
                string jsonstring = File.ReadAllText(Path.Combine(path, "MonstersInfo.json"));
                Monsters = JsonSerializer.Deserialize<List<MonsterInfo>>(jsonstring) ?? new List<MonsterInfo>();
            }
        }
        
        public Item             GetItem(string name)
        {
            foreach (WeaponInfo i in Weapons)
            {
                if (i.Name == name)
                {
                    Item item = new Item
                    {
                        type = ITEMTYPE.WEAPON,
                        name = i.Name,
                        damage = i.Damage,
                        value = i.Value,
                    };

                    return item;
                }
            }
            foreach (ArmorInfo i in Armors)
            {
                if (i.Name == name)
                {
                    Item item = new Item
                    {
                        type = ITEMTYPE.ARMOR,
                        name = i.Name,
                        armor = i.Defence,
                        value = i.Value,
                    };

                    return item;
                }
            }
            return null;
        }
        public List<MapInfo>    GetMapInfo()
        {
            return Maps;
        }           
        public List<Item>       GetItemRange(ITEMTYPE type, int min, int max)
        {

            List<Item> items = new List<Item>();
            if (type == ITEMTYPE.WEAPON)
            {
                foreach (WeaponInfo item in Weapons)
                {
                    if (item.Damage >= min && item.Damage <= max)
                    {
                        Item itemtemp = new Item();
                        itemtemp.type = type;
                        itemtemp.name = item.Name;
                        itemtemp.damage = item.Damage;
                        itemtemp.value = item.Value;
                        items.Add(itemtemp);
                    }
                }
            }
            else if (type == ITEMTYPE.ARMOR)
            {
                foreach (ArmorInfo item in Armors)
                {
                    if (item.Defence > min && item.Defence <= max)
                    {
                        Item itemtemp = new Item();
                        itemtemp.type = type;
                        itemtemp.name = item.Name;
                        itemtemp.armor = item.Defence;
                        itemtemp.value = item.Value;
                        items.Add(itemtemp);
                    }
                }
            }
            return items;
        }
        public List<Monster>    GetMonsterRange(int min, int max)
        {
            List<Monster> mobs = new List<Monster>();
            foreach (MonsterInfo monster in Monsters)
            {
                if (monster.Damage >= min && monster.Damage <= max)
                    mobs.Add(new Monster { Name = monster.Name, AttackCount = monster.AttackCount, Damage = monster.Damage, Health = monster.Health });
            }

            return mobs;
        }

        List<WeaponInfo> Weapons    = new List<WeaponInfo>();
        List<ArmorInfo> Armors      = new List<ArmorInfo>();
        List<MonsterInfo> Monsters  = new List<MonsterInfo>();
        List<MapInfo> Maps          = new List<MapInfo>();
        
        public string path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private static FileManager instance;
    }
}
