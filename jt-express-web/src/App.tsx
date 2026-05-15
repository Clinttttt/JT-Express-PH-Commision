import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Navbar from "./components/shared/Navbar/Navbar";
import Footer from "./components/shared/Footer/Footer";
import HomePage from "./features/home/HomePage";
import ServicesPage from "./features/services/ServicesPage";
import RatesPage from "./features/rates/RatesPage";
import RatesAdminPage from "./features/rates/RatesAdminPage";
import TrackingPage from "./features/tracking/TrackingPage";
import ShipmentsAdminPage from "./features/tracking/ShipmentsAdminPage";
import BranchesPage from "./features/branches/BranchesPage";
import BranchesAdminPage from "./features/branches/BranchesAdminPage";
import SignupPage from "./features/auth/SignupPage";
import LoginPage from "./features/auth/LoginPage";

function AppRoutes() {
  return (
    <div className="app-shell">
      <Navbar />
      <main className="app-main">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/home" element={<HomePage />} />
          <Route path="/services" element={<ServicesPage />} />
          <Route path="/rates" element={<RatesPage />} />
          <Route path="/admin/rates" element={<RatesAdminPage />} />
          <Route path="/tracking" element={<TrackingPage />} />
          <Route path="/admin/shipments" element={<ShipmentsAdminPage />} />
          <Route path="/branches" element={<BranchesPage />} />
          <Route path="/admin/branches" element={<BranchesAdminPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignupPage />} />
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </main>
      <Footer />
    </div>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AppRoutes />
    </BrowserRouter>
  );
}
