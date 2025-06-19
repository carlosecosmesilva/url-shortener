import React, { useState } from "react";
import api from "../api";
import { useNavigate } from "react-router-dom";
import styles from "./Login.module.css";

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [isRegister, setIsRegister] = useState(false);
    const [registerSuccess, setRegisterSuccess] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        try {
            const response = await api.post("/auth/login", { username, password });
            localStorage.setItem("accessToken", response.data.accessToken);
            localStorage.setItem("refreshToken", response.data.refreshToken);
            navigate("/");
        } catch {
            setError("Login failed");
        }
    };

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setRegisterSuccess(false);
        try {
            await api.post("/auth/register", { username, password });
            setRegisterSuccess(true);
            setIsRegister(false);
            setUsername("");
            setPassword("");
        } catch {
            setError("Registration failed");
        }
    };

    return (
        <div className={styles["login-container"]}>
            <h1 className={styles["login-title"]}>{isRegister ? "Create Account" : "Login"}</h1>
            <form
                className={styles["login-form"]}
                onSubmit={isRegister ? handleRegister : handleLogin}
            >
                <label htmlFor="username">Username:</label>
                <input
                    id="username"
                    type="text"
                    value={username}
                    onChange={e => setUsername(e.target.value)}
                    required
                />
                <label htmlFor="password">Password:</label>
                <input
                    id="password"
                    type="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    required
                />
                <button type="submit">{isRegister ? "Register" : "Login"}</button>
            </form>
            {error && <div className="error-message">{error}</div>}
            {registerSuccess && (
                <div style={{ color: "green", marginTop: 12 }}>
                    Registration successful! You can now log in.
                </div>
            )}
            <div style={{ marginTop: 16 }}>
                {isRegister ? (
                    <button
                        type="button"
                        className={styles["toggle-btn"]}
                        onClick={() => {
                            setIsRegister(false);
                            setError(null);
                        }}
                    >
                        Back to Login
                    </button>
                ) : (
                    <button
                        type="button"
                        className={styles["toggle-btn"]}
                        onClick={() => {
                            setIsRegister(true);
                            setError(null);
                            setRegisterSuccess(false);
                        }}
                    >
                        Create Account
                    </button>
                )}
            </div>
        </div>
    );
}