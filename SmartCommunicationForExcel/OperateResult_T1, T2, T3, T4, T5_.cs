using System;
using System.Runtime.CompilerServices;

namespace SmartCommunicationForExcel
{
	/// <summary>
	/// 操作结果的泛型类，允许带五个用户自定义的泛型对象，推荐使用这个类
	/// </summary>
	/// <typeparam name="T1">泛型类</typeparam>
	/// <typeparam name="T2">泛型类</typeparam>
	/// <typeparam name="T3">泛型类</typeparam>
	/// <typeparam name="T4">泛型类</typeparam>
	/// <typeparam name="T5">泛型类</typeparam>
	public class OperateResult<T1, T2, T3, T4, T5> : OperateResult
	{
		/// <summary>
		/// 用户自定义的泛型数据1
		/// </summary>
		public T1 Content1
		{
			get;
			set;
		}

		/// <summary>
		/// 用户自定义的泛型数据2
		/// </summary>
		public T2 Content2
		{
			get;
			set;
		}

		/// <summary>
		/// 用户自定义的泛型数据3
		/// </summary>
		public T3 Content3
		{
			get;
			set;
		}

		/// <summary>
		/// 用户自定义的泛型数据4
		/// </summary>
		public T4 Content4
		{
			get;
			set;
		}

		/// <summary>
		/// 用户自定义的泛型数据5
		/// </summary>
		public T5 Content5
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