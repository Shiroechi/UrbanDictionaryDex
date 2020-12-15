using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using UrbanDictionaryDex.Exceptions;
using UrbanDictionaryDex.Models;

namespace UrbanDictionaryDex.Client
{
	/// <summary>
	/// API client for Urban Dictionary.
	/// </summary>
	public class UrbanDictionaryClient
	{
		#region Member

		private HttpClient _HttpClient;
		private readonly bool _Supplied;

		private readonly string _ApiBaseUrl = "https://api.urbandictionary.com/v0/";

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create client instance of Urban Dictionary.
		/// </summary>
		/// <param name="client">
		///		Http client for sending and recieving response.
		///	</param>
		public UrbanDictionaryClient(HttpClient client = null)
		{
			if (client == null)
			{
				this._HttpClient = new HttpClient();
				this._Supplied = false;
			}
			else
			{
				this._HttpClient = client;
				this._Supplied = true;
			}

			this.AddHttpUserAgent();

			ServicePointManager.SecurityProtocol = 
				SecurityProtocolType.Tls12 | 
				SecurityProtocolType.Tls11 | 
				SecurityProtocolType.Tls | 
				SecurityProtocolType.Ssl3;
		}

		/// <summary>
		///		Release all resource that <see cref="UrbanDictionaryClient"/> create.
		/// </summary>
		~UrbanDictionaryClient()
		{
			if (this._Supplied == false)
			{
				this._HttpClient.Dispose();
			}
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <summary>
		///		Get JSON response from url.
		/// </summary>
		/// <typeparam name="T">
		///		The type of the object to deserialize.
		///	</typeparam>
		/// <param name="url">
		///		API URL. 
		/// </param>
		/// <returns>
		///		The instance of <typeparamref name="T"/> being deserialized.
		///	</returns>
		/// <exception cref="HttpResponseException">
		///		Unexpected error occured.
		/// </exception>
		/// <exception cref="HttpRequestException">
		///		The request failed due to an underlying issue such as network connectivity, DNS
		///     failure, server certificate validation or timeout.
		/// </exception>
		/// <exception cref="JsonException">
		///		The JSON is invalid.
		/// </exception>
		protected async Task<T> GetJsonResponseAsync<T>(string url)
		{
			try
			{
				using (var request = new HttpRequestMessage(HttpMethod.Get, url))
				using (var response = await this._HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
				using (var stream = await response.Content.ReadAsStreamAsync())
				{
					if (response.IsSuccessStatusCode)
					{
						try
						{
							return await JsonSerializer.DeserializeAsync<T>(stream);
						}
						catch (JsonException)
						{
							throw;
						}
					}

					throw new HttpResponseException(
						$"Unexpected error occured.\n Status code = { (int)response.StatusCode }\n Reason = { response.ReasonPhrase }.");
				}
			}
			catch (HttpRequestException)
			{
				throw;
			}
		}

		/// <summary>
		/// Read JSON data.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected DefinitionData ReadDefinitionData(JsonElement item)
		{
			return new DefinitionData(
				item.GetProperty("defid").GetUInt32(), 
				item.GetProperty("word").GetString(), 
				item.GetProperty("author").GetString(), 
				item.GetProperty("definition").GetString(), 
				item.GetProperty("example").GetString(), 
				item.GetProperty("permalink").GetString(),
				item.GetProperty("thumbs_up").GetInt32(), 
				item.GetProperty("thumbs_down").GetInt32());
		}

		/// <summary>
		/// Add http user agent if not exist.
		/// </summary>
		protected void AddHttpUserAgent()
		{
			if (this._HttpClient.DefaultRequestHeaders.UserAgent.Count == 0)
			{
				this._HttpClient.DefaultRequestHeaders.Add(
					"User-Agent",
					"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");
			}
		}

		#endregion Protected Method

		#region Public Method

		/// <summary>
		///		Get definitions based on Urban Dictionary terms.
		/// </summary>
		/// <param name="term">
		///		The definition of term you want to know.
		///	</param>
		/// <returns>
		///		Array of <see cref="DefinitionData"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The term is empty or null.
		/// </exception>
		/// <exception cref="HttpResponseException">
		///		Unexpected error occured.
		/// </exception>
		/// <exception cref="HttpRequestException">
		///		The request failed due to an underlying issue such as network connectivity, DNS
		///     failure, server certificate validation or timeout.
		/// </exception>
		/// <exception cref="JsonException">
		///		The JSON is invalid.
		/// </exception>
		public async Task<DefinitionData[]> SearchTerm(string term)
		{
			if (term == null || term.Trim() == "")
			{
				throw new ArgumentNullException(nameof(term), "The term to search can't empty or null.");
			}

			var query = $"{ this._ApiBaseUrl }define?term={ term }";
			
			JsonElement array;

			var definitions = new List<DefinitionData>();

			using (var obj = await this.GetJsonResponseAsync<JsonDocument>(query))
			{
				array = obj.RootElement.GetProperty("list").Clone();
			}

			if (array.GetArrayLength() == 0)
			{
				throw new DefinitioNotFound($"The \"{ term }\" term definition is not found.");
			}

			foreach (var item in array.EnumerateArray())
			{
				definitions.Add(this.ReadDefinitionData(item));
			}

			return definitions.ToArray();
		}

		/// <summary>
		///		Get definitions based on Urban Dictionary terms.
		/// </summary>
		/// <remarks>
		///		If the term is not found, it will continue to get the next term
		///		and not throw a exception.
		/// </remarks>
		/// <param name="terms">
		///		The definition of terms you want to know.
		///	</param>
		/// <returns>
		///		Array of <see cref="DefinitionData"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The term is empty or null.
		/// </exception>
		/// <exception cref="HttpResponseException">
		///		Unexpected error occured.
		/// </exception>
		/// <exception cref="HttpRequestException">
		///		The request failed due to an underlying issue such as network connectivity, DNS
		///     failure, server certificate validation or timeout.
		/// </exception>
		/// <exception cref="JsonException">
		///		The JSON is invalid.
		/// </exception>
		public async Task<DefinitionData[]> SearchTerm(string[] terms)
		{
			if (terms == null || terms.Length <= 0)
			{
				throw new ArgumentNullException(nameof(terms), "The term to search can't empty or null.");
			}

			var definitions = new List<DefinitionData>();

			JsonElement array;

			string query;

			foreach (var term in terms)
			{
				query = $"{ this._ApiBaseUrl }define?term={ term }";

				using (var obj = await this.GetJsonResponseAsync<JsonDocument>(query))
				{
					array = obj.RootElement.GetProperty("list").Clone();
				}

				if (array.GetArrayLength() == 0)
				{
					continue;
				}

				foreach (var item in array.EnumerateArray())
				{
					definitions.Add(this.ReadDefinitionData(item));
				}
			}

			return definitions.ToArray();
		}

		/// <summary>
		///		Get definitions based on Urban Dictionary terms.
		/// </summary>
		/// <param name="id">
		///		The definition id from Urban Dictionary.
		///	</param>
		/// <returns>
		///		A <see cref="DefinitionData"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The term is empty or null.
		/// </exception>
		/// <exception cref="HttpResponseException">
		///		Unexpected error occured.
		/// </exception>
		/// <exception cref="HttpRequestException">
		///		The request failed due to an underlying issue such as network connectivity, DNS
		///     failure, server certificate validation or timeout.
		/// </exception>
		/// <exception cref="JsonException">
		///		The JSON is invalid.
		/// </exception>
		public async Task<DefinitionData> SearchTerm(uint id)
		{
			var query = $"{ this._ApiBaseUrl }define?defid={ id }";

			JsonElement array;

			using (var obj = await this.GetJsonResponseAsync<JsonDocument>(query))
			{
				array = obj.RootElement.GetProperty("list").Clone();
			}

			if (array.GetArrayLength() == 0)
			{
				throw new DefinitioNotFound($"There's no definition with id \"{ id }\".");
			}

			foreach (var item in array.EnumerateArray())
			{
				return this.ReadDefinitionData(item);
			}

			throw new DefinitioNotFound("Unexpected error occured.");
		}

		/// <summary>
		///		Get definitions based on Urban Dictionary terms.
		/// </summary>
		/// <remarks>
		///		If the ids is not found, it will continue to get the next ids
		///		and not throw a exception.
		/// </remarks>
		/// <param name="ids">
		///		The definition id from Urban Dictionary.
		///	</param>
		/// <returns>
		///		Array of <see cref="DefinitionData"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The term is empty or null.
		/// </exception>
		/// <exception cref="HttpResponseException">
		///		Unexpected error occured.
		/// </exception>
		/// <exception cref="HttpRequestException">
		///		The request failed due to an underlying issue such as network connectivity, DNS
		///     failure, server certificate validation or timeout.
		/// </exception>
		/// <exception cref="JsonException">
		///		The JSON is invalid.
		/// </exception>
		public async Task<DefinitionData[]> SearchTerm(uint[] ids)
		{
			if (ids == null || ids.Length <= 0)
			{
				throw new ArgumentNullException(nameof(ids), "The id's to search can't empty or null.");
			}

			var definitions = new List<DefinitionData>();

			JsonElement array;

			string query;

			foreach (var id in ids)
			{
				query = $"{ this._ApiBaseUrl }define?defid={ id }";

				using (var obj = await this.GetJsonResponseAsync<JsonDocument>(query))
				{
					array = obj.RootElement.GetProperty("list").Clone();
				}

				if (array.GetArrayLength() == 0)
				{
					continue;
				}

				foreach (var item in array.EnumerateArray())
				{
					definitions.Add(this.ReadDefinitionData(item));
				}
			}

			return definitions.ToArray();
		}

		/// <summary>
		///		Get 10 random term definition from Urban Dictionary.
		/// </summary>
		/// <returns>
		///		Array of <see cref="DefinitionData"/>.
		/// </returns>
		public async Task<DefinitionData[]> GetRandomTerm()
		{
			var query = $"{ this._ApiBaseUrl }random";

			JsonElement array;

			var definitions = new List<DefinitionData>();

			using (var obj = await this.GetJsonResponseAsync<JsonDocument>(query))
			{
				array = obj.RootElement.GetProperty("list").Clone();
			}

			foreach (var item in array.EnumerateArray())
			{
				definitions.Add(this.ReadDefinitionData(item));
			}

			return definitions.ToArray();
		}

		/// <summary>
		///		Get definitions based on Urban Dictionary terms.
		/// </summary>
		/// <param name="term">
		///		The definition of term you want to know.
		///	</param>
		/// <returns>
		///		Array of <see cref="DefinitionData"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The term is empty or null.
		/// </exception>
		/// <exception cref="HttpResponseException">
		///		Unexpected error occured.
		/// </exception>
		/// <exception cref="HttpRequestException">
		///		The request failed due to an underlying issue such as network connectivity, DNS
		///     failure, server certificate validation or timeout.
		/// </exception>
		/// <exception cref="JsonException">
		///		The JSON is invalid.
		/// </exception>
		public async Task<string[]> AutoCompleteTerm(string term)
		{
			if (term == null || term.Trim() == "")
			{
				throw new ArgumentNullException(nameof(term), "The term to auto complete can't empty or null.");
			}

			var query = $"{ this._ApiBaseUrl }autocomplete?term={ term }";

			JsonElement array;

			var words = new List<string>();

			using (var obj = await this.GetJsonResponseAsync<JsonDocument>(query))
			{
				array = obj.RootElement.Clone();
			}

			if (array.GetArrayLength() == 0)
			{
				throw new DefinitioNotFound($"The \"{ term }\" term have no auto complete.");
			}

			foreach (var item in array.EnumerateArray())
			{
				words.Add(item.GetString());
			}

			return words.ToArray();
		}

		#endregion Public Method
	}
}
