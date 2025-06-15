# 🌐 URL Shortener Client

This is the **React + TypeScript** frontend for the URL Shortener project.  
It provides a modern, user-friendly interface for shortening URLs, managing your links, and authenticating users via JWT.

---

## 🚀 Features

-   **Shorten URLs:**  
    Paste any long URL and get a short, shareable link.

-   **Authentication:**  
    Secure login with JWT and refresh token support.

-   **Protected Routes:**  
    Only authenticated users can access the main application.

-   **Responsive UI:**  
    Clean and responsive design using CSS modules.

---

## 🗂️ Project Structure

```
client/
├── public/                # Static assets
├── src/
│   ├── api/               # Axios instance and API config
│   ├── components/        # Reusable React components (e.g., UrlForm)
│   ├── pages/             # Main pages (Home, Login)
│   ├── routes/            # App routes and route guards
│   ├── styles/            # Global styles
│   ├── types/             # TypeScript types
│   ├── App.tsx
│   ├── main.tsx
│   └── vite-env.d.ts
├── .env                   # API URL and environment variables
├── package.json
├── tsconfig.json
└── vite.config.ts
```

---

## ⚙️ Setup & Usage

### Prerequisites

-   Node.js (v18+ recommended)
-   The backend API running (see [../server/README.md](../server/README.md))

### Installation

```sh
cd client
npm install
```

### Running the Development Server

```sh
npm run dev
```

Open [http://localhost:5173](http://localhost:5173) in your browser.

---

## 🔗 API Configuration

Set the backend API URL in the `.env` file:

```
VITE_API_URL=https://localhost:7115/api
```

or for HTTP:

```
VITE_API_URL=http://localhost:5142/api
```

---

## 🔒 Authentication Flow

-   Users log in via the `/login` page.
-   On successful login, the access and refresh tokens are stored in `localStorage`.
-   All API requests automatically include the JWT in the `Authorization` header.
-   Protected routes redirect unauthenticated users to the login page.

---

## 🧩 Main Dependencies

-   [React](https://react.dev/)
-   [TypeScript](https://www.typescriptlang.org/)
-   [Vite](https://vitejs.dev/)
-   [Axios](https://axios-http.com/)
-   [React Router](https://reactrouter.com/)

---

## 📝 Customization

-   Edit styles in `src/pages/*.module.css` or `src/styles/global.css`.
-   Add new pages to `src/pages/` and routes to `src/routes/index.tsx`.
-   Update API endpoints in `src/api/index.ts` if needed.

---

## 📜 License

This project is licensed under the terms of the [MIT License](../LICENSE)
