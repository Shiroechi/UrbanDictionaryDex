# UrbanDictionary

 API wrapper for [Urban Dictionary](https://urbandictionary.com/).
 
 [![CodeFactor](https://www.codefactor.io/repository/github/shiroechi/urbandictionarydex/badge?style=for-the-badge)](https://www.codefactor.io/repository/github/shiroechi/urbandictionarydex)
 
# Download

[![Nuget](https://img.shields.io/nuget/v/UrbanDictionaryDex?style=for-the-badge)](https://www.nuget.org/packages/UrbanDictionaryDex/)

# Feature

- Search a definition of term(s).
- Search a definition from Urban Dictionary id(s).
- Get auto complete word from Urban Dictionary.
- Get a random definitions.

# Example

## Search a definition of term or word

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
