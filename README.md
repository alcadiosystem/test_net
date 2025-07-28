# Prueba Técnica

El presente repositorio corresponde a la **prueba técnica para desarrollador backend .NET**.  
El objetivo de esta misma es evaluar mis habilidades para desarrollar una **API REST** utilizando **.NET** con una **estructura limpia (Clean Architecture)**, aplicando principios **SOLID** y buenas prácticas de desarrollo.

Se cumplieron los requisitos previamente estipulados en el documento enviado, creando **CRUD completos** por cada una de las entidades.

### Herramientas utilizadas
1. .NET 8
2. Visual Studio Code para ambiente macOS
3. Visual Studio para ambiente Windows
4. PostgreSQL

Durante cada fase del desarrollo, se realizaron pruebas en ambos entornos para verificar el correcto funcionamiento.

---

## Instrucciones de compilación

El proyecto ha sido creado sobre **.NET 8** y probado en esta versión.  
Ejecutarlo en una versión superior o inferior podría generar errores.

### Compilación sobre Windows

1. Clonar el repositorio.
2. Abrir Visual Studio y seleccionar **“Abrir un proyecto o una solución”**.
3. Buscar y seleccionar la carpeta `test_net`.
4. Abrir el archivo `Pizzeria.sln`.
5. Visual Studio configurará inicialmente la solución. Luego:
    1. Abrir el **Explorador de soluciones**, buscar `Pizzeria.Backend`.
    2. Editar el archivo `appsettings.json`, modificando la propiedad **ConnectionStrings** con tus credenciales de PostgreSQL.
    3. Abrir **Herramientas → Administrador de paquetes NuGet → Consola del Administrador**.
    4. Ejecutar:
       ```powershell
       Update-Database
       ```
       Esto instalará la base de datos.
    5. Compilar la solución: **Compilar → Compilar Solución** o `Ctrl + Mayús + B`.
    6. Asegurarse de que `Pizzeria.Backend` esté configurado como proyecto de inicio y presionar **Run**.
    7. Se abrirá Swagger en el navegador para probar la API.

---

### Compilación sobre macOS

1. Clonar el repositorio.
2. Abrir la solución con Visual Studio Code o cualquier editor.
3. Ejecutar:
   ```bash
   dotnet build
   ```
4. Ingresar al `Pizzeria.Backend`.
5. Editar el archivo `appsettings.json`, modificando la propiedad **ConnectionStrings** con tus credenciales de PostgreSQL.
6. Aplicar las migraciones:
    ```bash
    dotnet ef database update
    ```
7. Compilar nuevamente:
    ```bash
    dotnet build
    ```
8. Ejecutar:
    ```bash
    dotnet watch run
    ```
Esto abrirá Swagger para probar la API.
