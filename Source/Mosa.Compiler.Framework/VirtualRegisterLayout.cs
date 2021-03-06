/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class VirtualRegisterLayout
	{

		#region Data members

		private IArchitecture architecture;

		private List<VirtualRegisterOperand> virtualRegisters = new List<VirtualRegisterOperand>();

		private StackLayout stackLayout;

		#endregion // Data members

		#region Properties

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualRegisterLayout"/> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="stackLayout">The stack layout.</param>
		public VirtualRegisterLayout(IArchitecture architecture, StackLayout stackLayout)
		{
			this.architecture = architecture;
			this.stackLayout = stackLayout;
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public VirtualRegisterOperand AllocateVirtualRegister(SigType type)
		{
			int index = virtualRegisters.Count + 1;

			VirtualRegisterOperand virtualRegister = new VirtualRegisterOperand(type, index);

			virtualRegisters.Add(virtualRegister);

			return virtualRegister;
		}


	}
}
