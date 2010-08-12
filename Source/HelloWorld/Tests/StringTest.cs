using Mosa.Platforms.x86;
using Mosa.Kernel;
using Mosa.Kernel.X86;
using System;

namespace Mosa.HelloWorld.Tests
{
	public static class StringTest
	{
		public static void Test()
		{
			Screen.SetCursor(23, 0);
			Screen.Write("String Test: ");

			PrintResult(ConcatTest1());
			PrintResult(ConcatTest2());
			PrintResult(ConcatTest3());
			PrintResult(SubStringTest());
			PrintResult(IndexOfTest());
			PrintResult(LengthTest());
		}

		public static bool ConcatTest1()
		{
			string part1 = "abc";
			string part2 = "def";
			string ab = "abcdef";
			string combined = string.Concat(part1, part2);

			return String.Equals(combined, ab);
		}

		public static bool ConcatTest2()
		{
			string part1 = "abc";
			string part2 = "def";
			string part3 = "ghi";
			string abc = "abcdefghi";
			string combined = string.Concat(part1, part2, part3);

			return String.Equals(combined, abc);
		}
		
		public static bool ConcatTest3()
		{
			string abcde = "abcddddd";
			string combined = "abc";
			
			for (int i = 0; i < 5; ++i)
				combined = string.Concat(part1, new string ('d', 1));

			return String.Equals(combined, abcde);
		}

		public static bool SubStringTest()
		{
			string main = "abcdefghi";
			string sub1 = main.Substring(6);
			string sub2 = main.Substring(0, 3);

			return string.Equals("ghi", sub1) && string.Equals("abc", sub2);
		}

		public static bool IndexOfTest()
		{
			string main = "abcdefghi";
			return main.IndexOf('c') == 2;
		}

		public static bool LengthTest()
		{
			string main = "123456789";

			return main.Length == 9;
		}

		public static void PrintResult(bool flag)
		{
			byte color = Screen.Color;
			if (flag)
			{
				Screen.Color = Colors.Green;
				Screen.Write("+");
			}
			else
			{
				Screen.Color = Colors.Red;
				Screen.Write("X");
			}
			Screen.Color = color;
		}
	}
}
