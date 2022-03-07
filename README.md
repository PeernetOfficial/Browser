[![Deploy Browser](https://github.com/PeernetOfficial/Browser/actions/workflows/deploy-Browser.yml/badge.svg)](https://github.com/PeernetOfficial/Browser/actions/workflows/deploy-Browser.yml)

# Peernet Browser
This is official GUI for [Peernet Command Line Client](https://github.com/PeernetOfficial/Cmd). It has been designed to deliver client's capabilities in user friendly manner. 
It is built on to of .NET 6.0 with use of WPF UI Framework [WPF documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-6.0). 

## Configuration
Peernet Browser configuration entry point is via _Peernet Browser.dll.config_ output file or _App.config_ from Solution view.
Configuration file includes following settings:

| Name/Key              | Description                                                                                                                                                                                                                                                                            | Default Value                         |
|-----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------- |
| Backend               | A path to the Peernet Command Line Client executable file. It supports both full path and just a file name. In case it is a file name it will look for it in the current working directory.                                                                                            | ``` Backend.exe ```                |
| ApiUrl                | Url address on which backend is hosted. This setting is optional.   If it is not provided, Browser will run Backend by itself using Backend setting.  If provided, the Backend process will not be started by Browser and the ApiUrl setting value will be used for the backend calls. | None                                  |
| DownloadPath          | A path to the directory to which all downloaded files will be written. By default it is written to User's Downloads folder.  The %userprofile% is an environmental variable which is expanded during the runtime.                                                                      | ``` %userprofile%\Downloads\ ```  |
| DefaultTheme          | A default application color scheme. The application supports two themes: _LightMode_ and _DarkMode_. The setting by default is not present in the configuration file.                                                                                                                  | None                                  |
| PluginsLocation       | A relative to the working directory path to the location of all plugin folders                                                                                                                                                                                                         | Plugins                               |

Each time the application starts it reads the configuration from the file. The configuration is mapped to application' in-memory object. 
Settings within the object may change at runtime. E.g.: If _ApiUrl_  is not set, it is being auto-generated based on random free TCP Port; 
User may change application theme accordingly to his preference.   
To ensure application state is preserved between separate runs, the application writes all the in-memory settings to the configuration file when it exits. 
At every consequtive startup, the application reads the configuration from the file. With that being said, the _DefaultTheme_ is being added to the 
configuration file when the application exits for the first time (it is not present in the configuration file by default) and when the application is ran again, 
the theme is being recovered.

There is one exception. Since _ApiUrl_ has specific behaviour, the application does not maintain the _ApiUrl_ setting. It will be omitted from saving 
to the configuration file and only user can manually modify it.

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
1. Latest [.NET Desktop Runtime for Windows x64](https://dotnet.microsoft.com/download/dotnet/6.0). The installer version is recommended.
2. The backend executable. You can compile the [Cmd project](https://github.com/PeernetOfficial/Cmd) and use that resulting executable.
3. For improved connectivity add a Windows Firewall rule to allow all connections for the backend executable (the linked Cmd project has the netsh command documented).

### Automated Setup

An automated setup for end-users is provided via a Windows installer through a MSI file. For more details see the [Peernet Browser Installer Project](https://github.com/PeernetOfficial/BrowserSetup).

This installation bundle contains the Browser and installs the required .NET framework, includes the backend, and also configures the Windows Firewall for admin users.

## Use

Run the `Peernet Browser.exe` file from the release folder and enjoy the features.

Change the configuration file `Peernet Browser.dll.config` as needed.

## Compile

The following steps provide a guide how to compile the Peernet Browser. Note that the backend executable is not part of this guide.

### Step 1 Install .NET SDK
In order to use generic driver for .NET CLI, .NET SDK needs to be installed.
.NET SDK installer can be downloaded from [Official Website](https://dotnet.microsoft.com/download/dotnet/6.0)

### Step 2 Clone the repository
Use GIT CLI to clone the repository

```
git clone https://github.com/PeernetOfficial/Browser.git
```

### Step 3 Install VisualStudio (optional)

This step is optional. It is not required to be able to **build** and **run** solution although VisualStudio as IDE provides numerous features besides obvious ability to edit, build and run the solution.

Peernet Browser is written with the latest as of now .NET 6.0. It requires [Visual Studio 2022](https://visualstudio.microsoft.com/pl/vs/) version or newer to be able to load.
There is a Community version that is free of charge for non-commercial use.

### Step 4 Build Solution

The solution can be built either by using Visual Studio, or using the command line tool [dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet).

Open command prompt (cmd.exe on Windows) and navigate to solution location (file with *.sln extension). Run following command. This will build the debug version.

```
dotnet build
```

Expected output:
```
D:\Sources\Peernet\Browser\Peernet.Browser.WPF>dotnet build
Microsoft (R) Build Engine 17.1.0+ae57d105c dla platformy .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored D:\Sources\Peernet\Browser\Peernet.Browser.WPF\Peernet.Browser.WPF.csproj (w 273 ms).
  2 of 3 projects are up-to-date for restore.
  Peernet.Browser.Application -> D:\Sources\Peernet\Browser\Peernet.Browser.Application\bin\Debug\net6.0\Application.dll
  Peernet.Browser.Infrastructure -> D:\Sources\Peernet\Browser\Peernet.Browser.Infrastructure\bin\Debug\net6.0\Infrastructure.dll
  Peernet.Browser.WPF -> D:\Sources\Peernet\Browser\Peernet.Browser.WPF\bin\Debug\net6.0-windows\win-x64\Peernet Browser.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed: 00:00:05.90
```

To compile the release version without the .NET dlls, use the following command. The compiled files will be in the `\Peernet.Browser.WPF\bin\Release\net5.0-windows\win-x64\publish` folder.

```
dotnet publish -c Release --no-self-contained
```

To sign the executable use the `signtool` from the Windows SDK. Note that all other related executables including dlls should also be signed. It makes sense signing all executables before creating the setup file.

```
signtool sign /a /fd SHA256 "Peernet Browser.exe"
```

### Step 5 Run the app
Having already built solution and installed runtime you are able to run the application. It can be achieved in few ways similar to building the solution.  

One way is to use Visual Studio. The other way is to use dotnet tool.

Open command prompt (cmd.exe on Windows) and navigate to Peernet.Browser.WPF project location (file with *.csproj extension). Run following command:
```
dotnet run
```

You can also simply run the application from .exe file which you can find in build output directory (bin folder).

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

## Plugins System
Peernet Browser has Plugins System implemented. It allows extending Peernet Browser functionality by loading assemblies which implement specific interfaces during the runtime.
The interfaces are defined as part of [Peernet SDK](https://github.com/PeernetOfficial/SDK). You can also find there a documentation on how to develop a plugin.
The most important interface is __IPlugin__ interface. This is the interface for which Peernet Browser looks for in the __PluginsLocation__ subfolders. PluginsLocation is an application
setting which specifies the relative path to the location of all plugin folders.
Peernet Browser scans all the DLLs from inside these folders, istantiates classes which implement __IPlugin__ interface and executes their _Load_ methods.

For given _PluginsLocation_ setting
>     <add key="PluginsLocation" value="Plugins" />


The tree structure would be similar to:  
```
└───Plugins
    └───SamplePlugin
            Peernet.Browser.Plugins.Template.dll
            Peernet.SDK.dll
```

Plugin folder should include all output files of your plugin project. It is recommanded to use [dotnet publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish)
to produce the output.

Peernet Browser allows customizing some parts of the UI via plugins by exposing interfaces binded to some UI elements.
For isntance __IPlayButtonPlug__ interface is linked to _Play_ buttons on Search & Explore & Directory Tab data grid rows. Whenever the button is clicked,
_Execute_ method of the interface is invoked. The logic of the _Execute_ method lays on the plugin side.  
The visibility of the button in the grid is controlled by _PlayButtonPlugEnabled_ setting. If it is enabled and there is no plugin implementing the interface,
the button will simply have no effect.

Multiple plugin implementing same interface will override each other, meaning the interface from last loaded plugin will be the one that have actual effect.

## Peernet Browser Insights

### Connection with the backend
Peernet Browser connects to the backend based on the _ApiUrl_ setting. The connection is being established 
on the application startup, before the UI controls are generated. 
The connection is represented by following statuses:
```
public enum ConnectionStatus
{
    Online,
    Offline,
    Connecting
}
```

Where each status has its connection indicator in the left corner of the footer.
Respectively Green Globe, Red Globe, Yellow Globe.
Peernet Browser requests the API Status from the backed every 3 seconds (the backend returns _Peers Count_ at the same time).
Before each Status Poll, the application changes the API Status to _Connecting_.
When the backend returns success HTTP Status Code, Peernet Browser sets the returned API status. 
If it is not a Success HTTP Status Code, the Poller will go idle for next 3 seconds without changing the API status (it will remain in _Connecting_ status).
Status Poller runs during whole application lifetime and is disposed when Peernet Browser exits.

### Icon

To change the application icon replace the file `Peernet.Browser.WPF\peernet.ico`. The minimum resolution should be 48x48. If a high-resolution version is available, it should be 256x256.
