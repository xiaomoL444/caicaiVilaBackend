using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaMsgBackend.Info;

namespace VillaMsgBackend.Api.Info
{
	internal class SearchRsp
	{
		public string text = string.Empty;
		public int page;

		public string avatar_url = string.Empty;//头像链接
		public string name = string.Empty;//名字
		public ulong time;//发送时间
		public string msgID = string.Empty;//消息ID

		public SearchRsp(RoomFeature roomFeature,MsgInstance msgInstance)
		{
			int index = Core.RoomInstance[roomFeature].MsgInstance.FindIndex(q => q == msgInstance);
			text = ((MsgContentObject)msgInstance.MsgContent).content.text;
			page = index / 50;
			avatar_url = ((MsgContentObject)msgInstance.MsgContent).user.portraitUri;
			name = ((MsgContentObject)msgInstance.MsgContent).user.name;
			time = msgInstance.MsgTime;
			msgID = msgInstance.MsgID;
		}
	}
}
