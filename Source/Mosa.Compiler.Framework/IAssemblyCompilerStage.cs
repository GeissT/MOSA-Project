/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// This interface represents a stage of compilation of an assembly.
	/// </summary>
	public interface IAssemblyCompilerStage : IPipelineStage
	{
		/// <summary>
		/// Sets up the assembly compiler stage.
		/// </summary>
		/// <param name="compiler">
		/// A <see cref="AssemblyCompiler"/> using the stage.
		/// </param>
		void Setup(AssemblyCompiler compiler);

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void Run();
	}
}
