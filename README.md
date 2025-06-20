# FolderRestrictor 🛡️

A Windows Forms application that enforces strict naming conventions on files and folders in a monitored directory.

## Features
- Real-time folder and file monitoring
- Deletes files/folders that do not match: `LastName, FirstName - 1234`
- Easy-to-use admin UI

## Requirements
- Windows 10+
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Building
```bash
dotnet publish FolderRestrictor/FolderRestrictor.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/
```

## Naming Convention
Accepted format:
```
LastName, FirstName - 1234
```
Any file or folder outside this format is deleted automatically.

## Configuration
- The admin password is stored in `App.config`. The monitored folder is selected via the UI.
- Delay before deletion: 10 seconds