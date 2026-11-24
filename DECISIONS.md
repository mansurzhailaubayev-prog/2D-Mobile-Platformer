# AI Assistance & Decision Log (Assignment 10)
Student: Mansur Zhailaubayev
Project: 2D Mobile Platformer

## Development & AI Integration Log:
- 2026-11-09: Designed FSM architecture for PatrolEnemy, ShooterEnemy, and ChargerEnemy using serialized fields and state machine patterns.
- 2026-11-10: Refactored ShooterEnemy and Boss scripts to handle dynamic target leading using Rigidbody2D projectile setup with direction vectors.
- 2026-11-10: Integrated bullet collision logic with the pre-existing Health component and Hazard base class (`Health.TakeDamage(1)`).
- 2026-11-10: Playtest feedback: Tester 1 took 4 tries, Tester 2 took 3 tries. Adjusted Shooter wind-up (0.4s -> 0.6s) and Charger speed (14 -> 10) to guarantee telegraph visibility and fair dodge timing.
## Assignment 11 — Data and Saves (2026-11-17)
- Moved all hardcoded enemy, boss, and player balance values into EnemyStats and PlayerStats ScriptableObjects (0 magic numbers in scripts).
- Created Level_01, Level_02, and Level_03 LevelData assets to dynamically spawn enemies and configure level timers via LevelLoader.
- Implemented SaveManager with JsonUtility and Application.persistentDataPath instead of BinaryFormatter; used List<int> for unlocked levels because JsonUtility ignores Dictionary.
- Added try/catch fallback to default GameData in LoadGame() to prevent crashes on missing or corrupted save.json files, plus auto-save on level exit, volume change, and OnApplicationPause(true).
## Assignment 12 — Sound Pass (2026-11-25)
- 2026-11-25: Converted Music and SFX volume sliders from linear scale to logarithmic decibels (Log10 * 20) in AudioManager so 50% volume feels natural.
- 2026-11-25: Added random pitch variation (0.92–1.08) and multiple hit clips to prevent repetitive robotic sound effects.
- 2026-11-25: Built a single DontDestroyOnLoad AudioManager with dual AudioSources for 1-second music crossfading without restarting tracks on scene reload.
- 2026-11-25: Configured a Paused AudioMixer snapshot (-12 dB attenuation and 900 Hz Lowpass filter) triggered by PauseManager to soften music on pause.
