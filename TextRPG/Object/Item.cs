enum ITEMTYPE
{
    NONE,
    WEAPON,
    ARMOR,
}
namespace TextRPG.Object
{
    internal class Item
    {
        public ITEMTYPE type = ITEMTYPE.NONE;
        public string name;
        public int damage;
        public int armor;
        public int value;
    }
}
