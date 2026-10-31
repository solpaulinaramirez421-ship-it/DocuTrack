# DocuTrack – Taller Integrador: Árboles Binarios (MVC en C#)

Universidad de Manizales – Facultad de Ciencias e Ingeniería
Asignatura: Programación III – Actividad colaborativa I

Módulo interno para gestionar de forma jerárquica el repositorio de documentos de DocuTrack S.A. mediante un **árbol binario de búsqueda (BST)**, implementado en C# (.NET, aplicación de consola) con arquitectura **MVC**.

## Integrantes y responsabilidades

| Integrante | Responsabilidades |
|------------|-------------------|
| [Sol Paulina Ramirez] | [Ej.: Model (Nodo, ArbolBinario), recorridos, configuración del repositorio] |
| [Mariana Jimenez Giraldo] | [Ej.: View (impresión ASCII), Controller, README y pruebas] |

> Completar los nombres y las responsabilidades reales antes de entregar. Todos los integrantes hicieron commit en este repositorio.

## Requisitos para ejecutar

- [SDK de .NET](https://dotnet.microsoft.com/download) (probado con .NET 10).
- Git (para clonar el repositorio).
- Una terminal (PowerShell, CMD, Terminal de VS Code, etc.).

## Cómo ejecutar

```bash
git clone https://github.com/solpaulinaramirez421-ship-it/DocuTrack.git
cd DocuTrack
dotnet build
dotnet run
```

No se requiere ninguna configuración adicional: el programa compila y ejecuta directamente. La consola usa UTF-8 para mostrar correctamente los conectores `├──`, `└──` y `│`.

## Estructura del proyecto

```
DocuTrack/
├── Models/
│   ├── Nodo.cs                      # Nombre, EsCarpeta, Izquierdo, Derecho
│   ├── ArbolBinario.cs              # Insertar, Buscar, Eliminar
│   └── ArbolBinario.Recorridos.cs   # Preorden, Inorden, Postorden, Por niveles, Altura
├── Views/
│   └── ConsolaView.cs               # Toda la presentación en consola (incluye el árbol ASCII)
├── Controllers/
│   └── DocuTrackController.cs       # Orquesta los casos de uso en orden
└── Program.cs                       # Solo invoca al Controller
```

### Responsabilidad de cada capa

- **Model:** clases de dominio y toda la lógica del BST. No tiene ninguna referencia a `Console`. Devuelve resultados (booleanos, nodos, listas, enumeraciones) en lugar de imprimir.
- **View:** solo presentación: imprime el árbol en ASCII, resultados de búsqueda, inserciones, eliminaciones, recorridos y métricas. No contiene lógica del árbol.
- **Controller:** secuencia los casos de uso y conecta Model con View. No contiene lógica estructural del BST.
- **Program.cs:** únicamente crea el Controller y lo ejecuta. No hay lógica de negocio en el punto de entrada.

## Decisiones de diseño

- **Clave del BST:** `Nombre`, comparado con `StringComparison.OrdinalIgnoreCase` (sin distinguir mayúsculas y minúsculas, comparación ordinal).
- **Política de duplicados:** las inserciones de un nombre ya existente se **rechazan** (`Insertar` devuelve `false`) y la View registra el intento en consola. Esto incluye nombres que solo difieren en mayúsculas (por ejemplo `ACTAS` frente a `Actas`).
- **Validaciones:** se rechazan nombres nulos, vacíos o compuestos solo por espacios.
- **Eliminar:**
  - *Hoja:* se desenlaza.
  - *Un hijo:* el hijo se reconecta con el padre.
  - *Dos hijos:* se reemplaza por el **sucesor** (mínimo del subárbol derecho) y luego se elimina el nodo movido. Se usa siempre el sucesor para mantener la consistencia.
  - La operación devuelve el caso ejecutado (`CasoEliminacion`) y la View lo informa.
- **Actualizar:** se implementa como **Eliminar(nombreAntiguo) + Insertar(nombreNuevo)**, validando antes que el nombre antiguo exista y que el nombre nuevo no exista. Se conserva si el nodo era carpeta o archivo.
- **Métrica de comparaciones:** se cuenta en `Insertar`, `Buscar` y `Eliminar` (en `Eliminar` incluye las comparaciones del descenso hasta el nodo y, en el caso de dos hijos, las de la eliminación del sucesor).
- **Altura:** se cuenta en **niveles** (árbol vacío = 0, solo la raíz = 1), no en aristas.
- **Recorrido por niveles:** implementado con `Queue<Nodo>`.

## Casos de uso que ejecuta el programa

El Controller ejecuta, en este orden y con encabezados claros en consola:

1. **Construcción del árbol:** inserta 14 nombres (mezcla de carpetas y archivos). La raíz tiene dos subárboles, hay hojas a ambos lados y nodos con un solo hijo. Todos los archivos quedan como hojas. Luego intenta insertar un duplicado (`ACTAS`) para mostrar que se rechaza, e imprime el árbol inicial en ASCII.
2. **Seis búsquedas:** 2 existentes en el subárbol izquierdo, 2 existentes en el derecho y 2 inexistentes, cada una con hallado/no hallado y número de comparaciones.
3. **Tres actualizaciones:** sobre una hoja, sobre un nodo con un solo hijo y sobre la raíz, imprimiendo el árbol tras cada una.
4. **Tres eliminaciones:** de una hoja, de un nodo con un solo hijo y de la raíz (dos hijos, con sucesor), imprimiendo el árbol tras cada una.
5. **Recorridos:** preorden, inorden, postorden y por niveles sobre el árbol final.
6. **Altura final** del árbol.

### Ejemplo de la salida del árbol

```
[C] Documentos
├── (I) [C] Contratos
│   ├── (I) [C] Balances
│   │   └── (I) [C] Actas
│   │       └── (I) [A] acta1.pdf
│   └── (D) [A] cotizaciones.xlsx
└── (D) [C] Proyectos
    ├── (I) [C] Informes
    │   ├── (I) [C] Facturas
    │   └── (D) [A] manual.docx
    └── (D) [C] Respaldos
        ├── (I) [A] reportes.pdf
        └── (D) [C] Usuarios
            └── (I) [A] tareas.txt
```

Leyenda: `[C]` carpeta, `[A]` archivo, `(I)` hijo izquierdo, `(D)` hijo derecho. Se marca el lado de cada hijo porque, cuando un nodo tiene un único hijo, el dibujo por sí solo no indica si está a la izquierda o a la derecha.

## Justificación: ¿qué recorrido confirma que el BST sigue ordenado?

El recorrido **inorden** (izquierdo, raíz, derecho). En un BST todos los nombres del subárbol izquierdo son menores que la raíz y todos los del subárbol derecho son mayores. Al visitar primero el subárbol izquierdo, luego la raíz y finalmente el derecho, y repetir esto de forma recursiva, los nodos se visitan siempre en **orden ascendente** según la clave.

Por eso, si después de las actualizaciones y eliminaciones el inorden sigue entregando los nombres ordenados alfabéticamente (sin distinguir mayúsculas), se confirma que la propiedad del BST se mantuvo. En la ejecución final del programa:

```
Inorden: acta1.pdf, Balances, Clientes, Contratos, Gerencia, Informes, manual_v2.docx, Proyectos, reportes.pdf, Respaldos, tareas.txt
```

Los preorden, postorden y por niveles no sirven para esta verificación, porque su orden depende de la forma del árbol y no de la ordenación de las claves.

## Limitaciones conocidas

- Se usa un BST simple, sin balanceo, de forma deliberada y con fines pedagógicos de la Unidad 1. El balanceo se abordará en unidades posteriores. En el peor caso (inserciones ya ordenadas) el árbol se degenera en una lista.
- La regla "un archivo es siempre hoja" se cumple por el conjunto de nombres y el orden de inserción elegidos para las pruebas. El modelo, por sí solo, no impide que un archivo reciba hijos si se insertan otros nombres en un orden distinto.
- El árbol se carga con datos fijos definidos en el Controller; el programa no es interactivo.
