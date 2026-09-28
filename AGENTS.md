# Kalma — Instrucciones de trabajo

Kalma es una PWA B2C para que una familia y una cuidadora coordinen el cuidado diario de una persona dependiente en España. El MVP es gratuito. Por ahora, este directorio contiene documentación de planificación y cimientos iniciales, no la aplicación completa descrita abajo.

## Fuentes de referencia

- Consultar [`docs/Arquitectura-Kalma.md`](docs/Arquitectura-Kalma.md) para reglas de dominio, arquitectura, ADR y alcance.
- Consultar [`backlog.md`](backlog.md) para orden de tickets, dependencias, criterios de aceptación e hitos. Los tickets sin marcar son trabajo pendiente; los ejemplos y casillas de diseño marcadas en la arquitectura no demuestran que exista código.
- Si los documentos se contradicen o falta una decisión de producto, señalarlo antes de implementar. No inventar reglas; actualizar el ADR correspondiente si cambia una decisión.

## Forma de trabajo

1. Tomar un solo ticket `KAL-xxx` cuyas dependencias estén terminadas. Dividirlo si supera el tamaño M previsto en el backlog.
2. Escribir primero el test que falla para cada criterio de aceptación; después, el código mínimo para hacerlo pasar y, por último, refactorizar. Para documentación o infraestructura sin tests ejecutables, comprobar directamente los criterios aplicables.
3. Mantener juntos implementación y tests. Ejecutar compilación, tests y comprobaciones de arquitectura aplicables cuando existan los proyectos; informar de los controles que no se pudieron ejecutar. No dar un ticket por terminado sin cumplir sus criterios y superar los controles disponibles.
4. No guardar secretos en el repositorio ni desplegar a producción o tratar datos reales de familias o salud como parte rutinaria de un ticket.

## Arquitectura e invariantes

- El monorrepositorio previsto tiene `backend/` (.NET, ASP.NET Core, EF Core y PostgreSQL) y `web/` (PWA React/TypeScript). En backend, las dependencias apuntan hacia dentro: API → Infraestructura → Aplicación → Dominio. Dominio no importa infraestructura. En frontend, las capas son `domain/`, `app/`, `infra/` y `ui/`, con dependencias hacia dentro.
- `EspacioDeCuidado` admite como máximo dos membresías; una cuenta, una membresía activa. Solo invita el `FamiliarAdministrador`; la persona dependiente no tiene cuenta y la cuidadora entra solo por invitación. El diario privado pertenece a la cuenta de la cuidadora, no al espacio.
- Los partes enviados son inmutables: una corrección crea otro parte que referencia al original. Los tramos usan `Europe/Madrid`: 07–12, 12–16, 16–20 y 20–07; la madrugada pertenece al día de cuidado anterior. Inyectar el reloj para que los tests sean deterministas.
- Partes, mensajes de chat y entradas de diario sin conexión usan IDs generados en el cliente, una cola en IndexedDB y recepción idempotente en el servidor. Las alertas no se encolan: sin conexión se bloquean y se sugiere llamar. SignalR sirve para recibir chat en tiempo real; el envío usa HTTP.
- El acceso usa ASP.NET Identity y verificación obligatoria de email. Comprobar membresía y rol en cada petición protegida; solo la dueña lee su diario. El PDF se descarga dentro de la app con sesión iniciada, nunca como adjunto de email.
- Antes del piloto con usuarios reales, resolver con un experto el tratamiento de datos del artículo 9 del RGPD (medicación, texto libre y diario): `KAL-140`.

## Punto de partida y comprobaciones

- Empezar por los cimientos de la fase 0 de `backlog.md` (`KAL-001` en adelante). No suponer que ya existen todos los proyectos, scripts o ejecutores descritos en la planificación.
- Después de `KAL-001`, ejecutar `dotnet build` y `dotnet test` en backend. Después de `KAL-004`, ejecutar los scripts configurados de tests y lint del frontend. Desde `KAL-002`, mantener verde el test de arquitectura; desde `KAL-003`, exigir los controles de CI. Consultar los comandos reales en los proyectos creados en vez de adivinarlos.
- El primer trabajo TDD de dominio cubre tramos y día de cuidado (`KAL-010`) y valores del checklist (`KAL-011`) antes de implementar `Parte` (`KAL-012`). El primer hito es registrar un parte desde el móvil, incluso después de perder la conexión (`KAL-084`).
