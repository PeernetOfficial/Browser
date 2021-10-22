[![Deploy Browser](https://github.com/PeernetOfficial/Browser/actions/workflows/deploy-Browser.yml/badge.svg)](https://github.com/PeernetOfficial/Browser/actions/workflows/deploy-Browser.yml)

# Peernet Browser
This is official GUI for [Peernet Command Line Client](https://github.com/PeernetOfficial/Cmd). It has been design to deliver client's capabilities in user friendly manner. 
It is built on to of .NET 5.0 with use of WPF UI Framework [WPF documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-5.0). 

## First boot {#first-boot}
Prior to first boot following steps are required:
#### Step 1 (.NET SDK)
In order to use generic driver for .NET CLI, .NET SDK needs to be installed.
.NET SDK installer can be downloaded from [Official Website](https://dotnet.microsoft.com/download/dotnet/5.0)

#### Step 2 (Clone the repository)
Use GIT CLI to clone the repository

```
git clone https://github.com/PeernetOfficial/Browser.git
```

#### Step 3 (Install VisualStudio) [Optional]{#step3}
This step is optional. It is not required to be able to **build** and **run** solution although VisualStudio as IDE provides numerous features besides obvious ability to edit, build and run the solution.

Peernet Browser is written with the latest as of now .NET 5.0. It requires [VisualStudio 2019](https://visualstudio.microsoft.com/pl/vs/) version or newer to be able to load.
There is **Community** version that is free of charge for non-commercial use.

#### Step 4 (Modify application settings)
Currently you can modify application settings only directly from the settings file (\Peernet.Browser.WPF\Properties\**Settings.settings**).
There are following settings that should be modified:
- **ApiUrl:** URL on which Peernet Command Line Client is hosted
- **SocketUrl:** URL on which Peernet Command Line Client socket is opened
- **CmdPath:** Path to the Peernet Command Line Client build output/working directory.
- **DownloadPath:** Path to the directory to which files will be downloaded.

#### Step 5 (Build solution)
Solution can be built in few ways.  
One way is to use Visual Studio (See [Step 3](#step3)).
The other way is to use [**dotnet CLI**](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).

Open command prompt (cmd.exe on Windows) and navigate to solution location (file with *.sln extension).  
Run following command:
```
dotnet build
```

Expected output:
```
D:\Sources\Peernet\Browser>dotnet build
Microsoft (R) Build Engine version 16.10.2+857e5a733 for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored D:\Sources\Peernet\Browser\Peernet.Browser.WPF\Peernet.Browser.WPF.csproj (in 290 ms).
  4 of 5 projects are up-to-date for restore.
  Peernet.Browser.Models -> D:\Sources\Peernet\Browser\Peernet.Browser.Models\bin\Debug\net5.0\Peernet.Browser.Models.dll
  Peernet.Browser.Application -> D:\Sources\Peernet\Browser\Peernet.Browser.Application\bin\Debug\net5.0\Peernet.Browser.Application.dll
  Peernet.Browser.Infrastructure -> D:\Sources\Peernet\Browser\Peernet.Browser.Infrastructure\bin\Debug\net5.0\Peernet.Browser.Infrastructure.dll
  Peernet.Browser.WPF -> D:\Sources\Peernet\Browser\Peernet.Browser.WPF\bin\Debug\net5.0-windows\win-x64\Peernet.Browser.WPF.dll
  Peernet.Browser.Tests -> D:\Sources\Peernet\Browser\Peernet.Browser.Tests\bin\Debug\net5.0\Peernet.Browser.Tests.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:07.34
```

#### Step 6 (Install runtime)
Last step before being able to run the application is to install the .NET runtime.
.NET runtime installer can be downloaded from [Official Website](https://dotnet.microsoft.com/download/dotnet/5.0)

#### Step 7 (Run the app)
Having already built solution and installed runtime you are able to run the application. It can be achieved in few ways similar to building the solution.  
One way is to use Visual Studio (See [Step 3](#step3)).
The other way is to use [**dotnet CLI**](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).

Open command prompt (cmd.exe on Windows) and navigate to **Peernet.Browser.WPF** project location (file with *.csproj extension).  
Run following command:
```
dotnet run
```

You can also simply run the application from **.exe** file which you can find in build output directory (**bin** folder).

>Peernet.Browser.WPF.exe

## Use
You can either follow [first boot](#first-boot) instruction or download the latest release of Peernet Command Line Client and Peernet Browser.
Run 'Peernet.Browser.WPF.exe' file and enjoy the features.