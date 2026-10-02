# Kalma — Backlog de implementación

Sep 27, 2026 · @René

## Cómo usar el backlog

54 tickets en 15 fases, ordenados por dependencias: cada ticket solo necesita los anteriores. El primer objetivo es el **Hito 1**: registrar un parte desde el móvil, en producción e incluso sin conexión (KAL-084).

**Reglas de trabajo**

1. Un ticket a la vez, en orden. Si un ticket depende de otro no terminado, no se empieza.
2. Cada criterio de aceptación es un test que se escribe **antes** del código (rojo → verde → refactor).
3. Los identificadores `ADR-xx` y `CU-xx` remiten al documento de arquitectura.

**Un ticket está hecho cuando**

- Todos sus criterios están marcados.
- Todos los tests pasan en CI (a partir de KAL-003).
- El test de arquitectura sigue en verde (a partir de KAL-002).
- Si cambió alguna decisión, el ADR correspondiente está actualizado.

**Tamaños**

| Tamaño | Esfuerzo | Equivale a |
| --- | --- | --- |
| S | ≈ 2 h | Una sesión |
| M | ≈ 4 h | Una semana a tu ritmo |

Ningún ticket supera M: si al empezarlo crece, se parte en dos.

*Estimación, no dato:* ≈ 104 h hasta el Hito 1 (≈ 21 semanas a 5 h/semana) y ≈ 164 h para todo el MVP (≈ 33 semanas). Las primeras semanas de TDD suelen ir más lentas.

## Hoja de ruta

El Hito 1 llega al final de la fase 8; las fases 9 a 13 añaden el resto del MVP y la 14 bloquea el piloto con usuarios reales.

| Fase | Qué se consigue | Tickets | Horas estimadas |
| --- | --- | --- | --- |
| 0 · Cimientos | Repositorio, capas vigiladas por tests, CI | KAL-001 a 004 | 8 |
| 1 · Dominio del parte | Tramos, día de cuidado y parte inmutable | KAL-010 a 013 | 12 |
| 2 · Espacio y membresías | Reglas de 2 miembros, roles e invitaciones | KAL-020 a 023 | 12 |
| 3 · Casos de uso del parte | Registrar, corregir y consultar, con idempotencia | KAL-030 a 032 | 8 |
| 4 · Persistencia | PostgreSQL, EF Core y tests de contrato | KAL-040 a 042 | 10 |
| 5 · Identidad y acceso | Registro, login, invitaciones y permisos | KAL-050 a 054 | 16 |
| 6 · API del parte | Endpoints HTTP del parte | KAL-060 | 4 |
| 7 · Frontend del parte | PWA, pantallas y cola sin conexión | KAL-070 a 075 | 20 |
| 8 · Despliegue | VPS, HTTPS, correo, copias y CI → **Hito 1** | KAL-080 a 084 | 14 |
| 9 · Mensajería | Chat en tiempo real y alertas | KAL-090 a 094 | 18 |
| 10 · Notificaciones push | Push de alertas y chat, guía para iPhone | KAL-100 a 102 | 10 |
| 11 · Diario y recursos | Espacio privado de la cuidadora | KAL-110 a 112 | 8 |
| 12 · Informe semanal | Informe de 7 días, PDF y aviso del lunes | KAL-120 a 122 | 12 |
| 13 · Ciclo de vida | Actividad, inactividad, borrado y salida | KAL-130 a 133 | 12 |
| 14 · Antes del piloto | Consulta RGPD art. 9 | KAL-140 | — |

## Fase 0 · Cimientos

Antes de la primera línea de lógica, el repositorio ya impide romper la regla de dependencias.

- [ ] **KAL-001 · Solución y estructura del repositorio** — ADR-29, ADR-34 · S
  - [ ] Repositorio `kalma/` con `backend/`, `web/` y `docs/adr/`
  - [ ] Solución .NET con los 4 proyectos de `src/` y los 3 de tests (xUnit)
  - [ ] Referencias: Application → Domain; Infrastructure → Application; Api → Infrastructure y Application
  - [ ] `dotnet build` y `dotnet test` pasan
  - [ ] `kalma-arquitectura.md` copiado a `docs/`
- [ ] **KAL-002 · Test de arquitectura** — ADR-29 · S · requiere KAL-001
  - [ ] Falla si Kalma.Domain referencia EF Core, ASP.NET o Infrastructure (p. ej. NetArchTest.Rules)
  - [ ] Falla si Application referencia Infrastructure
  - [ ] Romperlo a propósito una vez y ver el rojo
- [ ] **KAL-003 · Integración continua** — S · requiere KAL-002
  - [ ] GitHub Actions compila y ejecuta todos los tests en cada push
  - [ ] Un test en rojo impide fusionar en `main`
- [ ] **KAL-004 · Proyecto web con capas y tests** — ADR-33 · S · requiere KAL-001
  - [ ] Vite + React + TypeScript en `web/`, con carpetas `domain/`, `app/`, `infra/` y `ui/`
  - [ ] Vitest ejecuta un primer test
  - [ ] Una regla de lint impide que `domain/` importe de `infra/` o `ui/` (p. ej. eslint-plugin-boundaries)
  - [ ] CI ejecuta también los tests del frontend

## Fase 1 · Dominio del parte

Sin base de datos, sin login y sin API: solo reglas y tests de milisegundos.

- [ ] **KAL-010 · Tramo y día de cuidado** — ADR-16, ADR-39 · M · requiere KAL-002
  - [ ] 07:00 → Mañana; 11:59 → Mañana; 12:00 → Mediodía; 16:00 → Tarde; 20:00 → Noche
  - [ ] Martes 02:00 → Noche del lunes; 06:59 → Noche del día anterior
  - [ ] Los cambios de hora (último domingo de marzo y de octubre) no rompen el cálculo
  - [ ] El instante llega en UTC y se convierte a Europe/Madrid; el reloj se inyecta con `TimeProvider`
- [ ] **KAL-011 · Objetos de valor del checklist** — ADR-16 · S · requiere KAL-002
  - [ ] `Valoracion` (Bien, Regular, Mal, NoAplica), `EstadoDeAnimo` (6 valores), `Medicacion` (Sí, No, NoAplica)
  - [ ] `Nota` opcional que rechaza más de 500 caracteres
  - [ ] Dos objetos con el mismo valor son iguales
- [ ] **KAL-012 · Entidad Parte** — ADR-10, ADR-16 · M · requiere KAL-010, KAL-011
  - [ ] `Parte.Registrar` es la única forma de crearlo; el test de ejemplo del documento pasa
  - [ ] Si falta cualquiera de los 6 campos, se rechaza
  - [ ] Guarda tramo, día de cuidado, hora del tramo como valor propio, autor e instante
  - [ ] Sin *setters* públicos: el compilador impide modificarlo
- [ ] **KAL-013 · Corrección de un parte** — ADR-10 · S · requiere KAL-012
  - [ ] `Parte.Corregir` crea un parte nuevo que referencia al original
  - [ ] La corrección conserva tramo y día de cuidado del original
  - [ ] El original no cambia

## Fase 2 · Espacio y membresías

Las reglas de quién puede estar en un espacio viven en el agregado, no en los controladores.

- [ ] **KAL-020 · Agregado EspacioDeCuidado** — ADR-06, ADR-07 · M · requiere KAL-002
  - [ ] Se crea con su PersonaDependiente (alias, sin datos médicos) y un FamiliarAdministrador
  - [ ] Añadir una tercera membresía se rechaza
- [ ] **KAL-021 · Roles y administración** — ADR-25 · S · requiere KAL-020
  - [ ] El administrador siempre es un Familiar
  - [ ] La administración solo se transfiere a otro Familiar
  - [ ] Sin otro Familiar, el administrador solo puede borrar el espacio
- [ ] **KAL-022 · Invitación** — ADR-12, ADR-32 · M · requiere KAL-021
  - [ ] Estados Pendiente → Aceptada, Caducada o Revocada; cualquier otra transición se rechaza
  - [ ] Solo el administrador invita
  - [ ] No se invita si el espacio ya tiene 2 miembros
  - [ ] Token de un solo uso con caducidad (plazo por decidir; propuesta: 7 días)
- [ ] **KAL-023 · Una membresía activa por cuenta** — ADR-13, ADR-20 · S · requiere KAL-022
  - [ ] Aceptar una invitación teniendo otra membresía activa se rechaza
  - [ ] Una cuenta sin espacio sí puede aceptar una invitación

## Fase 3 · Casos de uso del parte

La capa de Aplicación se prueba con repositorios en memoria y un reloj falso.

- [ ] **KAL-030 · Caso de uso RegistrarParte** — CU-01, ADR-09, ADR-13 · M · requiere KAL-012, KAL-020
  - [ ] `IRepositorioPartes` e `IComprobadorMembresia` definidos en Application
  - [ ] Quien no es miembro del espacio es rechazado
  - [ ] El mismo Id dos veces no duplica y responde "ya existía" (idempotencia)
- [ ] **KAL-031 · Caso de uso CorregirParte** — ADR-10 · S · requiere KAL-013, KAL-030
  - [ ] Solo se corrige un parte existente del mismo espacio
  - [ ] Es idempotente igual que el registro
- [ ] **KAL-032 · Consulta de un día de cuidado** — S · requiere KAL-030
  - [ ] Devuelve los 4 tramos con su parte o "Sin registro"
  - [ ] Si hubo corrección, muestra la última versión y marca que fue corregido

## Fase 4 · Persistencia

PostgreSQL entra ahora como un detalle: los repositorios reales deben comportarse igual que los falsos.

- [ ] **KAL-040 · PostgreSQL local con Docker Compose** — ADR-38 · S · requiere KAL-001
  - [ ] `docker compose up` levanta PostgreSQL
  - [ ] La cadena de conexión vive en *user-secrets*, nunca en el repositorio
- [ ] **KAL-041 · DbContext y migraciones** — M · requiere KAL-040, KAL-012, KAL-020
  - [ ] Mapeo de Parte, Espacio, Membresía y PersonaDependiente configurado en Infrastructure, sin tocar el Dominio
  - [ ] La clave del parte es el Id generado por el cliente
  - [ ] Migración inicial aplicada
- [ ] **KAL-042 · Repositorios EF y tests de contrato** — Liskov · M · requiere KAL-041, KAL-030
  - [ ] La misma batería de tests pasa contra el repositorio en memoria y contra el de EF
  - [ ] Tests de integración contra PostgreSQL real (p. ej. Testcontainers)
  - [ ] Guardar un Id repetido no lanza excepción

## Fase 5 · Identidad y acceso

Seguridad delegada en ASP.NET Identity; el email entra como un puerto con dos adaptadores.

- [ ] **KAL-050 · Puerto de email** — ADR-40 · S · requiere KAL-001
  - [ ] `IEnviadorEmail` definido en Application
  - [ ] Adaptador de desarrollo que captura los correos para verlos (p. ej. Mailpit en Docker)
  - [ ] Adaptador SMTP de Hostinger (puerto 465, SSL) configurado por variables de entorno
- [ ] **KAL-051 · Registro de familiar** — CU-05, ADR-24, ADR-37 · M · requiere KAL-041, KAL-050
  - [ ] Email y contraseña con ASP.NET Identity
  - [ ] El registro crea cuenta, espacio y persona dependiente, con el familiar como administrador
  - [ ] No se puede entrar sin verificar el email
- [ ] **KAL-052 · Inicio de sesión y recuperación** — ADR-24 · M · requiere KAL-051
  - [ ] El login devuelve un JWT; bloqueo tras varios intentos fallidos
  - [ ] Recuperación de contraseña por email con enlace de un solo uso
- [ ] **KAL-053 · Aceptar invitación** — CU-06, ADR-32 · M · requiere KAL-022, KAL-051
  - [ ] El administrador crea la invitación y el enlace llega por email
  - [ ] Registrarse desde el enlace crea cuenta y membresía con el rol invitado
  - [ ] Una cuidadora no puede registrarse sin invitación
- [ ] **KAL-054 · Políticas de autorización** — ADR-13, ADR-25 · S · requiere KAL-052
  - [ ] Políticas `MiembroDelEspacio` y `AdministradorDelEspacio`
  - [ ] Un miembro de otro espacio recibe 403

## Fase 6 · API del parte

Los controladores son *Humble Objects*: traducen HTTP a casos de uso y nada más.

- [ ] **KAL-060 · Endpoints del parte** — CU-01 · M · requiere KAL-042, KAL-054
  - [ ] `POST /espacios/{id}/partes` responde 201 si es nuevo y 200 si el Id ya existía
  - [ ] Endpoint de corrección
  - [ ] `GET /espacios/{id}/dias/{fecha}` devuelve los 4 tramos
  - [ ] Ningún controlador contiene reglas de negocio

## Fase 7 · Frontend del parte

La lógica sin conexión vive en `app/` y se prueba con TDD sin navegador; los componentes solo pintan.

- [ ] **KAL-070 · Base de la PWA** — ADR-04 · M · requiere KAL-004
  - [ ] Manifest, iconos y *service worker* (p. ej. vite-plugin-pwa)
  - [ ] Sin conexión, la app abre y muestra lo último cargado
- [ ] **KAL-071 · Pantallas de acceso** — CU-05 · M · requiere KAL-052, KAL-053, KAL-070
  - [ ] Registro de familiar, inicio de sesión, recuperar contraseña y aceptar invitación
  - [ ] Los errores explican qué falló y cómo arreglarlo
- [ ] **KAL-072 · Tramos y día de cuidado en TypeScript** — ADR-39 · S · requiere KAL-004
  - [ ] Los mismos casos de prueba que KAL-010, en `web/src/domain`
- [ ] **KAL-073 · Pantalla de tramos del día** — S · requiere KAL-060, KAL-072
  - [ ] Los 4 tramos con "Sin registro" o registrado; el tramo actual destacado
- [ ] **KAL-074 · Formulario del parte** — ADR-16 · M · requiere KAL-073
  - [ ] Los 6 campos obligatorios; "Guardar parte" desactivado hasta completarlos
  - [ ] Contador 0/500 y aviso "No incluyas datos médicos"
- [ ] **KAL-075 · Cola de envío sin conexión** — ADR-09 · M · requiere KAL-074
  - [ ] El Id (UUID) se genera en el cliente antes de guardar
  - [ ] Sin conexión, el parte se guarda en IndexedDB y aparece como "pendiente de envío"
  - [ ] Al volver la conexión se envía en orden; un 200 "ya existía" cuenta como éxito

## Fase 8 · Despliegue

Al terminar esta fase se alcanza el **Hito 1**: un parte registrado desde el móvil llega a producción, también tras un corte de conexión.

- [ ] **KAL-080 · Preparar el VPS** — ADR-38 · M
  - [ ] VPS de Hostinger en un centro de datos de la UE
  - [ ] SSH solo con clave, cortafuegos abierto solo en 22, 80 y 443, y actualizaciones de seguridad automáticas
- [ ] **KAL-081 · Docker Compose de producción** — ADR-38 · M · requiere KAL-080
  - [ ] API, PostgreSQL y Caddy con HTTPS automático en tu dominio
  - [ ] Secretos en variables de entorno del servidor
- [ ] **KAL-082 · DNS y correo** — ADR-40 · S · requiere KAL-080
  - [ ] SPF, DKIM y DMARC configurados
  - [ ] Un email de verificación llega a la bandeja de entrada, no a spam
- [ ] **KAL-083 · Copias de seguridad** — S · requiere KAL-081
  - [ ] Copia diaria automática de PostgreSQL fuera del VPS
  - [ ] Restauración probada al menos una vez
- [ ] **KAL-084 · Despliegue desde CI** — S · requiere KAL-003, KAL-081
  - [ ] Fusionar en `main` despliega solo si todos los tests pasan
  - [ ] Prueba del Hito 1 hecha en un móvil real

## Fase 9 · Mensajería

Chat y alertas comparten entidad; lo que las distingue es una regla, no una jerarquía de clases.

- [ ] **KAL-090 · Dominio Mensaje** — ADR-08, ADR-14 · S · requiere KAL-020
  - [ ] Un único `Mensaje` con `Tipo` chat o alerta, sin herencia
  - [ ] `PuedeEncolarse`: el chat sí, la alerta no
- [ ] **KAL-091 · Casos de uso EnviarMensaje y EnviarAlerta** — CU-02, CU-03 · M · requiere KAL-090, KAL-054
  - [ ] Idempotentes, igual que el parte
  - [ ] Publican los eventos de dominio `MensajeEnviado` y `AlertaEnviada`
- [ ] **KAL-092 · Persistencia y API de mensajes** — M · requiere KAL-091
  - [ ] Repositorio de mensajes con los mismos tests de contrato que el de partes
  - [ ] `POST` idempotente y `GET` del historial paginado
- [ ] **KAL-093 · Tiempo real con SignalR** — ADR-28 · M · requiere KAL-092
  - [ ] `INotificadorTiempoReal` en Application; SignalR es su adaptador
  - [ ] Solo reciben los miembros del espacio
  - [ ] Si SignalR falla, los mensajes aparecen al refrescar
- [ ] **KAL-094 · Pantalla de chat y alertas** — ADR-09, ADR-14 · M · requiere KAL-093, KAL-075
  - [ ] Los mensajes sin conexión usan la cola de envío
  - [ ] Sin conexión, el botón de alerta se bloquea y sugiere llamar por teléfono

## Fase 10 · Notificaciones push

Las push escuchan eventos: Mensajería no sabe que existen.

- [ ] **KAL-100 · Suscripción Web Push** — ADR-11 · M · requiere KAL-070
  - [ ] Claves VAPID generadas y guardadas como secreto
  - [ ] La PWA pide permiso y guarda la `SuscripcionPush` de la cuenta
- [ ] **KAL-101 · Notificador push** — CU-04 · M · requiere KAL-091, KAL-100
  - [ ] Escucha `AlertaEnviada` y `MensajeEnviado`
  - [ ] Alertas y chat son canales separados que se silencian por separado
  - [ ] Un test de arquitectura impide que Mensajería dependa de Notificaciones
- [ ] **KAL-102 · Guía de instalación en iPhone** — ADR-11 · S · requiere KAL-100
  - [ ] Detecta un iPhone sin la PWA instalada y explica cómo añadirla a la pantalla de inicio

## Fase 11 · Diario y recursos

El diario es la única pieza que cuelga de la Cuenta y no del Espacio.

- [ ] **KAL-110 · Diario privado** — CU-08, ADR-15 · M · requiere KAL-054
  - [ ] `EntradaDiario` pertenece a la Cuenta
  - [ ] Ni el administrador del espacio puede leerlo (test de autorización)
- [ ] **KAL-111 · Diario sin conexión** — ADR-41 · S · requiere KAL-110, KAL-075
  - [ ] Las entradas usan la misma cola de envío y el Id generado en el cliente
- [ ] **KAL-112 · Recursos de apoyo** — CU-08 · S · requiere KAL-070
  - [ ] Contenido estático en el frontend, disponible sin conexión

## Fase 12 · Informe semanal

El informe es una consulta de solo lectura: no se guarda, se calcula al abrirlo.

- [ ] **KAL-120 · Consulta del informe de 7 días** — CU-07, ADR-27 · M · requiere KAL-060
  - [ ] Consulta de solo lectura (CQRS ligero), sin pasar por el agregado
  - [ ] Distingue parte registrado, sin registro y corregido
- [ ] **KAL-121 · PDF del informe** — ADR-21, ADR-27 · M · requiere KAL-120
  - [ ] El PDF indica "del X al Y, generado el Z"
  - [ ] Solo se descarga con sesión iniciada
  - [ ] Licencia de la librería PDF revisada antes de usarla (p. ej. QuestPDF)
- [ ] **KAL-122 · Aviso de informe nuevo** — ADR-17, ADR-31 · M · requiere KAL-101, KAL-120
  - [ ] Proceso programado: lunes a las 9:00 (Europe/Madrid), push a los miembros
  - [ ] Indicador "informe nuevo" en la app hasta que se abre
  - [ ] Probado con reloj falso

## Fase 13 · Ciclo de vida del espacio

Borrar datos es irreversible: cada regla se prueba con reloj falso antes de activarla en producción.

- [ ] **KAL-130 · Registro de actividad** — ADR-26, ADR-41 · S · requiere KAL-060
  - [ ] Un parte, un mensaje o entrar en la app actualizan la última actividad del espacio
- [ ] **KAL-131 · Tarea diaria de inactividad** — ADR-22, ADR-26 · M · requiere KAL-130, KAL-050
  - [ ] A los 5 meses sin actividad se envía el email de aviso
  - [ ] A los 6 meses se borra el espacio
  - [ ] Un espacio con actividad tras el aviso no se borra
- [ ] **KAL-132 · Borrado real del espacio** — ADR-18 · M · requiere KAL-131
  - [ ] Se borran partes, mensajes, invitaciones, membresías y persona dependiente
  - [ ] El diario de la cuidadora se conserva
- [ ] **KAL-133 · Salir del espacio y transferir la administración** — ADR-25 · S · requiere KAL-021, KAL-054
  - [ ] Un miembro que no es administrador puede salir
  - [ ] El administrador transfiere antes de salir, con las reglas de KAL-021

## Fase 14 · Antes del piloto

No es código, pero bloquea el uso con familias reales.

- [ ] **KAL-140 · Consulta RGPD art. 9** — ADR-19 · bloqueante
  - [ ] Decidir con un experto cómo tratar la medicación, las notas y el diario
  - [ ] Aplicar lo decidido (por ejemplo: cifrado, consentimiento explícito, registro de accesos)
  - [ ] Política de privacidad publicada en la app
