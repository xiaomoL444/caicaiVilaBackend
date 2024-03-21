using Newtonsoft.Json;
using System;
using Tool;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class GetPinMsg : ApiInterface
	{
		public string url => "/pinMsg";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);

			List<PullRsp> pullRspList = new List<PullRsp>();

			for (int i = 0; i < Core.RoomInstance[roomFeature].PinMsg.Count; i++)
			{
				try
				{
					var pullRsp = new PullRsp()
					{
						msgContent = Core.RoomInstance[roomFeature].PinMsg[i].MsgContent,
						msgType = Core.RoomInstance[roomFeature].PinMsg[i].MsgType,
						msgID = Core.RoomInstance[roomFeature].PinMsg[i].MsgID,
						msgTime = Core.RoomInstance[roomFeature].PinMsg[i].MsgTime,
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
