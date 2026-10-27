import { useEffect, useState } from "react";

type Company = {
  id: string;
  name: string;
  countryCode: string;
  currencyCode: string;
  isActive: boolean;
};

type Dashboard = {
  totalCompanies: number;
  totalCustomers: number;
  totalSuppliers: number;
  totalProducts: number;
  totalRevenue: number;
  totalExpenses: number;
  netResult: number;
};

export default function App() {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [dashboard, setDashboard] = useState<Dashboard | null>(null);
  const [name, setName] = useState("");

  useEffect(() => {
    fetch("https://localhost:5001/api/Companies")
      .then((res) => res.json())
      .then((data) => setCompanies(data))
      .catch(() => setCompanies([]));

    fetch("https://localhost:5001/api/Dashboard")
      .then((res) => res.json())
      .then((data) => setDashboard(data))
      .catch(() => setDashboard(null));
  }, []);

  const handleSubmit = async () => {
    await fetch("https://localhost:5001/api/Companies", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        name,
        countryCode: "TN",
        currencyCode: "TND",
        taxRegistrationNumber: "123456"
      })
    });

    setName("");
    window.location.reload();
  };

  return (
    <div style={{ padding: 24, fontFamily: "Arial", maxWidth: 1100, margin: "0 auto" }}>
      <h1>ERP-TN Global Suite</h1>

      <div style={{ display: "flex", gap: 12, marginBottom: 24, flexWrap: "wrap" }}>
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Nom de la société"
          style={{ padding: 10, width: 240 }}
        />
        <button onClick={handleSubmit} style={{ padding: "10px 18px" }}>Ajouter</button>
      </div>

      <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))", gap: 12, marginBottom: 30 }}>
        <StatCard label="Sociétés" value={dashboard?.totalCompanies ?? 0} />
        <StatCard label="Clients" value={dashboard?.totalCustomers ?? 0} />
        <StatCard label="Fournisseurs" value={dashboard?.totalSuppliers ?? 0} />
        <StatCard label="Produits" value={dashboard?.totalProducts ?? 0} />
        <StatCard label="Chiffre d'affaires" value={`${(dashboard?.totalRevenue ?? 0).toFixed(2)} TND`} />
        <StatCard label="Charges" value={`${(dashboard?.totalExpenses ?? 0).toFixed(2)} TND`} />
      </div>

      <div style={{ background: "#f5f5f5", padding: 16, borderRadius: 8 }}>
        <h2>Sociétés</h2>
        <ul>
          {companies.map((company) => (
            <li key={company.id}>
              {company.name} — {company.countryCode} — {company.currencyCode}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}

function StatCard({ label, value }: { label: string; value: string | number }) {
  return (
    <div style={{ background: "#111827", color: "white", borderRadius: 8, padding: 16 }}>
      <div style={{ fontSize: 12, opacity: 0.8 }}>{label}</div>
      <div style={{ fontSize: 22, fontWeight: 700, marginTop: 8 }}>{value}</div>
    </div>
  );
}
