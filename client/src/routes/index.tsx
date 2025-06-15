import { Routes, Route, Navigate } from "react-router-dom";
import Home from "../pages/Home";
import Login from "../pages/Login";
import type { JSX } from "react";

function PrivateRoute({ children }: { children: JSX.Element }) {
    const token = localStorage.getItem("accessToken");
    return token ? children : <Navigate to="/login" />;
}

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/login" element={<Login />} />
            <Route
                path="/home"
                element={
                    <PrivateRoute>
                        <Home />
                    </PrivateRoute>
                }
            />
            <Route
                path="/home/new"
                element={
                    <PrivateRoute>
                        <Home showFormInitial={true} />
                    </PrivateRoute>
                }
            />
            <Route
                path="/home/edit/:id"
                element={
                    <PrivateRoute>
                        <Home editMode={true} />
                    </PrivateRoute>
                }
            />
            {/* Redireciona "/" para "/home" */}
            <Route path="/" element={<Navigate to="/home" />} />
        </Routes>
    );
}