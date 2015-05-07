using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Debug = System.Diagnostics.Debug;

namespace d_f_32.KanColleCacher
{
	enum Direction
	{
		Discharge_Response,		//Local cache file is absent -> Ignore request and send HTTP GET directly to game server
		Return_LocalFile,		//Local cache file is present -> Return request with files stored locally
		Verify_LocalFile,		//Local cache file is present but needs to be verified -> Query game server for metadata
	}

    class FiddlerRules
    {
        static CacheCore cache;
		
		static public void Initialize ()
		{
			cache = new CacheCore();
			
			_AppendToFiddler();
		}
		
        static void _AppendToFiddler()
        {
            FiddlerApplication.BeforeRequest += _BeforeRequest;
            FiddlerApplication.BeforeResponse += _BeforeResponse;
            FiddlerApplication.AfterSessionComplete += _AfterComplete;
        }

		static bool _Filter(Session oSession)
		{
			return oSession.PathAndQuery.StartsWith("/kcs/");
		}

        #region     Fiddler handling of client requests
        //This event fires when a client request is received by Fiddler
        static void _BeforeRequest(Session oSession)
        {
            if (!Settings.Current.CacheEnabled) return;
			if (!_Filter(oSession)) return;

            string filepath;
			var direction = cache.GotNewRequest(oSession.fullUrl, out filepath);
			
			if (direction == Direction.Return_LocalFile)
			{
				//Return local files
				oSession.utilCreateResponseAndBypassServer();
				oSession.ResponseBody = File.ReadAllBytes(filepath);
				_CreateResponseHeader(oSession, filepath);

				//Debug.WriteLine("CACHR> 【ReturnLocal】" + filepath);
			}
			else if (direction == Direction.Verify_LocalFile)
			{
				//Query server for metadata
				//oSession.oRequest.headers["If-Modified-Since"] = result;
				oSession.oRequest.headers["If-Modified-Since"] = _GetModifiedTime(filepath);
				oSession.bBufferResponse = true;

				//Debug.WriteLine("CACHR> 【FileVerify】" + oSession.PathAndQuery);
			}
			else 
			{ 
				//Do nothing and forward client request to game server
			}
        }
        #endregion

        #region     Fiddler handling of server responses
        //This event fires when a server response is received by Fiddler
        static void _BeforeResponse(Session oSession)
        {
			if (!_Filter(oSession)) return;
			if (oSession.responseCode == 304)
			{
				string filepath = TaskRecord.GetAndRemove(oSession.fullUrl);
				//Only modify headers for those files mentioned in TaskRecord
				if (!string.IsNullOrEmpty(filepath))
				{
					//If response HTTP 304 is received, then the local file is up to date and can be used 
					oSession.bBufferResponse = true;
					oSession.ResponseBody = File.ReadAllBytes(filepath);
					oSession.oResponse.headers.HTTPResponseCode = 200;
					oSession.oResponse.headers.HTTPResponseStatus = "200 OK";
					oSession.oResponse.headers["Last-Modified"] = oSession.oRequest.headers["If-Modified-Since"];
					oSession.oResponse.headers["Accept-Ranges"] = "bytes";
					oSession.oResponse.headers.Remove("If-Modified-Since");
					oSession.oRequest.headers.Remove("If-Modified-Since");
					if (filepath.EndsWith(".swf"))
						oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";

					//Debug.WriteLine(oSession.oResponse.headers.ToString());
					//Debug.WriteLine("");
					//Debug.WriteLine("CACHR> 【HTTP 304】" + oSession.PathAndQuery);
					//Debug.WriteLine("		" + filepath);
				}
			}
        }
        #endregion

        #region     Actions before closing a HTTP session
        static void _AfterComplete(Session oSession)
        {
            if (!Settings.Current.CacheEnabled) return;
			if (!_Filter(oSession)) return;

			//Download new files following a HTTP 200 response 
			if (oSession.responseCode == 200)
			{
				string filepath = TaskRecord.GetAndRemove(oSession.fullUrl);

				//Only write files mentioned in TaskRecord to disk
				if (!string.IsNullOrEmpty(filepath))
				{
					if (File.Exists(filepath))
						File.Delete(filepath);

					//Save file in response and write "Last-Modified" Metadata
					try
					{
						oSession.SaveResponseBody(filepath);
						//cache.RecordNewModifiedTime(oSession.fullUrl,
						//	oSession.oResponse.headers["Last-Modified"]);
						_SaveModifiedTime(filepath, oSession.oResponse.headers["Last-Modified"]);
						//Debug.WriteLine("CACHR> 【FileDownloaded】" + oSession.PathAndQuery);
					}
					catch (Exception ex)
					{
						Log.Exception(oSession, ex, "An error occured while writing downloaded file to disk");
					}
				}
			}
        }
        #endregion

		static void _CreateResponseHeader(Session oSession, string filename)
		{
			oSession.oResponse.headers["Server"] = "Apache";
			oSession.oResponse.headers["Cache-Control"] = "max-age=18000, public";
			oSession.oResponse.headers["Date"] = GMTHelper.ToGMTString(DateTime.Now);
			oSession.oResponse.headers["Connection"] = "close";
			oSession.oResponse.headers["Accept-Ranges"] = "bytes";
			filename.ToLower();
			if (filename.EndsWith(".swf"))
				oSession.oResponse.headers["Content-Type"] = "application/x-shockwave-flash";
			else if (filename.EndsWith(".mp3"))
				oSession.oResponse.headers["Content-Type"] = "audio/mpeg";
			else if (filename.EndsWith(".png"))
				oSession.oResponse.headers["Content-Type"] = "image/png";
			
		}




		static void _SaveModifiedTime(string filepath, string gmTime)
		{
			FileInfo fi;
			try 
			{
				fi = new FileInfo(filepath);
				fi.LastWriteTime = GMTHelper.GMT2Local(gmTime);
			}
			catch (Exception ex)
			{
				Log.Exception("FiddlerRules", ex, "An error occured while saving file metadata");
			}
		}

		static string _GetModifiedTime(string filepath)
		{
			FileInfo fi;
			try
			{
				fi = new FileInfo(filepath);
				return GMTHelper.ToGMTString(fi.LastWriteTime);
			}
			catch (Exception ex)
			{
                Log.Exception("FiddlerRules", ex, "An error occured while saving file metadata");
				return "";
			}
		}

        #region Unused code
        //
        //_BeforeResponse = {
        //        oSession.utilDecodeResponse();
        //        oSession.ResponseBody = File.ReadAllBytes(filename);
        //        oSession.oResponse.headers.HTTPResponseCode = 200;
        //        oSession.oResponse.headers.HTTPResponseStatus = "200 OK";
        //    }
        //
        //_BeforeRequest = {
        //        oSession.bBufferResponse = true;
        //    }

        #endregion

    }
}
