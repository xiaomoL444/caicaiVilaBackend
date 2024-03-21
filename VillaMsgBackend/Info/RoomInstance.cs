using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Info;

/// <summary>
/// 房间实例
/// </summary>
internal record RoomInstance
{
	/// <summary>
	/// 房间名称
	/// </summary>
	public string RoomName { get; set; }

	/// <summary>
	/// 消息实例
	/// </summary>
	public List<MsgInstance> MsgInstance { get; set; } = new();

	/// <summary>
	/// 发言过的用户
	/// </summary>
	public List<User> SpokenUser { get; set; } = new();

	/// <summary>
	/// 置顶的消息
	/// </summary>
	public List<MsgInstance> PinMsg { get; set; } = new();
}