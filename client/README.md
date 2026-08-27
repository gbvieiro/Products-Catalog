# Products Catalog - Client

Frontend em React + TypeScript + Vite para a Api em `../src/ProductsCatalog.Api`.

## Organizacao (feature-based, o padrao mais comum em times React hoje)

```
src/
├── app/            # composition root do frontend: providers globais, rotas, <App/>
├── shared/         # codigo sem dono de feature especifica
│   ├── api/        # instancia do axios (com interceptors de auth) + helpers de erro
│   ├── auth/        # AuthContext (sessao/login/logout, localStorage) + ProtectedRoute
│   ├── types/      # tipos compartilhados (ex: PagedResult<T>, Role)
│   └── ui/         # componentes puramente visuais e reutilizaveis (Layout, StatusMessage)
└── features/
    ├── auth/       # tela de login (POST /api/auth/login via shared/auth/AuthContext)
    ├── orders/     # feature de referencia, 100% funcional
    │   ├── api/         # 1 funcao por chamada HTTP, sempre via shared/api/httpClient
    │   ├── hooks/       # useOrders/useCreateOrder/useCancelOrder (React Query)
    │   ├── components/  # componentes especificos da feature
    │   ├── pages/       # componente de rota (o que entra em app/routes.tsx)
    │   └── types.ts     # tipos que espelham os DTOs/Commands do backend
    ├── books/      # feature de apoio (lista + criacao), usada pelo formulario de pedidos
    ├── customers/  # CRUD de clientes (create/list/editar/excluir), usada pelo formulario de pedidos
    ├── stocks/     # CRUD de estoque por livro (create/list/editar quantidade/excluir)
    └── users/      # CRUD de usuarios (create/list/editar/excluir) - so Administrator
```

Todas as features de CRUD (orders, books, stocks, customers, users) seguem a
mesma estrutura vertical acima. `stocks`, `customers` e `users` tem um
componente a mais que `orders` nao precisa: um `XxxForm.tsx` que serve tanto
para criar quanto para editar (troca de modo via uma prop `editingXxx`
opcional), ja que create/read/list em `orders` nao tem edicao. `auth` foge um
pouco do padrao por ser so uma tela (sem lista/CRUD): tem `types.ts` e
`api/authApi.ts`, mas a logica de sessao (persistir o token, expor
`user`/`login`/`logout`/`hasRole`) mora em `shared/auth/AuthContext.tsx`, ja
que precisa ser acessada de qualquer lugar da arvore (menu, rotas
protegidas, etc.) - nao so da tela de login.

Cada feature e "vertical": tudo que ela precisa (api, hooks, componentes, tipos)
mora dentro dela. `shared/` so tem o que realmente e usado por mais de uma
feature. Isso evita o classico "pastas por tipo de arquivo" (todos os hooks
juntos, todos os componentes juntos) que fica dificil de navegar conforme o
projeto cresce.

## Autenticacao

- `shared/auth/AuthContext.tsx`: guarda a sessao atual (token + dados do
  usuario) em `localStorage` (sobrevive a um refresh da pagina) e em memoria;
  expõe `login`/`logout`/`user`/`isAuthenticated`/`hasRole(...)` via
  `useAuth()`. `shared/api/httpClient.ts` injeta automaticamente o header
  `Authorization: Bearer <token>` em toda chamada (via interceptor) e chama
  `logout()` sozinho sempre que a Api responde 401 (token ausente/expirado/
  invalido) - nenhuma feature precisa tratar isso na mao.
- `shared/auth/ProtectedRoute.tsx`: usado em `app/routes.tsx` para exigir
  login (redireciona pra `/login` se nao autenticado) e, opcionalmente, uma
  role especifica (`allowedRoles`) - as telas administrativas
  (books/stocks/customers/users) sao Administrator-only tanto na rota quanto
  no menu (`shared/ui/Layout.tsx`, que filtra os itens de nav por
  `hasRole()`).
- `shared/types/role.ts`: `Role` (Administrator=1, Seller=2) espelha o enum
  `ERole` do backend. Usa um objeto `const` em vez de `enum` porque o
  tsconfig do projeto tem `erasableSyntaxOnly: true` (Vite/TS 5.8+ exigem
  sintaxe "erasable" - `enum` gera codigo em runtime e nao se qualifica).

## Stack

- **Vite + React + TypeScript**
- **React Router** para rotas
- **TanStack Query (React Query)** para cache/sincronizacao com o servidor -
  isso substitui `useState` + `useEffect` manual para chamadas HTTP
- **Axios** como HTTP client

## Como rodar

```bash
npm install
cp .env.example .env   # opcional, so se for apontar para uma Api que nao seja localhost:5145
npm run dev
```

Em desenvolvimento, o Vite faz proxy de `/api/*` para a Api ASP.NET Core
(`http://localhost:5145` por padrao, configuravel via `VITE_API_PROXY_TARGET`)
- assim nao ha problema de CORS mesmo a Api estando em outra porta.

**Nota:** `package.json`/`package-lock.json` ja foram gerados rodando
`npm install` localmente - se voce clonar o repo em outra maquina, so
precisa rodar `npm install` de novo (node_modules nao vai pro git).
