# Guía para agentes en Kalma

Kalma es una PWA B2C para coordinar cuidados entre una familia y una cuidadora. Este repositorio está en fase de planificación: no presupongas que la estructura, los tests o los tickets descritos abajo ya están implementados.

## Fuentes de verdad

1. [`Kalma — Documento de arquitectura.md`](Kalma%20%E2%80%94%20Documento%20de%20arquitectura.md): alcance, reglas del dominio, arquitectura, ADR y estrategia de pruebas.
2. [`Kalma — Backlog de implementación.md`](Kalma%20%E2%80%94%20Backlog%20de%20implementaci%C3%B3n.md): orden, dependencias y criterios de aceptación de cada ticket `KAL-xxx`.

Antes de implementar, identificá el ticket y sus dependencias; no marques criterios como terminados sin evidencia. Si surge una decisión que contradice o modifica la arquitectura, explicitá el conflicto y actualizá la decisión correspondiente junto con el cambio. No conviertas ejemplos o propuestas del backlog en requisitos cerrados (por ejemplo, el plazo de caducidad de una invitación).

## Cómo trabajar

- Avanzá un ticket a la vez, respetando sus dependencias. El primer hito es KAL-084; el primer ciclo de dominio comienza por el parte de tramo después de preparar los cimientos.
- Seguí TDD estricto: escribí un criterio como test, observá el rojo, implementá lo mínimo para el verde y refactorizá. No afirmes haber ejecutado pruebas si todavía no existe un runner o no se ejecutaron.
- Mantené la regla de dependencias: `Domain` no depende de otras capas; `Application` depende de `Domain`; `Infrastructure` implementa los puertos de `Application`; `Api` compone y expone HTTP. En la web, separá `domain`, `app`, `infra` y `ui`, sin llevar reglas de negocio a React.
- Evitá infraestructura especulativa: el MVP es gratuito, un monolito modular en una instancia, sin pagos, organización, ficha clínica ni doble factor.
- Al cerrar un ticket, comprobá sus criterios, los tests disponibles y la regla de arquitectura; actualizá el ADR si cambió una decisión. Consultá el backlog para los comandos cuando la solución y CI estén creados; no inventes comandos de verificación.

## Invariantes que no se negocian

- Un espacio admite como máximo dos membresías; una cuenta, como máximo una membresía activa. Solo un familiar administrador invita y la administración solo se transfiere a otro familiar.
- Un parte es inmutable: corregirlo crea otro parte que referencia al original. Los cuatro tramos usan `Europe/Madrid`; la noche de 20:00 a 07:00 pertenece al día de cuidado en que empezó.
- Partes, chat y diario pueden quedar pendientes sin conexión con ID generado en el cliente e idempotencia en el servidor. Las alertas **no** se encolan: sin conexión se bloquean y se sugiere llamar.
- El diario pertenece a la cuenta de cada usuario y es privado incluso frente al administrador del espacio. Toda operación sobre un espacio comprueba membresía y rol.
- No uses datos de familias reales en un piloto hasta resolver la consulta experta sobre el artículo 9 del RGPD (KAL-140). No guardes secretos en el repositorio.

## Estructura prevista, no existente aún

KAL-001 prepara `backend/src/{Kalma.Domain,Kalma.Application,Kalma.Infrastructure,Kalma.Api}` y `backend/tests/`; KAL-004 prepara `web/src/{domain,app,infra,ui}`. `docs/adr/` se incorpora con los cimientos. Usá los criterios del ticket para decidir qué crear, en vez de levantar todo el MVP de una vez.
