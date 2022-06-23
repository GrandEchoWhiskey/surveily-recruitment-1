using System;
using System.Threading;

namespace Api
{
	public class Controller
	{

		const bool USE_REAL_FILE_NAME = true;

		public static void checkAndDownload(string url, string path, string name)
		{

			MyUrl m_url = new MyUrl(url);

			if (!m_url.is_UrlInterface() && !m_url.is_UrlIP())
			{
				Console.WriteLine("Podany adres: " + url + " nie może zostać odczytany.");
				return;
			}

			if (!m_url.is_UrlPing())
			{
				Console.WriteLine("Podany adres: " + url + " nie odpowiada.");
				return;
			}

			MyDownloader m_dl = new MyDownloader(m_url.Url, path + name);

			if (!m_dl.Connected)
            {
				Console.WriteLine("Brak dostępu do: " + url + " lub adres nie odpowiada.");
				m_dl.Close();
				return;
            }

			if (!m_dl.FileOpen)
            {
				Console.WriteLine("Plik nie może zostać zapisany: " + path + name);
				m_dl.Close();
				return;
            }

			if (!m_dl.download())
            {
				Console.WriteLine("Błąd konwersji: " + path + name);
				m_dl.Close();
				return;
			}

			Console.WriteLine("Plik zapisany: " + path + name);
			m_dl.Close();

		}

		public static void StartDownloading(string[] urls, string path)
		{
			path = path[path.Length-1] == '\\' ? path : path + '\\';
			for (int i=0; i<urls.Length; i++)
			{
				string url = urls[i];
				string name;

                try
                {
					if (url.Contains(".json") && url.Contains("/") && USE_REAL_FILE_NAME)
					{
						string[] temp = url.Split("/");
						string t = temp[temp.Length - 1];
						t = t.Split(".json")[0];
						name = t + ".json";
					}
					else { throw new Exception(); }
                }
                catch (Exception)
				{
					name = "download_" + (i + 1).ToString() + ".json";
				}

				ThreadStart child = new ThreadStart(() => checkAndDownload(url, path, name));
				Thread ct = new Thread(child);
				ct.Start();
			}
		}

	}
}

