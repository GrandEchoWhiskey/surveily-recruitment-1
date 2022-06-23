using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Api
{
	public class MyUrl
	{

		private string _url;

		public MyUrl(string url)
		{
			this._url = url;
		}

		public bool is_UrlIP()
		{
			try
			{
				IPAddress address;
				if (IPAddress.TryParse(this._url, out address))
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

		public bool is_UrlPing()
		{

			string address = this._url;
			if (this.is_UrlInterface())
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

		public bool is_UrlInterface()
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
