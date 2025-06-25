# CmdletHelpViewer
Copy of the source for the old PowerShell CmdletHelpViewer, previously hosted on Codeplex.

In my humble opinion, despite its age and very simple design (you have to manually open MAML help files), the layout is *miles* better than the built-in `-ShowWindow` design still in use with PowerShell 7, unchanged since 5.1. I'd love to learn more C# and update this to support integration with markdown, automatic detection of module and cmdlet help, and even a module that adds a `Show-CmdletHelp` (or something) to integrate more closely with PowerShell, so the terribly clunky `-ShowWindow` can be avoided.

*Incoming tirade* — Yes, I know about Sapien's free PowerShell Help Viewer, and I honestly hate it. Despite having features like scanning for help content on startup, having the benefit of to-the-year updates compared to CmdletHelpViewer's last update from 2010, and yet still uses an embedded IE control to render HTML instead of the lovely, dynamic, togglable Avalon controls used by CmdletHelpViewer. Also, it's horribly buggy in my experience, often stops responding and must be restarted if you open more than one tab (_seriously_!), and generally just sucks, especially coming from such a major player in the PowerShell world. They can do better, and should do better. Even free, I don't want it until it receives a *major* overhaul. Despite that, no shade against Sapien — PowerShell Studio has its place — but I'm not a fan of their free tools, personally. They feel thrown-together, using a mish-mash of old and new technologies like nice Actipro UI elements along with terrible embedded IE objects. Seriously!

## Description
Cmdlet Help Viewer is a WPF based client to view help about PowerShell cmdlets. It parses the XML help file and displays the contents of the help file in a user friendly way.

- [Release Post](https://cerebrata.com/blog/cerebrata-releases-a-wpf-based-windows-powershell-cmdlet-help-viewer-utility-on-codeplex)
- [Codeplex Archive page](https://codeplexarchive.org/project/cmdlethelpviewer)
- [Original URL (dead)](http://cmdlethelpviewer.codeplex.com/)

## Screenshot
![cmdlethelpviewerscreenshot](https://github.com/user-attachments/assets/64dddd94-5d4b-4e8f-abea-e79496212a7b)
