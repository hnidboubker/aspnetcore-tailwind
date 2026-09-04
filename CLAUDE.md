# CLAUDE.md

Project-level governance for aspnetcore-tailwind. This file complements, and does not replace,
the global instructions at `C:\Users\DevOps\.claude\CLAUDE.md` — read that one
too; where the two disagree, the global file wins.

For what this project actually is (stack, structure, current state), see
`AI_CONTEXT.md` and `.agents/05_ARCHITECTURE.md` — this file is governance only.

## Mandatory reading order

Before any work in this repository, read, in this order:

1. `PROJECT_MEMORY.md` — current state, decisions, open questions, constraints
2. `.agents/00_START_HERE.md` — project overview and governance entry point
3. `.agents/02_QUESTION_PROTOCOL.md` — when and how to ask before acting
4. `.agents/01_RULES.md` — non-negotiable rules
5. `.agents/03_CHECKLIST_BEFORE_COMMIT.md` — pre-commit checklist
6. `.agents/04_LANGUAGE_SPECIFIC.md` — language/stack conventions for this repo
7. `.agents/05_ARCHITECTURE.md` — project layout and responsibilities
8. `.agents/06_SKILLS_AVAILABLE.md` — skills registered for this project
9. `.agents/07_AUDIT_REQUIREMENTS.md` — what must be verified before work is "done"
10. `.agents/08_AUTO_ISSUE_SKILL.md` — priority skill for automatic issue creation
11. `.agents/09_MCP_GITHUB_CONFIG.md` — GitHub MCP token setup for auto-issue creation
12. `.agents/10_COMPLETE_WORKFLOW.md` — the three priority skills chained end to end
13. `.agents/11_MAINTENANCE_AI_CONTEXT_INDEX.md` — keeping `AI_CONTEXT.md` and
    `PROJECT_INDEX.md` current

## Core rules (see `.agents/01_RULES.md` for full detail)

- Never assume, never modify without validation, never delete without explicit
  agreement from Houssine.
- Git is human-controlled: agents stop at `READY_FOR_COMMIT`; only Houssine runs
  `git commit`, `git push`, or `git merge`.
- A bug, error, failing test, or CI failure triggers `auto-issue-on-bug-detection`
  automatically, before diagnosis (`issue-resolution`) begins.
- Conversation happens in French; code, documentation, and commit messages are
  written in English.

## Navigation

- `AI_CONTEXT.md` — fast project orientation
- `PROJECT_INDEX.md` — which file to read for which task
- `AGENTS.md` — pointer to this file, for tools that look for `AGENTS.md` by
  convention instead of `CLAUDE.md`
