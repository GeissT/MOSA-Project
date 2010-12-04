﻿/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.Numbers
{
	public static class Variations
	{

		public static IEnumerable<int> GetUpTo(int end)
		{
			for (int i = 0; i < end; i++)
				yield return i;
		}

		public static IEnumerable<int> SmallNumbers
		{
			get
			{
				yield return 0;
				yield return 1;
				yield return 2;
				yield return 4;
				yield return 6;
				yield return 7;
				yield return 8;
				yield return 10;
			}
		}

		#region I1 Types

		public static IEnumerable<object[]> I1_I1
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1WithoutZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1Zero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					yield return new object[2] { a, (sbyte)0 };
			}
		}

		public static IEnumerable<object[]> I1_I1AboveZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1BelowZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I1
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (sbyte b in I1.Samples)
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region U1 Types

		public static IEnumerable<object[]> U1_U1
		{
			get
			{
				foreach (byte a in U1.Samples)
					foreach (byte b in U1.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U1_U1WithoutZero
		{
			get
			{
				foreach (byte a in U1.Samples)
					foreach (byte b in U1.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U1_U1Zero
		{
			get
			{
				foreach (byte a in U1.Samples)
					yield return new object[2] { a, (byte)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_U1
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (byte b in U1.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		#endregion

		#region I2 Types

		public static IEnumerable<object[]> I2_I2
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2WithoutZero
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2Zero
		{
			get
			{
				foreach (short a in I2.Samples)
					yield return new object[2] { a, (short)0 };
			}
		}

		public static IEnumerable<object[]> I2_I2AboveZero
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2BelowZero
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I2
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (short b in I2.Samples)
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region U2 Types

		public static IEnumerable<object[]> U2_U2
		{
			get
			{
				foreach (ushort a in U2.Samples)
					foreach (ushort b in U2.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U2_U2WithoutZero
		{
			get
			{
				foreach (ushort a in U2.Samples)
					foreach (ushort b in U2.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U2_U2Zero
		{
			get
			{
				foreach (ushort a in U2.Samples)
					yield return new object[2] { a, (ushort)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_U2
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (ushort b in U2.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		#endregion
	}
}