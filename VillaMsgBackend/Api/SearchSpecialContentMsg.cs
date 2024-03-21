using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Tool;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class SearchSpecialContentMsg : ApiInterface
	{
		public string url => "/searchSpecialContentMsg";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);
			//var startTs = (DateTime.ParseExact(@params["s"],"yyyy-MM-dd",null,System.Globalization.DateTimeStyles.None)-Core.standTimeEpoch).TotalMilliseconds;
			//var endTs= (DateTime.ParseExact(@params["e"],"yyyy-MM-dd",null,System.Globalization.DateTimeStyles.None)-Core.standTimeEpoch).TotalMilliseconds;
			string msg = HttpUtility.UrlDecode(@params["msg"]);
			string id = @params["id"];

			List<SearchRsp> res = Core.RoomInstance[roomFeature].MsgInstance.Where(q =>
			{

				try
				{
					if (id != "0" && q.MsgContent.user.id != id) return false;
				//if(!(startTs<=q.MsgTime&&q.MsgTime<=endTs)) return false;
					return q.MsgContent.content.text.Contains(msg);
				}
				catch (Exception)
				{
					Logger.LogWarnning("搜索文字错误");
					return false;
				}

			}).Select(q => new SearchRsp(roomFeature,q)).ToList();
			return JsonConvert.SerializeObject(res);
		}
	}
}
