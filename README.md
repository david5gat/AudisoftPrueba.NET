# AudisoftPrueba

API desarrollada en `.NET 8` utilizando ASP.NET Core.

## Descripción general

Este proyecto expone una API REST construida con ASP.NET Core, configurada para trabajar junto a un frontend en Angular (servidor de desarrollo en `http://localhost:4200`). Incluye configuración de CORS y Swagger para la documentación interactiva de la API.

## Requisitos previos

- SDK de .NET 8 instalado.
- Visual Studio 2022, Visual Studio Code u otro IDE compatible con .NET.
- Git instalado (opcional, si se clona desde el repositorio remoto).
- (Opcional) Node.js y Angular CLI si se va a usar el frontend Angular.

## Instalación y ejecución

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/david5gat/AudisoftPrueba.NET.git
   ```

2. Ingresar al directorio del proyecto:

   ```bash
   cd AudisoftPrueba
   ```

3. Restaurar paquetes (si es necesario):

   ```bash
   dotnet restore
   ```

4. Ejecutar la aplicación:

   ```bash
   dotnet run
   ```

5. La API quedará disponible normalmente en una URL similar a:

   - `https://localhost:5001`
   - o el puerto HTTPS que muestre la consola al ejecutar `dotnet run`.

## Swagger (documentación de la API)

En ambiente de desarrollo, Swagger se habilita automáticamente. Una vez la aplicación esté en ejecución, se puede acceder a la interfaz de Swagger en:

- `https://localhost:5001/swagger`  
- o la URL HTTPS indicada por la consola al iniciar la aplicación.

## CORS y comunicación con Angular

En `Program.cs` se define una política de CORS llamada `AngularPolicy` que permite el origen:

- `http://localhost:4200`

Asegúrese de que el frontend Angular se ejecute en ese puerto para evitar problemas de CORS.

## Estructura general del proyecto

- `Program.cs`: punto de entrada de la aplicación y configuración del pipeline HTTP.
- `Controllers/`: controladores de la API.
- `Properties/launchSettings.json`: perfiles de ejecución locales.
- `AudisoftPrueba.csproj`: archivo de proyecto de .NET 8.

## Publicación

Para generar una versión publicable del proyecto:

```bash
 dotnet publish -c Release -o ./publish
```

El contenido de la carpeta `publish` puede desplegarse en un servidor o servicio de hosting compatible con aplicaciones ASP.NET Core.

## Documentación adicional

Para una guía más detallada sobre instalación, uso y capturas de pantalla sugeridas, consulte el documento:

- `doc/Documento_Instalacion_y_Uso.md`

## Licencia

Este proyecto se distribuye bajo los términos definidos en el repositorio original (si aplica).