import React, { useEffect, useState } from "react";

type TabKey = "dashboard" | "accounting" | "payroll" | "purchases" | "sales" | "stock" | "security";

type Account = { id: string; code: string; name: string; type: string };
type Voucher = { id: string; number: string; journalCode: string; totalAmount: number; status: string };
type PayrollRun = { id: string; period: string; employeeCount: number; grossTotal: number; netTotal: number; status: string };
type StockItem = { id: string; productCode: string; productName: string; quantity: number; unitPrice: number; warehouse: string };
type Purchase = { id: string; number: string; supplierName: string; totalAmount: number; status: string };
type Sale = { id: string; number: string; customerName: string; totalAmount: number; status: string };

type PanelProps = {
  title: string;
  children: React.ReactNode;
};

function Panel({ title, children }: PanelProps) {
  return (
    <div style={{ background: "#111827", color: "white", borderRadius: 12, padding: 18, boxShadow: "0 8px 18px rgba(0,0,0,0.12)" }}>
      <h3 style={{ marginTop: 0 }}>{title}</h3>
      {children}
    </div>
  );
}

function Stat({ label, value }: { label: string; value: string }) {
  return (
    <div style={{ background: "#0f172a", borderRadius: 10, padding: 14, minWidth: 150 }}>
      <div style={{ fontSize: 12, color: "#a5b4fc" }}>{label}</div>
      <div style={{ fontSize: 22, fontWeight: 700, marginTop: 10 }}>{value}</div>
    </div>
  );
}

export default function App() {
  const [tab, setTab] = useState<TabKey>("dashboard");
  const [token, setToken] = useState<string>("" );
  const [username, setUsername] = useState("admin");
  const [password, setPassword] = useState("admin123");
  const [accounts, setAccounts] = useState<Account[]>([]);
  const [vouchers, setVouchers] = useState<Voucher[]>([]);
  const [payrollRuns, setPayrollRuns] = useState<PayrollRun[]>([]);
  const [stock, setStock] = useState<StockItem[]>([]);
  const [purchases, setPurchases] = useState<Purchase[]>([]);
  const [sales, setSales] = useState<Sale[]>([]);

  const api = (endpoint: string, method = "GET", body?: unknown) => {
    return fetch(`https://localhost:5001${endpoint}`, {
      method,
      headers: {
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {})
      },
      body: body ? JSON.stringify(body) : undefined
    }).then(r => r.json());
  };

  const login = async () => {
    const result = await fetch("https://localhost:5001/api/Auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password })
    });

    if (result.ok) {
      const data = await result.json();
      setToken(data.token);
      alert(`Connecté en tant que ${data.role}`);
    } else {
      alert("Identifiants invalides");
    }
  };

  const loadData = async () => {
    if (!token) return;

    try {
      const [acct, vouchersData, payroll, stockData, purchaseData, salesData] = await Promise.all([
        api("/api/Accounting/accounts"),
        api("/api/Accounting/vouchers"),
        api("/api/Payroll"),
        api("/api/Inventory"),
        api("/api/Purchases"),
        api("/api/Sales")
      ]);

      setAccounts(acct);
      setVouchers(vouchersData);
      setPayrollRuns(payroll);
      setStock(stockData);
      setPurchases(purchaseData);
      setSales(salesData);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    if (token) {
      loadData();
    }
  }, [token]);

  const tabs: { key: TabKey; label: string }[] = [
    { key: "dashboard", label: "Tableau de bord" },
    { key: "accounting", label: "Comptabilité" },
    { key: "payroll", label: "Paie" },
    { key: "purchases", label: "Achats" },
    { key: "sales", label: "Ventes" },
    { key: "stock", label: "Stock" },
    { key: "security", label: "Sécurité" }
  ];

  return (
    <div style={{ fontFamily: "Arial, sans-serif", background: "#e5e7eb", minHeight: "100vh", padding: 24 }}>
      <div style={{ maxWidth: 1400, margin: "0 auto" }}>
        <header style={{ display: "flex", alignItems: "center", justifyContent: "space-between", background: "#111827", color: "white", borderRadius: 14, padding: "16px 20px", marginBottom: 20 }}>
          <div>
            <div style={{ fontSize: 12, opacity: 0.75 }}>ERP-TN Global Suite</div>
            <h2 style={{ margin: 0 }}>Client / Serveur Production</h2>
          </div>
          <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
            <input value={username} onChange={e => setUsername(e.target.value)} style={{ padding: 10, borderRadius: 8, border: "none" }} placeholder="Utilisateur" />
            <input type="password" value={password} onChange={e => setPassword(e.target.value)} style={{ padding: 10, borderRadius: 8, border: "none" }} placeholder="Mot de passe" />
            <button onClick={login} style={{ padding: "10px 16px", borderRadius: 8, border: "none", cursor: "pointer", background: "#22c55e", color: "white", fontWeight: 700 }}>Connexion</button>
          </div>
        </header>

        <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 20 }}>
          {tabs.map(item => (
            <button
              key={item.key}
              onClick={() => setTab(item.key)}
              style={{
                background: tab === item.key ? "#0f172a" : "white",
                color: tab === item.key ? "white" : "#111827",
                border: "none",
                borderRadius: 10,
                padding: "10px 14px",
                fontWeight: 700,
                cursor: "pointer"
              }}
            >
              {item.label}
            </button>
          ))}
        </nav>

        {tab === "dashboard" && (
          <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))", gap: 16 }}>
            <Stat label="Sociétés" value="12" />
            <Stat label="Clients" value="340" />
            <Stat label="Fournisseurs" value="190" />
            <Stat label="Produits" value="620" />
            <Stat label="CA" value="487500 TND" />
            <Stat label="Charges" value="321200 TND" />
            <Stat label="Résultat" value="166300 TND" />
            <Stat label="Access" value={token ? "JWT OK" : "non connecté"} />
          </div>
        )}

        {tab === "accounting" && (
          <div style={{ display: "grid", gap: 16 }}>
            <Panel title="Plan comptable Tunisie">
              <table style={{ width: "100%", borderCollapse: "collapse", color: "white" }}>
                <thead>
                  <tr style={{ background: "#1f2937" }}>
                    <th style={{ padding: 8, textAlign: "left" }}>Code</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Libellé</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Type</th>
                  </tr>
                </thead>
                <tbody>
                  {accounts.slice(0, 10).map(a => (
                    <tr key={a.id}>
                      <td style={{ padding: 8 }}>{a.code}</td>
                      <td style={{ padding: 8 }}>{a.name}</td>
                      <td style={{ padding: 8 }}>{a.type}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </Panel>

            <Panel title="Pièces comptables">
              <table style={{ width: "100%", borderCollapse: "collapse", color: "white" }}>
                <thead>
                  <tr style={{ background: "#1f2937" }}>
                    <th style={{ padding: 8, textAlign: "left" }}>Numéro</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Journal</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Montant</th>
                    <th style={{ padding: 8, textAlign: "left" }}>Statut</th>
                  </tr>
                </thead>
                <tbody>
                  {vouchers.map(v => (
                    <tr key={v.id}>
                      <td style={{ padding: 8 }}>{v.number}</td>
                      <td style={{ padding: 8 }}>{v.journalCode}</td>
                      <td style={{ padding: 8 }}>{v.totalAmount}</td>
                      <td style={{ padding: 8 }}>{v.status}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </Panel>
          </div>
        )}

        {tab === "payroll" && (
          <Panel title="Paie et RH">
            <table style={{ width: "100%", borderCollapse: "collapse", color: "white" }}>
              <thead>
                <tr style={{ background: "#1f2937" }}>
                  <th style={{ padding: 8, textAlign: "left" }}>Période</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Employés</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Brut</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Net</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Statut</th>
                </tr>
              </thead>
              <tbody>
                {payrollRuns.map(p => (
                  <tr key={p.id}>
                    <td style={{ padding: 8 }}>{p.period}</td>
                    <td style={{ padding: 8 }}>{p.employeeCount}</td>
                    <td style={{ padding: 8 }}>{p.grossTotal}</td>
                    <td style={{ padding: 8 }}>{p.netTotal}</td>
                    <td style={{ padding: 8 }}>{p.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Panel>
        )}

        {tab === "purchases" && (
          <Panel title="Achats fournisseurs">
            <table style={{ width: "100%", borderCollapse: "collapse", color: "white" }}>
              <thead>
                <tr style={{ background: "#1f2937" }}>
                  <th style={{ padding: 8, textAlign: "left" }}>N°</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Fournisseur</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Montant</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Statut</th>
                </tr>
              </thead>
              <tbody>
                {purchases.map(p => (
                  <tr key={p.id}>
                    <td style={{ padding: 8 }}>{p.number}</td>
                    <td style={{ padding: 8 }}>{p.supplierName}</td>
                    <td style={{ padding: 8 }}>{p.totalAmount}</td>
                    <td style={{ padding: 8 }}>{p.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Panel>
        )}

        {tab === "sales" && (
          <Panel title="Ventes clients">
            <table style={{ width: "100%", borderCollapse: "collapse", color: "white" }}>
              <thead>
                <tr style={{ background: "#1f2937" }}>
                  <th style={{ padding: 8, textAlign: "left" }}>N°</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Client</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Montant</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Statut</th>
                </tr>
              </thead>
              <tbody>
                {sales.map(s => (
                  <tr key={s.id}>
                    <td style={{ padding: 8 }}>{s.number}</td>
                    <td style={{ padding: 8 }}>{s.customerName}</td>
                    <td style={{ padding: 8 }}>{s.totalAmount}</td>
                    <td style={{ padding: 8 }}>{s.status}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Panel>
        )}

        {tab === "stock" && (
          <Panel title="Gestion de stock">
            <table style={{ width: "100%", borderCollapse: "collapse", color: "white" }}>
              <thead>
                <tr style={{ background: "#1f2937" }}>
                  <th style={{ padding: 8, textAlign: "left" }}>Code</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Produit</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Qté</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Prix</th>
                  <th style={{ padding: 8, textAlign: "left" }}>Entrepôt</th>
                </tr>
              </thead>
              <tbody>
                {stock.map(s => (
                  <tr key={s.id}>
                    <td style={{ padding: 8 }}>{s.productCode}</td>
                    <td style={{ padding: 8 }}>{s.productName}</td>
                    <td style={{ padding: 8 }}>{s.quantity}</td>
                    <td style={{ padding: 8 }}>{s.unitPrice}</td>
                    <td style={{ padding: 8 }}>{s.warehouse}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Panel>
        )}

        {tab === "security" && (
          <Panel title="Sécurité et rôles">
            <ul style={{ color: "white", lineHeight: 2 }}>
              <li>JWT activé sur l’API</li>
              <li>Rôles : Admin, Finance, Achats, Ventes, Logistique</li>
              <li>Policies d’autorisation appliquées</li>
              <li>API protégée par bearer token</li>
              <li>Client serveur prêt pour exploitation</li>
            </ul>
          </Panel>
        )}
      </div>
    </div>
  );
}
