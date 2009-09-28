﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 lgdt instruction.
    /// </summary>
    public sealed class LgdtInstruction : OneOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LgdtInstruction"/> class.
        /// </summary>
        public LgdtInstruction() :
            base()
        {
        }

        #endregion // Construction

        #region OneOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86 lgdt {0} ", context.Operand1);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Lgdt(context);
		}

        #endregion // OneOperandInstruction Overrides
    }
}