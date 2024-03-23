using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tool;
using System.IO;

namespace VillaMsgBackend
{
	internal static class ApiListener
	{

		public static List<ApiInterface> ApiList = new();

		static TcpListener listener;

		static bool isListener = false;

		static System.Timers.Timer timer;//定时重启伺服器

		public static void Start()
		{

			int port = 3280; // 设置端口号
			listener = new TcpListener(IPAddress.IPv6Any, port);

			listener.Start();

			isListener = true;
			Console.WriteLine($"HTTP 服务已启动，监听端口：{port}");
			while (isListener)
			{
				using (TcpClient client = listener.AcceptTcpClient())
				{
					try
					{

						HandleClient(client);

					}
					catch (Exception e)
					{
						Logger.Log($"{e.Message} {e.StackTrace} {e.Source}");
						RespondWithError(client.GetStream(), HttpStatusCode.BadRequest, @"{""msg"":""error""}");
					}
				}
			}
		}
		public static void Stop()
		{
			isListener = false;

			listener.Stop();
		}		static ApiListener()
		{
			//Logger.loggerLevel = Logger.LoggerLevel.Log;

			//ApiList设置
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(ApiInterface))) != null))
			{
				var a = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(ApiInterface))).Select(Activator.CreateInstance);
				foreach (var api in a)
				{
					ApiList.Add((ApiInterface)api!);
				}
			}
			//timer = new System.Timers.Timer(3600 * 1000);
			//timer.Elapsed += (_, _) => { Logger.LogWarnning("伺服器定时重启"); Stop(); Thread.Sleep(1 * 1000); Start(); };
			//timer.Start();
			//Start();
		}
		static void HandleClient(TcpClient client)
		{
			using (NetworkStream stream = client.GetStream())
			{
				byte[] buffer = new byte[1024];
				int numBytesRead = stream.Read(buffer, 0, buffer.Length);

				string request = Encoding.ASCII.GetString(buffer, 0, numBytesRead);
				//Console.WriteLine($"接收到请求：\n{request}");

				string[] tokens = request.Split(' ');
				string httpMethod = tokens[0];
				string httpUrl = tokens[1];

				Logger.Log(httpUrl);

				if (httpMethod == "GET" || httpMethod == "POST")
				{
					Respond(stream, httpMethod, httpUrl);
				}
				else
				{
					RespondWithError(stream, HttpStatusCode.NotImplemented, "不支持此 HTTP 方法");
				}
			}
		}

		static async void Respond(NetworkStream stream, string httpMethod, string httpUrl)
		{

			//分割url与参数
			string url = string.Empty;
			Dictionary<string, string> param;

			try
			{
				url = httpUrl.Split('?')[0];
				param = httpUrl.Split('?')[1].Split('&').ToDictionary(k => k.Split('=')[0], v => v.Split('=')[1]);
			}
			catch (Exception ex)
			{
				param = new();
			}

			try
			{
				//匹配Api的接口
				if (ApiList.Any(api => "/dby" + api.url == url && api.httpMethod == httpMethod))
				{
					string fileContent = ApiList.First(api => "/dby" + api.url == url && api.httpMethod == httpMethod).GetContent(param);

					byte[] responseBytes = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\nAccess-Control-Allow-Origin: *\n\n{fileContent}");
					//Console.Write($"{fileContent}");
					stream.Write(responseBytes, 0, responseBytes.Length);
				}
			}
			catch (Exception)
			{
				Console.Write("匹配失败\n");
			}

		}

		static void RespondWithError(NetworkStream stream, HttpStatusCode statusCode, string errorMessage)
		{
			string response = $"HTTP/1.1 {(int)statusCode} {statusCode}\n\n{errorMessage}";
			byte[] responseBytes = Encoding.ASCII.GetBytes(response);
			stream.Write(responseBytes, 0, responseBytes.Length);
		}
	}
}
