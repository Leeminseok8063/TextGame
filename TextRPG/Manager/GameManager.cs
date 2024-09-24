using TextRPG.Object;

namespace TextRPG.Manager
{
    //ENUMS
    enum MAPTYPE
    {
        NONE,
        MAIN,
        STORE,
        FILED,
        IMFORMATION,
        INVEN,
        REST,
        EXIT,


    }
    enum JOBTYPE
    {
        NONE,
        WARRIOR,
        KNIGHT,
        BIKING,
        ASSASIN,
    }
    enum PlayerState
    {
        DAMAGE,
        ARMOR,
    }

    internal class GameManager
    {
        public static GameManager Instance()
        {
            if(instance == null)
                instance = new GameManager();
            return instance;
        }

        //MainSection
        void CreatePlayer()
        {
            Player player = new Player();
            Console.WriteLine("[저장 되어있는 캐릭터가 없습니다, 캐릭터 생성을 시작합니다.]");
            Console.Write("이름 : ");
            while (player.name == null) { player.name = Console.ReadLine();}
                
            Console.Clear();
            Console.WriteLine("이름 : " + player.name);
            Console.WriteLine("==============직업 선택================");
            Console.WriteLine("1.광전사  : 무시무시한 근력으로 적을 쪼개버리지만 낮은 정확도를 가진다.");
            Console.WriteLine("2.기사    : 균형잡힌 전투기술로 많은적을 상대하는 군인.");
            Console.WriteLine("3.바이킹  : 민첩하게 움직여 빠른속도로 적을 타격한다.");
            Console.WriteLine("4.암살자  : 엄청난 정확도로 적을 공격하지만 약한 공격을 가한다.");

            int flag = GetKey();
            switch (flag)
            {
                case 1: 
                    player.job = JOBTYPE.WARRIOR;
                    player.health = 20;
                    player.maxhealth = player.health;
                    player.damage = 20;
                    player.accuracy = 0.2f;
                    break;
                case 2: 
                    player.job = JOBTYPE.KNIGHT;
                    player.health = 25;
                    player.maxhealth = player.health;
                    player.damage = 8;
                    player.accuracy = 0.5f;
                    break;
                case 3: 
                    player.job = JOBTYPE.BIKING;
                    player.health = 15;
                    player.maxhealth = player.health;
                    player.damage = 5;
                    player.accuracy = 0.8f;
                    break;
                case 4: 
                    player.job = JOBTYPE.ASSASIN;
                    player.health = 10;
                    player.maxhealth = player.health;
                    player.damage = 2;
                    player.accuracy = 1f;
                    break;
            }

            player.maxhealth = player.health;
            player.money = 1000;
            player.level = 1;

            player.Init();
            player.Save();
            NowPlayer = player;
        }
        void FirstPlayCheck()
        {
            if(File.Exists(FileManager.Instance().path + "\\Player.json"))
            {
                Player player = new Player();
                player.Init();
                player.Load();
                NowPlayer = player;
                return;
            }

            CreatePlayer();
        }            
        public void Init()
        {
            FirstPlayCheck();
            Monsters = new List<Monster>();
        }
        public void Update()
        {
            switch(NowMap)
            {
                case MAPTYPE.MAIN:
                    MainScene();
                    break;
                case MAPTYPE.IMFORMATION:
                    ImformationScene();
                    break;
                case MAPTYPE.INVEN:
                    InvenScene();
                    break;
                case MAPTYPE.STORE:
                    StoreScene();
                    break;
                case MAPTYPE.FILED:
                    FiledScene();
                    break;
                case MAPTYPE.REST:
                    RestScene();
                    break;
                case MAPTYPE.EXIT:
                    NowPlayer.Save();
                    Environment.Exit(0);
                    break;
                    
            } 
        }      
        
        //VillageContents
        void MainScene()
        {
            isBattle = false;

            Console.Clear();
            Console.WriteLine("===========================================================");
            Console.WriteLine("세계의 중앙 제국의 해안 도시에서 입장했습니다");
            Console.WriteLine("바다의 향기와 사람들의 웅성이는 소리가 가득찹니다.");
            Console.WriteLine("===========================================================");
            Console.WriteLine("[1]:상태 [2]:가방 [3]:상점 [4]:모험 [5]여관 [6]저장 및 종료");

            int key = GetKey();
            if (key == 1)
                NowMap = MAPTYPE.IMFORMATION;
            else if (key == 2)
                NowMap = MAPTYPE.INVEN;
            else if (key == 3)
                NowMap = MAPTYPE.STORE;
            else if (key == 4)
                NowMap = MAPTYPE.FILED;
            else if (key == 5)
                NowMap = MAPTYPE.REST;
            else if (key == 6)
                NowMap = MAPTYPE.EXIT;          
            
        }
        void ImformationScene()
        {
            Console.Clear();
            Console.WriteLine("===========================================================");
            Console.WriteLine("이름         : " + NowPlayer.name);
            Console.WriteLine("직업         : " + NowPlayer.job);
            Console.WriteLine("레벨         : " + NowPlayer.level);
            Console.WriteLine("생명력(최대) : " + NowPlayer.maxhealth);
            Console.WriteLine("생명력(일반) : " + NowPlayer.health);
            Console.WriteLine("방어력       : " + NowPlayer.shield + " + " + (NowPlayer.armor != null ? NowPlayer.armor.armor + " (" + NowPlayer.armor.name + ")":"0 (방어구 없음)"));
            Console.WriteLine("공격력       : " + NowPlayer.damage + " + " + (NowPlayer.weapon != null ? NowPlayer.weapon.damage + " (" + NowPlayer.weapon.name + ")" : "0 (무기 없음)"));
            Console.WriteLine("소지금       : " + NowPlayer.money);
            Console.WriteLine("명중률       : " + NowPlayer.accuracy);
            Console.WriteLine("===========================================================");
            Console.WriteLine("[0]:종료");

            if (GetKey() == 0)
                NowMap = MAPTYPE.MAIN;

        }
        void InvenScene()
        {
            Console.Clear();
            List<Item> items = NowPlayer.Inventory;
            int index = 1;
            Console.WriteLine("===========================================================");
            Console.WriteLine("[현재 장비 현황]");
            Console.WriteLine("[무기] : " + 
                (NowPlayer.weapon != null ? NowPlayer.weapon.name : "X") + " [방어구] : " + 
                (NowPlayer.armor != null ? NowPlayer.armor.name : "X"));
            foreach(Item item in items)
            {
                Console.WriteLine("[" + index + "]" + (item != null ? item.name : "빈칸"));
                index++;
            }
            Console.WriteLine("===========================================================");
            Console.WriteLine("[0]:종료 [번호]:장착");
            int input = GetKey();
            if(input == 0)
            {
                NowMap = MAPTYPE.MAIN;
                return;
            }

            if(input <= NowPlayer.Inventory.Count)
                NowPlayer.SwapItem(input - 1);
        }
        void StoreScene()
        {
            List<Item> weapons = FileManager.Instance().GetItemRange
                (
                    ITEMTYPE.WEAPON,
                    0,
                    NowPlayer.level + 10
                );
            List<Item> armors = FileManager.Instance().GetItemRange
                (
                    ITEMTYPE.ARMOR,
                    0,
                    NowPlayer.level + 10
                );
            Console.Clear();
            Console.WriteLine("[상점]" + " [나의 소지금:" + NowPlayer.money + "]");
            Console.WriteLine("[레벨이 높아 질 수록 더 많은 아이템이 생성됩니다.]");
            Console.WriteLine("====================================");
            foreach( Item item in weapons)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("▶" + item.name);
                Console.ResetColor();
                Console.WriteLine
               (" [공격력:" + item.damage + "]" + " [가격:" + item.value + "]");
            }
            Console.WriteLine("====================================");
            foreach (Item item in armors)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("▶" + item.name);
                Console.ResetColor();
                Console.WriteLine
                (" [내구도:" + item.armor + "]" + " [가격:" + item.value + "]");
                
            }
            Console.WriteLine("====================================");
            Console.WriteLine("[0]:종료 [아이템이름]:구입 [SELL]:판매모드"); 
            Console.ForegroundColor= ConsoleColor.Green;
            Console.Write(">> ");
            Console.ResetColor();

            string input = Console.ReadLine();
            if(input == "0")
            {
                NowMap = MAPTYPE.MAIN;
                return;
            }
            if(input == "SELL")
            {
                while(true)
                {
                    Console.Clear();
                    List<Item> items = NowPlayer.Inventory;
                    int index = 1;
                    Console.WriteLine("===========================================================");
                    Console.WriteLine("[아이템 판매]" + " [소지금:" + NowPlayer.money + "]");
                    foreach (Item item in items)
                    {
                        Console.WriteLine("[" + index + "]" + (item != null ? item.name : "빈칸"));
                        index++;
                    }
                    Console.WriteLine("===========================================================");
                    Console.WriteLine("[0]:종료 [번호]:판매");
                    int inputselling = GetKey();
                    if (inputselling == 0) return;                    
                    NowPlayer.SellItem(inputselling - 1);
                }
                
            }
            //CHEAT
            if(input == "MONEY_HONEY")
            {
                NowPlayer.money += 99999;
            }
            if (input == "I_AM_GROOT")
            {
                LevelUp();
            }

            Item tempitem = FileManager.Instance().GetItem(input);
            if (tempitem == null) return;
            if (tempitem.value > NowPlayer.money) return;

            NowPlayer.money -= tempitem.value;
            NowPlayer.Inventory.Add(tempitem);
            Console.WriteLine("가방에 " + tempitem.name + " 이(가) 추가 되었습니다. [Enter..]");
            Console.ReadLine();
        }
        void RestScene()
        {
            Console.Clear();
            Console.WriteLine("===========================================================");
            Console.WriteLine("평화로운 여관에서 하루 동안 휴식을 취하면 20만큼의 체력을 회복합니다");
            Console.WriteLine("[하루 : 50골드] [현재 소지금 : " + NowPlayer.money + "] [현재 생명력 : " + NowPlayer.health + "]");
            Console.WriteLine("===========================================================");
            Console.WriteLine("[원하는 일 수]:구입 [0]:종료");
            int input = GetKey();
            int res = input * 50;

            if (input == 0)
            {
                NowMap = MAPTYPE.MAIN;
                return;
            }
            if (NowPlayer.money < res)
            {
                Console.WriteLine("금액이 부족합니다. [Enter..]");
                Console.ReadLine();
                return;
            }
            NowPlayer.money -= 50;
            NowPlayer.health = (int)MathF.Min(NowPlayer.maxhealth, NowPlayer.health + input * 20);
            Console.WriteLine("휴식을 취했습니다. [Enter..]");
            Console.ReadLine();
        }

        //BattleContents
        void FiledScene()
        {
            
            if(!isBattle)
            {
                int index = 0;
                Console.Clear();
                List<MapInfo> maps = FileManager.Instance().GetMapInfo();
                foreach(MapInfo map in maps)
                {
                    Console.WriteLine("[" + index + "]" + ":" + map.Name + " [RECOMMEND LEVEL] : " + map.LevelLimit);
                    Console.WriteLine("INFO : " + map.Description + "\n");
                    index++;
                }

                int input = GetKey();
                if (input > index)
                    return;

                isBattle = true;
                StageDiff = input;
                BattleSetting();
            }
            else
            {
                UpdateBattleUI();
                int input = GetKey();
                if(input == 2)
                {
                    Console.WriteLine("전투에서 도주했습니다.  [Enter..]");
                    Console.ReadLine();
                    NowMap = MAPTYPE.MAIN;
                    return;
                }
                else if (input == 1)
                {
                    Battle();
                }
            }
        }
        void BattleSetting()
        {
            Random random = new Random();
            Monsters.Clear();
            MobCount = random.Next(2, 10);

            switch (StageDiff)
            {
                //Pls Init Map Setting!
                case 0:
                    Monsters = FileManager.Instance().GetMonsterRange(1, 2);
                    reward = random.Next(100, 300);
                    break;
                case 1:
                    Monsters = FileManager.Instance().GetMonsterRange(2, 10);
                    reward = random.Next(1000, 5000);
                    break;
                case 2:
                    Monsters = FileManager.Instance().GetMonsterRange(11, 15);
                    reward = random.Next(5000, 10000);
                    break;
                case 3:
                    Monsters = FileManager.Instance().GetMonsterRange(16, 22);
                    reward = random.Next(50000, 100000);
                    break;
                case 4:
                    Monsters = FileManager.Instance().GetMonsterRange(24, 30);
                    reward = random.Next(100000, 200000);
                    break;
                case 5:
                    Monsters = FileManager.Instance().GetMonsterRange(32, 40);
                    reward = random.Next(200000, 400000);
                    break;


            }

            foreach (Monster mob in Monsters)
                mob.MaxHealth = mob.Health;

            NowMonster = Monsters[random.Next(0, Monsters.Count)];
        }
        void Battle()
        {

            Random rand = new Random();
            int playerDamage = GetPlayerState()[(int)PlayerState.DAMAGE];
            int playerArmor = GetPlayerState()[(int)PlayerState.ARMOR];

            //attack
            if (rand.Next(0, 1000) / 1000f > NowPlayer.accuracy)
            {
                Console.WriteLine(NowMonster.Name + "이(가) 당신의 공격을 피했습니다! [Enter..]");
                Console.ReadLine();
            }
            else
            {
                NowMonster.Health -= playerDamage;
                Console.WriteLine(NowMonster.Name + "에게 " + playerDamage + "만큼의 피해를 주었습니다. [Enter..]");
                Console.ReadLine();
            }

            //damaged
            if (playerArmor > 0 && NowPlayer.armor != null)
            {
                NowPlayer.armor.armor -= 1;
                Console.WriteLine("방어구 " + NowPlayer.armor.name + "(이)가 당신을 보호합니다. [Enter..]");
                if (NowPlayer.armor.armor <= 0)
                {
                    Console.WriteLine(NowPlayer.armor.name + "(이)가 많이 손상되어 파괴되었습니다. [Enter..]");
                    NowPlayer.armor = null;
                }
                Console.ReadLine();
            }
            else
            {
                float hit = rand.Next(0, 1000) / 1000f;
                if (hit > 0.5f)
                {
                    NowPlayer.health -= NowMonster.Damage;
                    Console.WriteLine("당신은 " + NowMonster.Damage + "만큼 피해를 입었습니다! [Enter..]");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("몬스터 " + NowMonster.Name + "의 공격이 빗나갔습니다! [Enter..]");
                    Console.ReadLine();
                }

            }

            //resault
            if (NowPlayer.health <= 0)
            {
                Console.Clear();
                Console.WriteLine("당신은 패배했습니다.");
                NowPlayer.money = NowPlayer.money / 2;
                NowPlayer.health = NowPlayer.maxhealth;
                Console.WriteLine("[총 " + NowPlayer.money / 2 + "만큼의 골드를 잃었습니다.]");
                Console.WriteLine("마을로 이동합니다.. [Enter..]");
                Console.ReadLine();
                NowMap = MAPTYPE.MAIN;
                return;
            }

            //kill mob
            if (NowMonster.Health <= 0)
            {
                NowMonster.Health = NowMonster.MaxHealth;

                Console.WriteLine("몬스터를 처지했습니다! [Enter..]");
                Console.ReadLine();
                NowMonster = Monsters[rand.Next(0, Monsters.Count)];
                --MobCount;

                if (MobCount <= 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("모든 적을 처치했습니다");
                    Console.ResetColor();
                    LevelUp();
                    Console.WriteLine("[총 " + reward + " 만큼의 보물을 발견했습니다!]");
                    Console.WriteLine("마을로 이동합니다.. [Enter..]");
                    Console.ReadLine();

                    NowPlayer.money += reward;
                    NowMap = MAPTYPE.MAIN;
                    return;
                }
            }

        }
        void UpdateBattleUI()
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("이름 : " + NowPlayer.name + " 체력 : " + NowPlayer.health);
            Console.WriteLine("공격력 : " + GetPlayerState()[(int)PlayerState.DAMAGE] + 
                              " 방어구 내구도: " + GetPlayerState()[(int)PlayerState.ARMOR]);
            Console.WriteLine("====================================");
            Console.WriteLine("                 vs                 ");
            Console.WriteLine("====================================");
            Console.WriteLine("이름 : " + NowMonster.Name + " 체력 : " + NowMonster.Health);
            Console.WriteLine("공격력 : " + NowMonster.Damage + " 공격 가능 횟수 : " + NowMonster.AttackCount);
            Console.WriteLine("남은 적 : " + MobCount);
            Console.WriteLine("====================================");
            Console.WriteLine("[1]:공격 [2]:도망");
        }
        
        //Utils
        int[] GetPlayerState()
        {
            int[] stat = new int[2];
            stat[0] = NowPlayer.damage + (NowPlayer.weapon != null ? NowPlayer.weapon.damage : 0);
            stat[1] = NowPlayer.shield + (NowPlayer.armor != null ? NowPlayer.armor.armor : 0);
            return stat;
        }
        int GetKey()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(">> ");
            int temp = 0;
            while (!int.TryParse(Console.ReadLine(), out temp))
            { Console.Write("잘못된 입력입니다 다시.. >> "); }
            Console.ResetColor(); ;

            return temp;
        }     
        void LevelUp(int level = 1, int damage = 1, int maxheal = 5)
        {
            NowPlayer.level     += level;
            NowPlayer.damage    += damage;
            NowPlayer.maxhealth += maxheal;
            Console.WriteLine("레벨이 " + level + "만큼 상승했습니다");
            Console.WriteLine("공격력이 " + damage + "만큼 상승했습니다");
            Console.WriteLine("최대 생명력이 " + maxheal + "만큼 상승했습니다");

        }

        //core==
        private static GameManager instance;
        private Player NowPlayer = null;
        private MAPTYPE NowMap = MAPTYPE.MAIN;
        
        //MonsterManage
        List<Monster> Monsters;
        Monster NowMonster;
        int MobCount    = 0;
        int StageDiff   = 0;
        int reward      = 0;
        bool isBattle   = false;
    
    }
}
