# .NET 10 SDK selection on remote main

## Objective and scope
Port only useful SDK-selection configuration from the local .NET 10 migration onto a branch based on remote `origin/main` (`23dcb79`). All seven projects on remote main already target `net10.0`; preserve existing code, tests, CI and history. User chose to publish a compatible branch after verification. No force push or main mutation.

## Constraints and checks
- Effective strict TDD from remote `AGENTS.md`; this infrastructure/doc-only change has no executable behavior needing a new test, so directly check SDK resolution and run the existing backend build/test/architecture suite. Runner: `DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet build backend/Kalma.sln` and same prefix `dotnet test backend/Kalma.sln`; SDK currently 10.0.401.
- Add a repository SDK policy accepting installed .NET 10 feature bands, and a short backend-specific guide for selecting a non-Snap dotnet installation. Do not transplant nested-root local commits, old structural tests, or historical KAL-001 docs. Preserve CI setup-dotnet 10.0.x behavior.
- Review workload forecast <100 authored changed lines; delivery strategy ask-on-risk. Single task, one coherent commit, then verify and publish new branch if safe.

## Tasks
- [ ] SDK10-PUBLISH-A — Pin a compatible .NET 10 SDK and document local invocation, verify seven TFMs and existing build/tests, commit on `feat/dotnet-10-sdk-config` based on origin/main, then push only that branch. Route: delegated writer for multi-file write. Status: in progress. Commit/push: pending.

## Evidence
- Remote main project tree at Git root has four source and three test projects already targeting net10.0, architecture tests, and `.github/workflows/backend-ci.yml` installing 10.0.x. No global.json or backend README. Original local branch lives in unrelated parent-root history and must not be pushed as-is.
- Delegated writer added only `global.json` (10.0.100, latestFeature) and `backend/README.md` with user-local SDK instructions; seven project files and CI untouched. Direct check selected SDK 10.0.401 and the full build passed with 0 warnings/errors; full test suite passed 5/5. Parent spot check `dotnet test backend/Kalma.sln --no-restore` passed 5/5. No artificial failing test was added for this config/doc-only change.
- Native review: pending work-unit commit. Push: pending.

## Next step
Commit the verified change, assess and inspect native review for that commit, then publish only `feat/dotnet-10-sdk-config` if no blocking review outcome.
