using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class SearchIDMsg : ApiInterface
	{
		public string url => "/search";

		public string httpMethod => "GET";


		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);
			var id = @params["id"];

			var msgInstance = Core.RoomInstance[roomFeature].MsgInstance.First(q => q.MsgID == id);
			var SearchRsp = new SearchRsp(roomFeature, msgInstance);

			switch (msgInstance.MsgType)
			{
				case "MHY:Text":
					var text = $"{msgInstance.MsgContent.user.name}: {msgInstance.MsgContent.content.text}";
					int textMaxLength = 15;
					if (text.Length > textMaxLength)
					{
						SearchRsp.text = text.Substring(0, 15) + "......";
					}
					else
					{
						SearchRsp.text = text;
					}
					break;
				case "MHY:Image":
					SearchRsp.text = "[图片]";
					break;
				case "MHY:Post":
				case "MHY:ShareVillaPost":
					SearchRsp.text = "[帖子]";
					break;
				default:
					SearchRsp.text = $"未知{msgInstance.MsgType}";
					break;
			}

			return JsonConvert.SerializeObject(SearchRsp);
		}
	}
}
