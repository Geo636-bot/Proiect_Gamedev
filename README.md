# Proiect_Gamedev – Team 6

A **top-down 2.5D** survival game made in Unity for the course *Introducere în Programarea Jocurilor pe Calculator* (FMI, University of Bucharest, 2026–2027).

> Working title from the team form: "Don't Starve". We will replace it with our own name.

## Game idea

You are alone on a wild map and you have to gather resources to survive.

- **Genre:** survival, top-down 2.5D (tilted top-down camera)
- **Core mechanic:** explore the map and gather wood, stone and berries.
- **Planned after Sprint 1:** hunger, day/night cycle, monsters at night, crafting.

## Team

| Member | GitHub | Role |
|---|---|---|
| Dîlgeanu Sergiu Nicolae | [v33sergiulica](https://github.com/v33sergiulica) | Team Leader |
| Luca George-Iulian | [Geo636-bot](https://github.com/Geo636-bot) | Gameplay / systems programmer |
| Voiculet Iulian-Alexandru | [Iulihix](https://github.com/Iulihix) | Level & game designer |
| Urda Teodor-Vladimir | [UrdaTeodor](https://github.com/UrdaTeodor) | UI / UX designer |
| Prodan Adrian-Andrei | [Andrehix](https://github.com/Andrehix) | Sound designer |

## How to run the game

**Ready-made build (Windows 10/11, no Unity needed):** unzip `Build_Executabil/ProiectGamedev_Sprint1_Windows.zip` and run `ProiectGamedev.exe` (details in [`Build_Executabil/`](Build_Executabil/)).

**From Unity:**

1. Install editor **6000.6.8f1** (Unity 6.6) from Unity Hub, with the Windows Build Support module.
2. Clone the repo: `git clone https://github.com/Geo636-bot/Proiect_Gamedev.git`
3. Unity Hub → **Add** → **Add project from disk** → pick the cloned folder.
4. Open the scene `Assets/_Scenes/Game.unity` and press **Play**.

No credentials or external services are needed.

## Controls

| Key | Action |
|---|---|
| WASD / arrow keys (left stick on gamepad) | move |
| Left Shift | sprint |
| E (A / Cross on gamepad) | gather the nearby resource |
| Mouse wheel | zoom (3 levels) |

Each press of **E** takes one unit from the resource. When the resource is empty, it disappears from the map.

## Architecture

Unity 6 project (URP) with the Input System and uGUI + TextMeshPro packages.

```
Assets/
  _Scenes/Game.unity          main scene
  _Scripts/
    World/      ResourceType, ResourceNode, WorldGenerator
    Player/     PlayerMovement, PlayerInventory, PlayerGatherer
    Camera/     CameraFollow
    UI/         HUDController
    Audio/      AudioManager
  _Prefabs/World/             Tree (+ variant Tree_Pine), Rock (+ variant Rock_Big), BerryBush
  _Input/                     PlayerControls.inputactions + the C# class generated from it
  _Materials/  _Audio/        materials, music and sound effects
  Settings/                   URP settings
Rapoarte_Sprint/              sprint reports
Build_Executabil/             Windows build
```

| Script | What it does |
|---|---|
| `WorldGenerator` | Spawner: at start it scatters trees, rocks and bushes at random (with a minimum distance between them and a free area around the player). The objects become children of the `ResourceSpawner` object. |
| `ResourceNode` | A resource on the map (type, name, amount). Each gather takes one unit and plays a small visual effect. |
| `PlayerMovement` | Reads the input (Input System) in `Update` and moves the `Rigidbody` in `FixedUpdate`, relative to the camera. |
| `PlayerGatherer` | 10 times per second it looks for the closest resource (`OverlapSphere` + the `Resource` tag + `TryGetComponent`) and gathers it when E is pressed. |
| `PlayerInventory` | Counts the resources and reports changes through a C# event. |
| `CameraFollow` | Top-down camera that follows the player smoothly (exponential smoothing, as in lecture 4), with 3 zoom levels. |
| `HUDController` | Shows the resources and the possible action near a resource (`[E] Chop` / `Break` / `Gather`). It updates only on events, not every frame. |
| `AudioManager` | Looping background music and sound effects when gathering (`PlayOneShot`, with a slightly different pitch each time). |

The scene hierarchy is grouped under separator objects: `--- CAMERA ---`, `--- ENVIRONMENT ---`, `--- PLAYER ---`, `--- UI ---`, `--- AUDIO ---`.

## Sprint 1 (weeks 5–7, Oct 29 – Nov 12)

| Goal | Owner | Status |
|---|---|---|
| Unity project + repo, `.gitignore`, folder structure, base scene (graybox: floor, walls, ruins) | Teodor | ✅ |
| Resources as prefabs (+ Prefab Variants) and the map generator | Iulian | ✅ |
| Player: movement with Input System + Rigidbody, camera with zoom, resource gathering | George | ✅ |
| HUD: collected resources, interaction hint, controls | Sergiu | ✅ |
| Background music and gather sound effects | Andrei | ✅ |
| Windows build and documentation | Sergiu | ✅ |

**Next sprint:** hunger bar, day/night cycle, first monsters, pause menu.

Sprint reports go in [`Rapoarte_Sprint/`](Rapoarte_Sprint/).

## Feedback

Open an [Issue](https://github.com/Geo636-bot/Proiect_Gamedev/issues) on this repository or message the Team Leader (Dîlgeanu Sergiu) on Teams.

## Credits

- Background music "The Unwinding Key": Andrei Prodan
- Sound effects: procedurally generated (placeholders)
- Font: Liberation Sans (TextMeshPro, OFL license)
