using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend
{
	internal static class ApiListener
	{

		public static List<ApiInterface> ApiList = new();

		static TcpListener listener;

		static bool isListener = false;

		public static void Start()
		{
			isListener = true;
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
						Console.Write($"{e.Message} {e.StackTrace}");
					}
				}
			}
		}
		public static void Stop()
		{
			isListener = false;
			listener.Stop();
		}
		static ApiListener()
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

			int port = 328; // 设置端口号
			listener = new TcpListener(IPAddress.Any, port);

			listener.Start();

			Console.WriteLine($"HTTP 服务已启动，监听端口：{port}");
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

				Console.Write(httpUrl);

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
				if (ApiList.Any(api => api.url == url && api.httpMethod == httpMethod))
				{
					string fileContent = ApiList.First(api => api.url == url && api.httpMethod == httpMethod).GetContent(param);

					byte[] responseBytes = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\nAccess-Control-Allow-Origin: *\n\n{fileContent}");
					Console.Write($"{fileContent}");
					stream.Write(responseBytes, 0, responseBytes.Length);
				}
			}
			catch (Exception)
			{
				Console.Write("匹配失败");
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
