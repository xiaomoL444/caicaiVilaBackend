using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class PullMsg : ApiInterface
	{
		public string url => "/pull";

		public string httpMethod => "GET";

		int pullLength = 50;

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);

			var index = int.Parse(@params["index"]);

			if (index * pullLength > Core.RoomInstance[roomFeature].MsgInstance.Count)
			{
				Console.WriteLine("失败，数组超出范围");
				return "{}";
			}

			int pullMaxLength = pullLength;
			if ((index + 1) * pullLength > Core.RoomInstance[roomFeature].MsgInstance.Count) pullMaxLength = Core.RoomInstance[roomFeature].MsgInstance.Count - index * pullLength;

			List<PullRsp> pullRspList = new List<PullRsp>();

			for (int i = index * pullLength; i < index * pullLength + pullMaxLength; i++)
			{
				try
				{
					var pullRsp = new PullRsp()
					{
						msgContent = Core.RoomInstance[roomFeature].MsgInstance[i].MsgContent,
						msgType = Core.RoomInstance[roomFeature].MsgInstance[i].MsgType,
						msgID = Core.RoomInstance[roomFeature].MsgInstance[i].MsgID,
						msgTime = Core.RoomInstance[roomFeature].MsgInstance[i].MsgTime,
					};

					pullRspList.Add(pullRsp);
				}
				catch (Exception e)
				{
					Logger.LogError($"Error: {e}\n");
				}
			}
			return JsonConvert.SerializeObject(pullRspList);
		}
	}


}
