# KAL-001 — Repository and .NET solution foundation

## Objective and scope
Create the repository layout and .NET solution described in ADR-29/34 and the KAL-001 backlog, without implementing domain behavior, architecture enforcement (KAL-002), CI (KAL-003), or a Vite frontend (KAL-004). The Git root is the parent `Proyectos/`; all ticket changes stay inside `Kalma_Free_Premium/`. The existing Spanish architecture document remains authoritative; copy it to `docs/kalma-arquitectura.md` for the named acceptance criterion.

## Constraints and verification
- Strict TDD: enabled by `AGENTS.md`, ADR-30 and the backlog. Runner: `dotnet test backend/Kalma.sln`; build: `dotnet build backend/Kalma.sln`. Bootstrap the test harness first, observe a meaningful RED for each structural acceptance criterion before implementing the missing scaffold, then GREEN and refactor/check again. Do not claim a test ran if its project does not yet exist.
- .NET SDK 8.0.407 is installed. Choose net8.0 for a compatible baseline; no database or web package dependencies at this ticket.
- Scope: `backend/` four source projects and three xUnit projects, `web/` directory placeholder, `docs/adr/` directory placeholder, architecture copy, and KAL-001 backlog evidence. No speculative modules or production behavior.
- Delivery strategy: ask-on-risk; forecast under ~400 handwritten changed lines excluding template-generated scaffolding and document copy. Native review candidate is a work-unit commit, not this checkbox. No push/PR/merge.

## Tasks
- [ ] KAL-001-A — Bootstrap the seven-project solution and verify its repository structure. Route: delegated writer (multi-file write and preparation trigger). Acceptance: repository has `backend/`, `web/`, `docs/adr/`; four source projects (Domain, Application, Infrastructure, Api), three xUnit test projects; exact inward project references Application → Domain, Infrastructure → Application, Api → Infrastructure and Application; `docs/kalma-arquitectura.md` is an exact copy of the authoritative architecture document. Write failing tests/checks first and observe RED, then make them pass. Run `dotnet build backend/Kalma.sln` and `dotnet test backend/Kalma.sln`; record actual results. Mark KAL-001 backlog checkboxes only with observed evidence. Finish with at least one scoped Conventional work-unit commit and record its identity. Status: in progress.

## Progress and evidence
- Exploration: no source, solution, CI or tests exist in the fresh tree; docs contain historical checkboxes that are not code evidence. Git branch `feat/kal-001-foundation` created from unborn `master` on parent Git root. `dotnet --version`: 8.0.407.
- RED: delegated writer ran `dotnet test backend/Kalma.sln` against the bootstrapped test harness: three structural tests failed for missing layout/projects/references and the architecture copy. Intermediate assembly-inspection assertion also failed and was replaced by a direct project-file assertion.
- GREEN: same command passed after scaffold (3 Domain, 1 Application, 1 Infrastructure; total 5). Writer ran `dotnet build backend/Kalma.sln` (0 warnings/errors) and `dotnet test backend/Kalma.sln` (5 passed). Parent spot check `dotnet test backend/Kalma.sln --no-restore` passed (5 total, 0 failed). Writer reported `cmp` showed the architecture copy identical and `git check-ignore` covered build outputs.
- Backlog: writer checked KAL-001 plus five criteria only after those observations. Removed leftover empty template test file. KAL-002/KAL-003/KAL-004 remain untouched.
- Commit: pending. Review/risk: RDD clone switch reports on; pending commit-scoped native assessment.

## Next step
Commit only ticket-owned files as one work unit, assess its committed range, then close the task with commit and review evidence.
