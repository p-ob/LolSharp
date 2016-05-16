# LolSharp
A C# wrapper for the League of Legends API

Provides C# methods and objects to easily consume the [League of Legends API](https://developer.riotgames.com) in C#. 

Example:

` var restClient = new RestClient(); // used for dependency injection and unit tests`
` var client = new RiotClient(restClient, "your-api-key-here", RiotRegion.Na);`  
` var drunk7Irishman = client.GetSummoner(25886496);`
