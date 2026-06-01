# Library Service API (Backend)

API ASP.NET Core 8 con autenticación JWT.

## Repositorio y despliegue

| Recurso | URL |
|---------|-----|
| Repositorio | https://github.com/SamCocho127/LibrariesApiTestInvestigacion.git |
| API desplegada | http://investigation2prograiv.runasp.net/ |
| Swagger (desarrollo) | http://127.0.0.1:5219/swagger |

## Credenciales demo (hardcoded, sin BD de usuarios)

- Email: `admin`
- Password: `1234`

## Desarrollo local

```bash
cd HackerRank1
dotnet run --urls "http://127.0.0.1:5219"
```

## CORS

Orígenes permitidos en `appsettings.json` → `Cors:AllowedOrigins`.  
Agrega la URL de tu frontend desplegado cuando la tengas.

## Tests

```bash
dotnet restore
dotnet build
dotnet test
```
