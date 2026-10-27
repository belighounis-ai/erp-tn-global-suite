# ERP-TN Global Suite

Suite ERP multi-pays, multi-sociétés et multi-devise, conçue pour un déploiement web + desktop + réseau local.

## Modules
- Gestion des sociétés / filiales
- Comptabilité générale
- Plan comptable local
- TVA et fiscalité
- Achats et fournisseurs
- Ventes et clients
- Stock et entrepôts
- Trésorerie et banque
- RH et paie
- Reporting / tableau de bord
- Audit et traçabilité
- Multi-pays via packs de localisation

## Pack local Tunisie inclus
- support du PCG tunisien
- devise TND
- TVA locale
- règles comptables tunisiennes
- paramètres multi-sociétés

## Stack
- Backend: ASP.NET Core 8
- Database: PostgreSQL
- Frontend web: React + TypeScript + Vite
- Desktop: Electron
- Container: Docker Compose

## Démarrage rapide

### 1. Démarrer PostgreSQL
```bash
docker compose up -d
```

### 2. Lancer l'API
```bash
cd backend
cd ERP.TN.Api
dotnet restore
dotnet run
```

### 3. Lancer le front web
```bash
cd web
npm install
npm run dev
```

### 4. Lancer l'application desktop
```bash
cd desktop
npm install
npm run start
```

## Structure principale
```text
backend/
  ERP.TN.Api/
  ERP.TN.Application/
  ERP.TN.Domain/
  ERP.TN.Infrastructure/
web/
desktop/
```

## Objectif
Cette base sert de point de départ pour construire un ERP de niveau entreprise, avec un modèle modulaire et des règles de localisation par pays.
