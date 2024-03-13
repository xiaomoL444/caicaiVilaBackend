using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend
{
	internal class PullMsg : ApiInterface
	{
		public string url => "/pull";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var index = Int32.Parse(@params["index"]);
			if (index * 100 > Core.Msg.msgContent.Count)
			{
				Console.WriteLine("失败，数组超出范围");
				return "";
			}

			int pullMaxLength = 100;
			if ((index + 1) * 100 > Core.Msg.msgContent.Count) pullMaxLength = Core.Msg.msgContent.Count - index * 100;

			List<PullRsp> pullRspList = new List<PullRsp>();

			for (int i = index * 100; i < index * 100 + pullMaxLength; i++)
			{
				try
				{
					var json = JObject.Parse(Core.Msg.msgContent[i]);
					var pullRsp = new PullRsp();
					switch (Core.Msg.msgType[i])
					{
						//User
						case "MHY:Text":
							pullRsp.user = SetUser(json);

							pullRsp.senderType = SenderType.User;
							pullRsp.msgType = MsgType.Text;

							pullRsp.content = new TextContent() { text = ((string?)json["content"]["text"]) };
							break;
						case "MHY:Image":
							pullRsp.user = SetUser(json);

							pullRsp.senderType = SenderType.User;
							pullRsp.msgType = MsgType.Image;

							pullRsp.content=new ImageContent() { url = ((string?)json["content"]["url"]) };
							break;

						//System
						case "MHY:SYS:VillaRoomCreated":
							pullRsp.content = new TextContent() { text = "欢迎来到{roomo_name}房间~" };
							break;
						case "MHY:SYS:PinMessage":
							pullRsp.content = new TextContent() { text = $"{json["user"]["name"]}置顶了一条消息 id:{json["content"]["message_id"]}" };
							break;
						default:
							pullRsp.content = new TextContent() { text = $"未解析消息{Core.Msg.msgType[i]}\n{Core.Msg[i].msgContent}" };
							break;
					}
					pullRspList.Add(pullRsp);
				}
				catch (Exception e)
				{
					pullRspList.Add(new PullRsp() { content = new TextContent { text = "解析失败的消息" } });
				}
			}


			return JsonConvert.SerializeObject(pullRspList);
		}
		public User SetUser(JObject json)
		{
			return new User() { avatar = ((string?)json["user"]["portraitUri"]), name = ((string?)json["user"]["name"]), uid = ((string?)json["user"]["id"]) };
		}
	}


}
