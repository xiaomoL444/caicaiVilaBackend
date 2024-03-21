using System.Text.RegularExpressions;
using Tool;
using VillaMsgBackend;

var directoryPath = "./Msg/";
if (!Directory.Exists(directoryPath))
{
	Directory.CreateDirectory(directoryPath);
}

var filesPath = Directory.GetFiles(directoryPath);

Logger.Log("读取数据中");
foreach (var file in filesPath)
{
	new Thread(() =>
	{
		try
		{
			Logger.Log($"尝试读取文件{file}");
			var match = Regex.Match(file, "./Msg/([\\s\\S]*).db").Groups[1].Value;
			string villa_id = match.Split("_")[0];
			string room_id = match.Split("_")[1];
			string room_name = match.Split("_")[2];
			Core.Init(file, villa_id, room_id, room_name);
		}
		catch (Exception)
		{
			Logger.LogError($"Error: {file}");
		}
	}).Start();

}
while (Core.loadingNum != filesPath.Length) ;

Console.WriteLine("完成");
Console.WriteLine("伺服器启动");
ApiListener.Start();
Console.WriteLine("done");

while (true) ;