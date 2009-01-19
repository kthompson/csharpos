// IInstruction.cs created with MonoDevelop
// User: kthompson at 20:26 01/18/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Translator
{
	public interface IInstruction
	{
		ITranslator Translator{ get; }
		IMethod Method { get; }
		long Offset { get; }
		byte[] GetBytes(long methodOffset);
	}
}
