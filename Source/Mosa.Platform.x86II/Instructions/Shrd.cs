﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86II.Instructions
{
	/// <summary>
	/// Representations the x86 shrd instruction.
	/// </summary>
	public class Shrd : ThreeOperandInstruction
	{
		#region Data Members

		private static readonly OpCode Register = new OpCode(new byte[] { 0x0F, 0xAD }, 4);
		private static readonly OpCode Constant = new OpCode(new byte[] { 0x0F, 0xAC }, 4);

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (third is RegisterOperand)
				return Register;
			if (third is ConstantOperand)
				return Constant;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="emitter"></param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, context.Operand1, context.Operand2);
			if (context.Operand2 is ConstantOperand)
			{
				ConstantOperand op = context.Operand2 as ConstantOperand;
				op = new ConstantOperand(BuiltInSigType.Byte, op.Value);
				emitter.Emit(opCode, context.Result, context.Operand1, op);
			}
			else
				emitter.Emit(opCode, context.Result, context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Shrd(context);
		}

		#endregion // Methods
	}
}
