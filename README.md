# LolSharp
A C# wrapper for the League of Legends API

Provides C# methods and objects to easily consume the [League of Legends API](https://developer.riotgames.com) in C#. 

Example:

` var client = new RiotClient("your-api-key-here", RiotRegion.Na);`  
` var drunk7Irishman = client.GetSummoner(25886496);`
