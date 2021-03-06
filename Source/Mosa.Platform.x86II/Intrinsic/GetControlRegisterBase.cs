﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using IR = Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86II.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal class GetControlRegisterBase : IIntrinsicMethod
	{

		private ControlRegister control;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetControlRegisterBase"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		protected GetControlRegisterBase(ControlRegister control)
		{
			this.control = control;
		}

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand result = context.Result;

			RegisterOperand imm = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);

			context.SetInstruction(IR.IRInstruction.Move, imm, new RegisterOperand(BuiltInSigType.UInt32, control));
			context.AppendInstruction(IR.IRInstruction.Move, result, imm);
		}

		#endregion // Methods

	}
}
