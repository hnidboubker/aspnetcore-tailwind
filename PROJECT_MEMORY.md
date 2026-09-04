File ID: PM-00
Prev: none
Next: none
Root: PROJECT_MEMORY.md

## Summary

TODO: one paragraph on what aspnetcore-tailwind is and does — fill in the next time real work
happens in this repo (see `AI_CONTEXT.md` once it is filled in, and
`.agents/05_ARCHITECTURE.md`).

This file and the `.agents/` governance set were scaffolded automatically by
`scaffold-governance.ps1` because they were missing, per Houssine's global rule
that governance structure stay identical across all projects. Governance-only
files were written in full; stack-dependent files (03, 04, 05, 07) were left as
TODO skeletons pending a session that actually works in this repo's code.

## Decisions

- All Git publish operations (commit, push, merge) are human-controlled only.
- Bugs/errors/failing tests trigger automatic GitHub issue creation before fix
  work begins.

## Open Questions

- TODO: fill in the project's real tech stack, architecture, and current state
  once someone actually works in this repo.

## Important Constraints

- Rotate this file at 300 lines into `PROJECT_MEMORY_01.md`, etc., keeping
  `File ID` / `Prev` / `Next` / `Root` consistent.
- Read order before any action: `CLAUDE.md` at repo root lists the full
  mandatory order.
