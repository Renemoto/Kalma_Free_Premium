# KAL-003 — Backend continuous integration

## Objective and scope
Add GitHub Actions CI for the current .NET 10 backend solution and establish a required passing check before merging into `main`. KAL-002 is present locally; KAL-004 web tests are out of scope. Branch: `feat/kal-003-ci` from `e310694` in the parent Git repository (`Kalma_Free_Premium/` is a subdirectory). Preserve pre-existing untracked files.

## Constraints and checks
- Strict TDD: required by `AGENTS.md` and ADR-30. Runner: `DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet build backend/Kalma.sln` and the same prefix with `dotnet test backend/Kalma.sln`. For CI configuration, validate an initially failing structural CI assertion before adding the workflow, then rerun and refactor.
- Workflow should run on every push and on pull requests to `main`; use the pinned .NET 10 SDK from `global.json`, build all seven projects and run all solution tests. No web build or deploy.
- The merge gate requires a GitHub ruleset/branch protection and a published check; local YAML alone cannot satisfy it. No push, PR, or remote settings change without explicit user decision. Mark only proven criteria complete.
- Delivery strategy: ask-on-risk; forecast ~120 authored lines (workflow, configuration test and task/backlog evidence). One CI work unit, with external gate follow-up. User selected preparation of a root-aligned Kalma branch without publication or main mutation; commit the tested nested work unit first, then create a root-aligned branch from the Kalma subtree and verify there. Native review candidate is the work-unit commit.

## Tasks
- [ ] KAL-003-A — Write a failing CI configuration test, implement the workflow, verify RED/GREEN and full backend build/tests, commit the coherent CI unit. Route: delegated writer (multiple non-trivial files). Acceptance: push and PR triggers, .NET 10 setup, solution build and test are structurally enforced and local tests pass. Status: in progress; user selected root-aligned branch preparation, without publication.
- [ ] KAL-003-B — Verify the workflow on GitHub and require the passing check for merges into `main`; update the KAL-003 backlog criteria only after evidence. Route: external verification/configuration requiring publication and repository administration. Status: pending, awaiting explicit publication/settings authorization.

## Progress and evidence
- Read-only map found no workflow in this checkout. Remote `main` belongs to a separately rooted history; its previous CI does not prove this candidate's CI or gate. Git root is `/home/renex/Proyectos`; pre-existing untracked `../.gitignore`, `.gitignore`, `AGENTS.md` are not in scope.
- Delegated implementation added `.github/workflows/backend-ci.yml` and a structural assertion in `backend/tests/Kalma.Domain.Tests/RepositoryFoundationTests.cs`. RED: focused test failed because workflow did not yet exist. GREEN: focused tests 4/4; full solution build 0 warnings/errors, tests 10/10. Review of files confirmed .NET 10 `global.json`, push and main PR triggers, checkout/build/test. `git diff --check` passed. These are local results only; no GitHub Actions run or merge rule was proven.
- Blocking layout: the local Git root is the parent `Proyectos/`, so a commit/push of this branch places the workflow at `Kalma_Free_Premium/.github/workflows/`, not the remote repository's root `.github/workflows/`. GitHub does not discover nested workflows. User chose a root-aligned Kalma branch prepared locally, without publishing or modifying `main`.

## Next step
Commit the tested CI work unit, prepare the root-aligned branch and verify its tree; keep remote CI and merge gate pending.
