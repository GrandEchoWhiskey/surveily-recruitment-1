using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class UrlTests
	{

		// Url IP checks

		[TestMethod]
		public void Check_on_isIPv6()
		{
			var url = new Api.MyUrl("2001:0db8:85a3::8a2e:0370:7334");

			var result = url.is_UrlIP();

			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void Check_on_isNotIPv6()
		{
			var url = new Api.MyUrl("2001:0db8:85a3:0000:0000:8ak2e::734");

			var result = url.is_UrlIP();

			Assert.AreEqual(false, result);
		}

		[TestMethod]
		public void Check_on_isIPv4()
		{
			var url = new Api.MyUrl("127.0.0.1");

			var result = url.is_UrlIP();

			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void Check_on_isNotIPv4()
		{
			var url = new Api.MyUrl("127.156.112.256");

			var result = url.is_UrlIP();

			Assert.AreEqual(false, result);
		}

		// Url Interface checks in [https, http, ftp]

		[TestMethod]
		public void Check_on_interface()
		{
			var url = new Api.MyUrl("https://somesite.com");

			var result = url.is_UrlInterface();

			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void Check_on_notInterface()
		{
			var url = new Api.MyUrl("ssl://somesite.com");

			var result = url.is_UrlInterface();

			Assert.AreEqual(false, result);
		}

		// Url ping tests with numeric and DNS translation

		[TestMethod]
		public void Check_on_ping_8_8_8_8()
		{
			var url = new Api.MyUrl("8.8.8.8");

			var result = url.is_UrlPing();

			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public void Check_on_ping_portfolio()
		{
			var url = new Api.MyUrl("https://grandechowhiskey.github.io");

			var result = url.is_UrlPing();

			Assert.AreEqual(true, result);
		}

	}

	[TestClass]
	public class DownloaderTests
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
	public class ControllerTests
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
