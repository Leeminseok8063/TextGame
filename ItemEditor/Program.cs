using System.Text.Json;
public class WeaponInfo
{
    public string   Name    { get; set; }
    public int      Damage  { get; set; }
    public int      Value   { get; set; }
}
public class ArmorInfo
{
    public string Name      { get; set; }
    public int Defence      { get; set; }
    public int Value        { get; set; }
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

namespace GameEditor
{
    
    internal class Program
    {
        
        static void Main(string[] args)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Creation");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
  
            List<WeaponInfo>    Weapons     = new List<WeaponInfo>();
            List<ArmorInfo>     Armors      = new List<ArmorInfo>();
            List<MonsterInfo>   Monsters    = new List<MonsterInfo>();
            List<MapInfo>       Maps        = new List<MapInfo>();

            int flag = FlagCheck();
            Console.Clear();

            while (true)
            {
                switch (flag)
                {
                    case 1:
                        MakeWeapon();
                        break;
                    case 2:
                        MakeArmor();
                        break;
                    case 3:
                        MakeMap();
                        break;
                    case 4:
                        MakeMonster();
                        break;
                    case 5:
                        RenderInfo();
                        break;
                }
                Console.WriteLine("작업완료");
                Console.ReadLine();
                Console.Clear();

            }

            void    LoadAll()
            {
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
            void    RenderInfo()
            {
                Console.WriteLine("[방어구]");
                foreach(ArmorInfo item in Armors)
                   Console.Write(item.Name + " ");

                Console.WriteLine();
                Console.WriteLine("[무기]");
                foreach (WeaponInfo item in Weapons)
                   Console.Write(item.Name + " ");

                Console.WriteLine();
                Console.WriteLine("[몬스터]");
                foreach (MonsterInfo item in Monsters)
                    Console.Write(item.Name + " ");

                Console.WriteLine();
                Console.WriteLine("[맵]");
                foreach (MapInfo item in Maps)
                    Console.Write(item.Name + " ");

                Console.WriteLine();
            }        
            int     FlagCheck()
            {
                LoadAll();
                Console.Write("1.무기 생성\n2.방어구 생성\n3.맵 생성\n4.몬스터 생성\n5.전체 리스트 확인\n");
                return int.Parse(Console.ReadLine());
            }
            
            void MakeWeapon()
            {
                Console.WriteLine("아이템 이름을 입력하세요.");
                string name = Console.ReadLine();

                Console.WriteLine("공격력을 입력하세요.");
                int damage = int.Parse(Console.ReadLine());

                Console.WriteLine("가격을 입력하세요.");
                int value = int.Parse(Console.ReadLine());

                WeaponInfo item = new WeaponInfo { Name = name, Damage = damage, Value = value };
                Weapons.Add(item);

                string jsonString = JsonSerializer.Serialize(Weapons, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(path, "WeaponsInfo.json"), jsonString);
            }
            void MakeArmor()
            {
                Console.WriteLine("아이템 이름을 입력하세요.");
                string name = Console.ReadLine();

                Console.WriteLine("방어횟수를 입력하세요.");
                int def = int.Parse(Console.ReadLine());

                Console.WriteLine("가격을 입력하세요.");
                int value = int.Parse(Console.ReadLine());

                ArmorInfo item = new ArmorInfo { Name = name, Defence = def, Value = value };
                Armors.Add(item);

                string jsonString = JsonSerializer.Serialize(Armors, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(path, "ArmorsInfo.json"), jsonString);
            }
            void MakeMap()
            {
                Console.WriteLine("맵 이름을 입력하세요.");
                string name = Console.ReadLine();

                Console.WriteLine("맵 설명을 입력하세요.");
                string desc = Console.ReadLine();

                Console.WriteLine("맵 레벨 제한을 입력하세요.");
                int level = int.Parse(Console.ReadLine());

                MapInfo item = new MapInfo { Name = name, Description = desc, LevelLimit = level };
                Maps.Add(item);

                string jsonString = JsonSerializer.Serialize(Maps, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(path, "MapsInfo.json"), jsonString);
            }
            void MakeMonster()
            {
                Console.WriteLine("몬스터 이름을 입력하세요.");
                string name = Console.ReadLine();

                Console.WriteLine("공격력을 입력하세요.");
                int damage = int.Parse(Console.ReadLine());

                Console.WriteLine("공격횟수를 입력하세요.");
                int count = int.Parse(Console.ReadLine());

                Console.WriteLine("체력을 입력하세요.");
                int hp = int.Parse(Console.ReadLine());

                MonsterInfo item = new MonsterInfo { Name = name, Damage = damage, AttackCount = count, Health = hp };
                Monsters.Add(item);

                string jsonString = JsonSerializer.Serialize(Monsters, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(path, "MonstersInfo.json"), jsonString);
            }

        }
    }
}
