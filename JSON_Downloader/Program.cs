﻿using System;
using Api;

namespace JSON_Downloader
{
	internal class Program
	{

		static void Main(string[] args)
		{
			// If no args, get the URLs from Console
			string urls;
			if (args.Length == 2)
            {
				urls = args[1];
            }
			else
            {
				Console.Write("Wpisz adresy URL oddzielone średnikiem: ");
				urls = Console.ReadLine();
            }

			// Get target path
			Console.Write("Wpisz ścieżkę zapisu: ");
			string dir = Console.ReadLine();

			// Prepare list and download data
			string[] list = urls.Split(';');
			Api.Controller.StartDownloading(list, dir);
		}
	}
}
