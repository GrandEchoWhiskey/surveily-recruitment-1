using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiTester
{
	[TestClass]
	public class Url
	{

		// Url IP checks

		[TestMethod]
		public void Check_on_IPv6()
		{

			string[] urls_working =
			{
				"2001:0db8:85a3::8a2e:0370:7334",
				"http://2001:0db8::8a2e:0370:7334",
				"https://2001:0db8::0370:7334/test.json"
			};

			string[] urls_notWorking =
			{
				"2001:0db8:85a3:::8a2e:0370:7334",
				"http://2001:vdb8:85a3::8a2e:0370:7334",
				"https://2001:vdb8:85a3::8a2e:0370:7334/test.json"
			};

			foreach (string url in urls_working)
            {
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlIP();
				Assert.IsTrue(result);
            }

			foreach (string url in urls_notWorking)
            {
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlIP();
				Assert.IsFalse(result);
			}

		}

		[TestMethod]
		public void Check_on_IPv4()
		{

			string[] urls_working =
			{
				"192.168.0.1",
				"http://127.23.0.250",
				"https://127.0.0.1/data.json"
			};

			string[] urls_notWorking =
			{
				"192.168.0.256",
				"http://127.23.0.256",
				"https://192.168.0.256/data.json"
			};

			foreach (string url in urls_working)
			{
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlIP();
				Assert.IsTrue(result);
			}

			foreach (string url in urls_notWorking)
			{
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlIP();
				Assert.IsFalse(result);
			}

		}

		// Url Interface checks in [https, http, ftp]

		[TestMethod]
		public void Check_on_Interface()
		{

			string[] urls_working =
			{
				"ftp://192.168.0.1",
				"http://127.23.0.250",
				"https://127.0.0.1/data.json"
			};

			string[] urls_notWorking =
			{
				"ssl://192.168.0.1",
				"http:/127.23.0.250",
				"https//127.0.0.1/data.json"
			};

			foreach (string url in urls_working)
			{
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlInterface();
				Assert.IsTrue(result);
			}

			foreach (string url in urls_notWorking)
			{
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlInterface();
				Assert.IsFalse(result);
			}

		}

		// Url ping tests with numeric and DNS translation

		[TestMethod]
		public void Check_on_Ping()
		{

			string[] urls_working =
			{
				"8.8.8.8",
				"https://grandechowhiskey.github.io",
				"https://127.0.0.1/data.json"
			};

			foreach (string url in urls_working)
			{
				var api_url = new Api.MyUrl(url);
				var result = api_url.is_UrlPing();
				Assert.IsTrue(result);
			}

		}

	}

	[TestClass]
	public class Downloader
	{

		// Download tests

		[TestMethod]
		public void Check_on_Download()
		{
			var full_path = System.IO.Path.GetTempPath() + "test.json";
			var url = "https://grandechowhiskey.github.io/testJson.json";
			var myDl = new Api.MyDownloader(url, full_path);

			var result = myDl.download();

			myDl.Close();
			System.IO.File.Delete(myDl.Path);

			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void Check_on_Download_errorCode()
		{
			var full_path = System.IO.Path.GetTempPath() + "test.json";
			var url = "https://grandechowhiskey.github.io/testJson.jsons";
			var myDl = new Api.MyDownloader(url, full_path);

			var result = myDl.download();

			myDl.Close();
			System.IO.File.Delete(myDl.Path);

			Assert.AreEqual(false, result);
		}

		[TestMethod]
		public void Check_on_Download_errorSave()
		{
			var full_path = "C\\Users\\" + System.Environment.UserName + "\\AppData\\Local\\Temp\\test.json";
			var url = "https://grandechowhiskey.github.io/testJson.json";
			var myDl = new Api.MyDownloader(url, full_path);

			var result = myDl.download();

			myDl.Close();

			Assert.AreEqual(false, result);
		}

		// Stream and Connection open tests

		[TestMethod]
		public void Check_on_FileNotOpen()
		{
			// Wrong Path missing ':' after Disk letter.
			var full_path = "C\\Users\\" + System.Environment.UserName + "\\AppData\\Local\\Temp\\test.json";
			var url = "https://grandechowhiskey.github.io/testJson.json";
			var myDl = new Api.MyDownloader(url, full_path);

			var result = myDl.FileOpen;

			myDl.Close();

			Assert.AreEqual(false, result);
		}

		[TestMethod]
		public void Check_on_ErrorCode()
		{
			var full_path = System.IO.Path.GetTempPath() + "test.json";
			var url = "https://grandechowhiskey.github.io/testJson.jsons";
			var myDl = new Api.MyDownloader(url, full_path);

			var result = myDl.Connected;

			myDl.Close();

			Assert.AreEqual(false, result);
		}

	}

	[TestClass]
	public class Controller
	{

		// Single Thread test

		[TestMethod]
		public void Check_on_DownloadingThread()
		{
			var path = System.IO.Path.GetTempPath();
			var url = "https://grandechowhiskey.github.io/testJson.json";
			var name = "test.json";

			Api.Controller.checkAndDownload(url, path, name);

			try
            {
				System.IO.File.Delete(path + name);
            }
			catch (System.Exception)
            {
				Assert.Fail();
            }
		}

		// Multi Thread test

        [TestMethod]
		public void Check_on_DownloadMultiThread()
        {
			var path = System.IO.Path.GetTempPath();
			var urls = "https://grandechowhiskey.github.io/testJson.json;" +
                "https://grandechowhiskey.github.io/testJson.json";

			Api.Controller.StartDownloading(urls.Split(';'), path, false);

			try
			{
				System.IO.File.Delete(path + "download_1.json");
				System.IO.File.Delete(path + "download_2.json");
			}
			catch (System.Exception)
			{
				Assert.Fail();
			}

		}

		// Get orginal filename

        [TestMethod]
		public void Check_on_getOrginalName()
        {
			var url = "https://testsite.io/test.json";

			var result = Api.Controller.getName(url);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Check_on_getWrongOrginalName()
		{
			var url = "https://testsite.io/test.jsn";

			var result = Api.Controller.getName(url);

			Assert.IsNull(result);
		}

	}

}
