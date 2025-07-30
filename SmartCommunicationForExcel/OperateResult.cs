using HslCommunication;
using HslCommunication.Language;
using System;
using System.Runtime.CompilerServices;

namespace SmartCommunicationForExcel
{
	/// <summary>
	/// 操作结果的类，只带有成功标志和错误信息<br />
	/// The class that operates the result, with only success flags and error messages
	/// </summary>
	/// <remarks>
	/// 当 <see cref="P:HslCommunication.OperateResult.IsSuccess" /> 为 True 时，忽略 <see cref="P:HslCommunication.OperateResult.Message" /> 及 <see cref="P:HslCommunication.OperateResult.ErrorCode" /> 的值
	/// </remarks>
	public class OperateResult
	{
		/// <summary>
		/// 具体的错误代码
		/// </summary>
		public int ErrorCode { get; set; } = 10000;

		/// <summary>
		/// 指示本次访问是否成功
		/// </summary>
		public bool IsSuccess
		{
			get;
			set;
		}

		/// <summary>
		/// 具体的错误描述
		/// </summary>
		public string Message { get; set; } = StringResources.Language.UnknownError;

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
		public OperateResult(string msg)
		{
			this.Message = msg;
		}

		/// <summary>
		/// 使用错误代码，消息文本来实例化对象
		/// </summary>
		/// <param name="err">错误代码</param>
		/// <param name="msg">错误消息</param>
		public OperateResult(int err, string msg)
		{
			this.ErrorCode = err;
			this.Message = msg;
		}

		/// <summary>
		/// 从另一个结果类中拷贝错误信息
		/// </summary>
		/// <typeparam name="TResult">支持结果类及派生类</typeparam>
		/// <param name="result">结果类及派生类的对象</param>
		public void CopyErrorFromOther<TResult>(TResult result)
		where TResult : OperateResult
		{
			if (result != null)
			{
				this.ErrorCode = result.ErrorCode;
				this.Message = result.Message;
			}
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T">目标数据类型</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T> CreateFailedResult<T>(OperateResult result)
		{
			return new OperateResult<T>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2> CreateFailedResult<T1, T2>(OperateResult result)
		{
			return new OperateResult<T1, T2>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3> CreateFailedResult<T1, T2, T3>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4> CreateFailedResult<T1, T2, T3, T4>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <typeparam name="T5">目标数据类型五</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4, T5> CreateFailedResult<T1, T2, T3, T4, T5>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4, T5>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <typeparam name="T5">目标数据类型五</typeparam>
		/// <typeparam name="T6">目标数据类型六</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6> CreateFailedResult<T1, T2, T3, T4, T5, T6>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <typeparam name="T5">目标数据类型五</typeparam>
		/// <typeparam name="T6">目标数据类型六</typeparam>
		/// <typeparam name="T7">目标数据类型七</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <typeparam name="T5">目标数据类型五</typeparam>
		/// <typeparam name="T6">目标数据类型六</typeparam>
		/// <typeparam name="T7">目标数据类型七</typeparam>
		/// <typeparam name="T8">目标数据类型八</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <typeparam name="T5">目标数据类型五</typeparam>
		/// <typeparam name="T6">目标数据类型六</typeparam>
		/// <typeparam name="T7">目标数据类型七</typeparam>
		/// <typeparam name="T8">目标数据类型八</typeparam>
		/// <typeparam name="T9">目标数据类型九</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
		/// </summary>
		/// <typeparam name="T1">目标数据类型一</typeparam>
		/// <typeparam name="T2">目标数据类型二</typeparam>
		/// <typeparam name="T3">目标数据类型三</typeparam>
		/// <typeparam name="T4">目标数据类型四</typeparam>
		/// <typeparam name="T5">目标数据类型五</typeparam>
		/// <typeparam name="T6">目标数据类型六</typeparam>
		/// <typeparam name="T7">目标数据类型七</typeparam>
		/// <typeparam name="T8">目标数据类型八</typeparam>
		/// <typeparam name="T9">目标数据类型九</typeparam>
		/// <typeparam name="T10">目标数据类型十</typeparam>
		/// <param name="result">之前的结果对象</param>
		/// <returns>带默认泛型对象的失败结果类</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateFailedResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(OperateResult result)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
			{
				ErrorCode = result.ErrorCode,
				Message = result.Message
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有一个参数对象
		/// </summary>
		/// <param name="value">类型的值对象</param>
		/// <returns>成功的结果对象</returns>
		public static OperateResult<dynamic> CreateSuccessDynamic(dynamic value)
		{
			return new OperateResult<object>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content = value
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象
		/// </summary>
		/// <returns>成功的结果对象</returns>
		public static OperateResult CreateSuccessResult()
		{
			return new OperateResult()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有一个参数对象
		/// </summary>
		/// <typeparam name="T">参数类型</typeparam>
		/// <param name="value">类型的值对象</param>
		/// <returns>成功的结果对象</returns>
		public static OperateResult<T> CreateSuccessResult<T>(T value)
		{
			return new OperateResult<T>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content = value
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有两个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2> CreateSuccessResult<T1, T2>(T1 value1, T2 value2)
		{
			return new OperateResult<T1, T2>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有三个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3> CreateSuccessResult<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
		{
			return new OperateResult<T1, T2, T3>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有四个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4> CreateSuccessResult<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
		{
			return new OperateResult<T1, T2, T3, T4>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有五个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <typeparam name="T5">第五个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <param name="value5">类型五对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4, T5> CreateSuccessResult<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
		{
			return new OperateResult<T1, T2, T3, T4, T5>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4,
				Content5 = value5
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有六个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <typeparam name="T5">第五个参数类型</typeparam>
		/// <typeparam name="T6">第六个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <param name="value5">类型五对象</param>
		/// <param name="value6">类型六对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6> CreateSuccessResult<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4,
				Content5 = value5,
				Content6 = value6
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有七个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <typeparam name="T5">第五个参数类型</typeparam>
		/// <typeparam name="T6">第六个参数类型</typeparam>
		/// <typeparam name="T7">第七个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <param name="value5">类型五对象</param>
		/// <param name="value6">类型六对象</param>
		/// <param name="value7">类型七对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4,
				Content5 = value5,
				Content6 = value6,
				Content7 = value7
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有八个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <typeparam name="T5">第五个参数类型</typeparam>
		/// <typeparam name="T6">第六个参数类型</typeparam>
		/// <typeparam name="T7">第七个参数类型</typeparam>
		/// <typeparam name="T8">第八个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <param name="value5">类型五对象</param>
		/// <param name="value6">类型六对象</param>
		/// <param name="value7">类型七对象</param>
		/// <param name="value8">类型八对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4,
				Content5 = value5,
				Content6 = value6,
				Content7 = value7,
				Content8 = value8
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有九个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <typeparam name="T5">第五个参数类型</typeparam>
		/// <typeparam name="T6">第六个参数类型</typeparam>
		/// <typeparam name="T7">第七个参数类型</typeparam>
		/// <typeparam name="T8">第八个参数类型</typeparam>
		/// <typeparam name="T9">第九个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <param name="value5">类型五对象</param>
		/// <param name="value6">类型六对象</param>
		/// <param name="value7">类型七对象</param>
		/// <param name="value8">类型八对象</param>
		/// <param name="value9">类型九对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4,
				Content5 = value5,
				Content6 = value6,
				Content7 = value7,
				Content8 = value8,
				Content9 = value9
			};
		}

		/// <summary>
		/// 创建并返回一个成功的结果对象，并带有十个参数对象
		/// </summary>
		/// <typeparam name="T1">第一个参数类型</typeparam>
		/// <typeparam name="T2">第二个参数类型</typeparam>
		/// <typeparam name="T3">第三个参数类型</typeparam>
		/// <typeparam name="T4">第四个参数类型</typeparam>
		/// <typeparam name="T5">第五个参数类型</typeparam>
		/// <typeparam name="T6">第六个参数类型</typeparam>
		/// <typeparam name="T7">第七个参数类型</typeparam>
		/// <typeparam name="T8">第八个参数类型</typeparam>
		/// <typeparam name="T9">第九个参数类型</typeparam>
		/// <typeparam name="T10">第十个参数类型</typeparam>
		/// <param name="value1">类型一对象</param>
		/// <param name="value2">类型二对象</param>
		/// <param name="value3">类型三对象</param>
		/// <param name="value4">类型四对象</param>
		/// <param name="value5">类型五对象</param>
		/// <param name="value6">类型六对象</param>
		/// <param name="value7">类型七对象</param>
		/// <param name="value8">类型八对象</param>
		/// <param name="value9">类型九对象</param>
		/// <param name="value10">类型十对象</param>
		/// <returns>成的结果对象</returns>
		public static OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateSuccessResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)
		{
			return new OperateResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
			{
				IsSuccess = true,
				ErrorCode = 0,
				Message = StringResources.Language.SuccessText,
				Content1 = value1,
				Content2 = value2,
				Content3 = value3,
				Content4 = value4,
				Content5 = value5,
				Content6 = value6,
				Content7 = value7,
				Content8 = value8,
				Content9 = value9,
				Content10 = value10
			};
		}

		/// <summary>
		/// 获取错误代号及文本描述
		/// </summary>
		/// <returns>包含错误码及错误消息</returns>
		public string ToMessageShowString()
		{
			return string.Format("{0}:{1}{2}{3}:{4}", new object[] { StringResources.Language.ErrorCode, this.ErrorCode, Environment.NewLine, StringResources.Language.TextDescription, this.Message });
		}
	}
}