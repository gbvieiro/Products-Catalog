-- Script para configurar o PostgreSQL para o Products Catalog
-- Execute este script como superusuário (postgres) no PostgreSQL

-- 1. Criar o usuário (se não existir)
DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_user WHERE usename = 'admin') THEN
        CREATE USER admin WITH PASSWORD 'admin';
    END IF;
END
$$;

-- 2. Criar o banco de dados (se não existir)
SELECT 'CREATE DATABASE product_catalog OWNER admin'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'product_catalog')\gexec

-- 3. Conceder privilégios ao usuário
GRANT ALL PRIVILEGES ON DATABASE product_catalog TO admin;

-- 4. Conectar ao banco e conceder privilégios no schema
\c product_catalog

-- Conceder privilégios no schema público
GRANT ALL ON SCHEMA public TO admin;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO admin;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO admin;

-- Configurar privilégios padrão para objetos futuros
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO admin;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO admin;

