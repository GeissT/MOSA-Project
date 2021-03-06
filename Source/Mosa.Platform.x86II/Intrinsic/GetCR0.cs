﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Mosa.Platform.x86II.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	internal sealed class GetCR0 : GetControlRegisterBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR0"/> class.
		/// </summary>
		public GetCR0()
			: base(ControlRegister.CR0)
		{
		}

	}
}
