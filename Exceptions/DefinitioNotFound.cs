using System;

namespace UrbanDictionary.Exceptions
{
	public class DefinitioNotFound : Exception
	{
		public DefinitioNotFound(string msg) : base(msg)
		{

		}

		public DefinitioNotFound(string msg, Exception e) : base(msg, e.InnerException)
		{

		}
	}
}
