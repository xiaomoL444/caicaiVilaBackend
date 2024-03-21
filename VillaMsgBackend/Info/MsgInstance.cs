using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Info;

/// <summary>
/// 消息实例
/// </summary>
public record MsgInstance
{
	/// <summary>
	/// 消息类型
	/// </summary>
	public string MsgType;
	/// <summary>
	/// 消息正文(json)
	/// </summary>
	//public string MsgContentJson;
	/// <summary>
	/// 被解析过后的消息物件（根类型）
	/// </summary>
	//public object MsgContentObject;
	/// <summary>
	/// 消息正文（自定义类型）
	/// </summary>
	public MsgContentObject MsgContent;
	/// <summary>
	/// 消息ID
	/// </summary>
	public string MsgID;
	/// <summary>
	/// 消息发送时间(ulong)
	/// </summary>
	public ulong MsgTime;
}
