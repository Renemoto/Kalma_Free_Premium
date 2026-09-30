# Migrate existing backend to .NET 10

## Objective and scope
Use the already installed user-local .NET 10 SDK to migrate all seven existing backend projects and their structural tests from net8.0 to net10.0. Do not modify the unrelated pre-existing untracked files or expand into KAL-002/CI/domain behavior. Branch: `feat/dotnet-10-migration` from `feat/kal-001-foundation` (`3d2b4e1`).

## Constraints and checks
- Strict TDD enabled by AGENTS.md / ADR-30; runner: `DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet test backend/Kalma.sln`; build uses the same environment and `dotnet build backend/Kalma.sln`. Require test-first RED where runnable, then GREEN, refactor and rerun; the environment interruption and delayed assertion RED are recorded below rather than represented as strict chronology.
- SDK `10.0.401` and ASP.NET/.NET runtimes `10.0.12` already exist under `$HOME/.dotnet`; default `/snap/bin/dotnet` is a separate 8.0.407 installation. No sudo, downloads or machine-wide package changes needed. The repo pins SDK 10.0.401 with `global.json`; this does not change the shell's chosen `dotnet` executable. Do not rewrite historical KAL-001 evidence.
- Workload forecast: ~60 authored changed lines, delivery strategy ask-on-risk. Commit one coherent migration with tests/docs; no push/PR/merge.

## Tasks
- [x] DOTNET10-A — Change the structural target-framework test first, record the initial runtime-blocked RED attempt and later observed assertion RED, migrate seven project TFMs to net10.0 using the installed SDK, verify the build and all tests under .NET 10, and document the SDK selection for future contributors. Route: delegated writer (multi-file). Status: done. Commit: `5758edefb78c61491b563246f02f0850cea7e51f`.

## Evidence
- Initial mapping: four production plus three xUnit projects all target net8.0; `backend/tests/Kalma.Domain.Tests/RepositoryFoundationTests.cs` hard-codes net8.0. The solution has no TFM. No global.json or CI workflow. Installed SDK verified by `$HOME/.dotnet/dotnet --list-sdks`: 10.0.401; runtimes 10.0.12.
- The structural test was changed first to expect net10.0. Its initial pre-migration run under the user-local SDK could not reach assertions because the net8.0 test host lacked a user-local runtime 8. After migration, the writer temporarily restored only the four source project TFMs to net8.0 while leaving test projects net10.0: the exact .NET 10 suite ran and failed on the intended assertion (expected net10.0, actual net8.0). This RED was observed after the initial GREEN, not before the production edits; the chronological strict-TDD sequence was interrupted by the environment. All source TFMs were restored to net10.0 and the exact suite passed 5/5 again.
- Build under SDK 10.0.401: passed, 0 warnings/errors; full suite: 5 passed, 0 failed. Parent spot check `dotnet test backend/Kalma.sln --no-restore` with local SDK: 5 passed. No package upgrades were necessary.
- Changes: seven project TFMs, one structural assertion/test name, `global.json` SDK pin and `backend/README.md` instructions. Commit `5758edefb78c61491b563246f02f0850cea7e51f` (`build: target .NET 10 across backend solution`), 48 additions and 9 deletions including tracking.
- Independent `gentle-ai-verify`: full build 0 warnings/errors, suite 5/5 passing; reviewed TFMs, SDK pin, README and scoped diff. Parent spot check 5/5 passing. CI unavailable until KAL-003.
- Native RDD switch on, but `assess` reported unassessable because three unrelated pre-existing untracked files remain in the parent Git root; `inspect` with explicit exclusion returned `empty_candidate_base_ref_required` for the clean workspace, and ordinary START with explicit committed base also returned blocked without lineage. No native receipt/acknowledgement claimed; independent verification supplied the fallback.

## Next step
Migration complete locally; next scheduled ticket remains KAL-002. Before ordinary use of `dotnet`, select `$HOME/.dotnet` ahead of Snap as documented in `backend/README.md`.
