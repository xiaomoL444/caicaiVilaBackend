using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend
{
	internal static class Core
	{
		public static string DatebasePath;
		public static string villa_id;
		public static string room_id;

		public static MsgList Msg;
		public static void Init()
		{
			Msg = GetMsg(DatebasePath, villa_id, room_id);
			Msg.msgType.Reverse();
			Msg.msgContent.Reverse();
		}
		static MsgList GetMsg(string filePath, string villa_id, string room_id)
		{

			var connectString = new SqliteConnectionStringBuilder() { Mode = SqliteOpenMode.ReadWriteCreate, DataSource = $"{filePath}", Cache = SqliteCacheMode.Shared }.ToString();
			var sqliteConnection = new SqliteConnection(connectString);
			sqliteConnection.Open();

			var command = sqliteConnection.CreateCommand();

			command.CommandText = @"SELECT * FROM 'Msg' LIMIT 0,100000";


			using (var reader = command.ExecuteReader())
			{
				int index = -1;
				var msgType = new List<string>();
				var msgContent = new List<string>();
				while (reader.Read())
				{
					index++;
					// 处理每一行的数据
					string time = reader.GetString(0);
					string dataType = reader.GetString(1);
					string content = reader.GetString(2);
					if (dataType != "rawData") continue;
					if (!content.StartsWith("61")) continue;
					//Console.WriteLine(Encoding.Default.GetString(FromHex(content)));
					var msgList = content.Split(StringToHex(room_id) + "0A").ToList();
					for (int i = 0; i < msgList.Count; i++)
					{
						try
						{
							if (index == 1594)
							{
								Console.Write("0");
							}
							msgList[i] = msgList[i].Split(StringToHex(villa_id) + "22")[1];
							string lighttype = msgList[i].Substring(2).Split("2A")[0];
							msgType.Add(Encoding.Default.GetString(FromHex(lighttype)));

							var spliteContent = msgList[i].Split("2A").ToList();
							spliteContent.RemoveAt(0);

							var combineStr = String.Join("", spliteContent);

							string lightcontent = combineStr.Substring(combineStr.IndexOf("7B")).Split("7D7D30")[0] + "7D7D";
							msgContent.Add(Encoding.Default.GetString(FromHex(lightcontent)));

							//Console.WriteLine(Encoding.Default.GetString(FromHex(msgType[i])));
							//var json = JObject.Parse(msgContent[i]);
							//if (string.IsNullOrEmpty((string?)json["content"]["target_user_id"])) continue;
							//var path = $"./dby/{(string?)json["user"]["id"]}_to_{((string?)json["content"]["target_user_id"])}_{json["content"]["action_id"]}.gif";
							//if (Path.Exists(path)) continue;
							//var request = new HttpRequestMessage(HttpMethod.Get, ((string?)json["content"]["url"]));
							//var res = await httpClient.SendAsync(request);

							//File.WriteAllBytes(path, await (res.Content.ReadAsByteArrayAsync()));

						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							Console.WriteLine($"读取发生错误 index={index},msgListIndex={i}\ncontent={msgList[i]}");
							continue;
						}
					}
					// ...
				}
				return new MsgList() { msgContent = msgContent, msgType = msgType = msgType };
			}
		}


		static byte[] FromHex(string hex)
		{
			hex = hex.Replace("-", "");
			byte[] raw = new byte[hex.Length / 2];
			for (int i = 0; i < raw.Length; i++)
			{
				raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
			}
			return raw;
		}
		static string StringToHex(string str)
		{
			return Convert.ToHexString(Encoding.Default.GetBytes(str)).Replace("-", "");
		}
	}
	public class MsgList
	{
		public List<string> msgType;
		public List<string> msgContent;
		public MsgInstance this[int index]
		{
			get { return new MsgInstance() { msgType = msgType[index], msgContent = msgContent[index] }; }
		}
	}
	public class MsgInstance
	{
		public string msgType;
		public string msgContent;
	}
}
