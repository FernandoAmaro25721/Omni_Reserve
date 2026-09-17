# OmniReserve API

## Configuración de Dependencias (Tarea 2)
Se encapsuló la inyección de dependencias en cada capa mediante métodos de extensión:
- **Application:** Contiene AddApplication() para registrar servicios de la lógica de negocio.
- **Infrastructure:** Contiene AddInfrastructure() para registrar servicios de acceso a datos y seguridad.
- **Api:** Invoca ambas capas en Program.cs.
