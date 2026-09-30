# KAL-003 — Backend continuous integration

## Objective and scope
Add GitHub Actions CI for the current .NET 10 backend solution and establish a required passing check before merging into `main`. KAL-002 is present locally; KAL-004 web tests are out of scope. Source branch: `feat/kal-003-ci` from `e310694` in the parent Git repository. Delivery branch: root-aligned `feat/kal-003-root-ci`, checked out at `/tmp/kalma-003-root-check`. Preserve pre-existing untracked files.

## Constraints and checks
- Strict TDD: required by `AGENTS.md` and ADR-30. Runner: `DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet build backend/Kalma.sln` and the same prefix with `dotnet test backend/Kalma.sln`. For CI configuration, validate an initially failing structural CI assertion before adding the workflow, then rerun and refactor.
- Workflow should run on every push and on pull requests to `main`; use the pinned .NET 10 SDK from `global.json`, build all seven projects and run all solution tests. No web build or deploy.
- The merge gate requires a GitHub ruleset/branch protection and a published check; local YAML alone cannot satisfy it. No push, PR, or remote settings change without explicit user decision. Mark only proven criteria complete.
- Delivery strategy: ask-on-risk; forecast ~120 authored lines (workflow, configuration test and task/backlog evidence). One CI work unit, with external gate follow-up. User selected preparation of a root-aligned Kalma branch without publication or main mutation; commit the tested nested work unit first, then create a root-aligned branch from the Kalma subtree and verify there. Native review candidate is the work-unit commit.

## Tasks
- [x] KAL-003-A — Write a failing CI configuration test, implement the workflow, verify RED/GREEN and full backend build/tests, commit the coherent CI unit. Route: delegated writer (multiple non-trivial files). Acceptance: push and PR triggers, .NET 10 setup, solution build and test are structurally enforced and local tests pass. Status: done; source commit `5b5f44cc27077a86203a99a38a65617a098cfa8e`, root-aligned equivalent `aa9d78c34ad97293f75fc298b9c4e809c0042ab0`.
- [ ] KAL-003-B — Verify the workflow on GitHub and require the passing check for merges into `main`; update the KAL-003 backlog criteria only after evidence. Route: external verification/configuration requiring publication and repository administration. Status: pending, awaiting explicit publication/settings authorization.

## Progress and evidence
- Read-only map found no workflow in this checkout. Remote `main` belongs to a separately rooted history; its previous CI does not prove this candidate's CI or gate. Git root is `/home/renex/Proyectos`; pre-existing untracked `../.gitignore`, `.gitignore`, `AGENTS.md` are not in scope.
- Delegated implementation added `.github/workflows/backend-ci.yml` and a structural assertion in `backend/tests/Kalma.Domain.Tests/RepositoryFoundationTests.cs`. RED: focused test failed because workflow did not yet exist. GREEN: focused tests 4/4; full solution build 0 warnings/errors, tests 10/10. Review of files confirmed .NET 10 `global.json`, push and main PR triggers, checkout/build/test. `git diff --check` passed. These are local results only; no GitHub Actions run or merge rule was proven.
- Root alignment: user chose a local root-aligned branch without publication. Source commit `5b5f44c` contains the tested unit; `git subtree split --prefix=Kalma_Free_Premium` created `feat/kal-003-root-ci` at `aa9d78c`. In its clean worktree, `.github/workflows/backend-ci.yml` is tracked at Git root; independent verifier built successfully (0 warnings/errors) and ran 10/10 tests. Parent spot check in source tree also ran 10/10. No hosted Actions run yet.
- Native review of root-aligned commit `aa9d78c` against `4858b0c` (56 changed lines, high risk) closed approved and exact acknowledgement burned authority for lineage `review-bcd14ee179a16b2b`. One informational resilience warning R4-001 was non-blocking; no correction transition. Review is not permission to publish. The root-aligned branch has no merge base with current remote `origin/main`, so a regular PR into that old history is not yet a viable integration plan.

## Next step
Obtain separate publication/integration decision for the unrelated remote `main`, then verify Actions on the published root-aligned branch and configure the required passing check. Do not mark KAL-003 backlog complete before remote evidence.
