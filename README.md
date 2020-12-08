CLIM for .NET core
=======

Command line interface menu (CLIM) engine for .NET core offers a very simple system to manage UI menus via the cli.

![p1](https://github.com/ianovir/CLIM_dotnet/blob/main/pics/ctrl_console.jpg)

# Motivation

CLIM may be useful when prototyping core libraries in their early stage, in absence of advanced GUI yet. For example, when you are developing core component for multi-platform environments and you need to use or distribute it in its early stage.

# Download

You can download the last compiled version of `CLIM` from the [releases](https://github.com/ianovir/CLIM_dotnet/releases) page, or import it from nuget.

## Nuget
```
PM> Install-Package CLIM
``` 

## .NET CLI
```
> dotnet add package CLIM
```

# Usage

See the `Program.cs` inside the CLIM Demo project for a simple example.

## Overview

The main components of CLIM are:
* Engine
* Menu
* Entry
* Streams

### Engine
`Engine` is the main component organizing menus, printing to `IOutputStream` and reading from `InputStream`. Basically, an `Engine` wraps a Stack collection of menus and allows navigation across them.

To create a new engine:
```csharp
Engine engine = new Engine("CLIM DEMO");
```

To start the engine:
```csharp
engine.start();
```

### Menu
A `Menu` is a list of entries.

The simplest way to create a menu is building it from the engine:
```csharp
Menu mainMenu = engine.BuildMenu("Main menu");
```

Then add as many entries as you want:
```csharp
mainMenu.AddEntry(newEntry);
```

Or add a sub-menu as entry:
```csharp
Menu secondMenu = engine.BuildMenu("Second menu", "cancel");
//...
mainMenu.AddSubMenu(secondMenu);
```

Finally add the menu to the engine:
```csharp
engine.AddOnTop(mainMenu);
```

### Entry
An `Entry` represents an option the user can choose. You need to implement the `OnAction()` action of an entry in order to specify its action.

To create an Entry:
```csharp
Entry newEntry = new Entry("New Entry") {
	OnAction = () =>{
		//implement entry's action...
	}
};
```

### Streams
CLIM's `Stream` is an object used for streaming input and output data. By default, the `Engine` uses `ScannerInputStream` and `SystemOutputStream` which uses the standard Console. You can define your custom streams by implementing the `InputStream` and `IOutputStream` classes.

It is possible to assign custom streams to the engine:
```csharp
engine.InputStream = customInputStream;
engine.OutStream = customOutputStream;
```

# Copyright
Copyright(c) 2020 Sebastiano Campisi - [ianovir.com](https://ianovir.com). 
Read LICENSE file for more details.