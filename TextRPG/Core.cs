using TextRPG.Manager;
using TextRPG.Object;

namespace TextRPG
{
    internal class Core
    {
        static void Main(string[] args)
        {
            //Init
            FileManager.Instance().Init();
            GameManager.Instance().Init();
            //Update
            while (true) { GameManager.Instance().Update();}         
        }
    }
}
