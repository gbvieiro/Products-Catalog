# Products Catalog - Client

Frontend em React + TypeScript + Vite para a Api em `../src/ProductsCatalog.Api`.

## Organizacao (feature-based, o padrao mais comum em times React hoje)

```
src/
├── app/            # composition root do frontend: providers globais, rotas, <App/>
├── shared/         # codigo sem dono de feature especifica
│   ├── api/        # instancia do axios + helpers de erro
│   ├── types/      # tipos compartilhados (ex: PagedResult<T>)
│   └── ui/         # componentes puramente visuais e reutilizaveis (Layout, StatusMessage)
└── features/
    ├── orders/     # feature de referencia, 100% funcional
    │   ├── api/         # 1 funcao por chamada HTTP, sempre via shared/api/httpClient
    │   ├── hooks/       # useOrders/useCreateOrder/useCancelOrder (React Query)
    │   ├── components/  # componentes especificos da feature
    │   ├── pages/       # componente de rota (o que entra em app/routes.tsx)
    │   └── types.ts     # tipos que espelham os DTOs/Commands do backend
    ├── books/      # feature de apoio (lista + criacao), usada pelo formulario de pedidos
    ├── stocks/     # placeholder - siga o padrao de orders/ para implementar
    └── users/      # placeholder - siga o padrao de orders/ para implementar
```

Cada feature e "vertical": tudo que ela precisa (api, hooks, componentes, tipos)
mora dentro dela. `shared/` so tem o que realmente e usado por mais de uma
feature. Isso evita o classico "pastas por tipo de arquivo" (todos os hooks
juntos, todos os componentes juntos) que fica dificil de navegar conforme o
projeto cresce.

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

**Nota:** este ambiente nao tinha acesso de rede estável o suficiente para
rodar `npm install` durante a geração deste projeto (o disco montado do
Windows tornou a escrita de milhares de arquivos em `node_modules` muito
lenta) - `package.json` esta correto e completo, mas rode `npm install` na
sua maquina para gerar o `package-lock.json` e o `node_modules`.
