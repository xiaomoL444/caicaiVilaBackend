using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Api.Info
{
	public class PullRsp
	{
		public string msgType = string.Empty;
		public object msgContent = new();
		public string msgID = string.Empty;
		public ulong msgTime;
	}
}
