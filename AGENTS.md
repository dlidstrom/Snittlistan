# Agent notes for this repo

## Never commit automatically

Do not run `git commit` (or anything that results in a commit) unless the
user explicitly asks for one in that turn. Leave changes staged/unstaged in
the working tree and describe what you changed; let the user decide when to
commit. This applies even if something else in the environment appears to
auto-commit on file edits — that is not you, and it is not wanted. If you
notice commits appearing on `main` that you didn't create, stop and flag it
to the user before making further edits.

## Building

This is a classic .NET Framework MVC web app (not .NET Core/5+). `dotnet
build` on macOS fails with `MSB4019` because it needs
`Microsoft.WebApplication.targets`, which only ships with a full Visual
Studio / MSBuild install. There is no working build on this machine — verify
changes by careful manual review instead of trying to compile.

## CSS: main.less vs main.css vs Site.css

- `Snittlistan.Web/Content/css/main.less` is the LESS source for the styles
  used by the **V2** area layout (`Areas/V2/Views/Shared/_Layout.cshtml`,
  which links `main.css` directly).
- `Snittlistan.Web/Content/css/main.css` is the compiled output, but there is
  **no build step** that compiles it automatically — the `.csproj` only
  includes `main.less` as inert `<Content>`. Historically someone has hand-
  edited `main.css` directly without updating `main.less`, so the two can
  drift (they did: a stray `body` rule, extra `.label-team-*` selector
  variants, and a `calc()` value differed until backported in this session).
- `Site.css` is a separate, plain hand-written stylesheet used only by the
  **V1** area layout (`Areas/V1/Views/Shared/_Layout.cshtml` via
  `Content.Css("Site.css", Url)`). It is *not* loaded by V2 pages — don't put
  V2-relevant fixes there.
- `lessc` (the LESS compiler, `npm install -g less`) is available in this
  environment now. When editing styles used by V2 pages, edit `main.less`
  and mirror the change into `main.css` by hand (or recompile with `lessc`
  and diff carefully against current `main.css` first, since a blind
  recompile can silently drop drifted hand-edits — check for that before
  overwriting).

## MatchResultAdminController manual match entry

- The manual 8-player match entry/edit form (`RegisterMatchViewModel.PostModel`)
  models each series as 4 "tables" pairing two of *our own* players, each
  sharing one win/loss (`MatchTable.Score` is 0 or 1, shared by both
  `Game1`/`Game2` players — see `MatchTable.cs`/`MatchSerie.cs`). This is
  real doubles-style scoring, not a hack: because wins always land on a pair,
  the sum of everyone's `TableWins` (bordpoäng) is mathematically always
  even. Don't "fix" that parity check without re-deriving why it's there.
- `TeamScore`/`OpponentScore` are free-typed aggregates and are **not**
  currently cross-checked against summed `TableWins` + lane points
  (banpoäng, 0–4, which can be less than 4 when a lane pin-count ties). The
  domain layer (`MatchResult.VerifyScores`) already allows
  `TeamScore + OpponentScore <= 20`, not forced `== 20`.
- Any `int?[]`/collection property on a model bound by `DefaultModelBinder`
  must **not** use an inline initializer (`= new int?[4]`) if the model is
  also used as an MVC POST action parameter. The binder treats a non-null
  existing array as "update this collection in place" and calls `Clear()`,
  which throws `NotSupportedException: Collection is read-only` for fixed-
  size arrays. Initialize such properties explicitly in GET-side factory
  methods (e.g. `ForCreate`/`ForEdit`) instead of via property initializer.
  (This bit `OpponentSeriesPins` — see git history around Sept 2026.)
