# KAL-003 — Backend continuous integration

## Objective and constraints
Add GitHub Actions build and full backend test execution on every push (and pull requests targeting main), then require the successful check for merges into main. Depends on completed KAL-002. Do not claim a remote run or merge protection from local workflow content alone. KAL-004 owns frontend CI; no frontend project exists yet. No architecture decision changes expected.

## TDD and checks
Mode: strict (AGENTS.md). Runner for backend: `cd backend && "$HOME/.dotnet/dotnet" test` and `cd backend && "$HOME/.dotnet/dotnet" build` (KAL-001/002 evidence); CI runner uses `dotnet` from setup-dotnet 10. Workflow acceptance: observe missing workflow as RED via a local structural check, then GREEN after implementation. Local build/tests prove the referenced solution works, not that GitHub ran it. Remote Actions and branch protection require separate observed evidence.

## Tasks
- [x] KAL-003-A — Created minimal backend workflow for all pushes and pull requests to main, using .NET 10 and build plus full solution tests. Route: delegated writer (workflow file) and independent verifier after native assessment was unassessable due to untracked files. RED: `test -f .github/workflows/backend-ci.yml` exited 1 before creation; GREEN: same check passed afterward. Writer and independent verifier observed local `cd backend && "$HOME/.dotnet/dotnet" build` (0 warnings/errors) and `cd backend && "$HOME/.dotnet/dotnet" test` (5 passed); parent spot check repeated the tests (5 passed). Readback found and corrected an initial push-to-main filter; final workflow triggers every push and PRs targeting main. Staged `git diff --cached --check` passed. Work-unit commit `43a3d28` (`ci(backend): build and test on pushes and pull requests`). No remote run observed; backlog criteria stay open.
- [ ] KAL-003-B — Verify GitHub Actions run and configure/check required status for main; demonstrate a failing check blocks merge. Route: remote verification/configuration pending access and authorization for repository settings. Do not mark either backlog criterion complete without evidence of the corresponding remote behavior.

## Delivery
Forecast: ~50–100 authored changed lines; strategy `ask-on-risk`. Starting branch point: `5d95761` on `feat/kal-002-architecture-tests`; branch `feat/kal-003-ci` created. Engram mirror pending because no callable Engram tools are available. No push or PR authorized. Native assessment returned `unassessable` for untracked candidate; independent verifier executed. Native inspect requires explicit intended-untracked selection before START; no lineage created.

## Next step
A GitHub Actions run, required status rule for main, and negative merge-block demonstration remain pending; `gh` is not installed and no remote evidence is available. No push or repository-settings change authorized.
