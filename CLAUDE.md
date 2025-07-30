# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

InfiniminerMono is a MonoGame port of the classic Infiniminer team-based multiplayer mining game. Players compete in two teams (Red vs Blue) to mine ore, build structures, and accumulate cash to win. This version has been fully migrated from XNA Game Studio 3.0 to MonoGame 3.8+ for modern platform compatibility.

## Build System

**Prerequisites:**
- .NET 8.0 SDK
- MonoGame 3.8.* framework
- Visual Studio 2022 or compatible IDE

**Build Commands:**
```bash
# Build entire solution
dotnet build InfiniminerMono.sln

# Build individual projects
dotnet build InfiniminerShared/InfiniminerShared.csproj
dotnet build InfiniminerServer/InfiniminerServer.csproj  
dotnet build InfiniminerClient/InfiniminerClient.csproj
```

**Project Structure:**
- `InfiniminerClient.csproj` - Game client (MonoGame desktop app)
- `InfiniminerServer.csproj` - Dedicated server (console app)
- `InfiniminerShared.csproj` - Shared code library
- All projects target .NET 8.0 with modern SDK-style project files

## Architecture

### Core Components

**Client Architecture:**
- `InfiniminerGame.cs` - Main game class, extends StateMasher.StateMachine
- **Namespace:** `InfiniminerMono`
- State-based system in `States/` directory:
  - `TitleState.cs` - Main menu
  - `ServerBrowserState.cs` - Server selection  
  - `TeamSelectionState.cs` - Team/class selection
  - `MainGameState.cs` - Core gameplay
  - `SettingsState.cs` - Configuration
- Rendering engines in `Engines/`:
  - `BlockEngine.cs` - Voxel world rendering
  - `PlayerEngine.cs` - Player/entity rendering
  - `ParticleEngine.cs` - Effects system
  - `InterfaceEngine.cs` - UI rendering
  - `SkyplaneEngine.cs` - Environment rendering

**Server Architecture:**
- `InfiniminerServer.cs` - Main server logic, handles 64x64x64 block world
- `InfiniminerNetServer.cs` - Network communication layer
- `CaveGenerator.cs` - Procedural world generation
- `MapSender.cs` - Threaded map transmission to clients
- **Namespace:** `InfiniminerServer`

**Shared Code:**
- `Defines.cs` - Game constants, definitions, color utilities
- `GeneralEnums.cs` - Network messages, game enums, data structures
- `Player.cs` - Player data model
- `BlockInformation.cs` - Block type definitions
- `DatafileLoader.cs` - Configuration file parsing
- **Namespace:** `InfiniminerShared`

### Key Systems

**Networking:**
- Uses Lidgren.Network 1.0.2+ for UDP networking
- Message-based communication defined in `InfiniminerMessage` enum
- Reliable/unreliable channels for different data types
- Compatible with modern .NET networking stack

**Block World:**
- 64x64x64 voxel world stored as `BlockType[,,]` array
- Ground level at Y=8, lava generation, ore distribution
- Team-based building with collision detection

**Game Mechanics:**
- Tool system: pickaxe, drill, construction gun, detonator, radar
- Resource management: ore collection, cash accumulation
- Win condition: reach cash threshold (default 10,000)
- Class-based gameplay with different capabilities per team

## Content Pipeline

**Assets (MonoGame Content Pipeline):**
- `Content/Content.mgcb` - MonoGame content project file
- All assets migrated from XNA ContentPipeline to MonoGame format
- **Block textures:** `Content/blocks/` - 42+ block texture files
- **UI textures:** `Content/icons/`, `Content/menus/`, `Content/ui/`
- **Audio files:** `Content/sounds/` (WAV), `Content/song_title.mp3`
- **Shaders:** Bloom effects, basic rendering, particle systems (.fx files)
- **Fonts:** SpriteFont files for UI text rendering

**Content Build Process:**
```bash
# Content is automatically built during project compilation
# Manual content building (if needed):
dotnet mgcb Content/Content.mgcb
```

## MonoGame Migration Notes

**Key Changes from XNA:**
- **Graphics API:** Updated to MonoGame rendering pipeline
- **SpriteBatch:** Modern BlendState instead of SpriteBlendMode
- **Effect System:** Uses `effect.CurrentTechnique.Passes[0].Apply()`
- **Render States:** Updated to RasterizerState/BlendState/DepthStencilState
- **Vertex Handling:** Modern vertex buffer management
- **Content Pipeline:** Uses MonoGame Content Builder (.mgcb files)

**Compatibility Features:**
- Full shader support with updated effect system
- Bloom post-processing effects
- 3D rendering with modern graphics pipeline
- Cross-platform compatibility (Windows, Linux, macOS)

## Development Commands

**Common Tasks:**
```bash
# Run client
dotnet run --project InfiniminerClient

# Run server  
dotnet run --project InfiniminerServer

# Clean build
dotnet clean && dotnet build

# Publish for distribution
dotnet publish InfiniminerClient -c Release -r win-x64 --self-contained
```

**Testing:**
- No specific test framework currently configured
- Manual testing through client/server execution
- Network testing requires running server and client instances

## Configuration

**Client Configuration:**
- Settings managed through in-game settings menu
- Key bindings configurable via UI
- Graphics settings for performance tuning

**Server Configuration:**
- Console-based administration commands
- Runtime configuration changes supported
- Automatic map backups every 5 minutes
- Admin system for moderation

## Known Issues and Limitations

**Current Status:**
- Core game systems fully ported to MonoGame
- Network multiplayer functionality preserved
- All rendering systems updated for modern graphics APIs
- Content pipeline fully migrated

**Potential Areas for Enhancement:**  
- Performance optimization for modern hardware
- Additional platform-specific features
- Modern networking protocols (if desired)
- Enhanced graphics effects using modern shaders