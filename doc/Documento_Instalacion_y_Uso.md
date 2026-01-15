# Documento de instalación y uso de la API `AudisoftPrueba`

## 1. Introducción

Este documento describe el proceso de instalación, configuración y ejecución de la API `AudisoftPrueba`, desarrollada en `.NET 8` con ASP.NET Core. También se indican los puntos en los que se pueden adjuntar o consultar capturas de pantalla de la aplicación.

## 2. Requisitos del sistema

Antes de iniciar, verifique que dispone de lo siguiente:

- Sistema operativo Windows, Linux o macOS compatible con .NET 8.
- SDK de .NET 8 instalado.
- Un IDE o editor de código (por ejemplo, Visual Studio 2022, Visual Studio Code o similar).
- Acceso a Internet para la restauración de paquetes NuGet.
- (Opcional) Node.js y Angular CLI si se utilizará el frontend Angular en `http://localhost:4200`.

## 3. Obtención del código fuente

1. Abra una terminal o consola de comandos.
2. Ejecute:

   ```bash
   git clone https://github.com/david5gat/AudisoftPrueba.NET.git
   ```

3. Ingrese al directorio del proyecto:

   ```bash
   cd AudisoftPrueba
   ```

> **Captura sugerida:** Pantalla de la consola mostrando el comando `git clone` ejecutado con éxito.

## 4. Restauración de dependencias

En el directorio raíz del proyecto, ejecute:

```bash
 dotnet restore
```

Este comando descargará y restaurará todos los paquetes NuGet necesarios para compilar y ejecutar la aplicación.

> **Captura sugerida:** Ventana de la terminal con el resultado exitoso de `dotnet restore`.

## 5. Ejecución del proyecto en ambiente de desarrollo

Para iniciar la API en modo desarrollo, utilice el siguiente comando:

```bash
 dotnet run
```

La consola mostrará la URL en la que se encuentra disponible la API, por ejemplo:

- `https://localhost:5001`
- o el puerto HTTPS que asigne la configuración local.

> **Captura sugerida:** Resultado en la consola después de ejecutar `dotnet run`, donde se observan las URLs de la aplicación.

## 6. Acceso a la documentación Swagger

Cuando el entorno está configurado como Desarrollo (`ASPNETCORE_ENVIRONMENT=Development`), la aplicación expone la interfaz de Swagger.

1. Con la API en ejecución, abra un navegador web.
2. Ingrese la URL de Swagger, por ejemplo:

   - `https://localhost:5001/swagger`

3. Verifique que se muestren los endpoints disponibles y su documentación.

> **Captura sugerida:** Vista de la interfaz de Swagger mostrando la lista de endpoints de la API.

## 7. Configuración de CORS para Angular

En el archivo `Program.cs` se declara una política de CORS denominada `AngularPolicy`, configurada de la siguiente manera (fragmento relevante):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // Servidor de desarrollo Angular
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

Esta configuración permite que un frontend Angular en ejecución en `http://localhost:4200` consuma la API sin problemas de bloqueo por CORS.

> **Captura sugerida:** Captura del navegador mostrando el frontend Angular consumiendo la API de `AudisoftPrueba` (por ejemplo, una pantalla con datos cargados desde la API).

## 8. Estructura básica del proyecto

Aunque la estructura puede variar, típicamente se tendrá:

- `Program.cs`: archivo principal donde se configuran los servicios, CORS, Swagger y el pipeline HTTP.
- Carpeta `Controllers/`: controladores que definen los endpoints de la API.
- Archivo `AudisoftPrueba.csproj`: configuración del proyecto, referencia al framework `.NET 8`, dependencias, etc.
- Carpeta `Properties/` (si existe): incluye `launchSettings.json` con perfiles de ejecución locales.

> **Captura sugerida:** Vista del explorador de soluciones en Visual Studio mostrando la estructura del proyecto.

## 9. Publicación para otros entornos

Para generar una compilación lista para despliegue, utilice el siguiente comando desde la raíz del proyecto:

```bash
 dotnet publish -c Release -o ./publish
```

- `-c Release` indica que se compila en configuración de lanzamiento.
- `-o ./publish` establece la carpeta de salida para los archivos publicados.

El contenido de la carpeta `publish` puede copiarse a un servidor o servicio de hosting compatible con ASP.NET Core (por ejemplo, IIS, Azure App Service, contenedores, etc.).

> **Captura sugerida:** Vista del explorador de archivos mostrando la carpeta `publish` generada.

## 10. Solución de problemas frecuentes

- **La API no responde en la URL indicada:**
  - Verifique que no haya errores en la consola al ejecutar `dotnet run`.
  - Confirme que el puerto no esté siendo utilizado por otra aplicación.
  - Asegúrese de que el firewall local permita el tráfico hacia el puerto utilizado.

- **Problemas de CORS desde Angular:**
  - Verifique que el frontend se esté ejecutando exactamente en `http://localhost:4200`.
  - Confirme que la política `AngularPolicy` se está aplicando con `app.UseCors("AngularPolicy");` en `Program.cs`.

- **Swagger no aparece en producción:**
  - Por seguridad, Swagger suele habilitarse solo en entornos de desarrollo. Revise la lógica condicional en `Program.cs` y la variable de entorno `ASPNETCORE_ENVIRONMENT`.

## 11. Notas finales

- Mantenga actualizado el SDK de .NET para aprovechar mejoras de rendimiento y seguridad.
- Se recomienda versionar el código utilizando Git y trabajar con ramas para nuevas funcionalidades.

La ortografía y la redacción de este documento han sido revisadas siguiendo el español estándar utilizado en Colombia.