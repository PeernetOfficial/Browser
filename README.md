[![Deploy Browser](https://github.com/PeernetOfficial/Browser/actions/workflows/deploy-Browser.yml/badge.svg)](https://github.com/PeernetOfficial/Browser/actions/workflows/deploy-Browser.yml)

# Peernet Browser
This is official GUI for [Peernet Command Line Client](https://github.com/PeernetOfficial/Cmd). It has been designed to deliver client's capabilities in user friendly manner. 
It is built on to of .NET 5.0 with use of WPF UI Framework [WPF documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-5.0). 

## Configuration
Peernet Browser configuration entry point is via _Peernet Browser.dll.config_ output file or _App.config_ from Solution view.
Configuration file includes following settings:

| Name/Key     | Description                                                                                                                                                                                                                                                                            | Default Value                    |
|--------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------|
| Backend      | A path to the Peernet Command Line Client executable file. It supports both full path and just a file name. In case it is a file name it will look for it in the current working directory.                                                                                            | ``` Backend.exe ```              |
| ApiUrl       | Url address on which backend is hosted. This setting is optional.   If it is not provided, Browser will run Backend by itself using Backend setting.  If provided, the Backend process will not be started by Browser and the ApiUrl setting value will be used for the backend calls. | None                             |
| DownloadPath | A path to the directory to which all downloaded files will be written. By default it is written to User's Downloads folder.  The %userprofile% is an environmental variable which is expanded during the runtime.                                                                      | ``` %userprofile%\Downloads\ ``` |


## Peernet Browser - Peernet Command Line Client Integration
Peernet Browser requirement is backend (Peernet Command Line Client) to be running. Every application view is generated with some data supplied by backend.
If the backend is not running, the application will not be functional.
Peernet Browser has ability to start backend process by itself. 
It decides whether backend needs to be started or some already existing backend process should be used, based on the [configuration](#configuration)'s ApiUrl setting. 
If Peernet Browser started the backend process it takes the responsibility of maintaining it through the whole application lifetime.  
Peernet Browser starts the Peernet Command Line Client with set of arguments:
 - ```-webapi=``` the address on which the backend will be hosted.  
 Peernet Browser finds a free Port on the current machine and constructs the valid localhost address with this Port to provide it in the argument's value.
- ```-apikey=``` the key used to identify authorized backend requests.  
Peernet Browser generates a single _Guid_ __apikey__ for the application lifetime. This key is supplied to the backend with the argument's value and 
is later used in the HTTP Header of all the backend requests.
- ```-watchpid=``` is ID of the process that starts the backend.  
Peernet Browser provides its process ID to the backend. In result backend monitors the Peernet Browser process and exits when the Peernet Browser terminates.

Backend process started by the Browser needs to be later disposed.  
When Peernet Browser application exits it sends ```/shutdown?action=0``` request to the backend. Backend is supposed to exit on such request. 
Although Browser makes sure backend terminated and kills the process if it didn't happen in 5 seconds since the __shutdown__ request.

## Deployment

### Requirements

These components are required:
1. Latest [.NET Desktop Runtime for Windows x64](https://dotnet.microsoft.com/download/dotnet/5.0). The installer version is recommended.
2. The backend executable. You can compile the [Cmd project](https://github.com/PeernetOfficial/Cmd) and use that resulting executable.
3. For improved connectivity add a Windows Firewall rule to allow all connections for the backend executable (the linked Cmd project has the netsh command documented).

### Automated Setup

An automated setup is provided via a Windows installer through a MSI file. For more details see the [Peernet Browser Installer Project](https://github.com/PeernetOfficial/BrowserSetup).

This installation bundle contains the Browser and installs the required .NET framework, includes the backend, and also configures the Windows Firewall for admin users.

## Use

Run 'Peernet Browser.exe' file and enjoy the features.

## Compile

The following steps provide a guide how to compile the Peernet Browser. Note that the backend executable is not part of this guide.

### Step 1 .NET SDK
In order to use generic driver for .NET CLI, .NET SDK needs to be installed.
.NET SDK installer can be downloaded from [Official Website](https://dotnet.microsoft.com/download/dotnet/5.0)

### Step 2 Clone the repository
Use GIT CLI to clone the repository

```
git clone https://github.com/PeernetOfficial/Browser.git
```

### Step 3 Install VisualStudio (optional)
This step is optional. It is not required to be able to **build** and **run** solution although VisualStudio as IDE provides numerous features besides obvious ability to edit, build and run the solution.

Peernet Browser is written with the latest as of now .NET 5.0. It requires [VisualStudio 2019](https://visualstudio.microsoft.com/pl/vs/) version or newer to be able to load.
There is **Community** version that is free of charge for non-commercial use.

### Step 4 Modify application settings
You should modify configuration accordingly to your needs. See [Configuration](#configuration).

### Step 5 Build solution
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
  Peernet.Browser.Models -> D:\Sources\Peernet\Browser\Peernet.Browser.Models\bin\Debug\net5.0\Models.dll
  Peernet.Browser.Application -> D:\Sources\Peernet\Browser\Peernet.Browser.Application\bin\Debug\net5.0\Application.dll
  Peernet.Browser.Infrastructure -> D:\Sources\Peernet\Browser\Peernet.Browser.Infrastructure\bin\Debug\net5.0\Infrastructure.dll
  Peernet.Browser.WPF -> D:\Sources\Peernet\Browser\Peernet.Browser.WPF\bin\Debug\net5.0-windows\win-x64\Peernet Browser.dll
  Peernet.Browser.Tests -> D:\Sources\Peernet\Browser\Peernet.Browser.Tests\bin\Debug\net5.0\Peernet.Browser.Tests.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:07.34
```

### Step 6 Install runtime
Last step before being able to run the application is to install the .NET runtime.
.NET runtime installer can be downloaded from [Official Website](https://dotnet.microsoft.com/download/dotnet/5.0)

### Step 7 Run the app
Having already built solution and installed runtime you are able to run the application. It can be achieved in few ways similar to building the solution.  
One way is to use Visual Studio (See [Step 3](#step3)).
The other way is to use [**dotnet CLI**](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).

Open command prompt (cmd.exe on Windows) and navigate to **Peernet.Browser.WPF** project location (file with *.csproj extension).  
Run following command:
```
dotnet run
```

You can also simply run the application from **.exe** file which you can find in build output directory (**bin** folder).

>Peernet Browser.exe

## Development

### Debugging Backend API

By default the backend API uses a randomized port and a randomized API key. For debugging purposes it can make sense to set the listening port of the API to a hardcoded value and disable the use of API key. This will make it easy to use 3rd party HTTP clients for debugging requests.

First, change the backend configuration file `Config.yaml` to include a hardcoded IP:Port to listen and disable the use of API key by setting it to the zero UUID:

```yaml
APIListen: ["127.0.0.1:112"]
APIKey:    "00000000-0000-0000-0000-000000000000"
```

In the file `Peernet Browser.dll.config` set the tag `ApiUrl` to the same IP:Port:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="Backend" value="Backend.exe" />
    <add key="ApiUrl" value="http://127.0.0.1:112" />
    <add key="DownloadPath" value="%userprofile%\Downloads\" />
  </appSettings>
</configuration>
```

Note: In this case you will have to start the backend executable manually before starting the Peernet Browser. You will also have to close the process yourself when done.
