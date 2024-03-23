using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Tool;
using VillaMsgBackend.Api.Info;
using VillaMsgBackend.Info;

namespace VillaMsgBackend
{
	internal static class Core
	{
		public static DateTime standTimeEpoch = new DateTime(2020, 8, 1, 0, 12, 28);
		public static Dictionary<RoomFeature, RoomInstance> RoomInstance { get; private set; } = new();

		//public static Dictionary<string, MsgList> Msg = new();//{villa_id}_{room_id} : MsgList
		public static int filesNum = 0;
		public static int loadingNum = 0;


		public static void Init(string filePath, string villa_id, string room_id, string room_name)
		{
			filesNum++;
			var roomInstance = GetMsg(filePath, villa_id, room_id, room_name);
			RoomFeature roomFeature = new(villa_id, room_id);
			RoomInstance.Add(roomFeature, roomInstance);
			loadingNum++;
			Logger.Log($"{filePath}读取完毕 {loadingNum}/{filesNum}");
		}
		/// <summary>
		/// 等待加载
		/// </summary>
		/// <returns></returns>
		public async static Task WaitForLoading()
		{
			while (loadingNum != filesNum) ;
			return;
		}
		static RoomInstance GetMsg(string filePath, string villa_id, string room_id, string room_name)
		{
			var roomInstance = new RoomInstance() { RoomName = room_name, MsgInstance = new() };

			var connectString = new SqliteConnectionStringBuilder() { Mode = SqliteOpenMode.ReadOnly, DataSource = $"{filePath}", Cache = SqliteCacheMode.Shared }.ToString();
			var sqliteConnection = new SqliteConnection(connectString);
			sqliteConnection.Open();

			var command = sqliteConnection.CreateCommand();

			command.CommandText = @"SELECT * FROM 'Msg'";

			using (var reader = command.ExecuteReader())
			{
				int index = -1;
				//var msgType = new List<string>();
				//var msgContent = new List<string>();
				//var msgID = new List<string>();//B0084A13<id>006A0070
				//var msgTime = new List<string>();//5E7A5C<json>9A01
				while (reader.Read())
				{
					index++;
					// 处理每一行的数据
					string time = reader.GetString(0);
					string dataType = reader.GetString(1);
					string content = reader.GetString(2);

					if (dataType != "rawData") continue;
					if (!content.StartsWith("61")) continue;
					var msgList = content.Split(StringToHex(room_id) + "0A").ToList();
					for (int i = 0; i < msgList.Count; i++)
					{
						try
						{
							if (msgList[i].Length <= 50) continue;

							string hexString = msgList[i];
							var tmpRes = ("", "");

							var msgInstance = new MsgInstance();

							//type
							hexString = hexString.SpliteString(StringToHex(villa_id) + "22").Item2;
							tmpRes = hexString.Substring(2).SpliteString("2A");
							string lighttype = tmpRes.Item1;
							hexString = tmpRes.Item2;

							msgInstance.MsgType = Encoding.Default.GetString(FromHex(lighttype));


							//content
							hexString = hexString.SpliteString("7B").Item2;
							tmpRes = hexString.SpliteString("7D30");

							string lightcontent = "7B" + tmpRes.Item1 + "7D";
							hexString = tmpRes.Item2;
							var content_json = Encoding.Default.GetString(FromHex(lightcontent));
							msgInstance.MsgContent = JsonConvert.DeserializeObject(content_json);
							msgInstance.MsgContentObject = JsonConvert.DeserializeObject<MsgContentObject>(content_json);

							//处理发言过的用户
							if (!roomInstance.SpokenUser.Any(q => q.id == msgInstance.MsgContentObject.user.id))
							{
								roomInstance.SpokenUser.Add(msgInstance.MsgContentObject.user);
							}

							//id
							hexString = hexString.SpliteString("4A13").Item2;
							tmpRes = hexString.SpliteString("500");

							string id = tmpRes.Item1;
							hexString = tmpRes.Item2;
							msgInstance.MsgID = Encoding.Default.GetString(FromHex(id));


							//time 
							string time_json;
							if (!hexString.Contains("7B22") || !hexString.Contains("7D9A"))
							{
								time_json = "7B7D";

							}
							else
							{
								hexString = hexString.SpliteString("7B22").Item2;
								tmpRes = hexString.SpliteString("7D9A");

								time_json = "7B22" + tmpRes.Item1 + "7D";

							}

							try
							{
								msgInstance.MsgTime = ((ulong)JObject.Parse(Encoding.Default.GetString(FromHex(time_json)))["status"]["ts"]);
							}
							catch (Exception)
							{

								msgInstance.MsgTime = 0;
							}

							roomInstance.MsgInstance.Add(msgInstance);
						}
						catch (Exception e)
						{
							Logger.LogError(e.Message);
							Logger.LogError($"读取发生错误 index={index},msgListIndex={i}\ncontent={msgList[i]}");
							continue;
						}
					}
					// ...
				}
			}

			sqliteConnection.Close();
			roomInstance.MsgInstance.Reverse();//倒序

			foreach (var msgInstance in roomInstance.MsgInstance.Where(q => q.MsgType == "MHY:SYS:PinMessage"))
			{
				if (msgInstance.MsgContentObject.content.operation == "pin")
					roomInstance.PinMsg.Add(roomInstance.MsgInstance.FirstOrDefault(q => q.MsgID == msgInstance.MsgContentObject.content.message_id));
				else
				{
					try
					{
						roomInstance.PinMsg.Remove(roomInstance.MsgInstance.FirstOrDefault(q => q.MsgID == msgInstance.MsgContentObject.content.message_id));
					}
					catch (Exception e)
					{
						Logger.LogError($"删除置顶信息失败{e}");
					}
				}
				if (roomInstance.PinMsg.Count >= 1 && roomInstance.PinMsg[roomInstance.PinMsg.Count - 1] == null)
				{
					roomInstance.PinMsg.RemoveAt(roomInstance.PinMsg.Count - 1);
				}
			}

			return roomInstance;
		}
		/// <summary>
		/// hex to bytes
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
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
		/// <summary>
		/// string to hex
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string StringToHex(string str)
		{
			return Convert.ToHexString(Encoding.Default.GetBytes(str)).Replace("-", "");
		}
		public static (string, string) SpliteString(this string source, string splitStr)
		{
			var tmpStrList = new List<string>();
			tmpStrList = source.Split(splitStr).ToList();
			var a = tmpStrList[0];
			tmpStrList.RemoveAt(0);
			var b = string.Join(splitStr, tmpStrList);
			return new(a, b);
		}
	}
}
