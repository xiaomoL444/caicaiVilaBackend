// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using VillaMsgBackend;

string DatebasePath;
string villa_id;
string room_id;
#if !DEBUG
Console.Write("输入文件路径:");
DatebasePath =  Console.ReadLine();
Console.Write("输入villa_id:");
villa_id = Console.ReadLine();
Console.Write("输入room_id:");
room_id = Console.ReadLine();
#else
DatebasePath = @"E:\code\TryVillaWss\TryVillaWss\bin\Debug\net7.0\Msg\463_16885_.db";
villa_id = "463";
room_id = "16885";
#endif
if (!File.Exists(DatebasePath))
{
	Console.WriteLine("文件不存在，按任意键退出程序");
	return;
}

Console.WriteLine("读取数据中");
Core.villa_id = villa_id;
Core.DatebasePath = DatebasePath;
Core.room_id = room_id;
Core.Init();
Console.WriteLine("完成");

ApiListener.Start();

Console.WriteLine("done");

while (true) ;