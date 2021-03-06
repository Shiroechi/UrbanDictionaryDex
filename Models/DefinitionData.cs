﻿using System.Text.Json.Serialization;

namespace UrbanDictionaryDex.Models
{
	/// <summary>
	/// Represent search result of term.
	/// </summary>
	public readonly struct DefinitionData
	{
		public DefinitionData(
			uint id,
			string word,
			string author,
			string definition, 
			string example,
			string permalink, 
			int thumbsUp, 
			int thumbsDown)
		{
			this.Id = id;
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
		[JsonPropertyName("definition")]
		public string Definition { get; }

		/// <summary>
		/// The author.
		/// </summary>
		[JsonPropertyName("author")]
		public string Author { get; }

		/// <summary>
		/// The target word of the definition.
		/// </summary>
		[JsonPropertyName("word")]
		public string Word { get; }

		/// <summary>
		/// The definiton id of this definiton.
		/// </summary>
		[JsonPropertyName("defid")]
		public uint Id { get; }

		/// <summary>
		/// The example for the usage of this word following the definiton.
		/// </summary>
		[JsonPropertyName("example")]
		public string Example { get; }

		/// <summary>
		/// The numbers of thumbs down.
		/// </summary>
		[JsonPropertyName("thumbs_down")]
		public int ThumbsDown { get; }

		/// <summary>
		/// The number of thumbs up.
		/// </summary>
		[JsonPropertyName("thumbs_up")]
		public int ThumbsUp { get; }

		/// <summary>
		/// The permalink of the definition.
		/// </summary>
		[JsonPropertyName("permalink")]
		public string Permalink { get; }
	}
}
