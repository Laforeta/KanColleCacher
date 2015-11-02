﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debug = System.Diagnostics.Debug;
using d_f_32.KanColleCacher.Configuration;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;


namespace d_f_32.KanColleCacher
{
    [Serializable]
    public class Settings : INotifyPropertyChanged
    {
        private static string filePath;

        public static Settings Current { get; private set; }

        /// <summary>
        /// Load settings
        /// </summary>
        public static void Load()
        {
			filePath = Directory.GetCurrentDirectory() + @"\Plugins\KanColleCacher.ini";

			if (!File.Exists(filePath))
			{
				var path = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
						"grabacr.net",
						"KanColleViewer",
						"KanColleCacher.ini"
						);
				if (File.Exists(path))
					filePath = path;
			}

			if (File.Exists(filePath))
			{
				var _Parser = ConfigParser.ReadIniFile(filePath);
				Current = _Parser.DeserializeObject<Settings>("Settings");

				try
				{
					Directory.CreateDirectory(Current.CacheFolder);
				}
				catch (Exception ex)
				{
					Current.CacheFolder = Directory.GetCurrentDirectory() + @"\MyCache";
                    Log.Exception(ex.InnerException, ex, "Cannot find or create MyCache folder");
				}
			}
			else
			{
				//设置文件丢失
				try
				{
					
				Current = new Settings();
				if (!Directory.Exists(Current.CacheFolder))
					Directory.CreateDirectory(Current.CacheFolder);
				Save();
				}
				catch (Exception ex)
				{
					Log.Exception("Settings.Load()", ex, "Current.new !ERROR");
					Debug.WriteLine(ex);
				}
			}

			try
			{
				
			Debug.WriteLine("Settings.Current.CacheFolder = " + Current.CacheFolder);
			
			}
			catch (Exception ex)
			{
				Log.Exception("Settings.Load()", ex, "Current== null");
				Debug.WriteLine(ex);
			}
			Current = Current ?? new Settings();
			Debug.Flush();
        }
        
        /// <summary>
        /// Save settings
        /// </summary>
        public static void Save()
        {
            try
            {
				var _Parser = File.Exists(filePath) 
					? ConfigParser.ReadIniFile(filePath) 
					: new ConfigParser();

				_Parser.SerializeObject<Settings>(Current, "Settings");
				_Parser.SaveIniFile(filePath);
            }
            catch (Exception ex)
            {
				Log.Exception(ex.InnerException, ex, "An error has occured whilst saving settings");
            }
        }

        
        public Settings ()
        {       
                _CacheFolder = Directory.GetCurrentDirectory() + @"\MyCache";
                _CacheEnabled = true;
                _HackEnabled = true;
                _HackTitleEnabled = true;
                _HackBookEnabled = false;
                _HackMusicRequestEnabled = false;

                _CacheEntryFiles = 2;
                _CachePortFiles = 2;
                _CacheSceneFiles = 2;
                _CacheResourceFiles = 2;
                _CacheSoundFiles = 2;

				_CheckFiles = 1;
				//_SaveApiStart2 = true;
         }
       


        private string _CacheFolder;
		[ExportMetadata("Comment","Cache Folder Path")]
        public string CacheFolder
        {
            get { return this._CacheFolder; }
            set
            {
                if (this._CacheFolder != value)
                {
                    this._CacheFolder = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _CacheEnabled;
        [ExportMetadata("Comment", "Enable Cache Manager")]
        public bool CacheEnabled
        {
            get { return this._CacheEnabled; }
            set
            {
                if (this._CacheEnabled != value)
                {
                    this._CacheEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _HackEnabled;
		[ExportMetadata("Comment", "Enable Mods")]
		public bool HackEnabled
        {
            get { return this._HackEnabled; }
            set
            {
                if (this._HackEnabled != value)
                {
                    this._HackEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _HackTitleEnabled;
        [ExportMetadata("Comment", "Enable Modding TitleCall* and WorldName* Files")]
		public bool HackTitleEnabled
        {
            get { return this._HackTitleEnabled; }
            set
            {
                if (this._HackTitleEnabled != value)
                {
                    this._HackTitleEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _HackBookEnabled;
		[ExportMetadata("Comment", "Unlock All Ship Cards")]
		public bool HackBookEnabled
        {
            get { return this._HackBookEnabled; }
            set
            {
                if (this._HackBookEnabled != value)
                {
                    this._HackBookEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _HackMusicRequestEnabled;
		[ExportMetadata("Comment", "Enable Special Handling for Jukebox API requests")]
		public bool HackMusicRequestEnabled
        {
            get { return this._HackMusicRequestEnabled; }
            set
            {
                if (this._HackMusicRequestEnabled != value)
                {
                    this._HackMusicRequestEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheEntryFiles;
        public int CacheEntryFiles
        {
            get { return this._CacheEntryFiles; }
            set
            {
                if (this._CacheEntryFiles != value)
                {
                    this._CacheEntryFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CachePortFiles;
        public int CachePortFiles
        {
            get { return this._CachePortFiles; }
            set
            {
                if (this._CachePortFiles != value)
                {
                    this._CachePortFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheSceneFiles;
        public int CacheSceneFiles
        {
            get { return this._CacheSceneFiles; }
            set
            {
                if (this._CacheSceneFiles != value)
                {
                    this._CacheSceneFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheResourceFiles;
        public int CacheResourceFiles
        {
            get { return this._CacheResourceFiles; }
            set
            {
                if (this._CacheResourceFiles != value)
                {
                    this._CacheResourceFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheSoundFiles;
        public int CacheSoundFiles
        {
            get { return this._CacheSoundFiles; }
            set
            {
                if (this._CacheSoundFiles != value)
                {
                    this._CacheSoundFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

		private int _CheckFiles;
		[ExportMetadata("Comment", @"Update check Options
; 0 - No check；1 - Don't check resource files；2 - Check for all .swf files
; If enabled, aache manager will verify that the local copy is the latest version available")]
		public int CheckFiles
		{
			get { return this._CheckFiles; }
			set
			{
				if (this._CheckFiles != value)
				{
					this._CheckFiles = value;
					this.RaisePropertyChanged();
				}
			}
		}

//		private bool _SaveApiStart2;
//		[ExportMetadata("Comment", @"保存 api_start2 通信数据以便生成 舰娘立绘的文件名列表。
//; 只有当缓存文件夹中的 api_start2.dat 不存在时才会进行保存。
//; 这一设置只有在游戏加载时才有效。")]
//		public bool SaveApiStart2
//		{
//			get { return this._SaveApiStart2; }
//			set
//			{
//				if (this._SaveApiStart2 != value)
//				{
//					this._SaveApiStart2 = value;
//					this.RaisePropertyChanged();
//				}
//			}
//		}

		#region PropertyChangedNotifications

		public event PropertyChangedEventHandler PropertyChanged;

		void RaisePropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}
