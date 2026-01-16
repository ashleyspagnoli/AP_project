# AI-Assisted 2D Game Engine and Asteroid Game in C# WinForms

A classic Asteroids arcade game built with C# and Windows Forms, featuring a custom game engine architecture without timer controls.

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ashleyspagnoli/AP_project
   ```

2. **Open the solution:**
   In Visual Studio, select "Open a project or solution" and choose `src.sln` in the root folder.

3. **Build and run:**
   Press `F5` (or click the Start button) to compile and run the application.

## Structure (src folder)

```
src/
├── Engine/                   # Game engine components
│   ├── CollisionManager.cs
│   ├── Entity.cs
│   ├── GameEngine.cs
│   ├── ICollidable.cs
│   ├── IEntity.cs
│   ├── InputManager.cs
│   ├── Scene.cs
│   └── Vector2.cs
├── Game/                   # Game-specific implementation
│   ├── GameScene.c
│   ├── Ship.cs 
│   ├── Asteroid.cs
│   └── Bullet.cs
├── GameForm.cs             # Main game window
└── Program.cs              # Application entry point

```

## How to Play

### Controls

| Key | Action |
|-----|--------|
| **Left Arrow** | Rotate ship counterclockwise |
| **Right Arrow** | Rotate ship clockwise |
| **Up Arrow** | Thrust forward |
| **Space** | Shoot |
| **R** | Restart game (when game over) |

### Gameplay

- Destroy asteroids by shooting them
- Large asteroids split into medium asteroids
- Medium asteroids split into small asteroids
- Avoid colliding with asteroids
- You have 3 lives
- Survive as long as possible and maximize your score!

### Scoring

- Large asteroids: **20 points**
- Medium asteroids: **50 points**
- Small asteroids: **100 points**