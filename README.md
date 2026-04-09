# Scholarlab Assignment

A Unity project with two educational mini-games: a **3D Chemistry Lab Simulation** and a **2D Animal Quiz Game**, accessible from a shared main menu.

---

## Prerequisites

- **Unity Editor 2022.3.21f1** (LTS) — install via [Unity Hub](https://unity.com/download)
- **Platform Modules** — install Windows Build Support and/or Android Build Support via Unity Hub

---

## How to Build & Run

1. Clone or download this repository.
2. Open **Unity Hub** → click **Open** → select the project folder.
3. Unity will detect the required version (**2022.3.21f1**) and prompt installation if needed.
4. Wait for asset import to complete (first time may take a few minutes).
5. If prompted, click **Import TMP Essentials** for TextMeshPro.
6. Open **File → Build Settings** and verify the scene order:

   | Index | Scene |
   |-------|-------|
   | 0 | `Assets/Scenes/EntryPoint.unity` |
   | 1 | `Assets/Scenes/Assignment 1.unity` |
   | 2 | `Assets/Scenes/Assignment 2.unity` |

7. Select your target platform (Windows / Android), then click **Build and Run**.

> To run in the editor, open `Assets/Scenes/EntryPoint.unity` and press **Play**.

---

## Project Structure

```
Assets/
├── Scenes/
│   ├── EntryPoint.unity          # Main menu
│   ├── Assignment 1.unity        # Chemistry Lab
│   └── Assignment 2.unity        # Animal Quiz
├── EntryPoint.cs                 # Menu scene controller
├── Assignment 1/
│   ├── Scripts/                  # Lab simulation logic
│   ├── Models/                   # 3D models (flasks, test tubes, character)
│   ├── Animations/               # Character animations
│   ├── Effects/                  # Particle effects (Cartoon FX Remaster)
│   ├── Materials/                # Lab materials
│   ├── Sounds/                   # Audio clips
│   └── Texture/                  # Environment textures
├── Assignment 2/
│   ├── Scripts/                  # Quiz game logic
│   ├── Data/Animals/             # 18 AnimalData ScriptableObjects
│   ├── Prefabs/                  # Card prefabs
│   └── Sprites/                  # Animal card images
└── Resources/
    └── guidance_steps.json       # Step-by-step guidance text
```

---

## Assignment 1 — Chemistry Lab Simulation

A 3D interactive experiment where the player pours chemicals into two flasks and shakes them to observe reactions.

**How to Play:**
1. Click **Flask A** to pour the chemical → click again to shake (color changes).
2. Click **Flask B** to pour the remaining chemical → click again to shake (smoke particle effect).
3. The character reacts with animations and a completion panel appears.
4. Use **Restart** or **Home** buttons to replay or return to the menu.

**Key Scripts:**

| Script | Purpose |
|--------|---------|
| `GameManager.cs` | Shows completion panel, handles scene navigation |
| `InteractionManager.cs` | Enforces step sequence (PourA → ShakeA → PourB → ShakeB → Complete) |
| `Flask.cs` | Handles click interactions, liquid color change, shaking animation |
| `TestTube.cs` | Animates pouring: moves to flask, tilts, fills liquid, returns |
| `GuidanceManager.cs` | Displays step-by-step guidance text from JSON |
| `AudioManager.cs` | Manages SFX playback |
| `CharacterAnimatorController.cs` | Triggers character reactions (Excited / Irritated) |

---

## Assignment 2 — Animal Quiz Game

A 2D card-sorting quiz where the player drags 18 animal cards into the correct bucket based on a randomly selected category.

**How to Play:**
1. A random category is selected (e.g., Flying vs Non-Flying). Buckets are labelled accordingly.
2. Click a card to view its name and description.
3. Drag each card into the correct bucket.
4. A finish screen shows your score and lists of correct/incorrect answers.

**Quiz Categories:** Flying vs Non-Flying, Insect vs Non-Insect, Omnivorous vs Herbivorous, Group vs Solo, Eggs vs Birth.

**Key Scripts:**

| Script | Purpose |
|--------|---------|
| `GameManager.cs` | Scene navigation (restart / home) |
| `QuizManager.cs` | Loads animals, selects category, spawns cards, evaluates answers |
| `UIManager.cs` | Updates bucket labels, feedback text, popups, and finish screen |
| `CardDragHandler.cs` | Drag-and-drop and tap-to-view card interactions |
| `Bucket.cs` | Receives dropped cards and triggers evaluation |
| `QuizEvents.cs` | Static event bus decoupling managers from UI |
| `AnimalData.cs` | ScriptableObject with animal traits and category matching |

---

## Dependencies

All packages are auto-resolved by Unity from `Packages/manifest.json`:

- TextMesh Pro 3.0.6
- Timeline 1.7.6
- Unity UI (uGUI) 1.0.0
- Visual Scripting 1.9.1

---

## Third-Party Assets

| Asset | Source | Purpose |
|-------|--------|---------|
| **Cartoon FX Remaster Free** | [Unity Asset Store](https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565) | Particle effects (smoke, chemical reactions) |
| **Sound Effects** (pouring, shaking) | Free online SFX resource | Audio feedback for flask interactions |

All other assets (3D models, shaders, textures, character, animations, animal sprites) were **provided as part of the assignment**.

---

## Troubleshooting

| Issue | Solution |
|-------|---------|
| Scenes not loading | Verify build order in **File → Build Settings** (EntryPoint=0, Assignment 1=1, Assignment 2=2) |
| Pink/magenta materials | Project uses Built-in Render Pipeline — do not switch to URP/HDRP |
| TMP font errors | Import TMP Essentials when prompted on first open |
| Android build fails | Install Android Build Support (SDK + NDK) via Unity Hub |
| "No Animal Data" warning | Ensure QuizManager has all 18 AnimalDataSO assets assigned in the Inspector |

---

## Author

**Harsh Sharma**

*Built with Unity 2022.3.21f1 LTS*