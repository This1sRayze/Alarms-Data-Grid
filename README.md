# Alarms Data Grid

## Overview
The **Alarms Data Grid** project is a C# library targeting **.NET Framework 4.7.2**.  
It provides a data grid component designed for displaying and managing alarms, useful in monitoring, SCADA, or enterprise applications where real-time event visualization is required.

## Features
- Custom DataGrid implementation
- Supports alarm/event visualization
- Configurable and extendable for integration into larger .NET applications
- Strong-named assembly (`NIAOU.snk` signing enabled)

## Project Structure
```
Alarms_Data_Grid/
├── Alarms_Data_Grid.sln         # Solution file
├── Alarms_Data_Grid/            # Main project source
│   ├── Alarms_Data_Grid.csproj  # Project file
│   ├── Properties/              # Assembly info and resources
│   └── ...                      # Source files
├── .gitignore
├── .gitattributes
└── .git/                        # Git repository
```

## Requirements
- **Visual Studio 2019 or later**
- **.NET Framework 4.7.2 SDK**
- Windows environment (for building and running)

## Build Instructions
1. Clone the repository:
   ```bash
   git clone <repo-url>
   cd Alarms_Data_Grid
   ```
2. Open the solution file `Alarms_Data_Grid.sln` in **Visual Studio**.
3. Restore NuGet packages (if any).
4. Build the solution (`Ctrl+Shift+B`).

## Usage
- Reference the compiled DLL (`Alarms_Data_Grid.dll`) in your project.
- Use the provided `DataGrid` component to display alarms or events in your application.


