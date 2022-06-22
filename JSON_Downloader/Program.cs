using System;
using System.Threading;
using Api;

namespace JSON_Downloader
{
	internal class Program
	{

		static void Main(string[] args)
		{

			// Get list of URLs from input & split them into urls list
			Console.Write("Wprowadź listę adresów URL oddzielone średnikiem (;): ");
			string[] urls = Console.ReadLine().Split(";");

			// Get target directory path
			Console.Write("Wprowadź ścieżkę docelową: ");
			string dir = Console.ReadLine();

			// Loop urls
			for (int i = 0; i < urls.Length; i++)
			{
				string current_url = urls[i];
                Console.WriteLine(current_url);
			}

			Url n = new Url("https://valibyte.com");
			if (n.CheckResponse())
            {
				Console.WriteLine("OK");
            }
			else
            {
				Console.WriteLine("Error");
			}
		}
	}
}
