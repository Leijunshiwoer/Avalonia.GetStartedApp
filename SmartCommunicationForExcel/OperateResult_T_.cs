using System;
using System.Runtime.CompilerServices;

namespace SmartCommunicationForExcel
{
	/// <summary>
	/// 操作结果的泛型类，允许带一个用户自定义的泛型对象，推荐使用这个类
	/// </summary>
	/// <typeparam name="T">泛型类</typeparam>
	public class OperateResult<T> : OperateResult
	{
		/// <summary>
		/// 用户自定义的泛型数据
		/// </summary>
		public T Content
		{
			get;
			set;
		}

		/// <summary>
		/// 实例化一个默认的结果对象
		/// </summary>
		public OperateResult()
		{
		}

		/// <summary>
		/// 使用指定的消息实例化一个默认的结果对象
		/// </summary>
		/// <param name="msg">错误消息</param>
		public OperateResult(string msg) : base(msg)
		{
		}

		/// <summary>
		/// 使用错误代码，消息文本来实例化对象
		/// </summary>
		/// <param name="err">错误代码</param>
		/// <param name="msg">错误消息</param>
		public OperateResult(int err, string msg) : base(err, msg)
		{
		}
	}
}