# ERP-TN Global Suite

Suite ERP multi-pays, multi-sociétés et multi-devise, conçue pour un déploiement web + desktop + réseau local.

## Modules inclus
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

## Module comptabilité ajouté
- plan de comptes
- journaux
- pièces comptables
- écritures comptables
- TVA
- comptes de tiers
- reporting financier

## Module RH / Paie ajouté
- employés
- paie
- congés
- absences
- cotisations
- bulletin de paie

## Modules achats / ventes / stock ajoutés
- fournisseurs
- clients
- commandes achats
- commandes ventes
- stock disponible
- inventaire
- suivi de mouvements

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
cd backend/ERP.TN.Api
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

## Version production
Cette version prépare un ERP de niveau entreprise avec les composants de base pour la production, l’audit, les modules de gestion et le support multi-pays.
