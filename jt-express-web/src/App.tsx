import { BrowserRouter, Routes, Route, Navigate, useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
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
import ForgotPasswordPage from "./features/auth/ForgotPasswordPage";
import "./transitions.css";

function AppRoutes() {
  const location = useLocation();
  const [displayLocation, setDisplayLocation] = useState(location);
  const [transitionStage, setTransitionStage] = useState("fadeIn");

  useEffect(() => {
    if (location !== displayLocation) setTransitionStage("fadeOut");
  }, [location, displayLocation]);

  return (
    <div className="app-shell">
      <Navbar />
      <main 
        className={`app-main ${transitionStage}`}
        onAnimationEnd={() => {
          if (transitionStage === "fadeOut") {
            setTransitionStage("fadeIn");
            setDisplayLocation(location);
          }
        }}
      >
        <Routes location={displayLocation}>
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
          <Route path="/forgot-password" element={<ForgotPasswordPage />} />
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
