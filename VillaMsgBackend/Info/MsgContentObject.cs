using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Info;

public class MsgContentObject
{
	/// <summary>
	/// 用户设备信息
	/// </summary>
	public object trace { get; set; }

	/// <summary>
	/// 用户信息
	/// </summary>
	public User user { get; set; } = new();

	/// <summary>
	/// 正文内容
	/// </summary>
	public Content content { get; set; } = new();
}

/// <summary>
/// 用户类
/// </summary>
public class User
{
	/// <summary>
	/// 头像
	/// </summary>
	public string portraitUri { get; set; } = string.Empty;

	/// <summary>
	/// extra内容
	/// </summary>
	public string extra { get; set; } = string.Empty;

	/// <summary>
	/// 用户名称
	/// </summary>
	public string name { get; set; } = string.Empty;

	/// <summary>
	/// 用户id
	/// </summary>
	public string id { get; set; } = string.Empty;
}

/// <summary>
/// 正文类
/// </summary>
public class Content
{

	/// <summary>
	/// 图片
	/// </summary>
	public List<object> images { get; set; } = new();

	/// <summary>
	/// 实体
	/// </summary>
	public List<Entities> entities { get; set; } = new();

	/// <summary>
	/// 文本
	/// </summary>
	public string text { get; set; } = string.Empty;

	#region PinMessage
	/// <summary>
	/// 消息ID
	/// </summary>
	public string message_id { get; set; } = string.Empty;
	public string operation { get; set; } = string.Empty;
	#endregion
}
public class Entities {
	public int offset;
	public int length;
	public Entity entity = new();
	public class Entity {
		public string user_id = string.Empty;
		public string type = string.Empty;
	}
}