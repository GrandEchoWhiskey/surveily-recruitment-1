using System;
using System.Net;
using System.IO;
using System.Text;

namespace Api
{
	public class MyDownloader
	{
		private readonly string _url;
		private readonly string _path;
		private readonly bool _already_exist;
		private HttpWebResponse _response;
		private FileStream _fstream;

		public MyDownloader(string url, string full_path)
		{
			this._url = url;
			this._path = full_path;
			this._already_exist = checkIfExist(full_path);
			this.Setup_Connection();
			this.Setup_File();
		}

		~MyDownloader()
		{
			this.Close();
		}

		public void Close()
        {
			this.Close_File();
			this.Close_Connection();
		}

		public void Remove()
		{
            try
            {
				File.Delete(this._path);
            }
			catch (Exception) { }
		}

		public bool Download()
        {
			if (!this.Connected || !this.FileOpen || this._already_exist)
				return false;

			Stream response_stream;
			try
            {
				response_stream = this._response.GetResponseStream();
			}
			catch (Exception)
			{
				return false;
			}

			StreamReader stream_reader;
            try
            {
				stream_reader = new StreamReader(response_stream);
            }
			catch (Exception)
			{
				response_stream.Close();
				return false;
			}

			try
            {
				// Copy bytes from respone stream to file stream
				byte[] data_byte_array = new UTF8Encoding(true).GetBytes(stream_reader.ReadToEnd());
				this._fstream.Write(data_byte_array, 0, data_byte_array.Length);
			}
			catch (Exception)
			{
				stream_reader.Close();
				response_stream.Close();
				return false;
			}

			stream_reader.Close();
			response_stream.Close();
			return true;
			
        }

		private bool checkIfExist(string path)
        {
			return File.Exists(path);
        }

		private void Setup_Connection()
        {
			try
			{
				HttpWebRequest m_request = (HttpWebRequest)WebRequest.Create(this._url);
				this._response = (HttpWebResponse)m_request.GetResponse();
			}
			catch (Exception) { }
		}

		private void Setup_File()
        {
            try
            {
				this._fstream = File.Create(this._path);
            }
			catch (Exception) { }
        }

		private void Close_Connection()
        {
			if (!this.Connected) return;
			try
            {
				this._response.Close();
            }
			catch (Exception) { }
			this._response = null;
		}

		private void Close_File()
        {
			if (!this.FileOpen) return;
			try
            {
				this._fstream.Close();
			}
			catch(Exception) { }
			this._fstream = null;
        }

		public string Url { get { return _url; } }
		public string Path { get { return _path; } }
		public bool Connected { get { return this._response != null; } }
		public bool FileOpen { get { return this._fstream != null; } }
	}
}

