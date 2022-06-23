using System;
using System.Net;
using System.IO;
using System.Text;

namespace Api
{
	public class MyDownloader
	{
		private string _url;
		private string _path;
		private HttpWebResponse _response;
		private FileStream _fstream;

		public MyDownloader(string url, string full_path)
		{
			this._url = url;
			this._path = full_path;
			this.setup_Connection();
			this.setup_File();
		}

		~MyDownloader()
		{
			this.Close();
		}

		public void Close()
        {
			this.close_File();
			this.close_Connection();
		}

		public bool download()
        {
			if (!this.Connected || !this.FileOpen)
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

		private void setup_Connection()
        {
			try
			{
				HttpWebRequest m_request = (HttpWebRequest)WebRequest.Create(this._url);
				this._response = (HttpWebResponse)m_request.GetResponse();
			}
			catch (Exception) { }
		}

		private void setup_File()
        {
            try
            {
				this._fstream = File.Create(this._path);
            }
			catch (Exception) { }
        }

		private void close_Connection()
        {
			if (!this.Connected) return;
			try
            {
				this._response.Close();
            }
			catch (Exception) { }
		}

		private void close_File()
        {
			if (!this.FileOpen) return;
			try
            {
				this._fstream.Close();
			}
			catch(Exception) { }
        }

		public string Url { get { return _url; } }
		public string Path { get { return _path; } }
		public bool Connected { get { return this._response != null; } }
		public bool FileOpen { get { return this._fstream != null; } }
	}
}

