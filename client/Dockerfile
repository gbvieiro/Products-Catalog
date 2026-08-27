# Client em modo dev (servidor do Vite) - uso local via docker-compose (servico
# "client"). Para rodar sem Docker, veja client/README.md.
FROM node:22-alpine

WORKDIR /app

COPY package.json ./
RUN npm install

COPY . .

EXPOSE 5173

# --host e necessario para o servidor do Vite aceitar conexoes vindas de fora
# do container (por padrao ele só escuta em localhost).
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]
