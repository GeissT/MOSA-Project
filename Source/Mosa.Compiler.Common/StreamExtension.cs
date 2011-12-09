/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.Compiler.Common
{
	public static class StreamExtension
	{

		/// <summary>
		/// Writes the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="buffer">The buffer.</param>
		public static void Write(this Stream stream, byte[] buffer)
		{
			stream.Write(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Writes the zero bytes.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="size">The size.</param>
		public static void WriteZeroBytes(this Stream stream, int size)
		{
			for (int i = 0; i < size; i++)
				stream.WriteByte(0);
		}
	}
}
