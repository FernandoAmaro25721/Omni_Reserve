# OmniReserve - Actividad 9: DbContext y RoomConfiguration con Fluent API

- Tarea 9: Configuraci�n de persistencia para User y Reservation con Fluent API.

- Actividad 10: Migraci�n del proyecto de SQL Server a PostgreSQL utilizando Entity Framework Core y .NET 10.0.

- Tarea 10: Optimizaci�n de contratos y limpieza de m�todos de persistencia.

- Actividad 11: B�squeda segura de habitaciones mediante traducci�n nativa LINQ.

- Tarea 11: Integraci�n de consultas con SQL crudo parametrizado mediante FromSql.

- Actividad 12: Configuraci�n de restricciones de borrado y mapeo relacional en Fluent API.

- Actividad 12: Configuraci�n de restricciones de borrado y mapeo relacional en Fluent API.

- Tarea 12: Aplicaci�n de migraciones y restricciones de integridad referencial.

- Actividad 13: Implementaci�n del repositorio de reservaciones y carga ansiosa (Eager Loading).

- Tarea 13: Implementaci�n de CQRS Query Handler y consumo seguro de propiedades de navegaci�n.

- Actividad 14: Introducci�n del Value Object EmailAddress para validaci�n estricta de dominio.

- Actividad 14: Integraci�n de Value Object EmailAddress con mapeo bidireccional en EF Core.

- Tarea 14: Integraci�n de Value Object EmailAddress con mapeo bidireccional en EF Core.

### Actividad 15 - Prevención de Double-Booking en Repositorio
- **Corrección / Rendimiento:** Se implementa `IsRoomAvailableAsync` evaluando el solapamiento de fechas directamente en base de datos mediante `AnyAsync` (`StartDate < end && EndDate > start`), evitando traer registros a memoria con `.ToList()` o `.AsEnumerable()`.
- **Criterio de fechas:** Se utiliza `<` y `>` para el cruce de rangos; si el negocio requiere días completamente distintos sin reservas el mismo día, puede ajustarse a `<=` y `>=`.

- Tarea 15: Aplicación de regla de negocio en CreateReservationCommandHandler validando disponibilidad antes de AddAsync y propagando excepciones sin tragarlas en el Handler.

- Actividad 16: Abstracción IPasswordHasher en Aplicación, implementación BCryptPasswordHasher en Infraestructura y refuerzo de PasswordHash en la entidad User.

- Tarea 16: Hashing de contraseña en CreateUserCommandHandler mediante IPasswordHasher antes de instanciar la entidad User.

- Actividad 17: Implementación de IJwtProvider y JwtProvider con IOptions<JwtSettings> sin exponer secretos ni datos sensibles en los claims.

- Tarea 17: Implementación de LoginUserCommandHandler orquestando búsqueda por correo, verificación con IPasswordHasher y generación de JWT con errores genéricos.

- Actividad 18: Configuración del esquema JwtBearer en el API con validación de emisor, audiencia, expiración y firma, respetando el orden UseAuthentication(), antes de UseAuthorization().

- Tarea 18: Verificación y consolidación del esquema JwtBearer con JwtSettings, TokenValidationParameters y orden UseAuthentication() antes de UseAuthorization() en el API.