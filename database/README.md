# Banco de Dados (SQLite)

Este diretório contém o schema SQL do projeto baseado no diagrama enviado:

- `schema.sql`: cria todas as tabelas, FKs, constraints e índices.

## Como criar o banco local

No root do projeto:

```bash
sqlite3 database/pedwm.db < database/schema.sql
```

## Como validar tabelas criadas

```bash
sqlite3 database/pedwm.db ".tables"
```

## Observações

- UUID foi modelado como `TEXT`.
- Datas foram modeladas como `TEXT` no formato ISO-8601.
- Enums foram modelados com `CHECK`.
- Herança do diagrama (`ProjectBase` e `TaskBase`) foi modelada com tabela base + tabelas filhas 1:1.
- O app Flutter foi preparado para criar esse schema automaticamente em `lib/database/`.
