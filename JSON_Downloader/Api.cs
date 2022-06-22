using System;
using System.Net;
using System.IO;

namespace Api
{
	public class Url
	{

		private string link;

		public Url(string link)
		{
			this.link = link;
		}

		public Stream GetResponseStream()
		{

			try
			{
				// Create Request and get a Response
				HttpWebRequest m_request = (HttpWebRequest)WebRequest.Create(this.link);
				HttpWebResponse m_response = (HttpWebResponse)m_request.GetResponse();

				// Status code 200
				Stream result = m_response.GetResponseStream();
				m_response.Close();
				return result;
			}

			// Error - f. ex. Url is not accessable (status code != 200)
			catch (Exception)
			{
				return null;
			}

		}

		public bool CheckUrl()
		{
			return this.CheckUrlDNS() || this.CheckUrlIP();
		}

		private bool CheckUrlIP()
		{

			IPAddress address;

			if (IPAddress.TryParse(this.link, out address))
			{
				switch (address.AddressFamily)
				{
					case System.Net.Sockets.AddressFamily.InterNetwork:
						return true;
					case System.Net.Sockets.AddressFamily.InterNetworkV6:
						return true;
				}
			}

			return false;

		}

		private bool CheckUrlDNS()
		{

			// Valid interface list
			string[] interfaces = {"https", "http"};

			bool pass = false;

			foreach (string interf in interfaces)
			{
				// Does not start with interface prefix
				if (this.link.IndexOf(interf + "://") == 0)
				{
					pass = true;
					break;
				}
			}

			if (!pass)
				return false;

			string after_interface = this.link.Split("://")[1];

			// Must contain dot (domain.com for example)
			if (after_interface.IndexOf(".") == -1)
				return false;

			// Subsite can't come before domain
			if (after_interface.IndexOf(".") > after_interface.IndexOf("/") && after_interface.IndexOf("/") != -1)
				return false;

			// OK - try it out
			return true;
		}

		public string Link { get { return this.link; } }
	}
}

