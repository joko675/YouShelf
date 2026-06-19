# YouShelf

Desktopowa aplikacja do zarządzania osobistą biblioteką książek. Umożliwia dodawanie, edytowanie i filtrowanie książek według statusu czytania, wraz z obsługą okładek przechowywanych lokalnie.

---

## Technologie

| Warstwa              | Technologia  | Wersja       |
| -------------------- | ------------ | ------------ |
| Framework desktopowy | Tauri        | 2.x          |
| Frontend framework   | Vue 3        | 3.5.13       |
| Język                | TypeScript   | ~5.6.2       |
| Bundler              | Vite         | 6.0.3        |
| Zarządzanie stanem   | Pinia        | 3.0.4        |
| Router               | Vue Router   | 5.0.4        |
| Biblioteka UI        | NuxtUI       | 4.6.1        |
| Stylowanie           | Tailwind CSS | 4.2.4        |
| Backend              | Rust         | 2021 edition |

---

## Wymagania

Przed uruchomieniem projektu upewnij się, że masz zainstalowane:

- **Node.js** (v18 lub nowszy)
- **npm** (v9 lub nowszy)
- **Rust** (instalacja przez [rustup.rs](https://rustup.rs/))
- **Xcode Command Line Tools** (macOS): `xcode-select --install`
- Na Windowsie: Microsoft C++ Build Tools i WebView2

---

## Instalacja i uruchomienie

```bash
# 1. Zainstaluj zależności Node.js
npm install

# 2. Uruchom aplikację w trybie deweloperskim (frontend + backend Tauri)
npm run tauri dev

# 3. Zbuduj wersję produkcyjną
npm run tauri build
```

> **Uwaga:** `npm run dev` uruchamia wyłącznie serwer Vite (frontend) bez warstwy Tauri. Pełna aplikacja działa tylko przez `npm run tauri dev`.

Dostępne skrypty npm:

| Komenda               | Opis                                              |
| --------------------- | ------------------------------------------------- |
| `npm run dev`         | Uruchamia serwer deweloperski Vite na porcie 1420 |
| `npm run build`       | Sprawdzenie typów TypeScript + build Vite         |
| `npm run tauri dev`   | Uruchamia pełną aplikację desktopową (dev)        |
| `npm run tauri build` | Buduje instalator aplikacji                       |

---

## Architektura projektu

Aplikacja oparta jest na architekturze **Tauri**, w której:

- **Frontend** (Vue 3 + Vite) renderuje interfejs użytkownika w wbudowanym widoku webowym (WebView).
- **Backend** (Rust) zapewnia dostęp do natywnych API systemu operacyjnego przez pluginy Tauri.
- **Komunikacja** między warstwami odbywa się przez system komend i pluginów Tauri (IPC).

---

## Warstwa frontendowa

### Zarządzanie stanem (Pinia)

#### UserDataStore

Plik: [src/stores/UserDataStore.ts](src/stores/UserDataStore.ts)

Główny "sklep" stanu aplikacji. Przechowuje wszystkie dane o książkach.

**Stan:**

| Pole       | Typ       | Opis                                |
| ---------- | --------- | ----------------------------------- |
| `userData` | `Book[]`  | Tablica wszystkich książek          |
| `loading`  | `boolean` | Flaga ładowania danych przy starcie |

**Akcje:**

| Akcja              | Opis                                                                      |
| ------------------ | ------------------------------------------------------------------------- |
| `updateUserData()` | Odczytuje dane z pliku, sortuje malejąco po ID, ustawia `loading = false` |
| `addBook(dto)`     | Wywołuje serwis dodawania i odświeża dane                                 |
| `editBook(dto)`    | Wywołuje serwis edycji i odświeża dane                                    |

**Watcher:** Głęboka obserwacja tablicy `userData` — każda zmiana automatycznie wywołuje `writeUserData()` i zapisuje dane na dysku.

---

#### BookFilterStore

Plik: [src/stores/BookFilterStore.ts](src/stores/BookFilterStore.ts)

Sklep odpowiedzialny za filtrowanie listy książek.

**Stan:**

| Pole             | Typ      | Wartość domyślna |
| ---------------- | -------- | ---------------- |
| `selectedStatus` | `string` | `"READING"`      |

**Computed:**

| Pole            | Opis                                                          |
| --------------- | ------------------------------------------------------------- |
| `filteredBooks` | Filtruje `userData` z `UserDataStore` według `selectedStatus` |

---

### Komponenty Vue

#### App.vue

Plik: [src/App.vue](src/App.vue)

Główny komponent aplikacji. Przy montowaniu wywołuje `updateUserData()` (ładowanie danych z dysku). Wyświetla stan ładowania podczas inicjalizacji.

---

#### MainView

Plik: [src/views/MainView.vue](src/views/MainView.vue)

Strona główna. Renderuje:

- Przycisk "Dodaj książkę" -> nawigacja do `/bookForm`
- Przycisk "Zmień motyw" -> przełączanie trybu jasny/ciemny
- Komponent `BookFilter` (filtrowanie statusu)
- Komponent `BookList` (siatka książek)

---

#### BookList

Plik: [src/components/BookList.vue](src/components/BookList.vue)

Wyświetla przefiltrowane książki w responsywnej siatce. Korzysta z `BookFilterStore.filteredBooks`.

- Układ: responsywny w zależności od wielkości okna -> 1/2/3 kolumny
- Każda karta (`UCard`) zawiera okładkę, tytuł, przycisk edycji
- Kliknięcie karty -> `/bookDetails/:id`
- Kliknięcie przycisku edycji -> `/bookForm/:id`

---

#### BookFilter

Plik: [src/components/BookFilter.vue](src/components/BookFilter.vue)

Grupa przycisków do filtrowania listy po statusie.

Dostępne filtry:

| Wartość     | Etykieta    |
| ----------- | ----------- |
| `READING`   | Czytane     |
| `READ`      | Przeczytane |
| `FINISHED`  | Skończone   |
| `CANCELLED` | Porzucone   |

---

#### BookForm

Plik: [src/components/BookForm.vue](src/components/BookForm.vue)

Formularz dodawania i edycji książki. Prop `bookId` (opcjonalny) przełącza formularz w tryb edycji.

**Pola formularza:**

| Pole           | Typ    | Wymagane | Opis                         |
| -------------- | ------ | -------- | ---------------------------- |
| `title`        | string | Tak      | Tytuł książki                |
| `author`       | string | Tak      | Autor                        |
| `rating`       | string | Nie      | Ocena użytkownika            |
| `releaseYear`  | number | Nie      | Rok wydania                  |
| `status`       | string | Nie      | Status (domyślnie `READING`) |
| `coverImgPath` | string | Nie      | Ścieżka do okładki           |

**Opcje statusu w formularzu:**

| Wartość     | Etykieta    |
| ----------- | ----------- |
| `READING`   | Czytana     |
| `READ`      | Przeczytana |
| `FINISHED`  | Skończona   |
| `CANCELLED` | Porzucona   |
| `PLANNING`  | Planowana   |

**Obsługa okładki:**

- Otwiera systemowe okno wyboru pliku — dozwolone: `jpg`, `png`, `jpeg`, `webp`
- Przycisk usuwania resetuje okładkę do `/blankCover.png`

**Walidacja:** Pola `title` i `author` są wymagane. Błędy wyświetlane są przez czerwone podkreślenie inputów i czyszczone po modyfikacji pola.

---

#### BookDetails

Plik: [src/components/BookDetails.vue](src/components/BookDetails.vue)

Widok szczegółów pojedynczej książki. Prop `bookId: number`.

Wyświetla:

- Okładkę
- Tytuł, autor, ocena (jeśli istnieje), rok wydania (jeśli istnieje)
- Status w formie listy przycisków

Układ responsywny: kolumna na małych ekranach, dwie kolumny na `lg+`.

---

### Serwisy

#### UserDataService

Plik: [src/services/UserDataService.ts](src/services/UserDataService.ts)

Warstwa dostępu do danych. Wszystkie operacje I/O przechodzą przez ten serwis z wykorzystaniem pluginu `@tauri-apps/plugin-fs`.

| Funkcja                  | Opis                                                                        |
| ------------------------ | --------------------------------------------------------------------------- |
| `createUserData()`       | Tworzy `userData.json` jeśli nie istnieje                                   |
| `createImagesFolder()`   | Tworzy katalog `coverImages` jeśli nie istnieje                             |
| `readUserData()`         | Odczytuje i parsuje `userData.json` -> `Book[]`                             |
| `writeUserData()`        | Serializuje i zapisuje tablicę `userData` do `userData.json`                |
| `addNewBookService(dto)` | Dodaje nową książkę: generuje ID, kopiuje okładkę, tworzy obiekt `Book`     |
| `editBookService(dto)`   | Edytuje istniejącą książkę: aktualizuje pola, opcjonalnie podmienia okładkę |
| `saveCoverImg()`         | Kopiuje plik okładki do `coverImages/{id}.{ext}`, zwraca ścieżkę lub `""`   |

**Generowanie ID:** `Math.max(...existingIds) + 1`, lub `1` dla pierwszej książki.

**Dozwolone formaty okładek:** `jpg`, `png`, `jpeg`, `webp`.

---

### Modele danych

Plik: [src/types/Book.ts](src/types/Book.ts)

#### `Book` — główny model danych

```typescript
interface Book {
  id: number; // Unikalne ID (auto-increment)
  title: string; // Tytuł
  author: string; // Autor
  rating: string | null; // Ocena użytkownika (opcjonalna)
  releaseYear: number | null; // Rok wydania (opcjonalny)
  status: string; // Status: READING | READ | FINISHED | CANCELLED | PLANNING
  coverImagePathFixed: string; // URL do okładki (http://asset.localhost/...)
}
```

#### `BookFormDto` — obiekt transferu danych z formularza

```typescript
interface BookFormDto {
  id?: number;
  title: string;
  author: string;
  rating: string | null;
  releaseYear: number | null;
  status: string;
  coverImgPath: string; // Ścieżka systemowa lub URL
}
```

---

### Stylowanie

**Stack:**

- **Tailwind CSS v4.2** — utility-first, klasy bezpośrednio w szablonach Vue
- **NuxtUI v4.6.1** — gotowe komponenty UI (`UButton`, `UCard`, `UInput`, `USelect`, `URadioGroup`, `UApp`)
- **@nuxtjs/color-mode** — tryb jasny / ciemny z przełącznikiem

**Globalne style** (`src/assets/main.css`):

---

## Warstwa backendowa (Tauri / Rust)

### Punkt wejścia

Plik: [src-tauri/src/main.rs](src-tauri/src/main.rs)

```rust
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

fn main() {
    youshelf_lib::run()
}
```

Dyrektywa `windows_subsystem = "windows"` ukrywa okno konsoli w buildach produkcyjnych na Windowsie.

---

Plik: [src-tauri/src/lib.rs](src-tauri/src/lib.rs)

Inicjalizacja buildera Tauri z pluginami i handlerami komend:

```rust
pub fn run() {
    tauri::Builder::default()
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_fs::init())
        .plugin(tauri_plugin_opener::init())
        .invoke_handler(tauri::generate_handler![greet])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
```

---

### Pluginy Tauri

| Plugin                | Wersja | Zastosowanie                                                   |
| --------------------- | ------ | -------------------------------------------------------------- |
| `tauri-plugin-fs`     | 2      | Operacje na systemie plików (odczyt, zapis, kopiowanie, mkdir) |
| `tauri-plugin-dialog` | 2      | Natywne okno wyboru pliku                                      |
| `tauri-plugin-opener` | 2      | Otwieranie zewnętrznych URL i plików                           |

---

### Uprawnienia i bezpieczeństwo

Plik: [src-tauri/capabilities/default.json](src-tauri/capabilities/default.json)

Wszystkie uprawnienia do systemu plików są ograniczone do katalogu `$RESOURCE/**` (katalog zasobów aplikacji).

| Uprawnienie                | Opis                                 |
| -------------------------- | ------------------------------------ |
| `fs:allow-create`          | Tworzenie plików                     |
| `fs:allow-read-text-file`  | Odczyt plików tekstowych             |
| `fs:allow-write-text-file` | Zapis plików tekstowych              |
| `fs:allow-exists`          | Sprawdzanie istnienia pliku/katalogu |
| `fs:allow-mkdir`           | Tworzenie katalogów                  |
| `fs:allow-copy-file`       | Kopiowanie plików                    |
| `dialog:allow-open`        | Otwieranie dialogu wyboru pliku      |

---

### Dane

Dane przechowywane są lokalnie w katalogu zasobów aplikacji (`$RESOURCE`):

```
$RESOURCE/
├── userData.json         # Główna baza danych (tablica obiektów Book)
└── coverImages/
    ├── 1.jpg             # Okładka książki o ID=1
    ├── 2.png             # Okładka książki o ID=2
    └── ...
```

**Format `userData.json`:**

```json
[
  {
    "id": 1,
    "title": "Przykładowa książka",
    "author": "Jan Kowalski",
    "rating": "8/10, fajna pozycja",
    "releaseYear": 2023,
    "status": "READING",
    "coverImagePathFixed": "http://asset.localhost/.../coverImages/1.jpg"
  }
]
```

**Automatyczny zapis:** Pinia watcher na `userData` (deep) wywołuje `writeUserData()` przy każdej zmianie stanu, zapewniając natychmiastową zmianę bez jawnego wywołania zapisu przez komponenty.

---

## Przepływ danych

### Start aplikacji

```
App.vue (onMounted)
  -> updateUserData()
    -> createUserData() + createImagesFolder()   // Inicjalizacja jeśli brak plików
    -> readUserData()                             // Odczyt userData.json
    -> sort by ID descending
    -> userData = Book[]                          // Ustawienie stanu Pinia
    -> loading = false
      -> MainView renderuje BookList z danymi
```

### Dodawanie książki

```
Użytkownik klika "Dodaj książkę"
  -> Nawigacja do /bookForm
  -> BookForm (pusty formularz)
  -> Użytkownik wypełnia pola, wybiera okładkę przez dialog
  -> Klik "Dodaj książkę"
    -> validateData()          // Walidacja tytułu i autora
    -> addBook(dto)
      -> addNewBookService(dto)
        -> Generowanie ID (max + 1)
        -> saveCoverImg()      // Kopiowanie okładki do coverImages/
        -> convertFileSrc()    // Konwersja ścieżki na URL
        -> userData.push(newBook)
          -> Pinia watcher
            -> writeUserData() // Zapis do userData.json
    -> updateUserData()        // Odświeżenie stanu
    -> router.push("/")        // Powrót do listy
```

### Edycja książki

```
Użytkownik klika przycisk edycji na karcie książki
  -> Nawigacja do /bookForm/:id
  -> BookForm (wypełniony danymi istniejącej książki)
  -> Użytkownik modyfikuje pola
  -> Klik "Zaktualizuj książkę"
    -> validateData()
    -> editBook(dto)
      -> editBookService(dto)
        -> Znalezienie książki po ID
        -> Opcjonalna podmiana okładki
        -> Aktualizacja pól w userData
          -> Pinia watcher -> writeUserData()
    -> updateUserData()
    -> router.push("/")
```

## Struktura katalogów

```
YouShelf/
├── index.html                  # Punkt wejścia HTML
├── package.json                # Zależności Node.js i skrypty
├── vite.config.ts              # Konfiguracja Vite
├── tsconfig.json               # Konfiguracja TypeScript
├── components.d.ts             # Autogenerowane typy komponentów
├── auto-imports.d.ts           # Autogenerowane importy
│
├── public/                     # Statyczne zasoby
│   ├── blankCover.png          # Domyślna okładka książki
│   └── ...
│
├── src/                        # Kod źródłowy frontendu
│   ├── main.ts                 # Inicjalizacja aplikacji Vue
│   ├── App.vue                 # Główny komponent (root)
│   ├── router.ts               # Definicja tras
│   ├── assets/
│   │   └── main.css            # Globalne style (Tailwind + NuxtUI)
│   ├── components/             # Komponenty wielokrotnego użytku
│   │   ├── BookList.vue        # Siatka książek
│   │   ├── BookFilter.vue      # Filtr statusu
│   │   ├── BookForm.vue        # Formularz dodawania/edycji
│   │   └── BookDetails.vue     # Widok szczegółów książki
│   ├── views/                  # Strony (powiązane z routerem)
│   │   ├── MainView.vue        # Strona główna
│   │   ├── BookFormView.vue    # Wrapper strony formularza
│   │   └── BookDetailsView.vue # Wrapper strony szczegółów
│   ├── stores/                 # Sklepy Pinia
│   │   ├── UserDataStore.ts    # Stan danych książek
│   │   └── BookFilterStore.ts  # Stan filtra
│   ├── services/
│   │   └── UserDataService.ts  # Operacje I/O na plikach
│   └── types/
│       └── Book.ts             # Interfejsy TypeScript
│
└── src-tauri/                  # Backend Tauri (Rust)
    ├── Cargo.toml              # Zależności Rust
    ├── tauri.conf.json         # Konfiguracja aplikacji Tauri
    ├── build.rs                # Skrypt budowania Rust
    ├── capabilities/
    │   └── default.json        # Uprawnienia bezpieczeństwa
    ├── icons/                  # Ikony aplikacji (różne rozmiary)
    └── src/
        ├── main.rs             # Punkt wejścia Rust
        └── lib.rs              # Inicjalizacja Tauri i komendy
```

---
