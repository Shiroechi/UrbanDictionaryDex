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

## Search a definition of multiple term or word at once

```C#
var client = new UrbanDictionaryClient();
var results = await client.SearchTerm(new string[] { "hentai", "anime" });
foreach(var item in results)
{
    Console.WriteLine(item.DefId);
    Console.WriteLine(item.Word);
    Console.WriteLine(item.Definition);
    Console.WriteLine();
}
```

## Get a definition from id

```C#
var client = new UrbanDictionaryClient();
var results = await client.SearchTerm(13675580);
foreach(var item in results)
{
    Console.WriteLine(item.DefId);
    Console.WriteLine(item.Word);
    Console.WriteLine(item.Definition);
    Console.WriteLine();
}
```

## Get a definition from multiple id at once

```C#
var client = new UrbanDictionaryClient();
var results = await client.SearchTerm(new uint[] { 13675580, 13675581 });
foreach(var item in results)
{
    Console.WriteLine(item.DefId);
    Console.WriteLine(item.Word);
    Console.WriteLine(item.Definition);
    Console.WriteLine();
}
```

## Get random definition

```C#
var client = new UrbanDictionaryClient();
var results = await client.GetRandomTerm();
foreach(var item in results)
{
    Console.WriteLine(item.DefId);
    Console.WriteLine(item.Word);
    Console.WriteLine(item.Definition);
    Console.WriteLine();
}
```

