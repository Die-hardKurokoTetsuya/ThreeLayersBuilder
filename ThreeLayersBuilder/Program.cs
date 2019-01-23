using Builder.Constructer.DAL;
using Builder.Constructer.Models;
using static System.Console;

namespace ThreeLayersBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    WriteLine("请输入数据库连接字符串");
            //    return;
            //}

            new General_Model().Construct();
            new General_DAL().Construct();

            WriteLine("执行完毕");

            ReadKey();
        }
    }
}
