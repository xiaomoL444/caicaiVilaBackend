using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend
{
	public class PullRsp
	{
		public string msgType = string.Empty;
		public string json = string.Empty;
	}
	//public class PullRsp
	//{
	//	/// <summary>
	//	/// 消息类型
	//	/// </summary>
	//	public MsgType msgType;

	//	/// <summary>
	//	/// 发送者类型
	//	/// </summary>
	//	public SenderType senderType;

	//	/// <summary>
	//	/// 用户信息
	//	/// </summary>
	//	public User user = new User();

	//	public Content content = new Content();
	//}
	//public enum MsgType
	//{
	//	Unknown = 0,
	//	Text = 1,
	//	Image = 2,
	//}
	//public enum SenderType
	//{
	//	System = 0,
	//	User = 1,
	//}
	//public class User
	//{
	//	public string name = string.Empty;
	//	public string member_name = string.Empty;
	//	public string member_color = string.Empty;
	//	public string uid = string.Empty;
	//	public string avatar = string.Empty;
	//}
	//public class Content
	//{
	//}
	//public class TextContent : Content
	//{
	//	public string text = string.Empty;
	//}
	//public class ImageContent : Content
	//{
	//	public string url = string.Empty;
	//	public ulong file_size;
	//	public Size size = new Size();
	//	public class Size
	//	{
	//		public int width;
	//		public int height;
	//	}
	//}
}
