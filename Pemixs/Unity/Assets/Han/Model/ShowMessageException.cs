using System;

namespace Remix
{
	public class ShowMessageException : Exception
	{
		public ShowMessageException (string message):base(message){}
		public ShowMessageException (string message, Exception inner) : base(message, inner)
		{
		}
	}
}

