using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Api
{
	public class MyUrl
	{

		private readonly string _url;

		public MyUrl(string url)
		{
			this._url = url;
		}

		public bool Is_UrlIP()
		{
			try
			{
				string url = this._url;

				if(url.Contains("://"))
                {
					url = url.Split("://")[1];
                }

				if(url.Contains("/"))
                {
					url = url.Split("/")[0];
                }

				IPAddress address;
				if (IPAddress.TryParse(url, out address))
				{
					switch (address.AddressFamily)
					{
						case System.Net.Sockets.AddressFamily.InterNetwork:
							return true;
						case System.Net.Sockets.AddressFamily.InterNetworkV6:
							return true;
					}
				}
			}
			catch (Exception) { }
			return false;
        }

		public bool Is_UrlPing()
		{

			string address = this._url;
			if (this.Is_UrlInterface())
            {
				address = address.Split("://")[1];
            }
			if (address.Contains('/'))
            {
				address = address.Split('/')[0];
            }

			try
			{
				
				Ping ping = new ();
				PingReply result = ping.Send(address);
				if (result.Status == IPStatus.Success)
				{
					return true;
				}
			}
			catch (Exception) { }
			return false;
		}

		public bool Is_UrlInterface()
		{
            try
            {
				string[] interfaces = { "https", "http", "ftp" };
				foreach (string interf in interfaces)
				{
					// Does start with interface prefix
					if (this._url.IndexOf(interf + "://") == 0)
					{
						return true;
					}
				}
            }
			catch (Exception) { }
			return false;
		}

		public string Url { get { return _url; } }
	}
}