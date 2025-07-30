using System;

namespace SmartCommunicationForExcel.Utils
{
	/// <summary>
	/// 系统版本类，由三部分组成，包含了一个大版本，小版本，修订版，还有一个开发者维护的内部版<br />
	/// System version class, consisting of three parts, including a major version, minor version, revised version, and an internal version maintained by the developer
	/// </summary>
	[Serializable]
	public sealed class SystemVersion
	{
		private int m_MainVersion = 2;

		private int m_SecondaryVersion = 0;

		private int m_EditVersion = 0;

		private int m_InnerVersion = 0;

		/// <summary>
		/// 修订版
		/// </summary>
		public int EditVersion
		{
			get
			{
				return this.m_EditVersion;
			}
		}

		/// <summary>
		/// 内部版本号，或者是版本号表示为年月份+内部版本的表示方式
		/// </summary>
		public int InnerVersion
		{
			get
			{
				return this.m_InnerVersion;
			}
		}

		/// <summary>
		/// 主版本
		/// </summary>
		public int MainVersion
		{
			get
			{
				return this.m_MainVersion;
			}
		}

		/// <summary>
		/// 次版本
		/// </summary>
		public int SecondaryVersion
		{
			get
			{
				return this.m_SecondaryVersion;
			}
		}

		/// <summary>
		/// 根据格式化字符串的版本号初始化，例如：1.0或1.0.0或1.0.0.0503<br />
		/// Initialize according to the version number of the formatted string, for example: 1.0 or 1.0.0 or 1.0.0.0503
		/// </summary>
		/// <param name="VersionString">格式化的字符串，例如：1.0或1.0.0或1.0.0.0503</param>
		public SystemVersion(string VersionString)
		{
			string[] strArrays = VersionString.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if ((int)strArrays.Length >= 1)
			{
				this.m_MainVersion = Convert.ToInt32(strArrays[0]);
			}
			if ((int)strArrays.Length >= 2)
			{
				this.m_SecondaryVersion = Convert.ToInt32(strArrays[1]);
			}
			if ((int)strArrays.Length >= 3)
			{
				this.m_EditVersion = Convert.ToInt32(strArrays[2]);
			}
			if ((int)strArrays.Length >= 4)
			{
				this.m_InnerVersion = Convert.ToInt32(strArrays[3]);
			}
		}

		/// <summary>
		/// 根据指定的主版本，次版本，修订版来实例化一个对象<br />
		/// Instantiate an object based on the specified major, minor, and revision
		/// </summary>
		/// <param name="main">主版本</param>
		/// <param name="sec">次版本</param>
		/// <param name="edit">修订版</param>
		public SystemVersion(int main, int sec, int edit)
		{
			this.m_MainVersion = main;
			this.m_SecondaryVersion = sec;
			this.m_EditVersion = edit;
		}

		/// <summary>
		/// 根据指定的主版本，次版本，修订版，内部版本来实例化一个对象<br />
		/// Instantiate an object based on the specified major, minor, revision, and build
		/// </summary>
		/// <param name="main">主版本</param>
		/// <param name="sec">次版本</param>
		/// <param name="edit">修订版</param>
		/// <param name="inner">内部版本号</param>
		public SystemVersion(int main, int sec, int edit, int inner)
		{
			this.m_MainVersion = main;
			this.m_SecondaryVersion = sec;
			this.m_EditVersion = edit;
			this.m_InnerVersion = inner;
		}

		/// <summary>
		/// 判断两个实例是否相等
		/// </summary>
		/// <param name="obj">版本号</param>
		/// <returns>是否一致</returns>
		public override bool Equals(object obj)
		{
			return this.Equals(obj);
		}

		/// <summary>
		/// 获取哈希值
		/// </summary>
		/// <returns>哈希值</returns>
		public override int GetHashCode()
		{
			return this.GetHashCode();
		}

		/// <summary>
		/// 判断是否相等
		/// </summary>
		/// <param name="SV1">第一个版本</param>
		/// <param name="SV2">第二个版本</param>
		/// <returns>是否相同</returns>
		public static bool operator ==(SystemVersion SV1, SystemVersion SV2)
		{
			bool flag;
			if (SV1.MainVersion != SV2.MainVersion)
			{
				flag = false;
			}
			else if (SV1.SecondaryVersion != SV2.SecondaryVersion)
			{
				flag = false;
			}
			else if (SV1.m_EditVersion == SV2.m_EditVersion)
			{
				flag = (SV1.InnerVersion == SV2.InnerVersion ? true : false);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		/// <summary>
		/// 判断一个版本是否大于另一个版本
		/// </summary>
		/// <param name="SV1">第一个版本</param>
		/// <param name="SV2">第二个版本</param>
		/// <returns>是否相同</returns>
		public static bool operator >(SystemVersion SV1, SystemVersion SV2)
		{
			bool flag;
			if (SV1.MainVersion > SV2.MainVersion)
			{
				flag = true;
			}
			else if (SV1.MainVersion < SV2.MainVersion)
			{
				flag = false;
			}
			else if (SV1.SecondaryVersion > SV2.SecondaryVersion)
			{
				flag = true;
			}
			else if (SV1.SecondaryVersion < SV2.SecondaryVersion)
			{
				flag = false;
			}
			else if (SV1.EditVersion > SV2.EditVersion)
			{
				flag = true;
			}
			else if (SV1.EditVersion < SV2.EditVersion)
			{
				flag = false;
			}
			else if (SV1.InnerVersion <= SV2.InnerVersion)
			{
				flag = (SV1.InnerVersion >= SV2.InnerVersion ? false : false);
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		/// <summary>
		/// 判断是否不相等
		/// </summary>
		/// <param name="SV1">第一个版本号</param>
		/// <param name="SV2">第二个版本号</param>
		/// <returns>是否相同</returns>
		public static bool operator !=(SystemVersion SV1, SystemVersion SV2)
		{
			bool flag;
			if (SV1.MainVersion != SV2.MainVersion)
			{
				flag = true;
			}
			else if (SV1.SecondaryVersion != SV2.SecondaryVersion)
			{
				flag = true;
			}
			else if (SV1.m_EditVersion == SV2.m_EditVersion)
			{
				flag = (SV1.InnerVersion == SV2.InnerVersion ? false : true);
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		/// <summary>
		/// 判断第一个版本是否小于第二个版本
		/// </summary>
		/// <param name="SV1">第一个版本号</param>
		/// <param name="SV2">第二个版本号</param>
		/// <returns>是否小于</returns>
		public static bool operator <(SystemVersion SV1, SystemVersion SV2)
		{
			bool flag;
			if (SV1.MainVersion < SV2.MainVersion)
			{
				flag = true;
			}
			else if (SV1.MainVersion > SV2.MainVersion)
			{
				flag = false;
			}
			else if (SV1.SecondaryVersion < SV2.SecondaryVersion)
			{
				flag = true;
			}
			else if (SV1.SecondaryVersion > SV2.SecondaryVersion)
			{
				flag = false;
			}
			else if (SV1.EditVersion < SV2.EditVersion)
			{
				flag = true;
			}
			else if (SV1.EditVersion > SV2.EditVersion)
			{
				flag = false;
			}
			else if (SV1.InnerVersion >= SV2.InnerVersion)
			{
				flag = (SV1.InnerVersion <= SV2.InnerVersion ? false : false);
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		/// <summary>
		/// 根据格式化为支持返回的不同信息的版本号<br />
		/// C返回1.0.0.0<br />
		/// N返回1.0.0<br />
		/// S返回1.0
		/// </summary>
		/// <param name="format">格式化信息</param>
		/// <returns>版本号信息</returns>
		public string ToString(string format)
		{
			string str;
			if (format == "C")
			{
				str = string.Format("{0}.{1}.{2}.{3}", new object[] { this.MainVersion, this.SecondaryVersion, this.EditVersion, this.InnerVersion });
			}
			else if (format != "N")
			{
				str = (format != "S" ? this.ToString() : string.Format("{0}.{1}", this.MainVersion, this.SecondaryVersion));
			}
			else
			{
				str = string.Format("{0}.{1}.{2}", this.MainVersion, this.SecondaryVersion, this.EditVersion);
			}
			return str;
		}

		/// <summary>
		/// 获取版本号的字符串形式，如果内部版本号为0，则显示时不携带
		/// </summary>
		/// <returns>版本号信息</returns>
		public override string ToString()
		{
			string str;
			str = (this.InnerVersion != 0 ? this.ToString("C") : this.ToString("N"));
			return str;
		}
	}
}