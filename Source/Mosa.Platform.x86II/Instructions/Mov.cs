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

namespace Mosa.Platform.x86II.Instructions
{
	/// <summary>
	/// Representations the x86 mov instruction.
	/// </summary>
	public sealed class Mov : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0xC7 }, 0); // Move imm32 to r/m32
		private static readonly OpCode M_C = R_C;
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x8B });
		private static readonly OpCode R_R_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode R_R_U8 = new OpCode(new byte[] { 0x88 });
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x8B }); // Move r/m32 to R32
		private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x8B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x89 });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x89 });
		private static readonly OpCode M_R_U8 = new OpCode(new byte[] { 0x88 }); // Move R8 to r/rm8
		private static readonly OpCode R_M_U8 = new OpCode(new byte[] { 0x8A }); // Move r/m8 to R8
		private static readonly OpCode R_CR = new OpCode(new byte[] { 0x0F, 0x20 });
		private static readonly OpCode CR_R = new OpCode(new byte[] { 0x0F, 0x22 });
		private static readonly OpCode SR_R = new OpCode(new byte[] { 0x8E });

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Gets a value indicating whether [result is input].
		/// </summary>
		/// <value>
		///   <c>true</c> if [result is input]; otherwise, <c>false</c>.
		/// </value>
		public override bool ResultIsInput { get { return false; } }

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (destination is RegisterOperand)
				if ((destination as RegisterOperand).Register is ControlRegister) return CR_R;
				else if ((destination as RegisterOperand).Register is SegmentRegister)
					return SR_R;

			if (source is RegisterOperand)
				if ((source as RegisterOperand).Register is ControlRegister) return R_CR;
				else if ((source as RegisterOperand).Register is SegmentRegister)
					throw new ArgumentException(@"TODO: No opcode for move from segment register");

			if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C;
			if ((destination is MemoryOperand) && (source is ConstantOperand)) return M_C;
			if ((destination is RegisterOperand) && (source is LabelOperand)) return R_C;
			if ((destination is MemoryOperand) && (source is LabelOperand)) return M_C;
			if ((destination is RegisterOperand) && (source is SymbolOperand)) return R_C;
			if ((destination is MemoryOperand) && (source is SymbolOperand)) return M_C;

			if ((destination is RegisterOperand) && (source is RegisterOperand))
			{
				if (IsByte(source) || IsByte(destination)) return R_R_U8;
				if (IsChar(source) || IsChar(destination) || IsShort(source) || IsShort(destination)) return R_R_16;
				return R_R;
			}
			if ((destination is RegisterOperand) && (source is MemoryOperand))
			{
				if (IsByte(destination)) return R_M_U8;
				if (IsChar(destination) || IsShort(destination)) return R_M_16;
				return R_M;
			}
			if ((destination is MemoryOperand) && (source is RegisterOperand))
			{
				if (IsByte(destination)) return M_R_U8;
				if (IsChar(destination) || IsShort(destination)) return M_R_16;
				return M_R;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, context.Operand1, null);
			emitter.Emit(opCode, context.Result, context.Operand1);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Mov(context);
		}

		#endregion
	}
}
