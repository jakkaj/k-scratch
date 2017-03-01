# k-scratch

This is a WIP - please provide feedback!

[![Build status](https://ci.appveyor.com/api/projects/status/3e3bkjyc7ia4gy59/branch/master?svg=true)](https://ci.appveyor.com/project/jakkaj/k-scratch/branch/master)

[Download link v01](https://xammelbs.blob.core.windows.net/downloads/ks_win10x64_v01.zip) (until builds are working)

 - Pulls Azure Function code to your local machine for editing
 - Monitors files for changes and quickly sends them back to Azure Functions for quick prototyping
 - Shows the Azure Function log stream with *color* highlights. 
 - (okay sure, it's actually using Kudu - which measn this will work for any App Service based site...)


K-Scratch allows you to pull down your Azure functions for local editing. File monitoring immediatly re-uploads changes to Azure when you save. Local log stream allows you to see what's going on as your function compiles and runs. 

It's probably best not to use this on your prod functions :)

#### Quick Intro Video
[![Quick Intro](http://img.youtube.com/vi/J6y_K6dUhSQ/0.jpg)](http://www.youtube.com/watch?v=J6y_K6dUhSQ "Quick Intro")

*YouTube*

#### Build

Until I get the builds going (I know, I know!) you can use dotnet run to get k-scratch running. 

You will need the [.NET Core SDK](https://www.microsoft.com/net/core/#windowsvs2017). 

I'll get the builds going soon!

**You need to be in the src\ks directory for all this stuff to work for now.**

#### Usage

```
usage: ks [-l] [-m] [-p <arg>] [-g]

    -l, --log           Output the Kudulog stream to the console
    -m, --monitor       Monitor the path for changes and send them up
    -p, --path <arg>    The base path of your function (blank for
                        current path)
    -g, --get           Download the Function app ready for editing
                        locally
```

First you need to create an Azure function. I tend to use consumption based ones, nice and cheap for my muck around functions... but you can use normal App Service for them too (with slots etc). 

Grab a publish profile from Azure. Click on *Function app settings* -> *Go to App Service Settings* -> (... if it's hidden) -> *Get publish profile*. 

<image src="https://cloud.githubusercontent.com/assets/5225782/23344608/ac7c44d4-fcd3-11e6-90f2-0291a31f1522.gif" width="800px"/>

Copy the publish profile in to a new empty folder. The publish profile can be in an folder up the path, but it's easiest to pop it in the root of your new editing spot. 

Now you can grab the function code and download it to your local folder ready for editing. 

```
dotnet run -g -p c:\pathtoyoureditinglocation
```

The *-g* option will grab the files to your local folder. I suggest not using this with other options as you might accidentally overwite local changes next time you run the command (but pressing up to see previous commands!).

The *-p* option sets the root path for your editing folder. This will default to the path you're currently running from if ommitted. 

Once you have the files you can begin monitoring and logging. 

```
dotnet run -l -m -p c:\pathtoyoureditinglocation
```

The *-l* option will pop the Kudu Logstream out to your local console. 

The *-m* option will use file system monitoring to check for new / existing file changes and send them immediately to your site in the offset location of where your base path is.

Once you send the files back they will be compiled by functions and will automatically run!

<img src="https://cloud.githubusercontent.com/assets/5225782/23344942/7b6a1f28-fcd9-11e6-8cdc-5ca5df20db37.gif" width="800px"/>
