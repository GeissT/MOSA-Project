﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86II.Intrinsic
{
	/// <summary>
	/// This instruction is used to test intrinsic calls.
	/// </summary>
	public sealed class Test : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			context.SetInstruction(X86.Mov, context.Result, context.Operand1);
		}

		#endregion
	}
}
