# AI Assistance & Decision Log (Assignment 10)
Student: Mansur Zhailaubayev
Project: 2D Mobile Platformer

## Development & AI Integration Log:
- 2026-11-09: Designed FSM architecture for PatrolEnemy, ShooterEnemy, and ChargerEnemy using serialized fields and state machine patterns.
- 2026-11-10: Refactored ShooterEnemy and Boss scripts to handle dynamic target leading using Rigidbody2D projectile setup with direction vectors.
- 2026-11-10: Integrated bullet collision logic with the pre-existing Health component and Hazard base class (`Health.TakeDamage(1)`).
- 2026-11-10: Playtest feedback: Tester 1 took 4 tries, Tester 2 took 3 tries. Adjusted Shooter wind-up (0.4s -> 0.6s) and Charger speed (14 -> 10) to guarantee telegraph visibility and fair dodge timing.
