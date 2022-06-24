using System;
using System.Threading;
using System.Collections.Generic;

namespace Api
{
	public class Controller
	{

		public static string getName(string url)
        {
			try
			{
				if (url.Contains(".json") && url.Contains("/"))
				{
					string[] temp = url.Split("/");
					string t = temp[temp.Length - 1];
					t = t.Split(".json")[0];
					return t + ".json";
				}
				else { throw new Exception(); }
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void checkAndDownload(string url, string path, string name)
		{

			// Create MyUrl object and try to pass tests
			MyUrl m_url = new (url);

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

			// Create MyDownloader object and try to pass tests
			MyDownloader m_dl = new (m_url.Url, path + name);

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

			// Try to download
			if (!m_dl.download())
            {
				Console.WriteLine("Błąd konwersji: " + path + name);
				m_dl.Close();
				return;
			}

			// Downloaded Successfuly
			Console.WriteLine("Plik zapisany: " + path + name);
			m_dl.Close();

		}

		public static void StartDownloading(string[] urls, string path, bool use_real_file_name=true)
		{
			List<Thread> threadlist = new ();

			// Add end slash if missing
			path = path[path.Length-1] == '\\' ? path : path + '\\';

			for (int i=0; i<urls.Length; i++)
			{
				string url = urls[i];

				// Get orginal name if possible & use_real_file_name=true
				// else name: download_{index}.json
				string name = getName(url);
				if (name == null || !use_real_file_name)
					name = "download_" + (i + 1).ToString() + ".json";
				
				// Run new thread and add to list
				ThreadStart thread_start = new (() => checkAndDownload(url, path, name));
				Thread thread = new (thread_start)
				{
					Name = name,
					Priority = ThreadPriority.AboveNormal
				};
				thread.Start();
				threadlist.Add(thread);
			}

			// Join Threads to MainThread
			foreach (Thread thread in threadlist)
            {
				thread.Join();
				//Console.WriteLine("Wątek podłączono: " + thread.Name);
            }
		}

	}
}

