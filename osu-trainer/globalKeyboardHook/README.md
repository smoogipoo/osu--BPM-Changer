# globalKeyboardHook
Create low-level keyboard hooks

This was originally created by StormySpike at [codeproject.com](https://www.codeproject.com/Articles/19004/A-Simple-C-Global-Low-Level-Keyboard-Hook)

Introduction
This article discusses a class that I wrote that wraps a global low level keyboard hook. The sample hooks the A and B keys, just for demonstration.

Background
I was trying to find a way for an application that I am writing to restore itself when a combination of keys was pressed. This was born from searching around for the answer.

Using the Code
First download the source, and add globalKeyboardHook.cs to your project. Then add...

```c#
     using Utilities;
```
... to the top of the file you are going to use it in. Next add an instance of globalKeyboardHook to your class:

```c#
     globalKeyboardHook gkh = new globalKeyboardHook() ;
```
When a globalKeyboardHook is constructed, it automatically installs the hook, so all there is left to do is add some keys for it to watch, and define some event handlers for the KeyDown and KeyUp events. I usually do this on the main form's load event handler, like this:

```c#
private void Form1_Load(object sender, EventArgs e) {
    gkh.HookedKeys.Add(Keys.A);
    gkh.HookedKeys.Add(Keys.B);
    gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
    gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
} 

void gkh_KeyUp(object sender, KeyEventArgs e) {
    lstLog.Items.Add("Up\t" + e.KeyCode.ToString());
    e.Handled = true ;
}

void gkh_KeyDown(object sender, KeyEventArgs e) {
    lstLog.Items.Add("Down\t" + e.KeyCode.ToString());
    e.Handled = true ;
} 
```

Here I have chosen to watch for the A and B keys, and defined handlers for the KeyUp and KeyDown events that both log to a listbox called lstLog. So whenever the user presses the A or B keys, no matter what has focus, the application will be notified. Setting e.Handled to true makes it so no other notifications for this event go out, in the sample, this effectively stops the user from typing an A or B. This can be useful in ensuring that key combinations are not also typed out when used.

You can add hooks for as many keys as you would like, just add them like above. Don't get frustrated if you add a hook for a key and it doesn't work, many of them, like Keys.Shift show up as other more specific keys, like Keys.LShiftKey or Keys.RShiftKey. Keys.Alt shows up as Keys.LMenu or Keys.RMenu, Keys.Control shows up as Keys.LControl or Keys.RControl, just to name a few.

If you would like to hook or unhook the keyboard hook at any point, just call your globalKeyboardHook's hook and unhook methods, like so:

```c#
//unhook
gkh.unhook() 
//set the hook again
gkh.hook() 
```

Points of Interest
The bulk of the work in this code is done in the globalKeyboardHook class, although it is a fairly simple piece of code itself. The hardest part of doing this was finding the correct parameters for SetWindowsHookEx.

```c#
[DllImport("user32.dll")]
static extern IntPtr SetWindowsHookEx
    (int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);
    ```
The parameters were worked out to be:

```c#
IntPtr hInstance = LoadLibrary("User32");
hhook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hInstance, 0);
```
The first parameter `WH_KEYBOARD_LL` is just saying that we want to hook the low level keyboard events, `hookProc` is the callback for the event, `hInstance` is a handle to User32.dll, where this event is first processed (I think). The last parameter is if you want to hook a specific thread, then you would just pass a thread id instead of using the hInstance.
