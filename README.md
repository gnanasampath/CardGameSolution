# CardGameSolution

Install .Net core 3.1

For Mac

1. To download .Net Framework file and install - https://download.visualstudio.microsoft.com/download/pr/46981fb5-7b16-4c5b-bedb-68a479939a41/b0295f7dda05c6ec038fc023168259dd/dotnet-sdk-3.1.412-osx-x64.pkg

2. dotnet is installed it on this path - /usr/local/share/dotnet

For Windows

https://download.visualstudio.microsoft.com/download/pr/c6a860af-a0ec-44d9-95bb-27213e6ae584/f042477c51e9e274bc2df2b3936cc75d/dotnet-runtime-3.1.18-win-x86.exe


Navigate to unit test project and run following commands to add dependencies

1. dotnet add package MSTest.TestFramework

2. dotnet add package Microsoft.NET.Test.Sdk --version 16.9.4

3. dotnet add package coverlet.collector --version 3.1.0

4. dotnet add package MSTest.TestAdapter

Compile project 

	Navigate to Card Game
	Run the following command to compile
	dotnet build CardGame.csproj

Compile Test project

	Navigate to CardGame.TestUnits
	Run the following command to compile
	dotnet build 

Run project 

	Navigate to CardGame/bin/netcoreapp3.1/ 
	Run the following command to run
	dotnet cardgame.dll 

Run Test project

	Navigate to CardGame.UnitTests
	Run the following command to run
	dotnet test
