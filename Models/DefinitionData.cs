namespace UrbanDictionaryDex.Models
{
	/// <summary>
	/// Represent search result of term.
	/// </summary>
	public readonly struct DefinitionData
	{
		public DefinitionData(
			uint defId,
			string word,
			string author,
			string definition, 
			string example,
			string permalink, 
			int thumbsUp, 
			int thumbsDown)
		{
			this.DefId = defId;
			this.Word = word;
			this.Author = author;
			this.Definition = definition;
			this.Example = example;
			this.Permalink = permalink;
			this.ThumbsUp = thumbsUp;
			this.ThumbsDown = thumbsDown;
		}

		/// <summary>
		/// The definition.
		/// </summary>
		public string Definition { get; }

		/// <summary>
		/// The author. (His name)
		/// </summary>
		public string Author { get; }

		/// <summary>
		/// The target word of the definition.
		/// </summary>
		public string Word { get; }

		/// <summary>
		/// The definiton id of this definiton.
		/// </summary>
		public uint DefId { get; }

		/// <summary>
		/// The example for the usage of this word following the definiton.
		/// </summary>
		public string Example { get; }

		/// <summary>
		/// The numbers of thumbs down.
		/// </summary>
		public int ThumbsDown { get; }

		/// <summary>
		/// The number of thumbs up.
		/// </summary>
		public int ThumbsUp { get; }

		/// <summary>
		/// The permalink of the definition.
		/// </summary>
		public string Permalink { get; }
	}
}
