using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GameViewer.Extensions
{
	internal static class MethodBaseExtensions
	{
		internal static string GetCurrentMethodFullName(
			this MethodBase methodBase,
			[CallerMemberName] string memberName = null
		)
		{
			if (methodBase == null)
				throw new ArgumentNullException(nameof(methodBase));

			if (memberName == null)
				throw new ArgumentNullException(nameof(memberName));

			var typeFullName = methodBase
				.DeclaringType
				.DeclaringType
				;
			return $"{typeFullName}.{memberName}";
		}
	}
}
