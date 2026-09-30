# Backend development

The backend targets .NET 10. SDK `10.0.401` is pinned in the repository's `global.json`. If your shell's `dotnet` resolves to another installation (for example, the Snap .NET 8 SDK), select the user-local SDK explicitly from the repository root:

```sh
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$HOME/.dotnet:$PATH"
dotnet build backend/Kalma.sln
dotnet test backend/Kalma.sln
```

Alternatively, invoke `$HOME/.dotnet/dotnet` directly. The PATH override matters because `global.json` selects an SDK only from the `dotnet` executable being invoked; it does not switch installations.
