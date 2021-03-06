﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86II.Instructions
{
	/// <summary>
	/// Representation the x86 jump instruction.
	/// </summary>
	public sealed class Jmp : X86Instruction
	{

		#region Data Members

		private static readonly byte[] JMP = new byte[] { 0xE9 };
		private static readonly OpCode JmpReg = new OpCode(new byte[] { 0xFF }, 4);

		#endregion
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Jmp"/>.
		/// </summary>
		public Jmp() :
			base(1, 0)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			Operand destinationOperand = context.Operand1;
			SymbolOperand destinationSymbol = destinationOperand as SymbolOperand;

			if (destinationSymbol != null)
			{
				emitter.WriteByte(0xE9);
				emitter.Call(destinationSymbol);
			}
			else
				if (context.Operand1 is RegisterOperand)
					emitter.Emit(JmpReg, context.Operand1);
				else
					emitter.EmitBranch(JMP, context.BranchTargets[0]);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Add(context);
		}

		#endregion // Methods

	}
}
