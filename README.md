# Echoes of Ruin : Group 7
# Members
Project Manager: Amber Gill ([agill32](https://github.com/agill32))\
Communications Lead: Renee Vo ([vrenee0](https://github.com/vrenee0))\
Git Master: Steven Tan ([stanlsu](https://github.com/stanlsu))\
Design Lead: Amy Tran ([Froggie-A](https://github.com/Froggie-A))\
Quality Assurance Tester: Kashvi Teli ([kashvii](https://github.com/kashvii-1))\
Quality Assurance Tester: Jalen Neverdon ([JJoestar4](https://github.com/JJoestar4))

# About Our Software

Echoes of Ruin is a game created in Unity that features a dungeon, a gacha, and a nurture mechanic. The main forest clearing houses the shop, the gacha, the skin changer, and the nurture. The shop is where you can use your coins to buy items to care for your animal in the nurture, which tracks your stats throughout the game and lowers your animal's needs accordingly-- as well as providing an attack bonus if high enough. The gacha is at the top of the clearing and features a fully animated and functional slot machine, which lets you spin for multiple skins of different rarities. Once won, these skins can be applied at the skin changer in the forest clearing. If you go down, you reach the dungeon which features three enemy types: melee, laser, and scatter. All three of these enemies have their own unique methods of attacking the player and unique tactics. There's also other small quality of life features like a save function and the ability to edit the music and sound volume.

## Platforms Tested on
- MacOS
- Windows

# Important Links
Kanban Board: [[link]](https://github.com/orgs/CSC-3380-Spring-2025/projects/15)\
Designs: [[link]](https://drive.google.com/drive/folders/1ehqeFgpFYq9sNc9cmNqDLMn-iJhndK-l?usp=sharing)\
Styles Guide(s): [[link]](https://google.github.io/styleguide/csharp-style.html)

# How to Run Dev and Test Environment

## Dependencies
- Unity/ Unity Editor (Version 6000.0.37f1)
- MongoDB Shell (Version 2.5.0)
- VS Code
- .Net (Version 9.0)

### Downloading Dependencies

- Unity [[link]](https://unity.com/download)
- MongoDB Shell [[link]](https://www.mongodb.com/try/download/shell)
- VS Code [[link]](https://code.visualstudio.com/Download)
- .Net [[link]](https://dotnet.microsoft.com/en-us/download/dotnet)

## Commands
Cloning repository and checking out the main branch.
Clone the GitHub repository by clicking "<> Code" and coping the line under HTTPS or using the line provided below. Once copied run the command in a new VS Code terminal or powershell. Make sure to change directories (cd) to the place you want the repository to copy:
```sh
git clone https://github.com/CSC-3380-Spring-2025/Team-7.git
```

Once in, make sure you're in the place where you cloned the repository and cd into the Team-7 folder.
```sh
cd Team-7
```

Next cd into the Echoes of Ruin folder.
```sh
cd Echoes of Ruin
```

Checkout the main branch.
```sh
git checkout main
```

Viewing and connecting the Database. After downloading .Net and MongoDB Shell, extract the zip folder. In a new terminal, follow these commands:

Cd into the extracted folder from your Downloads folder. (You might have to cd again into the folder. Depending how it is extracted)
```sh
cd mongosh-2.5.0-win32-x64
```

Make sure that your in the bin folder.
```sh
cd bin
```

Next follow these commands:
```sh
./mongosh
```

For this next command, "YOUR_USERNAME" and "YOUR_ACTUAL_PASSWORD" are placeholders for the actual username and actual password.
```sh
./mongodb+srv://YOUR_USERNAME:YOUR_ACTUAL_PASSWORD@cluster0.qbgga.mongodb.net/
```

```sh
use PlayerData
```

```sh
db.players.find().pretty()
```

## Running the actual project

Downloading Unity:
For Unity, use the download for your operating system (Windows or MacOS). Once downloaded, you will be asked to sign in. You can sign in with your Google account, use an existing Unity account, or make a new account. In the Unity Hub, make sure that the version editor is 6000.0.37f1. If it is not this version, navigate to the "Installs" bar on the side and click "Install Editor". Install version 6000.0.37f1.

Linking the project ot Unity:
Once the installation is finished, go back to the "Projects" Bar and click "Add". "Add the project from disk", and go to where you cloned the repository. Make sure that you open the "Echoes of Ruin" folder. If you try to open the "Team-7" folder, it will pop up as an invalid project. Run the project.

Settings before pressing play:
Once the project is opened, you should see the home screen. If not, go to the scenes folder and open the "Homescreen" scene. Double click the "Game" tab, so the window can take up more of the screen. Make sure it is 16:9 Aspect and the Scale is x1.3.
Press Play at the top. 

Coneecting tot the databasae for save and load functionality:
In your terminal, navigate into the API's project directory: You need to be inside the folder that contains the GameDataApi.csproj file.
Run command line:

windows: 
```sh
cd C:\Users\YourName\Documents\YourProject\GameDataApi
```

Mac/Linux: 
```sh
cd /Users/yourname/Documents/YourProject/GameDataApi
```

After getting in the GameDataApi directory, Initialize User Secrets.
Run command line: 
```sh
dotnet user-secrets init
```

After that, set the secret: This command stores your connection string. Use the exact key name: GameDatabase:ConnectionString
Replace the example connection string in the command below with your complete, correct connection string.
Run command line:
```sh
dotnet user-secrets set "GameDatabase:ConnectionString" "mongodb+srv://YOUR_USERNAME:YOUR_ACTUAL_PASSWORD@yourcluster.mongodb.net/?retryWrites=true&w=majority"
```

After that, make sure you are still in the GameDataApi directory in your terminal.
Run command line:
```sh
dotnet run
```

Code Blocks:

```c#
static void Main(){
	Console.WriteLine("Hello, World!");
	Debug.Log("Hi.")
}
```

```c#
public class MyClass: MonoBehaviour {
	public int Numbers;
	private string numberNames;
}
```



```c#
//This is a comment.
```

