[![nuget][nuget-badge]][nuget-url]
[![stable](https://img.shields.io/badge/stable-stable-green.svg)](https://github.com/loamen/KeyMouseHook/) 
[![build](https://img.shields.io/shippable/5444c5ecb904a4b21567b0ff.svg)](https://travis-ci.org/loamen/KeyMouseHook)
[![license](https://img.shields.io/badge/license-MIT-red.svg?style=flat)](https://raw.githubusercontent.com/loamen/KeyMouseHook/master/LICENSE)
[![platforms](https://img.shields.io/badge/platform-Windows-yellow.svg?style=flat)]()
[![download_count](https://img.shields.io/github/downloads/loamen/KeyMouseHook/total.svg?style=plastic)](https://github.com/loamen/KeyMouseHook/releases) 
[![release](https://img.shields.io/github/release/loamen/KeyMouseHook.svg?style=flat)](https://github.com/loamen/KeyMouseHook/releases) 

[nuget-badge]: https://img.shields.io/badge/nuget-v1.0.5-blue.svg
[nuget-url]: https://www.nuget.org/packages/KeyMouseHook
[source-url]: https://github.com/loamen/KeyMouseHook
[mousekeyhook-url]: https://github.com/gmamaladze/globalmousekeyhook
[inputsimulator-url]: https://github.com/michaelnoonan/inputsimulator
[readme-url]: https://github.com/loamen/KeyMouseHook/blob/master/README.zh-CN.md

![Keyboard and Mouse Hooking and Simulator Library in c#(winform)](https://github.com/loamen/KeyMouseHook/raw/master/documents/images/keyboard-mouse-hook-logo.png)

[中文介绍][readme-url]

## What it does?

This is a extended library use [globalmousekeyhook][mousekeyhook-url] and [InputSimulator][inputsimulator-url] .it allows you to tap keyboard and mouse, to detect and record their activity event when an application is inactive and runs in background.The Windows Input Simulator provides a simple .NET(C#) interface to simulate Keyboard or Mouse input using the Win32 SendInput method.

## Prerequisites

* **Windows:** .Net Framework 4.0+

## Installation and sources


> nuget install KeyMouseHook


* [NuGet package][nuget-url]
* [Source code][source-url]

## Usage

```csharp
private readonly KeyMouseFactory eventHookFactory = new KeyMouseFactory(HookType.GlobalEvents);
private readonly KeyboardWatcher keyboardWatcher;
private readonly MouseWatcher mouseWatcher;
private List<MouseKeyEvent> _mouseKeyEvents;

public FormMain()
{
   InitializeComponent();

   keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
   keyboardWatcher.OnKeyboardInput += (s, e) =>
   {
	if (_mouseKeyEvents != null)
	    _mouseKeyEvents.Add(e);
   };

   mouseWatcher = eventHookFactory.GetMouseWatcher();
   mouseWatcher.OnMouseInput += (s, e) =>
   {
	if (_mouseKeyEvents != null)
	    _mouseKeyEvents.Add(e);
   };
}

private void StartWatch(IKeyboardMouseEvents events = null)
{
    _macroEvents = new List<MacroEvent>();
    keyboardWatcher.Start(events);
    mouseWatcher.Start(events);
}

private void StopWatch()
{
   keyboardWatcher.Stop();
   mouseWatcher.Stop();
}

private void Playback()
{
   var sim = new InputSimulator();
   sim.PlayBack(_macroEvents);
}
```

```csharp
keyboardWatcher = eventHookFactory.GetKeyboardWatcher().Disable(MacroEventType.KeyDown | MacroEventType.KeyUp).Enable(MacroEventType.KeyPress);
mouseWatcher = eventHookFactory.GetMouseWatcher().Enable(MacroEventType.MouseDoubleClick | MacroEventType.MouseDragStarted).Disable(MacroEventType.MouseDragFinished | MacroEventType.MouseMove);
var sim = new InputSimulator().Enable(MacroEventType.MouseDoubleClick | MacroEventType.KeyPress).Disable(MacroEventType.MouseMove | MacroEventType.KeyDown | MacroEventType.KeyUp);
```

(also have a look at the Demo app included with the source)

## Screen Shots

![Keyboard and Mouse Hooking and Simulator Library in c#(winform)](https://github.com/loamen/KeyMouseHook/raw/master/documents/images/screen-shots.png)

## Thanks

* [globalmousekeyhook][mousekeyhook-url] (MIT License)
* [InputSimulator][inputsimulator-url] (MIT License)

## Quick contributing guide

 - Fork and clone locally
 - Create a topic specific branch. Add some nice feature.
 - Send a Pull Request!

## License

The MIT license see: [LICENSE](https://github.com/loamen/KeyMouseHook/blob/master/LICENSE)
