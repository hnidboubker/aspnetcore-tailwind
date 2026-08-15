# Context Handoff Manager Skill

## Purpose

Maintain continuous autonomous execution when the context window approaches its limit.

This skill automatically monitors context usage, creates a structured handoff checkpoint, switches the agent into Auto Mode, and continues execution in a new session without losing progress.

---

# Operating Mode

## Auto Mode

When this skill is triggered, the agent must automatically enable:

```
AUTO_MODE = ENABLED
```

While Auto Mode is active:

* Continue execution without requesting confirmation.
* Preserve all previous decisions and completed work.
* Resume unfinished tasks automatically.
* Execute the next defined action from the handoff state.
* Avoid unnecessary explanations or interruptions.
* Stop only when blocked by missing information, permissions, external dependencies, or safety constraints.

Priority order:

1. Preserve current state.
2. Preserve completed progress.
3. Restore execution context.
4. Continue task completion.

---

# Context Monitoring

Monitor context usage continuously.

Thresholds:

```
70%  → Start preparing context memory.
85%  → Create preliminary checkpoint.
90%  → Mandatory handoff and new session workflow.
```

Trigger:

```
IF context_usage >= 90%
THEN activate Context Handoff Manager.
```

---

# Context Handoff Workflow

## Step 1 — Enable Auto Mode

Immediately activate:

```
AUTO_MODE = ENABLED
```

The agent must continue the handoff process automatically.

---

## Step 2 — Analyze Current State

Collect and structure:

* Main objective
* Project goal
* Current progress
* Completed tasks
* Tasks currently in progress
* Remaining tasks
* Important decisions
* User requirements
* Constraints
* Modified files
* Created artifacts
* Errors and blockers
* Exact next action

---

## Step 3 — Create Checkpoint

Generate:

```
CONTEXT_HANDOFF.md
```

Content format:

```markdown
# CONTEXT HANDOFF REPORT

## Execution Mode

AUTO_MODE: ENABLED

## Main Objective

{main_objective}

## Current Project Status

{project_status}

## Completed Tasks

{completed_tasks}

## Tasks In Progress

{ongoing_tasks}

## Remaining Tasks

{pending_tasks}

## Decisions Made

{decisions}

## Requirements And Constraints

{constraints}

## Files And Artifacts

{files_and_artifacts}

## Errors And Blockers

{blockers}

## Next Action

{next_action}

## Resume Instruction

Continue automatically from this checkpoint.
Do not repeat completed tasks.
Maintain previous decisions.
```

---

# Session Migration

When a new session is created:

The agent must:

1. Load `CONTEXT_HANDOFF.md`.
2. Enable Auto Mode.
3. Validate the project state.
4. Identify the last unfinished task.
5. Continue execution immediately.

Startup command:

```
AUTO_MODE ENABLED.

Load previous checkpoint.
Verify current state.
Resume execution from the last unfinished task.
```

---

# Continuity Rules

The agent must:

* Never restart completed work.
* Never discard previous decisions.
* Maintain task history.
* Preserve project consistency.
* Update the checkpoint after major milestones.
* Create a new handoff before future context exhaustion.

---

# Autonomous Execution Rules

While AUTO_MODE is enabled:

The agent should:

* Plan the next action automatically.
* Execute available tasks sequentially.
* Validate results after each major step.
* Maintain progress tracking.
* Continue until completion or a blocking condition occurs.

---

# Stop Conditions

Auto Mode can stop only when:

* The main objective is completed.
* Required user input is unavailable.
* External access or permissions are missing.
* A safety restriction prevents continuation.

Otherwise:

```
CONTINUE EXECUTION.
```
