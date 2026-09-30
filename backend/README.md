# Backend

## .NET SDK

The repository's `global.json` requests .NET SDK 10.0.100 with `latestFeature` roll-forward, so any installed .NET 10 feature band can be selected. It does not install or switch the `dotnet` executable. If the system `dotnet` is the .NET 8 Snap, put the user-local .NET installation first on `PATH` and set `DOTNET_ROOT` when running from the repository root:

```sh
DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet --version
DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet build backend/Kalma.sln
DOTNET_ROOT="$HOME/.dotnet" PATH="$HOME/.dotnet:$PATH" dotnet test backend/Kalma.sln
```
