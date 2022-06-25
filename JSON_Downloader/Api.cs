using System;
using System.Threading;
using System.Collections.Generic;

namespace Api
{
	public class Controller
	{

		public static string GetName(string url)
        {
			try
			{
				if (url.Contains(".json") && url.Contains("/"))
				{
					string[] temp = url.Split("/");
					string t = temp[^1];
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

		public static void CheckAndDownload(string url, string path, string name)
		{

			// Create MyUrl object and try to pass tests
			MyUrl m_url = new (url);

			if (!m_url.Is_UrlInterface() && !m_url.Is_UrlIP())
			{
				Console.WriteLine("Podany adres: " + url + " nie może zostać odczytany.");
				return;
			}

			if (!m_url.Is_UrlPing())
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
				m_dl.Remove();
				return;
            }

			if (!m_dl.FileOpen)
            {
				Console.WriteLine("Plik nie może zostać zapisany: " + path + name);
				m_dl.Close();
				m_dl.Remove();
				return;
            }

			// Try to download
			if (!m_dl.Download())
            {
				Console.WriteLine("Błąd konwersji: " + path + name);
				m_dl.Close();
				m_dl.Remove();
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
			path = path[^1] == '\\' ? path : path + '\\';

			for (int i=0; i<urls.Length; i++)
			{
				string url = urls[i];

				// Get orginal name if possible & use_real_file_name=true
				// else name: download_{index}.json
				string name = GetName(url);
				if (name == null || !use_real_file_name)
					name = "download_" + (i + 1).ToString() + ".json";
				
				// Run new thread and add to list
				ThreadStart thread_start = new (() => CheckAndDownload(url, path, name));
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

