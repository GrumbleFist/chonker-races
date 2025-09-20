# Chonker Races - Project Setup Guide

This guide will help you set up the Chonker Races project in Unity and get it running.

## Prerequisites

- Unity 2022.3 LTS or later
- Git (for version control)
- Basic knowledge of Unity and C#

## Unity Setup

### 1. Create New Unity Project

1. Open Unity Hub
2. Click "New Project"
3. Select "3D (Built-in Render Pipeline)" template
4. Name your project "Chonker Races"
5. Choose a location and click "Create"

### 2. Import Required Packages

1. Open Package Manager (Window > Package Manager)
2. Install the following packages:
   - **Input System** (for cross-platform input)
   - **Mirror Networking** (for multiplayer)
   - **TextMeshPro** (for better text rendering)

### 3. Project Settings

1. Go to Edit > Project Settings
2. **XR Plug-in Management**: Disable if not needed
3. **Player Settings**:
   - Set Company Name: "Your Company"
   - Set Product Name: "Chonker Races"
   - Set Version: "1.0.0"
   - **Android Settings** (if targeting mobile):
     - Set Minimum API Level to 21 (Android 5.0)
     - Set Target API Level to latest
     - Set Scripting Backend to IL2CPP
     - Set Target Architectures to ARM64

### 4. Input System Setup

1. Go to Edit > Project Settings > Player
2. Under "Configuration", set "Active Input Handling" to "Input System Package (New)"
3. Restart Unity when prompted

## Scene Setup

### 1. Create Basic Scene

1. Create a new scene: File > New Scene > 3D
2. Save as "MainMenu"
3. Create another scene: File > New Scene > 3D
4. Save as "RaceTrack"

### 2. Race Track Scene Setup

#### A. Create Terrain
1. Right-click in Hierarchy > 3D Object > Terrain
2. Use Terrain tools to create a basic track
3. Add textures and details

#### B. Create Car
1. Create empty GameObject named "Car"
2. Add the following components:
   - Rigidbody
   - CarController script
   - CarInputActions script
   - NetworkedCar script (for multiplayer)

#### C. Create Wheels
1. Create 4 empty GameObjects under Car:
   - FrontLeftWheel
   - FrontRightWheel
   - RearLeftWheel
   - RearRightWheel
2. Add WheelCollider to each
3. Create visual wheel models as children

#### D. Create Track Elements
1. **Checkpoints**: Create empty GameObjects with Checkpoint script
2. **Power-ups**: Create GameObjects with PowerUp script
3. **Damage Zones**: Create GameObjects with DamageZone script
4. **Bridges**: Create 3D models for bridges
5. **Jumps**: Create ramps and landing areas

#### E. Create UI
1. Create Canvas in Hierarchy
2. Add RaceHUD script to a GameObject
3. Create UI elements (text, buttons, sliders)

#### F. Create Race Manager
1. Create empty GameObject named "RaceManager"
2. Add RaceManager script
3. Assign references to checkpoints and UI elements

### 3. Main Menu Scene Setup

1. Create Canvas
2. Add MainMenuUI script
3. Create UI panels for different menus
4. Add NetworkManager GameObject with NetworkManager script

## Scripts Integration

### 1. Car Controller Setup

1. Attach CarController to your car GameObject
2. Assign wheel colliders and transforms
3. Set motor force, brake force, and steering values
4. Configure damage and power-up settings

### 2. Input Actions

1. Create Input Actions asset: Right-click in Project > Create > Input Actions
2. Name it "CarInputActions"
3. Set up actions for Move, Brake, and Boost
4. Assign to CarInputActions script

### 3. Network Setup

1. Add NetworkManager to scene
2. Create player prefab with NetworkedCar script
3. Set up spawn points
4. Configure network settings

## Building the Game

### 1. PC Build

1. File > Build Settings
2. Add scenes to build
3. Select "PC, Mac & Linux Standalone"
4. Choose target platform (Windows, Mac, or Linux)
5. Click "Build"

### 2. Android Build

1. Install Android Build Support in Unity Hub
2. File > Build Settings
3. Switch platform to Android
4. Player Settings:
   - Set Package Name
   - Set Minimum API Level
   - Configure icons and splash screen
5. Click "Build"

## Testing

### 1. Single Player Testing

1. Play the scene in Unity
2. Test car controls (WASD or Arrow Keys)
3. Test power-ups and damage zones
4. Test race progression

### 2. Multiplayer Testing

1. Build the game
2. Run one instance as host
3. Run another instance as client
4. Test network synchronization

## Troubleshooting

### Common Issues

1. **Input not working**: Check Input System settings
2. **Network not connecting**: Verify NetworkManager setup
3. **Car physics issues**: Check Rigidbody and WheelCollider settings
4. **UI not displaying**: Verify Canvas settings and script references

### Performance Optimization

1. Use LOD groups for distant objects
2. Optimize textures and models
3. Use object pooling for power-ups
4. Implement culling for off-screen objects

## Next Steps

1. Create more detailed track designs
2. Add more power-up types
3. Implement different car models
4. Add sound effects and music
5. Create more race modes
6. Add leaderboards and achievements

## Resources

- [Unity Documentation](https://docs.unity3d.com/)
- [Mirror Networking Documentation](https://mirror-networking.com/)
- [Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/)
- [Unity Terrain System](https://docs.unity3d.com/Manual/terrain-UsingTerrains.html)

## Contributing

Feel free to contribute to this project by:
- Adding new features
- Fixing bugs
- Improving documentation
- Creating new tracks
- Adding new power-ups

Happy racing! 🏎️
