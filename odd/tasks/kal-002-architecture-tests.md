# KAL-002 — Architecture dependency tests

## Objective and constraints
Enforce ADR-29 dependency direction before implementing domain behavior. KAL-001 is complete; KAL-003 (CI) is not in scope. The backlog calls this KAL-002 (the user's KAL-0002 is interpreted as that ticket). Detect forbidden project/package/framework references in Domain (EF Core, ASP.NET, Infrastructure) and forbidden Infrastructure references in Application. A deliberately broken reference must cause a failing test and be removed. Keep production architecture unchanged; no speculative libraries or ADR changes unless a decision actually changes.

## TDD and checks
Mode: strict (AGENTS.md, backlog criterion, ADR-30). Runner: `cd backend && "$HOME/.dotnet/dotnet" test` and `cd backend && "$HOME/.dotnet/dotnet" build`, established in KAL-001. Capture RED on a temporarily inserted forbidden reference, then GREEN after removing it. Full suite and build before closure. No CI yet.

## Tasks
- [x] KAL-002-A (verified and committed as `5b11b02`) — Added xUnit tests in Domain.Tests and Application.Tests and checked the three acceptance criteria in the backlog. RED observed by delegated writer for temporary ASP.NET FrameworkReference and both backslash-separated Domain/Application → Infrastructure project references; after finding a Linux path-separator blind spot, RED also observed for Domain → Application using the compiled test with `--no-build`. All temporary references restored; GREEN and full suite/build observed by writer. Independent verifier confirmed both assertions and reran `cd backend && "$HOME/.dotnet/dotnet" test` (5 passed), `cd backend && "$HOME/.dotnet/dotnet" build` (0 warnings/errors), and `git diff --check` (passed). Route: delegated writer (multiple nontrivial files), with independent verification after corrections. No ADR decision changed. Work-unit commit `5b11b02` (`test(backend): enforce architecture dependencies for KAL-002`) contains tests and acceptance criteria.

## Delivery
Forecast: roughly 120–220 authored changed lines; default `ask-on-risk`, below the ~400-line delivery threshold. Branch `feat/kal-002-architecture-tests` was created from `feat/kal-001-foundation`. User explicitly authorized the local commit; work-unit commit `5b11b02` contains 141 additions and 4 deletions in tests and backlog. This evidence update will be committed separately; no push or PR authorized. Engram mirror pending: no callable memory tools available in this session. Native review inspect previously requested intended-untracked selection; its continuation returned `consent-binding-expired` without a lineage or mutation. Review remains pending; no approval or receipt claimed. Native review candidate is a work-unit commit or PR slice, not the checkbox.

## Next step
KAL-002 is implemented, tested, and locally committed. Persist this tracking update and resolve native review only through fresh authority and consent. No CI claim until KAL-003; next implementation ticket requires its own scope.
