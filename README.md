# YouShelf

Desktopowa aplikacja do zarządzania osobistą biblioteką książek. Pozwala dodawać, edytować i przeglądać książki z podziałem na status czytania.

## Technologie

- **Tauri 2** — framework desktopowy (Rust + WebView)
- **Vue 3** + TypeScript — interfejs użytkownika
- **Pinia** — zarządzanie stanem
- **NuxtUI** + Tailwind CSS — stylowanie
- **Vite** — bundler

## Wymagania

- Node.js (v18+)
- Rust 1.77.2+ (instalacja przez [rustup.rs](https://rustup.rs/))
- Xcode Command Line Tools (macOS): `xcode-select --install`

## Instalacja i uruchomienie

```bash
npm install
npm run tauri dev
```

> `npm run dev` uruchamia tylko frontend. Pełna aplikacja działa przez `npm run tauri dev`.

## Struktura projektu

```
src/
├── components/       # Komponenty Vue (BookList, BookForm, BookDetails, BookFilter)
├── views/            # Strony powiązane z routerem
├── stores/           # Pinia — stan książek i filtrów
├── services/         # Operacje na plikach (zapis/odczyt JSON)
└── types/            # Interfejsy TypeScript (Book, BookFormDto)

src-tauri/            # Backend Rust — inicjalizacja pluginów Tauri
```

## Jak działa aplikacja

Dane książek zapisywane są lokalnie w pliku `userData.json` w katalogu zasobów aplikacji. Okładki książek kopiowane są do folderu `coverImages/`. Zapis następuje automatycznie przy każdej zmianie stanu (watcher Pinia).

### Statusy książek

| Wartość | Opis |
|---|---|
| READING | Aktualnie czytana |
| READ | Przeczytana |
| FINISHED | Ukończona |
| CANCELLED | Porzucona |
| PLANNING | Planowana |
