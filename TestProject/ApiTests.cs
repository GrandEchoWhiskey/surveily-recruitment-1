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

		[TestMethod]
		public void Check_on_FileNotOpen()
		{
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
	}

}
