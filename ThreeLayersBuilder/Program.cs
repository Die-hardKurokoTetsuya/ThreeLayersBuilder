using Builder.Constructer.DAL;
using Builder.Constructer.Models;
using static System.Console;

namespace ThreeLayersBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                WriteLine("请输入命名空间");
                return;
            }

            string nameSpace = args[0];

            new General_Model(nameSpace).Construct();
            new General_DAL(nameSpace).Construct();
            new CreateFactoryFile(nameSpace).Construct();

            WriteLine("执行完毕");

            ReadKey();
        }
    }
}
