using System;
using System.Collections.Generic;
using System.Drawing;
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
	/// 面板信息
	/// </summary>
	public object panel { get; set; }
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

	#region Image
	public Size size { get; set; } = new();
	public string id { get; set; } = string.Empty;
	public ulong file_size { get; set; }
	public string url { get; set; } = string.Empty;
	#endregion

	#region PinMessage
	public string message_send_time { get; set; } = string.Empty;
	/// <summary>
	/// 消息ID
	/// </summary>
	public string message_id { get; set; } = string.Empty;
	/// <summary>
	/// 操作
	/// </summary>
	public string operation { get; set; } = string.Empty;
	#endregion

	#region  MHY:SYS:VillaActiveLabelNotify
	public object members { get; set; } = new();
	public string notify_content { get; set; } = string.Empty;
	public ulong label_id { get; set; }
	#endregion

	#region  MHY:Post
	public bool has_image;
	public string post_id;
	public string subject;
	#endregion

	#region MHY:AvatarEmoticon
	public string action_id;
	public string target_user_id;
	#endregion

	#region MHY:ShareVillaPost
	public string title = string.Empty;
	#endregion

	#region MHY:VillaCard
	public string villa_name = string.Empty;
	public string villa_id = string.Empty;
	#endregion

	#region MHY:SYS:BannedFromVilla
	public string reason = string.Empty;
	public BannedUser user = new();
	public class BannedUser
	{
		public string uid = string.Empty;
		public string avatar_url = string.Empty;
		public string nickname = string.Empty;
	}
	#endregion

	#region MHY:Emotion
	public string emoticon = string.Empty;
	#endregion

}
//额外的实体
public class Entities
{
	public int offset;
	public int length;
	public Entity entity = new();
	public class Entity
	{
		public string user_id = string.Empty;
		public string type = string.Empty;
	}
}
public class Size
{
	public int width;
	public int height;
}