using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VillaMsgBackend.Info;

namespace VillaMsgBackend
{
	internal static class Util
	{
		public static RoomFeature GetRoomFeature(Dictionary<string, string> @param)
		{
			return new RoomFeature(param["villa_id"], param["room_id"]);
		}
	}
}
