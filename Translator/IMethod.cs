// ITranslatedMethod.cs created with MonoDevelop
// User: kthompson at 20:21 01/18/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Translator
{
	public interface IMethod
	{
		MethodDefinition OriginalMethod{ get; }
		IInstruction[] Instructions { get; }
		byte[] GetBytes(long destinationOffset);
	}
}
