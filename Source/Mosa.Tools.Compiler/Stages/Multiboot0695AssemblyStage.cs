/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;
using System.IO;
using System.Text;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Platform.x86;
using CPUx86 = Mosa.Platform.x86.CPUx86;

namespace Mosa.Tools.Compiler.Stages
{

	/*
	 * FIXME:
	 * - Allow video mode options to be controlled by the command line
	 * - Allow the specification of additional load modules on the command line
	 * - Write the multiboot compliant entry point, which parses the the boot
	 *   information structure and populates the appropriate fields in the 
	 *   KernelBoot entry.
	 */

	/// <summary>
	/// Writes a multiboot v0.6.95 header into the generated binary.
	/// </summary>
	/// <remarks>
	/// This assembly compiler stage writes a multiboot header into the
	/// the data section of the binary file and also creates a multiboot
	/// compliant entry point into the binary.<para/>
	/// The header and entry point written by this stage is compliant with
	/// the specification at 
	/// http://www.gnu.org/software/grub/manual/multiboot/multiboot.html.
	/// </remarks>
	public sealed class Multiboot0695AssemblyStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{
		#region Constants

		/// <summary>
		/// Magic value in the multiboot header.
		/// </summary>
		private const uint HEADER_MB_MAGIC = 0x1BADB002U;

		/// <summary>
		/// Multiboot flag, which indicates that additional modules must be
		/// loaded on page boundaries.
		/// </summary>
		private const uint HEADER_MB_FLAG_MODULES_PAGE_ALIGNED = 0x00000001U;

		/// <summary>
		/// Multiboot flag, which indicates if memory information must be
		/// provided in the boot information structure.
		/// </summary>
		private const uint HEADER_MB_FLAG_MEMORY_INFO_REQUIRED = 0x00000002U;

		/// <summary>
		/// Multiboot flag, which indicates that the supported video mode
		/// table must be provided in the boot information structure.
		/// </summary>
		private const uint HEADER_MB_FLAG_VIDEO_MODES_REQUIRED = 0x00000004U;

		/// <summary>
		/// Multiboot flag, which indicates a non-elf binary to boot and that
		/// settings for the executable file should be read From the boot header
		/// instead of the executable header.
		/// </summary>
		private const uint HEADER_MB_FLAG_NON_ELF_BINARY = 0x00010000U;

		#endregion // Constants

		#region Data members

		private IAssemblyLinker linker;

		/// <summary>
		/// Holds the multiboot video mode.
		/// </summary>
		public uint VideoMode { get; set; }

		/// <summary>
		/// Holds the videoWidth of the screen for the video mode.
		/// </summary>
		public uint VideoWidth { get; set; }

		/// <summary>
		/// Holds the height of the screen for the video mode.
		/// </summary>
		public uint VideoHeight { get; set; }

		/// <summary>
		/// Holds the depth of the video mode in bits per pixel.
		/// </summary>
		public uint VideoDepth { get; set; }

		/// <summary>
		/// Holds true if the second stage is reached
		/// </summary>
		private bool secondStage;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Multiboot0695AssemblyStage"/> class.
		/// </summary>
		public Multiboot0695AssemblyStage()
		{
			VideoMode = 1;
			VideoWidth = 80;
			VideoHeight = 25;
			VideoDepth = 0;
			secondStage = false;
		}

		#endregion // Construction

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);
			linker = RetrieveAssemblyLinkerFromCompiler();

			if (compiler.CompilerOptions.Multiboot.VideoDepth.HasValue)
				this.VideoDepth = compiler.CompilerOptions.Multiboot.VideoDepth.Value;
			if (compiler.CompilerOptions.Multiboot.VideoHeight.HasValue)
				this.VideoHeight = compiler.CompilerOptions.Multiboot.VideoHeight.Value;
			if (compiler.CompilerOptions.Multiboot.VideoMode.HasValue) 
				this.VideoMode = compiler.CompilerOptions.Multiboot.VideoMode.Value;
			if (compiler.CompilerOptions.Multiboot.VideoWidth.HasValue) 
				this.VideoWidth = compiler.CompilerOptions.Multiboot.VideoWidth.Value;
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			if (!secondStage)
			{
				IntPtr entryPoint = WriteMultibootEntryPoint();
				WriteMultibootHeader(entryPoint);
				secondStage = true;
			}
			else
			{
				ITypeInitializerSchedulerStage typeInitializerSchedulerStage = this.compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();

				SigType I4 = BuiltInSigType.Int32;
				RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);
				RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
				RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);

				InstructionSet instructionSet = new InstructionSet(16);
				Context ctx = new Context(instructionSet);

				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I4, 0x200000));
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, ecx.Register, new IntPtr(0x0)), eax);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, ecx.Register, new IntPtr(0x4)), ebx);

				SymbolOperand entryPoint = SymbolOperand.FromMethod(typeInitializerSchedulerStage.Method);

				ctx.AppendInstruction(CPUx86.Instruction.CallInstruction, null, entryPoint);
				ctx.AppendInstruction(CPUx86.Instruction.RetInstruction);

				LinkerGeneratedMethod method = LinkTimeCodeGenerator.Compile(this.compiler, @"MultibootInit", instructionSet, typeSystem);
				this.linker.EntryPoint = this.linker.GetSymbol(method.ToString());
			}
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internals

		/// <summary>
		/// Writes the multiboot entry point.
		/// </summary>
		/// <returns>The virtualAddress of the real entry point.</returns>
		private IntPtr WriteMultibootEntryPoint()
		{
			/*
			 * FIXME:
			 * 
			 * We can't use the standard entry point of the module. Instead
			 * we write a multiboot compliant entry point here, which populates
			 * the boot structure - so that it can be retrieved later.
			 * 
			 * Unfortunately this means, we need to define the boot structure
			 * in the kernel to be able to access it later.
			 * 
			 */

			return IntPtr.Zero;
		}

		private const string MultibootHeaderSymbolName = @"<$>mosa-multiboot-header";

		/// <summary>
		/// Writes the multiboot header.
		/// </summary>
		/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
		private void WriteMultibootHeader(IntPtr entryPoint)
		{
			// HACK: According to the multiboot specification this header must be within the first 8K of the
			// kernel binary. Since the text section is always first, this should take care of the problem.
			using (Stream stream = this.linker.Allocate(MultibootHeaderSymbolName, SectionKind.Text, 64, 4))
			using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII))
			{
				// flags - multiboot flags
				uint flags = /*HEADER_MB_FLAG_VIDEO_MODES_REQUIRED | */HEADER_MB_FLAG_MEMORY_INFO_REQUIRED | HEADER_MB_FLAG_MODULES_PAGE_ALIGNED;
				// The multiboot header checksum 
				uint csum = 0;
				// header_addr is the load virtualAddress of the multiboot header
				uint header_addr = 0;
				// load_addr is the base virtualAddress of the binary in memory
				uint load_addr = 0;
				// load_end_addr holds the virtualAddress past the last byte to load From the image
				uint load_end_addr = 0;
				// bss_end_addr is the virtualAddress of the last byte to be zeroed out
				uint bss_end_addr = 0;
				// entry_point the load virtualAddress of the entry point to invoke
				//uint entry_point = (uint)entryPoint.ToInt32();

				// Are we linking an ELF binary?
				if (!(this.linker is Elf32LinkerStage || this.linker is Elf64LinkerStage))
				{
					// Check the linker layout settings
					if (this.linker.LoadSectionAlignment != this.linker.VirtualSectionAlignment)
						throw new LinkerException(@"Load and virtual section alignment must be identical if you are booting non-ELF binaries with a multiboot bootloader.");

					// No, special multiboot treatment required
					flags |= HEADER_MB_FLAG_NON_ELF_BINARY;

					header_addr = (uint)(this.linker.GetSection(SectionKind.Text).VirtualAddress.ToInt64() + this.linker.GetSymbol(MultibootHeaderSymbolName).SectionAddress);
					load_addr = (uint)this.linker.BaseAddress;
					load_end_addr = 0;
					bss_end_addr = 0;
				}

				// Calculate the checksum
				csum = unchecked(0U - HEADER_MB_MAGIC - flags);

				bw.Write(HEADER_MB_MAGIC);
				bw.Write(flags);
				bw.Write(csum);
				bw.Write(header_addr);
				bw.Write(load_addr);
				bw.Write(load_end_addr);
				bw.Write(bss_end_addr);

				this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, MultibootHeaderSymbolName, (int)stream.Position, 0, @"Mosa.Tools.Compiler.LinkerGenerated.<$>MultibootInit()", IntPtr.Zero);

				bw.Write(VideoMode);
				bw.Write(VideoWidth);
				bw.Write(VideoHeight);
				bw.Write(VideoDepth);
			}
		}

		#endregion // Internals
	}
}