using System;
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
					Log.Exception(ex.InnerException, ex, "CacheFolder cannot be found, and an error occured during an attempt to create it");
				}
			}
			else
			{
				//Missing settings file
			}

			Current = Current ?? new Settings();
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
				Log.Exception(ex.InnerException, ex, "An error occured while saving settings");
            }
        }

        
        public Settings ()
        {       
                _CacheFolder = Directory.GetCurrentDirectory() + @"\MyCache";
                _CacheEnabled = true;
                _HackEnabled = true;
                _HackTitleEnabled = true;

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
		[ExportMetadata("Comment", "Enable Cache Functions")]
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
		[ExportMetadata("Comment", "Enable Modding")]
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
		[ExportMetadata("Comment", "Enable Special Handling for TitleCall* and WorldName* Files")]
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
; 0 - No verification；1 - No verification for asset files；2 - Verify all .swf files
; Verification ensures the integrity of cached files，however the game will load slower as the file data is checked with those hosted on the game servers")]
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
