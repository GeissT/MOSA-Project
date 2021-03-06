﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Intrinsics
{

	public sealed class InternalAllocateString : IIntrinsicMethod
	{
		private const string StringClassMethodTableSymbolName = @"System.String$mtable";

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			SymbolOperand callTargetOperand = this.GetInternalAllocateStringCallTarget(typeSystem);
			SymbolOperand methodTableOperand = new SymbolOperand(BuiltInSigType.IntPtr, StringClassMethodTableSymbolName);
			Operand lengthOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IR.IRInstruction.Call, result, callTargetOperand, methodTableOperand, lengthOperand);
		}

		private SymbolOperand GetInternalAllocateStringCallTarget(ITypeSystem typeSystem)
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Internal.Runtime");
			RuntimeMethod callTarget = runtimeType.FindMethod(@"AllocateString");

			return SymbolOperand.FromMethod(callTarget);
		}
	}
}
