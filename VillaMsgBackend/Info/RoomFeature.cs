using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Info
{
	internal struct RoomFeature
	{
		public string villa_id;
		public string room_id;
		public RoomFeature(string villa_id, string room_id) {
			this.villa_id = villa_id;
			this.room_id = room_id;
		}
	}
}
