# UrbanDictionary
 API wrapper for urbandictionary.com

# Download
- [Nuget](https://www.nuget.org/packages/UrbanDictionaryDex/)

# Feature
- Search a definition of term(s).
- Search a definition from Urban Dictionary id(s).
- Get auto complete word from Urban Dictionary.
- Get a random definitions.

# Example

```C#
var client = new UrbanDictionaryClient();
var results = await client.SearchTerm("hentai");
foreach(var item in results)
{
    Console.WriteLine(item.DefId);
    Console.WriteLine(item.Word);
    Console.WriteLine(item.Definition);
    Console.WriteLine();
}
```
