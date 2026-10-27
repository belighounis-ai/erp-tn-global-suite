import React, { useEffect, useMemo, useState } from "react";

type JwtResponse = { token: string; username: string; role: string };

type TvaItem = { code: string; label: string; rate: number; collected: number; deductible: number };

type Account = { id: string; code: string; name: string; type: string; countryCode: string };
type Journal = { id: string; code: string; name: string; countryCode: string };
type ReportingSummary = { totalRevenue: number; totalExpenses: number; netResult: number; totalTvaCollected: number; totalTvaDeductible: number; netTva: number };

type VoucherLineForm = { accountCode: string; accountName: string; debitCredit: "DEBIT" | "CREDIT"; amount: number };

type VoucherPayload = {
  number: string;
  journalCode: string;
  reference: string;
  voucherDate: string;
  lines: VoucherLineForm[];
};

type ImportExportType = "product" | "customer" | "supplier" | "chartaccount" | "voucher" | "employee" | "journal";

export default function App() {
  const [token, setToken] = useState<string>("");
  const [username, setUsername] = useState("admin");
  const [password, setPassword] = useState("admin123");
  const [menu, setMenu] = useState("dashboard");
  const [accounts, setAccounts] = useState<Account[]>([]);
  const [journalList, setJournalList] = useState<Journal[]>([]);
  const [report, setReport] = useState<ReportingSummary | null>(null);
  const [tva, setTva] = useState<TvaItem[]>([]);
  const [journalCode, setJournalCode] = useState("GEN");
  const [journalName, setJournalName] = useState("Journal Général");
  const [voucherNumber, setVoucherNumber] = useState("PC-2026-001");
  const [reference, setReference] = useState("Facture fournisseur");
  const [voucherLines, setVoucherLines] = useState<VoucherLineForm[]>([
    { accountCode: "211", accountName: "Banques", debitCredit: "DEBIT", amount: 1200 },
    { accountCode: "401", accountName: "Fournisseurs", debitCredit: "CREDIT", amount: 1200 }
  ]);
  const [importType, setImportType] = useState<ImportExportType>("product");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [importResult, setImportResult] = useState<{ totalRows: number; importedRows: number; rejectedRows: number; errors: string[] } | null>(null);

  const api = async (endpoint: string, method = "GET", body?: unknown, customHeaders?: Record<string, string>) => {
    const resp = await fetch(`https://localhost:5001${endpoint}`, {
      method,
      headers: {
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...(customHeaders ?? {})
      },
      body: body ? JSON.stringify(body) : undefined
    });

    if (!resp.ok) {
      throw new Error(`Erreur API: ${resp.status}`);
    }

    return await resp.json();
  };

  const login = async () => {
    try {
      const result: JwtResponse = await fetch("https://localhost:5001/api/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
      }).then(r => {
        if (!r.ok) throw new Error("Identifiants invalides");
        return r.json();
      });

      setToken(result.token);
      alert(`Connecté: ${result.role}`);
    } catch (e) {
      alert("Connexion refusée");
    }
  };

  const loadData = async () => {
    if (!token) return;

    try {
      const [acc, journals, reportData, tvaData] = await Promise.all([
        api("/api/Accounting/accounts"),
        api("/api/Journal"),
        api("/api/Reporting/summary"),
        api("/api/Reporting/tva")
      ]);

      setAccounts(acc);
      setJournalList(journals);
      setReport(reportData);
      setTva(tvaData);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    if (token) loadData();
  }, [token]);

  const createJournal = async () => {
    if (!token) return alert("Connectez-vous");
    const payload = { code: journalCode, name: journalName, countryCode: "TN" };
    await api("/api/Journal", "POST", payload);
    await loadData();
    alert("Journal créé");
  };

  const addVoucherLine = () => {
    setVoucherLines(prev => [...prev, { accountCode: "601", accountName: "Achats", debitCredit: "DEBIT", amount: 0 }]);
  };

  const updateLine = (index: number, field: keyof VoucherLineForm, value: string | number) => {
    setVoucherLines(prev => prev.map((line, i) => i === index ? { ...line, [field]: value } : line));
  };

  const createVoucher = async () => {
    if (!token) return alert("Connectez-vous");

    const payload: VoucherPayload = {
      number: voucherNumber,
      journalCode: journalCode,
      reference: reference,
      voucherDate: new Date().toISOString(),
      lines: voucherLines
    };

    await api("/api/Accounting/vouchers", "POST", payload);
    await loadData();
    alert("Pièce comptable créée");
  };

  const downloadTemplate = async (format: "xlsx" | "csv") => {
    const response = await fetch(`https://localhost:5001/api/ImportExport/template/${importType}?format=${format}`, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${importType}-template.${format}`;
    a.click();
    window.URL.revokeObjectURL(url);
  };

  const importFile = async () => {
    if (!selectedFile) return alert("Sélectionnez un fichier");
    const formData = new FormData();
    formData.append("file", selectedFile);

    const response = await fetch(`https://localhost:5001/api/ImportExport/import/${importType}`, {
      method: "POST",
      headers: token ? { Authorization: `Bearer ${token}` } : {},
      body: formData
    });

    const data = await response.json();
    setImportResult(data);
    alert(`Import terminé: ${data.importedRows} lignes importées, ${data.rejectedRows} rejetées`);
  };

  const exportFile = async (format: "xlsx" | "csv") => {
    const response = await fetch(`https://localhost:5001/api/ImportExport/export/${importType}?format=${format}`, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${importType}.${format}`;
    a.click();
    window.URL.revokeObjectURL(url);
  };

  const summaryCards = useMemo(() => [
    { label: "Chiffre d'affaires", value: `${report?.totalRevenue ?? 0} TND` },
    { label: "Charges", value: `${report?.totalExpenses ?? 0} TND` },
    { label: "Résultat net", value: `${report?.netResult ?? 0} TND` },
    { label: "TVA nette", value: `${report?.netTva ?? 0} TND` }
  ], [report]);

  if (!token) {
    return (
      <div style={{ minHeight: "100vh", background: "linear-gradient(135deg, #0f172a, #1e293b)", display: "flex", alignItems: "center", justifyContent: "center" }}>
        <div style={{ width: 420, background: "#fff", borderRadius: 18, padding: 24, boxShadow: "0 20px 50px rgba(0,0,0,0.25)" }}>
          <h2 style={{ marginTop: 0, color: "#0f172a" }}>ERP-TN Global Suite</h2>
          <p style={{ color: "#475569", marginBottom: 20 }}>Connexion sécurisée</p>
          <div style={{ display: "grid", gap: 12 }}>
            <input value={username} onChange={e => setUsername(e.target.value)} placeholder="Utilisateur" style={{ padding: 12, borderRadius: 10, border: "1px solid #cbd5e1" }} />
            <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Mot de passe" style={{ padding: 12, borderRadius: 10, border: "1px solid #cbd5e1" }} />
            <button onClick={login} style={{ background: "#0f172a", color: "white", padding: 12, border: "none", borderRadius: 10, fontWeight: 700, cursor: "pointer" }}>Se connecter</button>
          </div>
          <div style={{ marginTop: 20, color: "#475569", fontSize: 12 }}>
            Comptes de test: admin/admin123, finance/finance123, achats/achats123, ventes/ventes123, logistique/logistique123
          </div>
        </div>
      </div>
    );
  }

  return (
    <div style={{ display: "flex", minHeight: "100vh", background: "#e5e7eb" }}>
      <aside style={{ width: 260, background: "#0f172a", color: "white", padding: 18 }}>
        <h3 style={{ marginTop: 0 }}>ERP-TN</h3>
        <div style={{ display: "grid", gap: 8 }}>
          {[
            ["dashboard", "Tableau de bord"],
            ["accounting", "Comptabilité"],
            ["payroll", "Paie"],
            ["purchases", "Achats"],
            ["sales", "Ventes"],
            ["stock", "Stock"],
            ["reporting", "Reporting"],
            ["import-export", "Import / Export"],
            ["security", "Sécurité"]
          ].map(([key, label]) => (
            <button
              key={key}
              onClick={() => setMenu(key)}
              style={{
                background: menu === key ? "#1d4ed8" : "#111827",
                color: "white",
                border: "none",
                borderRadius: 10,
                padding: "10px 12px",
                textAlign: "left",
                cursor: "pointer",
                fontWeight: 700
              }}
            >
              {label}
            </button>
          ))}
        </div>
      </aside>

      <main style={{ flex: 1, padding: 24 }}>
        {menu === "dashboard" && (
          <div>
            <h2>Tableau de bord</h2>
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))", gap: 16, marginBottom: 20 }}>
              {summaryCards.map(s => <div key={s.label} style={{ background: "#0f172a", color: "white", borderRadius: 12, padding: 18 }}><div style={{ fontSize: 12, opacity: 0.8 }}>{s.label}</div><div style={{ fontSize: 26, fontWeight: 700, marginTop: 8 }}>{s.value}</div></div>)}
            </div>
            <div style={{ background: "white", borderRadius: 14, padding: 18 }}>
              <h3>Vue d’ensemble</h3>
              <p>ERP multi-société, multi-devise, multi-pays, prêt pour l’exploitation.</p>
            </div>
          </div>
        )}

        {menu === "accounting" && (
          <div style={{ display: "grid", gap: 18 }}>
            <div style={{ background: "white", borderRadius: 14, padding: 18 }}>
              <h3>Créer un journal</h3>
              <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr auto", gap: 12 }}>
                <input value={journalCode} onChange={e => setJournalCode(e.target.value)} placeholder="Code" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                <input value={journalName} onChange={e => setJournalName(e.target.value)} placeholder="Nom du journal" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                <button onClick={createJournal} style={{ background: "#0f172a", color: "white", border: "none", borderRadius: 8, padding: "10px 16px", cursor: "pointer" }}>Créer</button>
              </div>
              <div style={{ marginTop: 16 }}>
                <strong>Journaux existants:</strong>
                <ul>
                  {journalList.map(j => <li key={j.id}>{j.code} - {j.name}</li>)}
                </ul>
              </div>
            </div>

            <div style={{ background: "white", borderRadius: 14, padding: 18 }}>
              <h3>Créer une pièce comptable</h3>
              <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 12, marginBottom: 16 }}>
                <input value={voucherNumber} onChange={e => setVoucherNumber(e.target.value)} placeholder="Numéro" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                <input value={journalCode} onChange={e => setJournalCode(e.target.value)} placeholder="Journal" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                <input value={reference} onChange={e => setReference(e.target.value)} placeholder="Référence" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
              </div>

              <div style={{ display: "grid", gap: 10 }}>
                {voucherLines.map((line, index) => (
                  <div key={index} style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr 1fr", gap: 10 }}>
                    <input value={line.accountCode} onChange={e => updateLine(index, "accountCode", e.target.value)} placeholder="Compte" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                    <input value={line.accountName} onChange={e => updateLine(index, "accountName", e.target.value)} placeholder="Libellé" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                    <select value={line.debitCredit} onChange={e => updateLine(index, "debitCredit", e.target.value)} style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }}>
                      <option value="DEBIT">Débit</option>
                      <option value="CREDIT">Crédit</option>
                    </select>
                    <input type="number" value={line.amount} onChange={e => updateLine(index, "amount", Number(e.target.value))} placeholder="Montant" style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }} />
                  </div>
                ))}
              </div>

              <div style={{ display: "flex", gap: 12, marginTop: 16, alignItems: "center" }}>
                <button onClick={addVoucherLine} style={{ background: "#e2e8f0", color: "#0f172a", border: "none", borderRadius: 8, padding: "10px 14px", cursor: "pointer", fontWeight: 700 }}>+ Ligne</button>
                <button onClick={createVoucher} style={{ background: "#16a34a", color: "white", border: "none", borderRadius: 8, padding: "10px 16px", cursor: "pointer", fontWeight: 700 }}>Valider pièce</button>
              </div>
            </div>

            <div style={{ background: "white", borderRadius: 14, padding: 18 }}>
              <h3>Plan comptable Tunisie</h3>
              <table style={{ width: "100%", borderCollapse: "collapse" }}>
                <thead>
                  <tr style={{ background: "#e2e8f0" }}>
                    <th style={{ padding: 8, textAlign: "left" }}>Code</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Libellé</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Type</th>
                  </tr>
                </thead>
                <tbody>
                  {accounts.map(a => (
                    <tr key={a.id}>
                      <td style={{ padding: 8 }}>{a.code}</td>
                      <td style={{ padding: 8 }}>{a.name}</td>
                      <td style={{ padding: 8 }}>{a.type}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {menu === "reporting" && (
          <div style={{ display: "grid", gap: 18 }}>
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))", gap: 12 }}>
              <div style={{ background: "#0f172a", color: "white", borderRadius: 12, padding: 18 }}><div style={{ opacity: 0.8 }}>CA</div><div style={{ fontSize: 26, fontWeight: 700 }}>{report?.totalRevenue ?? 0} TND</div></div>
              <div style={{ background: "#0f172a", color: "white", borderRadius: 12, padding: 18 }}><div style={{ opacity: 0.8 }}>Charges</div><div style={{ fontSize: 26, fontWeight: 700 }}>{report?.totalExpenses ?? 0} TND</div></div>
              <div style={{ background: "#0f172a", color: "white", borderRadius: 12, padding: 18 }}><div style={{ opacity: 0.8 }}>Résultat net</div><div style={{ fontSize: 26, fontWeight: 700 }}>{report?.netResult ?? 0} TND</div></div>
            </div>
            <div style={{ background: "white", borderRadius: 12, padding: 18 }}>
              <h3>TVA Tunisienne</h3>
              <table style={{ width: "100%", borderCollapse: "collapse" }}>
                <thead>
                  <tr style={{ background: "#e2e8f0" }}>
                    <th style={{ padding: 8, textAlign: "left" }}>Code</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Libellé</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Taux</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Collectée</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Déductible</th>
                  </tr>
                </thead>
                <tbody>
                  {tva.map(item => (
                    <tr key={item.code}>
                      <td style={{ padding: 8 }}>{item.code}</td>
                      <td style={{ padding: 8 }}>{item.label}</td>
                      <td style={{ padding: 8 }}>{item.rate}</td>
                      <td style={{ padding: 8 }}>{item.collected}</td>
                      <td style={{ padding: 8 }}>{item.deductible}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {menu === "import-export" && (
          <div style={{ background: "white", borderRadius: 14, padding: 20 }}>
            <h3>Import / Export Excel & CSV</h3>
            <div style={{ display: "grid", gap: 14 }}>
              <div style={{ display: "flex", gap: 10, alignItems: "center", flexWrap: "wrap" }}>
                <select value={importType} onChange={e => setImportType(e.target.value as ImportExportType)} style={{ padding: 10, borderRadius: 8, border: "1px solid #cbd5e1" }}>
                  <option value="product">Produits</option>
                  <option value="customer">Clients</option>
                  <option value="supplier">Fournisseurs</option>
                  <option value="chartaccount">Comptes</option>
                  <option value="voucher">Pièces comptables</option>
                  <option value="employee">Employés</option>
                  <option value="journal">Journaux</option>
                </select>
                <button onClick={() => downloadTemplate("xlsx")} style={{ background: "#0f172a", color: "white", border: "none", borderRadius: 8, padding: "10px 14px", cursor: "pointer" }}>Télécharger modèle Excel</button>
                <button onClick={() => downloadTemplate("csv")} style={{ background: "#475569", color: "white", border: "none", borderRadius: 8, padding: "10px 14px", cursor: "pointer" }}>Télécharger modèle CSV</button>
                <button onClick={() => exportFile("xlsx")} style={{ background: "#2563eb", color: "white", border: "none", borderRadius: 8, padding: "10px 14px", cursor: "pointer" }}>Exporter Excel</button>
                <button onClick={() => exportFile("csv")} style={{ background: "#16a34a", color: "white", border: "none", borderRadius: 8, padding: "10px 14px", cursor: "pointer" }}>Exporter CSV</button>
              </div>

              <div style={{ border: "1px dashed #94a3b8", borderRadius: 12, padding: 18 }}>
                <input type="file" accept=".xlsx,.xls,.csv" onChange={e => setSelectedFile(e.target.files?.[0] ?? null)} />
                <div style={{ marginTop: 12 }}>
                  <button onClick={importFile} style={{ background: "#dc2626", color: "white", border: "none", borderRadius: 8, padding: "10px 16px", cursor: "pointer" }}>Importer le fichier</button>
                </div>
              </div>

              {importResult && (
                <div style={{ background: "#f8fafc", borderRadius: 10, padding: 14 }}>
                  <div><strong>Lignes totales:</strong> {importResult.totalRows}</div>
                  <div><strong>Importées:</strong> {importResult.importedRows}</div>
                  <div><strong>Rejetées:</strong> {importResult.rejectedRows}</div>
                  {importResult.errors.length > 0 && (
                    <div style={{ marginTop: 8 }}>
                      <strong>Erreurs:</strong>
                      <ul>
                        {importResult.errors.slice(0, 5).map((err, index) => <li key={index}>{err}</li>)}
                      </ul>
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
        )}

        {menu === "security" && (
          <div style={{ background: "white", borderRadius: 14, padding: 20 }}>
            <h3>Sécurité et production</h3>
            <ul>
              <li>JWT activé sur l’API</li>
              <li>Rôles : Admin / Finance / Achats / Ventes / Logistique</li>
              <li>Policies d’accès strictes</li>
              <li>API protégée par token</li>
              <li>Import / export Excel / CSV prêt</li>
            </ul>
          </div>
        )}

        {menu === "payroll" && <div style={{ background: "white", borderRadius: 14, padding: 20 }}><h3>Paie</h3><p>Module paie prêt à étendre avec bulletins, cotisations et déclarations sociales.</p></div>}
        {menu === "purchases" && <div style={{ background: "white", borderRadius: 14, padding: 20 }}><h3>Achats</h3><p>Paiements, fournisseurs et commandes d’achat.</p></div>}
        {menu === "sales" && <div style={{ background: "white", borderRadius: 14, padding: 20 }}><h3>Ventes</h3><p>Commandes clients, facturation et suivi de vente.</p></div>}
        {menu === "stock" && <div style={{ background: "white", borderRadius: 14, padding: 20 }}><h3>Stock</h3><p>Gestion d’entrepôts, inventaire et mouvements de stock.</p></div>}
      </main>
    </div>
  );
}
