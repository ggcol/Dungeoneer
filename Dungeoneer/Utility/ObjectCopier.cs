﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dungeoneer.Utility
{
	/// <span class="code-SummaryComment"><summary></span>
	/// Provides a method for performing a deep copy of an object.
	/// Binary Serialization is used to perform the copy.
	/// <span class="code-SummaryComment"></summary></span>
	public static class ObjectCopier
	{
		/// <span class="code-SummaryComment"><summary></span>
		/// Perform a deep Copy of the object.
		/// <span class="code-SummaryComment"></summary></span>
		/// <span class="code-SummaryComment"><typeparam name="T">The type of object being copied.</typeparam></span>
		/// <span class="code-SummaryComment"><param name="source">The object instance to copy.</param></span>
		/// <span class="code-SummaryComment"><returns>The copied object.</returns></span>
		public static T Clone<T>(this T source)
		{
			if (!typeof(T).IsSerializable)
			{
				throw new ArgumentException("The type must be serializable.", "source");
			}

			// Don't serialize a null object, simply return the default for that object
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}
	}
}
